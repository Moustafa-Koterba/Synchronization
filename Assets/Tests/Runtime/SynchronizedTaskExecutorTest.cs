using MKoterba.Synchronization;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SynchronizedTaskExecutorTest
    {
        private SynchronizedTaskExecutor executor;

        [SetUp]
        public void Setup()
        {
            executor = new GameObject().AddComponent<SynchronizedTaskExecutor>();
        }

        [UnityTest]
        public IEnumerator ExecuteSingleAsynchronizedTask()
        {            
            var executableParameter = new ExecutableParameter(
                ExecutableParameter.MODE.ASYNCHRONIZED,
                new AsynchronizedExecutableTest());
            executor.ExecutablesParameters.Add(executableParameter);
            executor.Execute();

            executor.ExecutablesParameters.ForEach(parameter =>
            {
                Assert.That(parameter.GetExecutable().State, Is.EqualTo(Executable.STATE.DONE));
            });

            yield return null;
        }

        [UnityTest]
        public IEnumerator ExecuteMultipleAsynchronizedTasks()
        {
            for (int i = 0; i < 2; i++)
            {
                var executableParameter = new ExecutableParameter(
                    ExecutableParameter.MODE.ASYNCHRONIZED,
                    new AsynchronizedExecutableTest());
                executor.ExecutablesParameters.Add(executableParameter);
            }

            executor.Execute();
            executor.ExecutablesParameters.ForEach(parameter =>
            {
                Assert.That(parameter.GetExecutable().State, Is.EqualTo(Executable.STATE.DONE));
            });

            yield return null;
        }

        [UnityTest]
        public IEnumerator ExecuteSingleSynchronizedTask()
        {
            int seconds = 1;
            var executableParameter = new ExecutableParameter(
                ExecutableParameter.MODE.SYNCHRONIZED,
                new SynchronizedExecutableTest(seconds));
            executor.ExecutablesParameters.Add(executableParameter);
            executor.Execute();

            executor.ExecutablesParameters.ForEach(parameter =>
            {
                Assert.That(parameter.GetExecutable().State, Is.EqualTo(Executable.STATE.EXECUTING));
            });

            yield return new WaitForSeconds(seconds);

            executor.ExecutablesParameters.ForEach(parameter =>
            {
                Assert.That(parameter.GetExecutable().State, Is.EqualTo(Executable.STATE.DONE));
            });
        }

        [UnityTest]
        public IEnumerator ExecuteSynchronizedThenAsynchronizedTasks()
        {
            int seconds = 1;
            var synchronizedExecutableParameter = new ExecutableParameter(
                ExecutableParameter.MODE.SYNCHRONIZED,
                new SynchronizedExecutableTest(seconds));

            var asynchronizedExecutableParameter = new ExecutableParameter(
                ExecutableParameter.MODE.ASYNCHRONIZED,
                new AsynchronizedExecutableTest());

            executor.ExecutablesParameters.Add(synchronizedExecutableParameter);
            executor.ExecutablesParameters.Add(asynchronizedExecutableParameter);
            executor.Execute();

            Assert.That(synchronizedExecutableParameter.GetExecutable().State, Is.EqualTo(Executable.STATE.EXECUTING));
            Assert.That(asynchronizedExecutableParameter.GetExecutable().State, Is.EqualTo(Executable.STATE.READY));            

            yield return new WaitForSeconds(seconds);

            Assert.That(synchronizedExecutableParameter.GetExecutable().State, Is.EqualTo(Executable.STATE.DONE));
            Assert.That(asynchronizedExecutableParameter.GetExecutable().State, Is.EqualTo(Executable.STATE.DONE));
        }


        [UnityTest]
        public IEnumerator ExecuteAsynchronizedThenSynchronizedTasks()
        {
            int seconds = 1;
            var synchronizedExecutableParameter = new ExecutableParameter(
                ExecutableParameter.MODE.SYNCHRONIZED,
                new SynchronizedExecutableTest(seconds));

            var asynchronizedExecutableParameter = new ExecutableParameter(
                ExecutableParameter.MODE.ASYNCHRONIZED,
                new AsynchronizedExecutableTest());

            executor.ExecutablesParameters.Add(asynchronizedExecutableParameter);
            executor.ExecutablesParameters.Add(synchronizedExecutableParameter);
            executor.Execute();

            Assert.That(asynchronizedExecutableParameter.GetExecutable().State, Is.EqualTo(Executable.STATE.DONE));
            Assert.That(synchronizedExecutableParameter.GetExecutable().State, Is.EqualTo(Executable.STATE.EXECUTING));

            yield return new WaitForSeconds(seconds);

            Assert.That(asynchronizedExecutableParameter.GetExecutable().State, Is.EqualTo(Executable.STATE.DONE));
            Assert.That(synchronizedExecutableParameter.GetExecutable().State, Is.EqualTo(Executable.STATE.DONE));
        }

        [UnityTest]
        public IEnumerator ExecuteMultipleTasks()
        {
            var count = 5;
            var seconds = 1;

            for (int i = 0; i < count; i++)
            {
                ExecutableParameter parameter;
                if (i % 2 == 0)
                {
                    parameter = new ExecutableParameter(
                        ExecutableParameter.MODE.ASYNCHRONIZED,
                        new AsynchronizedExecutableTest());
                } 
                else
                {
                    parameter = new ExecutableParameter(
                        ExecutableParameter.MODE.SYNCHRONIZED,
                        new SynchronizedExecutableTest(seconds));
                }

                executor.ExecutablesParameters.Add(parameter);
            }

            executor.Execute();

            yield return new WaitForSeconds(seconds * count);
                        
            for (int i = 0; i < count; i++)
            {
                var parameter = executor.ExecutablesParameters[i];
                Assert.That(parameter.GetExecutable().State, Is.EqualTo(Executable.STATE.DONE));
            }
        }
    }

    public class AsynchronizedExecutableTest : Executable
    {
        public override void Execute()
        {
            // do nothing to simulate asynchronism
        }

        public override void Execute(TaskCompletionSource<bool> completion)
        {
            throw new System.NotImplementedException();
        }
    }

    public class SynchronizedExecutableTest : Executable
    {
        private int seconds;

        public SynchronizedExecutableTest(int seconds)
        {
            this.seconds = seconds;
        }

        public override void Execute()
        {
            throw new System.NotImplementedException();
        }

        public override void Execute(TaskCompletionSource<bool> completion)
        {
            ExecuteAsync(completion);
        }

        private async void ExecuteAsync(TaskCompletionSource<bool> completion)
        {
            await Task.Delay(seconds);
            completion.SetResult(true);
        }
    }
}
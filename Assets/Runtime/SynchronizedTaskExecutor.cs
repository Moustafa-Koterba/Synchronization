using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MKoterba.Synchronization
{
    public class SynchronizedTaskExecutor: MonoBehaviour
    {
        [SerializeField]
        private bool executeAtStart;

        [SerializeField]
        private List<ExecutableParameter> executablesParameters = new List<ExecutableParameter>();
    
        public List<ExecutableParameter> ExecutablesParameters { get { return executablesParameters; } }
    
        private TaskCompletionSource<bool> completion = new TaskCompletionSource<bool>();

        private void Start()
        {
            if (executeAtStart)
            {
                ExecuteAsync(true);
            }
        }

        public void Execute()
        {
            ExecuteAsync();        
        }

        private async void ExecuteAsync(bool waitFrameEnd = false)
        {
            if (waitFrameEnd)
            {
                await Task.Yield();
            }

            var count = executablesParameters.Count;
            for (int i = 0; i < count; i++)
            {
                var parameter = executablesParameters[i];
                if (parameter.GetMode() == ExecutableParameter.MODE.SYNCHRONIZED)
                {
                    parameter.GetExecutable().DoExecute(completion);
                    await completion.Task;
                    completion = new TaskCompletionSource<bool>();
                }
                else
                {
                    parameter.GetExecutable().DoExecute();
                }
            }
        }

    }
}
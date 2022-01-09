using System.Threading.Tasks;
using UnityEngine;

namespace MKoterba.Synchronization
{
    public abstract class Executable : MonoBehaviour
    {
        private STATE state = STATE.READY;
        public STATE State
        {
            get { return state; }
        }

        public abstract void Execute();
        public abstract void Execute(TaskCompletionSource<bool> completion);

        private void PreExecute()
        {
            state = STATE.EXECUTING;
        }

        public void DoExecute()
        {
            PreExecute();
            Execute();
            AfterExecute();
        }
        public async void DoExecute(TaskCompletionSource<bool> completion)
        {
            PreExecute();
            Execute(completion);
            await completion.Task;
            AfterExecute();
        }

        private void AfterExecute()
        {
            state = STATE.DONE;
        }

        public enum STATE
        {
            READY,
            EXECUTING,
            DONE
        }
    }
}
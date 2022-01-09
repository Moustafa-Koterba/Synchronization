using System;
using UnityEngine;

namespace MKoterba.Synchronization
{
    [Serializable]
    public class ExecutableParameter
    {
        [SerializeField]
        private MODE mode;

        [SerializeField, SerializeReference]
        private Executable executable;

        public ExecutableParameter(MODE mode, Executable executable)
        {
            this.mode = mode;
            this.executable = executable;
        }

        public MODE GetMode() => mode;
        public void SetMode(MODE mode) => this.mode = mode;

        public Executable GetExecutable() => executable;
        public void SetExecutable(Executable executable) => this.executable = executable;

        public enum MODE
        {
            SYNCHRONIZED,
            ASYNCHRONIZED
        }
    }
}
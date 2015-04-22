using Okra.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra
{
    internal class ActivatedEventArgs : EventArgs, IActivatedEventArgs
    {
        public ActivatedEventArgs(ActivationKind kind, ApplicationExecutionState previousExecutionState)
        {
            PreviousExecutionState = previousExecutionState;
            Kind = kind;
        }
        
        public ActivationKind Kind { get; private set; }

        public ApplicationExecutionState PreviousExecutionState { get; private set; }
    }
}

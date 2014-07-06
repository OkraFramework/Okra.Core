using System;
using System.Collections.Generic;
using System.Text;

namespace Okra.Navigation
{
    public delegate void StateChangedEventHandler(object sender, StateChangedEventArgs e);

    public class StateChangedEventArgs : EventArgs
    {
        public StateChangedEventArgs(string stateKey)
        {
            this.StateKey = stateKey;
        }

        public string StateKey
        {
            get;
            private set;
        }
    }
}

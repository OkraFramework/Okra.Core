using Okra.Helpers;
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
            if (string.IsNullOrEmpty(stateKey))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), nameof(stateKey));

            this.StateKey = stateKey;
        }

        public string StateKey
        {
            get;
        }
    }
}

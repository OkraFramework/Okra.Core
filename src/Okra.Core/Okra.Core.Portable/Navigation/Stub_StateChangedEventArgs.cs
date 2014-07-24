using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    public delegate void StateChangedEventHandler(object sender, StateChangedEventArgs e);

    public class StateChangedEventArgs : EventArgs
    {
        public StateChangedEventArgs(string stateKey)
        {
            throw new NotImplementedException();
        }

        public string StateKey
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}

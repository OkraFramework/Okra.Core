using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    [DataContract]
    public class PageInfo
    {
        // *** Events ***

        public event StateChangedEventHandler StateChanged;

        // *** Constructors ***

        public PageInfo(string pageName, object arguments)
        {
            throw new NotImplementedException();
        }

        // *** Properties ***

        public string PageName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        // *** Methods ***

        public T GetArguments<T>()
        {
            throw new NotImplementedException();
        }

        public T GetState<T>(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetState<T>(string key, out T value)
        {
            throw new NotImplementedException();
        }

        public void SetState<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        // *** Protected Methods ***

        protected void OnStateChanged(string stateKey)
        {
            throw new NotImplementedException();
        }
    }
}

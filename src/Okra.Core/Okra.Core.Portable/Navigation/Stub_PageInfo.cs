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

#pragma warning disable 0067 // Ignore CS0067 "The event 'StateChanged' is never used" (since this is only a stub)
        public event StateChangedEventHandler StateChanged;
#pragma warning restore 0067

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

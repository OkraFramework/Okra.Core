using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Okra.Activation
{
    public class UniversalAppActivationRequest : IAppActivationRequest
    {
        // *** Constructors ***

        public UniversalAppActivationRequest(IActivatedEventArgs args)
        {
            this.EventArgs = args;
        }

        // *** Properties ***

        public IActivatedEventArgs EventArgs
        {
            get;
        }
    }
}

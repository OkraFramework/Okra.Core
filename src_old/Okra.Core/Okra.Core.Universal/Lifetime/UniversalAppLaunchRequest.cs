using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Okra.Lifetime
{
    public class UniversalAppLaunchRequest : IAppLaunchRequest
    {
        // *** Constructors ***

        public UniversalAppLaunchRequest(IActivatedEventArgs args)
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

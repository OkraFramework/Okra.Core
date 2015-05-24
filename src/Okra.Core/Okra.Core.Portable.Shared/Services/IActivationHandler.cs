using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#if NETFX_CORE
using Windows.ApplicationModel.Activation;
#endif

namespace Okra.Services
{
    public interface IActivationHandler
    {
        // *** Methods ***

        Task<bool> Activate(IActivatedEventArgs activatedEventArgs);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#if NETFX_CORE
using Windows.ApplicationModel.Activation;
#endif

namespace Okra.Services
{
    public interface IActivationManager
    {
        // *** Events ***

        event EventHandler<IActivatedEventArgs> Activating;
        event EventHandler<IActivatedEventArgs> Activated;

        // *** Methods ***

        Task<bool> Activate(IActivatedEventArgs activatedEventArgs);
        void Register(IActivationHandler service);
        void Unregister(IActivationHandler service);
    }
}

using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#if NETFX_CORE
using Windows.ApplicationModel.Activation;
#endif

namespace Okra.Services
{
    public class LaunchActivationHandler : ILaunchActivationHandler
    {
        // *** Fields ***

        private readonly INavigationManager navigationManager;

        // *** Constructors ***

        public LaunchActivationHandler(IActivationManager activationManager, INavigationManager navigationManager)
        {
            // Validate parameters

            if (activationManager == null)
                throw new ArgumentNullException("activationManager");

            if (navigationManager == null)
                throw new ArgumentNullException("navigationManager");

            // Store state

            this.navigationManager = navigationManager;

            // Register with the activation manager

            activationManager.Register(this);
        }

        // *** Methods ***

        public Task<bool> Activate(IActivatedEventArgs activatedEventArgs)
        {
            // Validate parameters

            if (activatedEventArgs == null)
                throw new ArgumentNullException("activatedEventArgs");

            // Call private internal method

            return ActivateInternal(activatedEventArgs);
        }

        private async Task<bool> ActivateInternal(IActivatedEventArgs activatedEventArgs)
        {
            if (activatedEventArgs.Kind == ActivationKind.Launch)
            {
                // If the previous execution state was terminated then attempt to restore the navigation stack

                if (activatedEventArgs.PreviousExecutionState == ApplicationExecutionState.Terminated)
                    await navigationManager.RestoreNavigationStack();

                // Otherwise navigate to the home page

                else
                    navigationManager.NavigateTo(navigationManager.HomePageName);

                return true;
            }

            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Navigation;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Okra.Services
{
    public class LaunchActivationHandler : ILaunchActivationHandler
    {
        // *** Fields ***

        private readonly INavigationManager navigationManager;

        // *** Constructors ***

        public LaunchActivationHandler(IActivationManager activationManager, INavigationManager navigationManager)
        {
            this.navigationManager = navigationManager;

            // Register with the activation manager

            activationManager.Register(this);
        }

        // *** Methods ***

        public async Task<bool> Activate(IActivatedEventArgs activatedEventArgs)
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

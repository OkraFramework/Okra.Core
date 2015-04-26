using Okra.Navigation;
using Okra.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

#if SILVERLIGHT
using Microsoft.Phone.Shell;
#endif

namespace Okra
{
    public class OkraApplication : Application
    {
        // *** Fields ***

        private const string AppTerminatedKey = "AppTerminated";

        private OkraBootstrapper bootstrapper;

        // *** Constructors ***

        public OkraApplication(OkraBootstrapper bootstrapper)
        {
            if (bootstrapper == null)
                throw new ArgumentNullException("bootstrapper");

            this.bootstrapper = bootstrapper;
            bootstrapper.Initialize(false);

            MainPage = new NavigationView(bootstrapper.NavigationManager);
        }

        // *** Overriden Base Methods ***

        protected override void OnStart()
        {
            base.OnStart();

            var applicationExecutionState = ApplicationExecutionState.NotRunning;

            if (Properties.ContainsKey(AppTerminatedKey))
            {
                Properties.Remove(AppTerminatedKey);
                applicationExecutionState = ApplicationExecutionState.Terminated;
            }          

            var args = new ActivatedEventArgs(ActivationKind.Launch, applicationExecutionState);
            Activate(args);
        }

        protected override void OnSleep()
        {
            if (bootstrapper.NavigationManager.NavigationStack.CanGoBack)
            {
                Properties.Add(AppTerminatedKey, true);
            }
            else if (Properties.ContainsKey(AppTerminatedKey))
            {
                Properties.Remove(AppTerminatedKey);
            }  

            var lifetimeManager = bootstrapper.LifetimeManager as ILifetimeAware;
            if (lifetimeManager != null)
            {
                lifetimeManager.OnSuspending().Wait();
            }

            base.OnSleep();
        }

        protected override void OnResume()
        {
            if (Properties.ContainsKey(AppTerminatedKey))
            {
                Properties.Remove(AppTerminatedKey);
            }   

            var lifetimeManager = bootstrapper.LifetimeManager as ILifetimeAware;
            if (lifetimeManager != null)
            {
                lifetimeManager.OnResuming().Wait();
            }

            base.OnResume();
        }

        // *** Private Methods ***

        private void Activate(IActivatedEventArgs args)
        {
            bootstrapper.Activate(args);
        }
    }
}

using Okra.Services;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using Xamarin.Forms;

namespace Okra
{
    public class OkraApplication : Application
    {
        // *** Fields ***

        private OkraBootstrapper bootstrapper;
        //private bool isApplicationInstancePreserved;

        // *** Constructors ***

        public OkraApplication(OkraBootstrapper bootstrapper)
        {
            if (bootstrapper == null)
                throw new ArgumentNullException("bootstrapper");

            this.bootstrapper = bootstrapper;
            bootstrapper.Initialize(false);

            var args = new ActivatedEventArgs(ActivationKind.Launch, ApplicationExecutionState.NotRunning);
            Activate(args);
        }

        // *** Overriden Base Methods ***

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnSleep()
        {
            var lifetimeManager = bootstrapper.LifetimeManager as ILifetimeAware;
            if (lifetimeManager != null)
            {
                lifetimeManager.OnSuspending().Wait();
            }

            base.OnSleep();

            //isApplicationInstancePreserved = true;
        }

        protected override void OnResume()
        {
            var lifetimeManager = bootstrapper.LifetimeManager as ILifetimeAware;
            if (lifetimeManager != null)
            {
                lifetimeManager.OnResuming().Wait();
            }

            //var applicationExecutionState = isApplicationInstancePreserved ? ApplicationExecutionState.Suspended : ApplicationExecutionState.Terminated;
            //var args = new ActivatedEventArgs(ActivationKind.Launch, applicationExecutionState);
            //Activate(args);

            base.OnResume();
        }

        // *** Private Methods ***

        private void Activate(IActivatedEventArgs args)
        {
            bootstrapper.Activate(args);
        }
    }
}

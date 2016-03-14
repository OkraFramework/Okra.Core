using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace Okra.Services
{
    public class LifetimeManager : LifetimeManagerBase
    {
        public LifetimeManager()
        {
            Application.Current.Suspending += this.OnSuspending;
            Application.Current.Resuming += this.OnResuming;
        }

        // *** Protected Methods ***

        protected async void OnSuspending(object sender, ISuspendingEventArgs e)
        {
            ISuspendingDeferral deferal = GetDeferral(e);

            await SuspendServicesAsync();

            deferal.Complete();
        }

        protected async void OnResuming(object sender, object e)
        {
            await ResumeServicesAsync();
        }

        protected virtual ISuspendingDeferral GetDeferral(ISuspendingEventArgs e)
        {
            // NB: Use a virtual method rather than getting the deferral directly to aid testing

            return e.SuspendingOperation.GetDeferral();
        }
    }
}

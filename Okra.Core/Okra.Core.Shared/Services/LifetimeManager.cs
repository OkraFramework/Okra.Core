using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Okra.Helpers;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;

namespace Okra.Services
{
    public class LifetimeManager : ILifetimeManager
    {
        // *** Fields ***

        private HashSet<ILifetimeAware> registeredServices = new HashSet<ILifetimeAware>();

        // *** Constructors ***

        public LifetimeManager()
        {
            Application.Current.Suspending += this.OnSuspending;
            Application.Current.Resuming += this.OnResuming;
            CoreApplication.Exiting += this.OnExiting;
        }

        // *** Methods ***

        public void Register(ILifetimeAware service)
        {
            // Validate parameters

            if (service == null)
                throw new ArgumentNullException("service");

            if (registeredServices.Contains(service))
                throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotRegisterServiceMultipleTimes"));

            // Add the service to the internal list

            registeredServices.Add(service);
        }

        public void Unregister(ILifetimeAware service)
        {
            // Validate parameters

            if (service == null)
                throw new ArgumentNullException("service");

            if (!registeredServices.Contains(service))
                throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotUnregisterUnregisteredService"));

            // Remove the service from the internal list

            registeredServices.Remove(service);
        }

        // *** Protected Methods ***

        protected async void OnSuspending(object sender, ISuspendingEventArgs e)
        {
            ISuspendingDeferral deferal = GetDeferral(e);

            IEnumerable<Task> resumingTasks = registeredServices.Select(service => service.OnSuspending());
            await Task.WhenAll(resumingTasks);

            deferal.Complete();
        }

        protected void OnResuming(object sender, object e)
        {
            IEnumerable<Task> resumingTasks = registeredServices.Select(service => service.OnResuming());
            Task.WhenAll(resumingTasks).Wait();
        }

        protected void OnExiting(object sender, object e)
        {
            IEnumerable<Task> resumingTasks = registeredServices.Select(service => service.OnExiting());
            Task.WhenAll(resumingTasks).Wait();
        }

        protected virtual ISuspendingDeferral GetDeferral(ISuspendingEventArgs e)
        {
            // NB: Use a virtual method rather than getting the deferral directly to aid testing

            return e.SuspendingOperation.GetDeferral();
        }
    }
}

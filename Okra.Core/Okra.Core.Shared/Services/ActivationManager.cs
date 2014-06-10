using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Okra.Helpers;
using Okra.Navigation;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Okra.Services
{
    public class ActivationManager : IActivationManager
    {
        // *** Fields ***

        private HashSet<IActivationHandler> registeredServices = new HashSet<IActivationHandler>();

        // *** Events ***

        public event EventHandler<IActivatedEventArgs> Activating;
        public event EventHandler<IActivatedEventArgs> Activated;

        // *** Methods ***

        public async Task<bool> Activate(IActivatedEventArgs activatedEventArgs)
        {
            // Raise Activating event

            EventHandler<IActivatedEventArgs> activatingEventHandler = Activating;

            if (activatingEventHandler != null)
                activatingEventHandler(this, activatedEventArgs);

            // Call activate on all activation handlers
            // NB: We convert to an array so that the Select is not called multiple times

            IEnumerable<Task<bool>> activationTasks = registeredServices.Select(service => service.Activate(activatedEventArgs)).ToArray();
            await Task.WhenAll(activationTasks);

            // Raise Activated event

            EventHandler<IActivatedEventArgs> activatedEventHandler = Activated;

            if (activatedEventHandler != null)
                activatedEventHandler(this, activatedEventArgs);

            // Determine if the activation was handled by any of the handlers

            Task<bool> firstHandlingTask = activationTasks.FirstOrDefault(task => task.Result == true);
            bool handled = firstHandlingTask != null;

            // If the activation was handled then activate the current window
            
            if (handled && Window.Current != null)
                Window.Current.Activate();

            return handled;
        }

        public void Register(IActivationHandler service)
        {
            // Validate parameters

            if (service == null)
                throw new ArgumentNullException("service");

            if (registeredServices.Contains(service))
                throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotRegisterServiceMultipleTimes"));

            // Add the service to the internal list

            registeredServices.Add(service);
        }

        public void Unregister(IActivationHandler service)
        {
            // Validate parameters

            if (service == null)
                throw new ArgumentNullException("service");

            if (!registeredServices.Contains(service))
                throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotUnregisterUnregisteredService"));

            // Remove the service from the internal list

            registeredServices.Remove(service);
        }
    }
}

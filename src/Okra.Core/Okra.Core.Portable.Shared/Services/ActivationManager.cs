using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Okra.Helpers;

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.ApplicationModel.Activation;
#endif

namespace Okra.Services
{
    public class ActivationManager : IActivationManager
    {
        // *** Fields ***

        private HashSet<IActivationHandler> _registeredServices = new HashSet<IActivationHandler>();

        // *** Events ***

        public event EventHandler<IActivatedEventArgs> Activating;
        public event EventHandler<IActivatedEventArgs> Activated;

        // *** Methods ***

        public Task<bool> Activate(IActivatedEventArgs activatedEventArgs)
        {
            // Validate parameters

            if (activatedEventArgs == null)
                throw new ArgumentNullException("activatedEventArgs");

            // Call internal async implementation

            return ActivateInternal(activatedEventArgs);
        }

        public void Register(IActivationHandler service)
        {
            // Validate parameters

            if (service == null)
                throw new ArgumentNullException("service");

            if (_registeredServices.Contains(service))
                throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotRegisterServiceMultipleTimes"));

            // Add the service to the internal list

            _registeredServices.Add(service);
        }

        public void Unregister(IActivationHandler service)
        {
            // Validate parameters

            if (service == null)
                throw new ArgumentNullException("service");

            if (!_registeredServices.Contains(service))
                throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotUnregisterUnregisteredService"));

            // Remove the service from the internal list

            _registeredServices.Remove(service);
        }

        // *** Private Methods ***

        private async Task<bool> ActivateInternal(IActivatedEventArgs activatedEventArgs)
        {
            // Validate parameters

            if (activatedEventArgs == null)
                throw new ArgumentNullException("activatedEventArgs");

            // Raise Activating event

            EventHandler<IActivatedEventArgs> activatingEventHandler = Activating;

            if (activatingEventHandler != null)
                activatingEventHandler(this, activatedEventArgs);

            // Call activate on all activation handlers
            // NB: We convert to an array so that the Select is not called multiple times

            IEnumerable<Task<bool>> activationTasks = _registeredServices.Select(service => service.Activate(activatedEventArgs)).ToArray();
            await Task.WhenAll(activationTasks);

            // Raise Activated event

            EventHandler<IActivatedEventArgs> activatedEventHandler = Activated;

            if (activatedEventHandler != null)
                activatedEventHandler(this, activatedEventArgs);

            // Determine if the activation was handled by any of the handlers

            Task<bool> firstHandlingTask = activationTasks.FirstOrDefault(task => task.Result == true);
            bool handled = firstHandlingTask != null;

#if NETFX_CORE
            // If the activation was handled then activate the current window

            if (handled && Window.Current != null)
                Window.Current.Activate();
#endif

            return handled;
        }
    }
}

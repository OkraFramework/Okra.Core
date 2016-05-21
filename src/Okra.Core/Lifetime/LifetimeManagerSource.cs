using Okra.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Lifetime
{
    public class LifetimeManagerSource : ILifetimeManagerSource
    {
        private readonly LifetimeManagerInternal _lifetimeManager;

        public LifetimeManagerSource()
        {
            _lifetimeManager = new LifetimeManagerInternal();
        }

        public ILifetimeManager LifetimeManager
        {
            get
            {
                return _lifetimeManager;
            }
        }

        public Task Activate()
        {
            return _lifetimeManager.Activate();
        }

        public Task Deactivate()
        {
            return _lifetimeManager.Deactivate();
        }

        // *** Private sub-classes ***

        private class LifetimeManagerInternal : ILifetimeManager
        {
            private HashSet<ILifetimeAware> _registeredServices = new HashSet<ILifetimeAware>();

            public void Register(ILifetimeAware service)
            {
                if (service == null)
                    throw new ArgumentNullException(nameof(service));

                if (_registeredServices.Contains(service))
                    throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotRegisterServiceMultipleTimes"));

                _registeredServices.Add(service);
            }

            public void Unregister(ILifetimeAware service)
            {
                if (service == null)
                    throw new ArgumentNullException(nameof(service));

                if (!_registeredServices.Contains(service))
                    throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotUnregisterUnregisteredService"));

                _registeredServices.Remove(service);
            }

            public Task Activate()
            {
                var activateTasks = _registeredServices.Select(service => service.Activate());
                return Task.WhenAll(activateTasks);
            }

            public Task Deactivate()
            {
                var deactivateTasks = _registeredServices.Select(service => service.Deactivate());
                return Task.WhenAll(deactivateTasks);
            }
        }
    }
}

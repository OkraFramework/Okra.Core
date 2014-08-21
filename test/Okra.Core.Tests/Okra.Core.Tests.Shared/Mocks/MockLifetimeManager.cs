using Okra.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okra.Tests.Mocks
{
    public class MockLifetimeManager : ILifetimeManager
    {
        // *** Fields ***

        public List<ILifetimeAware> RegisteredServices = new List<ILifetimeAware>();

        // *** Methods ***

        public void Register(ILifetimeAware service)
        {
            RegisteredServices.Add(service);
        }

        public void Unregister(ILifetimeAware service)
        {
            RegisteredServices.Remove(service);
        }

        // *** Mock Methods ***

        public void Suspend()
        {
            foreach (ILifetimeAware service in RegisteredServices)
                service.OnSuspending().Wait();
        }

        public void Resume()
        {
            foreach (ILifetimeAware service in RegisteredServices)
                service.OnResuming().Wait();
        }
    }
}

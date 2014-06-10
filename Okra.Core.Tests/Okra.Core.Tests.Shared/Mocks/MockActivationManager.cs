using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Services;
using Windows.ApplicationModel.Activation;

namespace Okra.Tests.Mocks
{
    public class MockActivationManager : IActivationManager
    {
        // *** Fields ***

        public List<IActivationHandler> RegisteredServices = new List<IActivationHandler>();

        // *** Events ***

        public event EventHandler<IActivatedEventArgs> Activating;
        public event EventHandler<IActivatedEventArgs> Activated;

        // *** Methods ***

        public Task<bool> Activate(IActivatedEventArgs activatedEventArgs)
        {
            throw new NotImplementedException();
        }

        public void Register(IActivationHandler service)
        {
            RegisteredServices.Add(service);
        }

        public void Unregister(IActivationHandler service)
        {
            RegisteredServices.Remove(service);
        }

        // *** Mock Helper Methods ***

        public void RaiseActivatingEvent(IActivatedEventArgs e)
        {
            if (Activating != null)
                Activating(this, e);
        }

        public void RaiseActivatedEvent(IActivatedEventArgs e)
        {
            if (Activated != null)
                Activated(this, e);
        }
    }
}

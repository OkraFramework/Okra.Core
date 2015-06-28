using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Navigation;
using Okra.Services;
using Okra.Tests.Mocks;
using Windows.ApplicationModel.Activation;
using Xunit;

namespace Okra.Tests.Services
{
    public class ActivationManagerFixture
    {
        // *** Method Tests ***

        [Fact]
        public async Task Activate_ReturnsFalseIfAllHandlersReturnFalse()
        {
            MockService service1 = new MockService(Task.FromResult(false));
            MockService service2 = new MockService(Task.FromResult(false));

            ActivationManager activationManager = CreateActivationManager(services: new[] { service1, service2 });

            bool result = await activationManager.Activate(new MockActivatedEventArgs());

            Assert.Equal(false, result);
        }

        [Fact]
        public async Task Activate_ReturnsTrueIfAnyHandlerReturnTrue()
        {
            MockService service1 = new MockService(Task.FromResult(false));
            MockService service2 = new MockService(Task.FromResult(true));

            ActivationManager activationManager = CreateActivationManager(services: new[] { service1, service2 });

            bool result = await activationManager.Activate(new MockActivatedEventArgs());

            Assert.Equal(true, result);
        }

        [Fact]
        public async Task Activate_RegistedServicesReceiveActivateEvents()
        {
            MockService service1 = new MockService();
            MockService service2 = new MockService();

            ActivationManager activationManager = CreateActivationManager(services: new[] { service1, service2 });

            await activationManager.Activate(new MockActivatedEventArgs());

            Assert.Equal(new[] { "Activating", "Activate", "Activated" }, service1.LifetimeEventCalls.Select(e => e.Item1).ToArray());
            Assert.Equal(new[] { "Activating", "Activate", "Activated" }, service2.LifetimeEventCalls.Select(e => e.Item1).ToArray());
        }

        [Fact]
        public async Task Activate_RegistedServicesReceiveActivatedEventArgs()
        {
            MockService service1 = new MockService();
            MockService service2 = new MockService();

            ActivationManager activationManager = CreateActivationManager(services: new[] { service1, service2 });

            MockActivatedEventArgs eventArgs = new MockActivatedEventArgs();
            await activationManager.Activate(eventArgs);

            Assert.Equal<IActivatedEventArgs>(new[] { eventArgs, eventArgs, eventArgs }, service1.LifetimeEventCalls.Select(e => e.Item2).ToArray());
            Assert.Equal<IActivatedEventArgs>(new[] { eventArgs, eventArgs, eventArgs }, service2.LifetimeEventCalls.Select(e => e.Item2).ToArray());
        }

        [Fact]
        public void Activate_DoesNotRaiseActivatedEventUntilAllHandlersComplete()
        {
            MockService service1 = new MockService();
            TaskCompletionSource<bool> service2Completion = new TaskCompletionSource<bool>();
            MockService service2 = new MockService(service2Completion.Task);

            ActivationManager activationManager = CreateActivationManager(services: new[] { service1, service2 });

            // NB: Do not await this as will only complete when all tasks complete!
            Task<bool> result = activationManager.Activate(new MockActivatedEventArgs());

            Assert.Equal(false, result.IsCompleted);
            Assert.Equal(new[] { "Activating", "Activate" }, service1.LifetimeEventCalls.Select(e => e.Item1).ToArray());
            Assert.Equal(new[] { "Activating", "Activate" }, service2.LifetimeEventCalls.Select(e => e.Item1).ToArray());
        }

        [Fact]
        public async void Activate_ThrowsException_IfEventArgsIsNull()
        {
            MockService service1 = new MockService(Task.FromResult(false));
            MockService service2 = new MockService(Task.FromResult(false));

            ActivationManager activationManager = CreateActivationManager(services: new[] { service1, service2 });

            var e = await Assert.ThrowsAsync<ArgumentNullException>(() => activationManager.Activate(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: activatedEventArgs", e.Message);
            Assert.Equal("activatedEventArgs", e.ParamName);
        }

        [Fact]
        public void Register_ThrowsException_IfServiceIsNull()
        {
            ActivationManager activationManager = CreateActivationManager();

            var e = Assert.Throws<ArgumentNullException>(() => activationManager.Register(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: service", e.Message);
            Assert.Equal("service", e.ParamName);
        }

        [Fact]
        public void Register_ThrowsException_WithMultipleRegistrationOfSameService()
        {
            MockService service = new MockService();
            ActivationManager activationManager = CreateActivationManager();

            activationManager.Register(service);

            var e = Assert.Throws<InvalidOperationException>(() => activationManager.Register(service));

            Assert.Equal("Cannot register the service as it is already registered.", e.Message);
        }

        [Fact]
        public void Unregister_ThrowsException_IfServiceIsNull()
        {
            ActivationManager activationManager = CreateActivationManager();

            var e = Assert.Throws<ArgumentNullException>(() => activationManager.Unregister(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: service", e.Message);
            Assert.Equal("service", e.ParamName);
        }

        [Fact]
        public void Unregister_ThrowsException_IfServiceIsNotRegistered()
        {
            MockService service1 = new MockService();
            MockService service2 = new MockService();
            ActivationManager activationManager = CreateActivationManager();

            activationManager.Register(service1);

            var e = Assert.Throws<InvalidOperationException>(() => activationManager.Unregister(service2));

            Assert.Equal("Cannot unregister the service as it is not currently registered.", e.Message);
        }

        // *** Private Methods ***

        private ActivationManager CreateActivationManager(MockService[] services = null)
        {
            ActivationManager activationManager = new ActivationManager();

            if (services != null)
            {
                foreach (MockService service in services)
                {
                    activationManager.Register(service);
                    activationManager.Activated += service.OnActivated;
                    activationManager.Activating += service.OnActivating;
                }
            }

            return activationManager;
        }

        // *** Private sub-classes ***

        private class MockService : IActivationHandler
        {
            // *** Fields ***

            private Task<bool> _activatingCompleteTask;

            // *** Constructors ***

            public MockService(Task<bool> activatingCompleteTask)
            {
                _activatingCompleteTask = activatingCompleteTask;
                this.LifetimeEventCalls = new List<Tuple<string, IActivatedEventArgs>>();
            }

            public MockService()
                : this(Task.FromResult<bool>(true))
            {
            }

            // *** Properties ***

            public IList<Tuple<string, IActivatedEventArgs>> LifetimeEventCalls { get; private set; }

            // *** Methods ***

            public Task<bool> Activate(IActivatedEventArgs activatedEventArgs)
            {
                LifetimeEventCalls.Add(Tuple.Create("Activate", activatedEventArgs));
                return _activatingCompleteTask;
            }

            public void OnActivating(object sender, IActivatedEventArgs activatedEventArgs)
            {
                LifetimeEventCalls.Add(Tuple.Create("Activating", activatedEventArgs));
            }

            public void OnActivated(object sender, IActivatedEventArgs activatedEventArgs)
            {
                LifetimeEventCalls.Add(Tuple.Create("Activated", activatedEventArgs));
            }
        }
    }
}

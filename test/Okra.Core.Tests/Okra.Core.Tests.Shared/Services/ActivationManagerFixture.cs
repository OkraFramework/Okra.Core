using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Navigation;
using Okra.Services;
using Okra.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.ApplicationModel.Activation;

namespace Okra.Tests.Services
{
    [TestClass]
    public class ActivationManagerFixture
    {
        // *** Method Tests ***

        [TestMethod]
        public async Task Activate_ReturnsFalseIfAllHandlersReturnFalse()
        {
            MockService service1 = new MockService(Task.FromResult(false));
            MockService service2 = new MockService(Task.FromResult(false));

            ActivationManager activationManager = CreateActivationManager(services: new[] { service1, service2 });

            bool result = await activationManager.Activate(new MockActivatedEventArgs());

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public async Task Activate_ReturnsTrueIfAnyHandlerReturnTrue()
        {
            MockService service1 = new MockService(Task.FromResult(false));
            MockService service2 = new MockService(Task.FromResult(true));

            ActivationManager activationManager = CreateActivationManager(services: new[] { service1, service2 });

            bool result = await activationManager.Activate(new MockActivatedEventArgs());

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task Activate_RegistedServicesReceiveActivateEvents()
        {
            MockService service1 = new MockService();
            MockService service2 = new MockService();

            ActivationManager activationManager = CreateActivationManager(services: new[] { service1, service2 });

            await activationManager.Activate(new MockActivatedEventArgs());

            CollectionAssert.AreEqual(new[] { "Activating", "Activate", "Activated" }, service1.LifetimeEventCalls.Select(e => e.Item1).ToArray());
            CollectionAssert.AreEqual(new[] { "Activating", "Activate", "Activated" }, service2.LifetimeEventCalls.Select(e => e.Item1).ToArray());
        }

        [TestMethod]
        public async Task Activate_RegistedServicesReceiveActivatedEventArgs()
        {
            MockService service1 = new MockService();
            MockService service2 = new MockService();

            ActivationManager activationManager = CreateActivationManager(services: new[] { service1, service2 });

            MockActivatedEventArgs eventArgs = new MockActivatedEventArgs();
            await activationManager.Activate(eventArgs);

            CollectionAssert.AreEqual(new[] { eventArgs, eventArgs, eventArgs }, service1.LifetimeEventCalls.Select(e => e.Item2).ToArray());
            CollectionAssert.AreEqual(new[] { eventArgs, eventArgs, eventArgs }, service2.LifetimeEventCalls.Select(e => e.Item2).ToArray());
        }

        [TestMethod]
        public void Activate_DoesNotRaiseActivatedEventUntilAllHandlersComplete()
        {
            MockService service1 = new MockService();
            TaskCompletionSource<bool> service2Completion = new TaskCompletionSource<bool>();
            MockService service2 = new MockService(service2Completion.Task);

            ActivationManager activationManager = CreateActivationManager(services: new[] { service1, service2 });

            // NB: Do not await this as will only complete when all tasks complete!
            Task<bool> result = activationManager.Activate(new MockActivatedEventArgs());

            Assert.AreEqual(false, result.IsCompleted);
            CollectionAssert.AreEqual(new[] { "Activating", "Activate" }, service1.LifetimeEventCalls.Select(e => e.Item1).ToArray());
            CollectionAssert.AreEqual(new[] { "Activating", "Activate" }, service2.LifetimeEventCalls.Select(e => e.Item1).ToArray());
        }

        [TestMethod]
        public void Activate_ThrowsException_IfEventArgsIsNull()
        {
            MockService service1 = new MockService(Task.FromResult(false));
            MockService service2 = new MockService(Task.FromResult(false));

            ActivationManager activationManager = CreateActivationManager(services: new[] { service1, service2 });

            Assert.ThrowsException<ArgumentNullException>(() => activationManager.Activate(null));
        }

        [TestMethod]
        public void Register_ThrowsException_IfServiceIsNull()
        {
            ActivationManager activationManager = CreateActivationManager();

            Assert.ThrowsException<ArgumentNullException>(() => activationManager.Register(null));
        }

        [TestMethod]
        public void Register_ThrowsException_WithMultipleRegistrationOfSameService()
        {
            MockService service = new MockService();
            ActivationManager activationManager = CreateActivationManager();

            activationManager.Register(service);

            Assert.ThrowsException<InvalidOperationException>(() => activationManager.Register(service));
        }

        [TestMethod]
        public void Unregister_ThrowsException_IfServiceIsNull()
        {
            ActivationManager activationManager = CreateActivationManager();

            Assert.ThrowsException<ArgumentNullException>(() => activationManager.Unregister(null));
        }

        [TestMethod]
        public void Unregister_ThrowsException_IfServiceIsNotRegistered()
        {
            MockService service1 = new MockService();
            MockService service2 = new MockService();
            ActivationManager activationManager = CreateActivationManager();

            activationManager.Register(service1);

            Assert.ThrowsException<InvalidOperationException>(() => activationManager.Unregister(service2));
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

            private Task<bool> activatingCompleteTask;

            // *** Constructors ***

            public MockService(Task<bool> activatingCompleteTask)
            {
                this.activatingCompleteTask = activatingCompleteTask;
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
                return activatingCompleteTask;
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

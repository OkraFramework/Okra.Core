using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Okra.Services;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using System.Collections;
using Xunit;

namespace Okra.Tests.Services
{
    public class LifetimeManagerFixture
    {
        // *** Method Tests ***

        [Fact]
        public void Register_ThrowsException_IfServiceIsNull()
        {
            LifetimeManager lifetimeManager = new LifetimeManager();

            var e = Assert.Throws<ArgumentNullException>(() => lifetimeManager.Register(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: service", e.Message);
            Assert.Equal("service", e.ParamName);
        }

        [Fact]
        public void Register_ThrowsException_WithMultipleRegistrationOfSameService()
        {
            MockService service = new MockService();
            LifetimeManager lifetimeManager = new LifetimeManager();

            lifetimeManager.Register(service);

            var e = Assert.Throws<InvalidOperationException>(() => lifetimeManager.Register(service));

            Assert.Equal("Cannot register the service as it is already registered.", e.Message);
        }

        [Fact]
        public void Unregister_ThrowsException_IfServiceIsNull()
        {
            LifetimeManager lifetimeManager = new LifetimeManager();

            var e = Assert.Throws<ArgumentNullException>(() => lifetimeManager.Unregister(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: service", e.Message);
            Assert.Equal("service", e.ParamName);
        }

        [Fact]
        public void Unregister_ThrowsException_IfServiceIsNotRegistered()
        {
            MockService service1 = new MockService();
            MockService service2 = new MockService();
            LifetimeManager lifetimeManager = new LifetimeManager();

            lifetimeManager.Register(service1);

            var e = Assert.Throws<InvalidOperationException>(() => lifetimeManager.Unregister(service2));

            Assert.Equal("Cannot unregister the service as it is not currently registered.", e.Message);
        }

        // *** Behavior Tests ***

        [Fact]
        public void RegistedServices_ReceiveSuspendingEvent()
        {
            MockService service1 = new MockService();
            MockService service2 = new MockService();

            TestableLifetimeManager lifetimeManager = CreateLifetimeManager(new[] { service1, service2 });

            lifetimeManager.Suspend(new MockSuspendingEventArgs());

            Assert.Equal<string>(new string[] { "OnSuspending" }, service1.LifetimeEventCalls);
            Assert.Equal<string>(new string[] { "OnSuspending" }, service2.LifetimeEventCalls);
        }

        [Fact]
        public void RegistedServices_ReceiveResumingEvent()
        {
            MockService service1 = new MockService();
            MockService service2 = new MockService();

            TestableLifetimeManager lifetimeManager = CreateLifetimeManager(new[] { service1, service2 });

            lifetimeManager.Resume();

            Assert.Equal<string>(new string[] { "OnResuming" }, service1.LifetimeEventCalls);
            Assert.Equal<string>(new string[] { "OnResuming" }, service2.LifetimeEventCalls);
        }

        [Fact]
        public void RegistedServices_ReceiveMultipleEvents()
        {
            MockService service1 = new MockService();
            MockService service2 = new MockService();

            TestableLifetimeManager lifetimeManager = CreateLifetimeManager(new[] { service1, service2 });

            lifetimeManager.Suspend(new MockSuspendingEventArgs());
            lifetimeManager.Resume();

            Assert.Equal<string>(new string[] { "OnSuspending", "OnResuming" }, service1.LifetimeEventCalls);
            Assert.Equal<string>(new string[] { "OnSuspending", "OnResuming" }, service2.LifetimeEventCalls);
        }

        [Fact]
        public async Task RegistedServices_CausesDeferralOfSuspension()
        {
            // Create two services which will complete suspension when their tasks complete

            TaskCompletionSource<bool> tcs1 = new TaskCompletionSource<bool>();
            TaskCompletionSource<bool> tcs2 = new TaskCompletionSource<bool>();

            MockService service1 = new MockService(tcs1.Task);
            MockService service2 = new MockService(tcs2.Task);

            // Create the LifetimeManager

            TestableLifetimeManager lifetimeManager = CreateLifetimeManager(new[] { service1, service2 });

            // Suspend the LifetimeManager

            MockSuspendingEventArgs suspendingEventArgs = new MockSuspendingEventArgs();
            lifetimeManager.Suspend(suspendingEventArgs);

            // Check that the suspension is deferred

            Assert.True(suspendingEventArgs.IsDeferred);

            // Check that the suspension is deferred on completion of first service

            tcs1.SetResult(true);
            await Task.Yield();
            Assert.True(suspendingEventArgs.IsDeferred);

            // Check that the suspension is completed on completion of second service

            tcs2.SetResult(true);
            await Task.Yield();
            Assert.False(suspendingEventArgs.IsDeferred);
        }

        // *** Private Methods ***

        private TestableLifetimeManager CreateLifetimeManager(ILifetimeAware[] services)
        {
            TestableLifetimeManager lifetimeManager = new TestableLifetimeManager();

            foreach (ILifetimeAware service in services)
                lifetimeManager.Register(service);

            return lifetimeManager;
        }

        // *** Private Sub-classes ***

        private class TestableLifetimeManager : LifetimeManager
        {
            // *** Methods ***

            public void Suspend(ISuspendingEventArgs e)
            {
                base.OnSuspending(null, e);
            }

            public void Resume()
            {
                base.OnResuming(null, null);
            }

            // *** Overriden base methods ***

            protected override ISuspendingDeferral GetDeferral(ISuspendingEventArgs e)
            {
                return ((MockSuspendingEventArgs)e).GetDeferral();
            }
        }

        private class MockService : ILifetimeAware
        {
            // *** Fields ***

            private Task _suspensionCompleteTask;

            // *** Constructors ***

            public MockService(Task suspensionCompleteTask)
            {
                _suspensionCompleteTask = suspensionCompleteTask;
                this.LifetimeEventCalls = new List<string>();
            }

            public MockService()
                : this(Task.FromResult<bool>(true))
            {
            }

            // *** Properties ***

            public IList<string> LifetimeEventCalls { get; private set; }

            // *** Methods ***

            public Task OnResuming()
            {
                LifetimeEventCalls.Add("OnResuming");
                return Task.FromResult(true);
            }

            public Task OnSuspending()
            {
                LifetimeEventCalls.Add("OnSuspending");
                return _suspensionCompleteTask;
            }
        }

        public class MockSuspendingEventArgs : ISuspendingEventArgs
        {
            // *** Fields ***

            public IList<MockSuspendingDeferral> Deferrals = new List<MockSuspendingDeferral>();

            // *** Mock Properties ***

            public int DeferralCount
            {
                get
                {
                    return Deferrals.Where(d => !d.IsComplete).Count();
                }
            }

            public bool IsDeferred
            {
                get
                {
                    return DeferralCount > 0;
                }
            }

            // *** ISuspendingEventArgs Properties ***

            public SuspendingOperation SuspendingOperation
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            // *** Testing Helper Methods ***

            public ISuspendingDeferral GetDeferral()
            {
                MockSuspendingDeferral deferral = new MockSuspendingDeferral();
                Deferrals.Add(deferral);
                return deferral;
            }
        }

        public class MockSuspendingDeferral : ISuspendingDeferral
        {
            // *** Fields ***

            public bool IsComplete;

            // *** Methods ***

            public void Complete()
            {
                IsComplete = true;
            }
        }
    }
}
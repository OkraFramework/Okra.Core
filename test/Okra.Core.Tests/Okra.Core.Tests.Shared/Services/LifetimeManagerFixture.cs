using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Okra.Services;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using System.Collections;

namespace Okra.Tests.Services
{
    [TestClass]
    public class LifetimeManagerFixture
    {
        // *** Method Tests ***

        [TestMethod]
        public void Register_ThrowsException_IfServiceIsNull()
        {
            LifetimeManager lifetimeManager = new LifetimeManager();

            Assert.ThrowsException<ArgumentNullException>(() => lifetimeManager.Register(null));
        }

        [TestMethod]
        public void Register_ThrowsException_WithMultipleRegistrationOfSameService()
        {
            MockService service = new MockService();
            LifetimeManager lifetimeManager = new LifetimeManager();

            lifetimeManager.Register(service);

            Assert.ThrowsException<InvalidOperationException>(() => lifetimeManager.Register(service));
        }

        [TestMethod]
        public void Unregister_ThrowsException_IfServiceIsNull()
        {
            LifetimeManager lifetimeManager = new LifetimeManager();

            Assert.ThrowsException<ArgumentNullException>(() => lifetimeManager.Unregister(null));
        }

        [TestMethod]
        public void Unregister_ThrowsException_IfServiceIsNotRegistered()
        {
            MockService service1 = new MockService();
            MockService service2 = new MockService();
            LifetimeManager lifetimeManager = new LifetimeManager();

            lifetimeManager.Register(service1);

            Assert.ThrowsException<InvalidOperationException>(() => lifetimeManager.Unregister(service2));
        }

        // *** Behavior Tests ***

        [TestMethod]
        public void RegistedServices_ReceiveSuspendingEvent()
        {
            MockService service1 = new MockService();
            MockService service2 = new MockService();

            TestableLifetimeManager lifetimeManager = CreateLifetimeManager(new[] { service1, service2 });

            lifetimeManager.Suspend(new MockSuspendingEventArgs());

            CollectionAssert.AreEqual(new string[] { "OnSuspending" }, service1.LifetimeEventCalls);
            CollectionAssert.AreEqual(new string[] { "OnSuspending" }, service2.LifetimeEventCalls);
        }

        [TestMethod]
        public void RegistedServices_ReceiveResumingEvent()
        {
            MockService service1 = new MockService();
            MockService service2 = new MockService();

            TestableLifetimeManager lifetimeManager = CreateLifetimeManager(new[] { service1, service2 });

            lifetimeManager.Resume();

            CollectionAssert.AreEqual(new string[] { "OnResuming" }, service1.LifetimeEventCalls);
            CollectionAssert.AreEqual(new string[] { "OnResuming" }, service2.LifetimeEventCalls);
        }

        [TestMethod]
        public void RegistedServices_ReceiveMultipleEvents()
        {
            MockService service1 = new MockService();
            MockService service2 = new MockService();

            TestableLifetimeManager lifetimeManager = CreateLifetimeManager(new[] { service1, service2 });

            lifetimeManager.Suspend(new MockSuspendingEventArgs());
            lifetimeManager.Resume();

            CollectionAssert.AreEqual(new string[] { "OnSuspending", "OnResuming" }, service1.LifetimeEventCalls);
            CollectionAssert.AreEqual(new string[] { "OnSuspending", "OnResuming" }, service2.LifetimeEventCalls);
        }

        [TestMethod]
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

            Assert.IsTrue(suspendingEventArgs.IsDeferred);

            // Check that the suspension is deferred on completion of first service

            tcs1.SetResult(true);
            await Task.Yield();
            Assert.IsTrue(suspendingEventArgs.IsDeferred);

            // Check that the suspension is completed on completion of second service

            tcs2.SetResult(true);
            await Task.Yield();
            Assert.IsFalse(suspendingEventArgs.IsDeferred);
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

            private Task suspensionCompleteTask;

            // *** Constructors ***

            public MockService(Task suspensionCompleteTask)
            {
                this.suspensionCompleteTask = suspensionCompleteTask;
                this.LifetimeEventCalls = new List<string>();
            }

            public MockService()
                : this(Task.FromResult<bool>(true))
            {
            }

            // *** Properties ***

            public IList LifetimeEventCalls { get; private set; }

            // *** Methods ***

            public Task OnResuming()
            {
                LifetimeEventCalls.Add("OnResuming");
                return Task.FromResult(true);
            }

            public Task OnSuspending()
            {
                LifetimeEventCalls.Add("OnSuspending");
                return suspensionCompleteTask;
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
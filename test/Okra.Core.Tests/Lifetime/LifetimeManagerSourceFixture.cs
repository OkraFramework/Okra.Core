using Okra.Lifetime;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Okra.Tests.Lifetime
{
    public class LifetimeManagerSourceFixture
    {
        [Fact]
        public void ReturnsLifetimeManager()
        {
            var lifetimeManagerSource = new LifetimeManagerSource();
            var lifetimeManager = lifetimeManagerSource.LifetimeManager;

            Assert.NotNull(lifetimeManager);
        }

        [Fact]
        public void ReturnsSameLifetimeManagerWithMultipleCalls()
        {
            var lifetimeManagerSource = new LifetimeManagerSource();

            var lifetimeManager1 = lifetimeManagerSource.LifetimeManager;
            var lifetimeManager2 = lifetimeManagerSource.LifetimeManager;

            Assert.Equal(lifetimeManager1, lifetimeManager2);
        }

        [Fact]
        public async void Activate_WithoutRegistration_Returns()
        {
            var lifetimeManagerSource = new LifetimeManagerSource();
            var lifetimeManager = lifetimeManagerSource.LifetimeManager;

            await lifetimeManagerSource.Activate();
        }

        [Fact]
        public async void Deactivate_WithoutRegistration_Returns()
        {
            var lifetimeManagerSource = new LifetimeManagerSource();
            var lifetimeManager = lifetimeManagerSource.LifetimeManager;

            await lifetimeManagerSource.Deactivate();
        }

        [Fact]
        public async void Activate_WithSingleService_ActivatesService()
        {
            var lifetimeManagerSource = new LifetimeManagerSource();
            var lifetimeManager = lifetimeManagerSource.LifetimeManager;

            var service1 = new MockLifetimeAwareService();
            lifetimeManager.Register(service1);

            await lifetimeManagerSource.Activate();

            Assert.Equal(1, service1.ActivateCount);
            Assert.Equal(0, service1.DeactivateCount);
        }

        [Fact]
        public async void Deactivate_WithSingleService_DeactivatesService()
        {
            var lifetimeManagerSource = new LifetimeManagerSource();
            var lifetimeManager = lifetimeManagerSource.LifetimeManager;

            var service1 = new MockLifetimeAwareService();
            lifetimeManager.Register(service1);

            await lifetimeManagerSource.Deactivate();

            Assert.Equal(0, service1.ActivateCount);
            Assert.Equal(1, service1.DeactivateCount);
        }

        [Fact]
        public async void Activate_WithMultipleServices_ActivatesServices()
        {
            var lifetimeManagerSource = new LifetimeManagerSource();
            var lifetimeManager = lifetimeManagerSource.LifetimeManager;

            var service1 = new MockLifetimeAwareService();
            var service2 = new MockLifetimeAwareService();
            lifetimeManager.Register(service1);
            lifetimeManager.Register(service2);

            await lifetimeManagerSource.Activate();

            Assert.Equal(1, service1.ActivateCount);
            Assert.Equal(0, service1.DeactivateCount);
            Assert.Equal(1, service2.ActivateCount);
            Assert.Equal(0, service2.DeactivateCount);
        }

        [Fact]
        public async void Deactivate_WithMultipleServices_DeactivatesServices()
        {
            var lifetimeManagerSource = new LifetimeManagerSource();
            var lifetimeManager = lifetimeManagerSource.LifetimeManager;

            var service1 = new MockLifetimeAwareService();
            var service2 = new MockLifetimeAwareService();
            lifetimeManager.Register(service1);
            lifetimeManager.Register(service2);

            await lifetimeManagerSource.Deactivate();

            Assert.Equal(0, service1.ActivateCount);
            Assert.Equal(1, service1.DeactivateCount);
            Assert.Equal(0, service2.ActivateCount);
            Assert.Equal(1, service2.DeactivateCount);
        }

        [Fact]
        public async void Activate_UnregisteredService_DoesNotActivate()
        {
            var lifetimeManagerSource = new LifetimeManagerSource();
            var lifetimeManager = lifetimeManagerSource.LifetimeManager;

            var service1 = new MockLifetimeAwareService();
            lifetimeManager.Register(service1);
            lifetimeManager.Unregister(service1);

            await lifetimeManagerSource.Activate();

            Assert.Equal(0, service1.ActivateCount);
            Assert.Equal(0, service1.DeactivateCount);
        }

        [Fact]
        public async void Dectivate_UnregisteredService_DoesNotDectivate()
        {
            var lifetimeManagerSource = new LifetimeManagerSource();
            var lifetimeManager = lifetimeManagerSource.LifetimeManager;

            var service1 = new MockLifetimeAwareService();
            lifetimeManager.Register(service1);
            lifetimeManager.Unregister(service1);

            await lifetimeManagerSource.Deactivate();

            Assert.Equal(0, service1.ActivateCount);
            Assert.Equal(0, service1.DeactivateCount);
        }

        [Fact]
        public void RegisterService_ThrowsException_IfServiceIsNull()
        {
            var lifetimeManagerSource = new LifetimeManagerSource();
            var lifetimeManager = lifetimeManagerSource.LifetimeManager;

            var e = Assert.Throws<ArgumentNullException>(() => lifetimeManager.Register(null));
            Assert.Equal("service", e.ParamName);
        }

        [Fact]
        public void RegisterService_ThrowsException_IfServiceIsRegisteredMultipleTimes()
        {
            var lifetimeManagerSource = new LifetimeManagerSource();
            var lifetimeManager = lifetimeManagerSource.LifetimeManager;

            var service = new MockLifetimeAwareService();

            lifetimeManager.Register(service);
            var e = Assert.Throws<InvalidOperationException>(() => lifetimeManager.Register(service));
            Assert.Equal("You cannot register a service multiple times.", e.Message);
        }

        [Fact]
        public void UnregisterService_ThrowsException_IfServiceIsNull()
        {
            var lifetimeManagerSource = new LifetimeManagerSource();
            var lifetimeManager = lifetimeManagerSource.LifetimeManager;

            var e = Assert.Throws<ArgumentNullException>(() => lifetimeManager.Unregister(null));
            Assert.Equal("service", e.ParamName);
        }

        [Fact]
        public void UnregisterService_ThrowsException_IfServiceIsNotRegistered()
        {
            var lifetimeManagerSource = new LifetimeManagerSource();
            var lifetimeManager = lifetimeManagerSource.LifetimeManager;

            var service1 = new MockLifetimeAwareService();
            var service2 = new MockLifetimeAwareService();

            lifetimeManager.Register(service1);
            var e = Assert.Throws<InvalidOperationException>(() => lifetimeManager.Unregister(service2));
            Assert.Equal("Cannot unregister the service as it is not currently registered.", e.Message);
        }

        // *** Mock Classes ***

        private class MockLifetimeAwareService : ILifetimeAware
        {
            public int ActivateCount = 0;
            public int DeactivateCount = 0;

            public async Task Activate()
            {
                await Task.Yield();
                ActivateCount++;
            }

            public async Task Deactivate()
            {
                await Task.Yield();
                DeactivateCount++;
            }
        }
    }
}

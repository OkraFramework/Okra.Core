using Okra.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Okra.Tests.DependencyInjection
{
    public class ServiceInjectorFixture
    {
        [Fact]
        public void HasValue_IsFalse_WhenServiceNotInjected()
        {
            var serviceInjector = new ServiceInjector<IMockService>();

            Assert.False(serviceInjector.HasValue);
        }

        [Fact]
        public void HasValue_IsTrue_WhenServiceIsInjected()
        {
            var serviceInjector = new ServiceInjector<IMockService>();
            var service = new MockService();

            serviceInjector.Service = service;

            Assert.True(serviceInjector.HasValue);
        }

        [Fact]
        public void GettingService_ReturnsSpecifiedService()
        {
            var serviceInjector = new ServiceInjector<IMockService>();
            var service = new MockService();

            serviceInjector.Service = service;

            Assert.Equal(service, serviceInjector.Service);
        }

        [Fact]
        public void GettingUninitialisedService_ThrowsException()
        {
            var serviceInjector = new ServiceInjector<IMockService>();

            var e = Assert.Throws<InvalidOperationException>(() => serviceInjector.Service);
            Assert.Equal("The service 'IMockService' has not been injected into the container.", e.Message);
        }

        [Fact]
        public void SettingServiceMultipleTimes_ThrowsException()
        {
            var serviceInjector = new ServiceInjector<IMockService>();
            var service1 = new MockService();
            var service2 = new MockService();

            serviceInjector.Service = service1;

            var e = Assert.Throws<InvalidOperationException>(() => serviceInjector.Service = service2);
            Assert.Equal("The 'IMockService' service has already been injected into the container.", e.Message);
        }

        public interface IMockService
        {
        }

        public class MockService : IMockService
        {
        }
    }
}

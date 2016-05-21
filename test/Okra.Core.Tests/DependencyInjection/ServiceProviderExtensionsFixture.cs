using Okra.DependencyInjection;
using Okra.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Okra.Tests.DependencyInjection
{
    public class ServiceProviderExtensionsFixture
    {
        [Fact]
        public void InjectService_SetsValueInServiceInjector()
        {
            var service = new MockService();
            var serviceInjector = new MockServiceInjector<IMockService>();
            var serviceProvider = new MockServiceProvider()
                                    .With<IServiceInjector<IMockService>>(serviceInjector);
            
            serviceProvider.InjectService<IMockService>(service);

            Assert.Equal(service, serviceInjector.Service);
        }

        [Fact]
        public void GedInjectedService_GetsValueFromServiceInjector()
        {
            var service = new MockService();
            var serviceInjector = new MockServiceInjector<IMockService>() { Service = service };
            var serviceProvider = new MockServiceProvider()
                                    .With<IServiceInjector<IMockService>>(serviceInjector);

            var result = serviceProvider.GetInjectedService<IMockService>();

            Assert.Equal(service, result);
        }

        public interface IMockService
        {
        }

        public class MockService : IMockService
        {
        }
    }
}

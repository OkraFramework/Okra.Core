using Microsoft.Extensions.DependencyInjection;
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
    public class ServiceCollectionExtensionsFixture
    {
        [Fact]
        public void AddInjected_ReturnsServiceCollection()
        {
            var serviceCollection = new MockServiceCollection();

            var result = serviceCollection.AddInjected<IMockService>();

            Assert.Equal(serviceCollection, result);
        }

        [Fact]
        public void AddInjected_AddsScopedServiceInjector()
        {
            var serviceCollection = new MockServiceCollection();

            serviceCollection.AddInjected<IMockService>();

            Assert.Contains(serviceCollection, i => i.ServiceType == typeof(IServiceInjector<IMockService>)
                                                 && i.Lifetime == ServiceLifetime.Scoped
                                                 && i.ImplementationType == typeof(ServiceInjector<IMockService>));
        }

        [Fact]
        public void AddInjected_AddsScopedServiceFactoryMethod()
        {
            var serviceCollection = new MockServiceCollection();

            serviceCollection.AddInjected<IMockService>();

            Assert.Contains(serviceCollection, i => i.ServiceType == typeof(IMockService)
                                                 && i.Lifetime == ServiceLifetime.Scoped
                                                 && i.ImplementationFactory != null);
        }

        public interface IMockService
        {
        }
    }
}

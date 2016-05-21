using Okra.DependencyInjection;
using Okra.Lifetime;
using Okra.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Okra.Tests.DependencyInjection
{
    public class AppContainerFactoryFixture
    {
        [Fact]
        public void CreateAppContainer_ReturnsANewAppContainer()
        {
            var serviceProvider = GetDefaultServiceProvider();
            var appContainerFactory = new AppContainerFactory(serviceProvider);

            var appContainer = appContainerFactory.CreateAppContainer();

            Assert.NotNull(appContainer);
        }

        [Fact]
        public void CreateAppContainer_SetsServiceProviderToHostingProvider()
        {
            var serviceProvider = GetDefaultServiceProvider();
            var appContainerFactory = new AppContainerFactory(serviceProvider);

            var appContainer = appContainerFactory.CreateAppContainer();

            Assert.Equal(serviceProvider, appContainer.Services);
        }

        private MockServiceProvider GetDefaultServiceProvider()
        {
            return new MockServiceProvider()
                        .With<IServiceInjector<IAppContainer>>(new MockServiceInjector<IAppContainer>())
                        .With<IServiceInjector<ILifetimeManager>>(new MockServiceInjector<ILifetimeManager>());
        }
    }
}

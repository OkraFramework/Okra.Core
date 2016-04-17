using Microsoft.Extensions.DependencyInjection;
using Okra.DependencyInjection;
using Okra.State;
using Okra.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Okra.Tests.DependencyInjection
{
    public class AppContainerFixture
    {
        [Fact]
        public void NewAppContainer_HasSpecifiedServiceProvider()
        {
            var serviceProvider = GetDefaultServiceProvider();

            var appContainer = new AppContainer(serviceProvider);

            Assert.Equal(serviceProvider, appContainer.Services);
        }

        [Fact]
        public void NewAppContainer_HasStateService()
        {
            var serviceProvider = GetDefaultServiceProvider();

            var appContainer = new AppContainer(serviceProvider);

            Assert.NotNull(appContainer.State);
        }

        [Fact]
        public void NewAppContainer_InjectsItselfIntoServiceProvider()
        {
            var appContainerInjector = new MockServiceInjector<IAppContainer>();
            var serviceProvider = GetDefaultServiceProvider()
                                    .With<IServiceInjector<IAppContainer>>(appContainerInjector);

            var appContainer = new AppContainer(serviceProvider);

            Assert.Equal(appContainer, appContainerInjector.Service);
        }

        [Fact]
        public void NewAppContainer_InjectsStateServiceIntoServiceProvider()
        {
            var stateServiceInjector = new MockServiceInjector<IStateService>();
            var serviceProvider = GetDefaultServiceProvider()
                                    .With<IServiceInjector<IStateService>>(stateServiceInjector);

            var appContainer = new AppContainer(serviceProvider);

            Assert.Equal(appContainer.State, stateServiceInjector.Service);
        }

        [Fact]
        public void ChildContainer_CanBeCreated()
        {
            var serviceProvider = GetDefaultServiceProvider();

            var parentAppContainer = new AppContainer(serviceProvider);
            var childAppContainer = parentAppContainer.CreateChildContainer();

            Assert.NotNull(childAppContainer);
            Assert.NotEqual(parentAppContainer, childAppContainer);
        }

        [Fact]
        public void ChildContainer_HasChildServiceProvider()
        {
            var childServiceProvider = GetDefaultServiceProviderWithoutChild();

            var parentServiceProvider = GetDefaultServiceProviderWithoutChild()
                                            .With<IServiceScopeFactory>(new MockServiceScopeFactory(childServiceProvider));

            var parentAppContainer = new AppContainer(parentServiceProvider);
            var childAppContainer = parentAppContainer.CreateChildContainer();

            Assert.Equal(childServiceProvider, childAppContainer.Services);
        }

        [Fact]
        public void ChildContainer_HasNewStateService()
        {
            var serviceProvider = GetDefaultServiceProvider();

            var parentAppContainer = new AppContainer(serviceProvider);
            var childAppContainer = parentAppContainer.CreateChildContainer();

            Assert.NotNull(childAppContainer.State);
            Assert.NotEqual(parentAppContainer.State, childAppContainer.State);
        }

        private MockServiceProvider GetDefaultServiceProvider()
        {
            var childServiceProvider = GetDefaultServiceProviderWithoutChild();

            var parentServiceProvider = GetDefaultServiceProviderWithoutChild()
                                            .With<IServiceScopeFactory>(new MockServiceScopeFactory(childServiceProvider));

            return parentServiceProvider;
        }

        private MockServiceProvider GetDefaultServiceProviderWithoutChild()
        {
            return new MockServiceProvider()
                        .With<IServiceInjector<IAppContainer>>(new MockServiceInjector<IAppContainer>())
                        .With<IServiceInjector<IStateService>>(new MockServiceInjector<IStateService>());
        }
    }
}

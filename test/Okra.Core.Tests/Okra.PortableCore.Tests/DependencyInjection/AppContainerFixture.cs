using Microsoft.Extensions.DependencyInjection;
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
        public void NewAppContainer_HasLifetimeManager()
        {
            var serviceProvider = GetDefaultServiceProvider();

            var appContainer = new AppContainer(serviceProvider);

            Assert.NotNull(appContainer.LifetimeManager);
        }

        [Fact]
        public void NewAppContainer_HasNullParent()
        {
            var serviceProvider = GetDefaultServiceProvider();

            var appContainer = new AppContainer(serviceProvider);

            Assert.Null(appContainer.Parent);
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
        public void NewAppContainer_InjectsLifetimeManagerIntoServiceProvider()
        {
            var lifetimeManagerInjector = new MockServiceInjector<ILifetimeManager>();
            var serviceProvider = GetDefaultServiceProvider()
                                    .With<IServiceInjector<ILifetimeManager>>(lifetimeManagerInjector);

            var appContainer = new AppContainer(serviceProvider);

            Assert.Equal(appContainer.LifetimeManager, lifetimeManagerInjector.Service);
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
        public void ChildContainer_HasNewLifetimeManager()
        {
            var serviceProvider = GetDefaultServiceProvider();

            var parentAppContainer = new AppContainer(serviceProvider);
            var childAppContainer = parentAppContainer.CreateChildContainer();

            Assert.NotNull(childAppContainer.LifetimeManager);
            Assert.NotEqual(parentAppContainer.LifetimeManager, childAppContainer.LifetimeManager);
        }

        [Fact]
        public void NewAppContainer_HasParent()
        {
            var serviceProvider = GetDefaultServiceProvider();

            var parentAppContainer = new AppContainer(serviceProvider);
            var childAppContainer = parentAppContainer.CreateChildContainer();

            Assert.Equal(parentAppContainer, childAppContainer.Parent);
        }

        [Fact]
        public async void ActivatingAppContainer_ActivatesServices()
        {
            var serviceProvider = GetDefaultServiceProvider();
            var appContainer = new AppContainer(serviceProvider);
            var serviceCallList = new List<string>();
            var service = new MockLifetimeAwareService("Service", serviceCallList);

            appContainer.LifetimeManager.Register(service);
            await appContainer.Activate();

            Assert.Equal(new string[] { "Service - Activate" }, serviceCallList);
        }

        [Fact]
        public async void DeactivatingAppContainer_DeactivatesServices()
        {
            var serviceProvider = GetDefaultServiceProvider();
            var appContainer = new AppContainer(serviceProvider);
            var serviceCallList = new List<string>();
            var service = new MockLifetimeAwareService("Service", serviceCallList);

            appContainer.LifetimeManager.Register(service);
            await appContainer.Deactivate();

            Assert.Equal(new string[] { "Service - Deactivate" }, serviceCallList);
        }

        [Fact]
        public async void ActivatingAppContainer_ActivatesChildContainerServices()
        {
            var serviceProvider = GetDefaultServiceProvider();
            var parentAppContainer = new AppContainer(serviceProvider);
            var childAppContainer = parentAppContainer.CreateChildContainer();
            var serviceCallList = new List<string>();
            var service = new MockLifetimeAwareService("Child Service", serviceCallList);

            childAppContainer.LifetimeManager.Register(service);
            await parentAppContainer.Activate();

            Assert.Equal(new string[] { "Child Service - Activate" }, serviceCallList);
        }

        [Fact]
        public async void DeactivatingAppContainer_DeactivatesChildContainerServices()
        {
            var serviceProvider = GetDefaultServiceProvider();
            var parentAppContainer = new AppContainer(serviceProvider);
            var childAppContainer = parentAppContainer.CreateChildContainer();
            var serviceCallList = new List<string>();
            var service = new MockLifetimeAwareService("Child Service", serviceCallList);

            childAppContainer.LifetimeManager.Register(service);
            await parentAppContainer.Deactivate();

            Assert.Equal(new string[] { "Child Service - Deactivate" }, serviceCallList);
        }

        [Fact]
        public async void ActivatingAppContainer_ActivatesParentBeforeChild()
        {
            var serviceProvider = GetDefaultServiceProvider();
            var parentAppContainer = new AppContainer(serviceProvider);
            var childAppContainer = parentAppContainer.CreateChildContainer();
            var serviceCallList = new List<string>();
            var parentService = new MockLifetimeAwareService("Parent Service", serviceCallList);
            var childService = new MockLifetimeAwareService("Child Service", serviceCallList);

            parentAppContainer.LifetimeManager.Register(parentService);
            childAppContainer.LifetimeManager.Register(childService);
            await parentAppContainer.Activate();

            Assert.Equal(new string[] { "Parent Service - Activate", "Child Service - Activate" }, serviceCallList);
        }

        [Fact]
        public async void DeactivatingAppContainer_DeactivatesChildBeforeParent()
        {
            var serviceProvider = GetDefaultServiceProvider();
            var parentAppContainer = new AppContainer(serviceProvider);
            var childAppContainer = parentAppContainer.CreateChildContainer();
            var serviceCallList = new List<string>();
            var parentService = new MockLifetimeAwareService("Parent Service", serviceCallList);
            var childService = new MockLifetimeAwareService("Child Service", serviceCallList);

            parentAppContainer.LifetimeManager.Register(parentService);
            childAppContainer.LifetimeManager.Register(childService);
            await parentAppContainer.Deactivate();

            Assert.Equal(new string[] { "Child Service - Deactivate", "Parent Service - Deactivate" }, serviceCallList);
        }

        [Fact]
        public void DisposingRootAppContainer_ThrowsException()
        {
            var serviceProvider = GetDefaultServiceProvider();

            var appContainer = new AppContainer(serviceProvider);

            var e = Assert.Throws<InvalidOperationException>(() => appContainer.Dispose());
            Assert.Equal("You cannot dispose the root AppContainer.", e.Message);
        }

        [Fact]
        public void DisposingChildContainer_DisposesChildServiceProvider()
        {
            var serviceProvider = GetDefaultServiceProvider();
            var parentAppContainer = new AppContainer(serviceProvider);
            var childAppContainer = parentAppContainer.CreateChildContainer();
            var childServiceProvider = childAppContainer.Services as MockServiceProvider;
            
            childAppContainer.Dispose();

            Assert.True(childServiceProvider.IsDisposed);
        }

        [Fact]
        public async void DisposingChildContainer_DoesNotReceiveFurtherActivations()
        {
            var serviceProvider = GetDefaultServiceProvider();
            var parentAppContainer = new AppContainer(serviceProvider);
            var childAppContainer = parentAppContainer.CreateChildContainer();
            var serviceCallList = new List<string>();
            var service = new MockLifetimeAwareService("Child Service", serviceCallList);

            childAppContainer.LifetimeManager.Register(service);
            childAppContainer.Dispose();

            serviceCallList.Clear();
            await parentAppContainer.Activate();

            Assert.Equal(new string[] { }, serviceCallList);
        }

        [Fact]
        public async void DisposingChildContainer_DoesNotReceiveFurtherDeactivations()
        {
            var serviceProvider = GetDefaultServiceProvider();
            var parentAppContainer = new AppContainer(serviceProvider);
            var childAppContainer = parentAppContainer.CreateChildContainer();
            var serviceCallList = new List<string>();
            var service = new MockLifetimeAwareService("Child Service", serviceCallList);

            childAppContainer.LifetimeManager.Register(service);
            childAppContainer.Dispose();

            serviceCallList.Clear();
            await parentAppContainer.Deactivate();

            Assert.Equal(new string[] { }, serviceCallList);
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
                        .With<IServiceInjector<ILifetimeManager>>(new MockServiceInjector<ILifetimeManager>());
        }

        // *** Mock Classes ***

        private class MockLifetimeAwareService : ILifetimeAware
        {
            private string _serviceName;
            private IList<string> _serviceCallList;

            public MockLifetimeAwareService(string serviceName, IList<string> serviceCallList)
            {
                _serviceName = serviceName;
                _serviceCallList = serviceCallList;
            }

            public async Task Activate()
            {
                await Task.Yield();
                _serviceCallList.Add($"{_serviceName} - Activate");
            }

            public async Task Deactivate()
            {
                await Task.Yield();
                _serviceCallList.Add($"{_serviceName} - Deactivate");
            }
        }
    }
}

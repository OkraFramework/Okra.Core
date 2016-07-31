using Microsoft.Extensions.DependencyInjection;
using Okra.DependencyInjection;
using Okra.Lifetime;
using Okra.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Okra.Tests.DependencyInjection
{
    public class AppContainerFactoryFixture
    {
        [Fact]
        public void NewAppContainer_HasNewServiceProviderScope()
        {
            var info = CreateAppContainer();

            Assert.Equal(info.ServiceProvider, info.AppContainer.Services);
        }

        [Fact]
        public void NewAppContainer_HasLifetimeManager()
        {
            var info = CreateAppContainer();

            Assert.NotNull(info.AppContainer.LifetimeManager);
        }

        [Fact]
        public void NewAppContainer_HasNullParent()
        {
            var info = CreateAppContainer();

            Assert.Null(info.AppContainer.Parent);
        }

        [Fact]
        public void NewAppContainer_InjectsItselfIntoServiceProvider()
        {
            var info = CreateAppContainer();
            var appContainerInjector = info.ServiceProvider.GetService<IServiceInjector<IAppContainer>>();

            Assert.Equal(info.AppContainer, appContainerInjector.Service);
        }

        [Fact]
        public void NewAppContainer_InjectsLifetimeManagerIntoServiceProvider()
        {
            var info = CreateAppContainer();
            var lifetimeManagerInjector = info.ServiceProvider.GetService<IServiceInjector<ILifetimeManager>>();

            Assert.Equal(info.AppContainer.LifetimeManager, lifetimeManagerInjector.Service);
        }

        [Fact]
        public void ChildContainer_CanBeCreated()
        {
            var info = CreateAppContainerWithChild();

            Assert.NotNull(info.ChildAppContainer);
            Assert.NotEqual(info.ParentAppContainer, info.ChildAppContainer);
        }

        [Fact]
        public void ChildContainer_HasChildServiceProvider()
        {
            var info = CreateAppContainerWithChild();

            Assert.Equal(info.ChildServiceProvider, info.ChildAppContainer.Services);
        }

        [Fact]
        public void ChildContainer_HasNewLifetimeManager()
        {
            var info = CreateAppContainerWithChild();

            Assert.NotNull(info.ChildAppContainer.LifetimeManager);
            Assert.NotEqual(info.ParentAppContainer.LifetimeManager, info.ChildAppContainer.LifetimeManager);
        }

        [Fact]
        public void NewAppContainer_HasParent()
        {
            var info = CreateAppContainerWithChild();

            Assert.Equal(info.ParentAppContainer, info.ChildAppContainer.Parent);
        }

        [Fact]
        public async void ActivatingAppContainer_ActivatesServices()
        {
            var info = CreateAppContainer();
            var serviceCallList = new List<string>();
            var service = new MockLifetimeAwareService("Service", serviceCallList);

            info.AppContainer.LifetimeManager.Register(service);
            await info.AppContainer.Activate();

            Assert.Equal(new string[] { "Service - Activate" }, serviceCallList);
        }

        [Fact]
        public async void DeactivatingAppContainer_DeactivatesServices()
        {
            var info = CreateAppContainer();
            var serviceCallList = new List<string>();
            var service = new MockLifetimeAwareService("Service", serviceCallList);

            info.AppContainer.LifetimeManager.Register(service);
            await info.AppContainer.Deactivate();

            Assert.Equal(new string[] { "Service - Deactivate" }, serviceCallList);
        }

        [Fact]
        public async void ActivatingAppContainer_ActivatesChildContainerServices()
        {
            var info = CreateAppContainerWithChild();
            var serviceCallList = new List<string>();
            var service = new MockLifetimeAwareService("Child Service", serviceCallList);

            info.ChildAppContainer.LifetimeManager.Register(service);
            await info.ParentAppContainer.Activate();

            Assert.Equal(new string[] { "Child Service - Activate" }, serviceCallList);
        }

        [Fact]
        public async void DeactivatingAppContainer_DeactivatesChildContainerServices()
        {
            var info = CreateAppContainerWithChild();
            var serviceCallList = new List<string>();
            var service = new MockLifetimeAwareService("Child Service", serviceCallList);

            info.ChildAppContainer.LifetimeManager.Register(service);
            await info.ParentAppContainer.Deactivate();

            Assert.Equal(new string[] { "Child Service - Deactivate" }, serviceCallList);
        }

        [Fact]
        public async void ActivatingAppContainer_ActivatesParentBeforeChild()
        {
            var info = CreateAppContainerWithChild();
            var serviceCallList = new List<string>();
            var parentService = new MockLifetimeAwareService("Parent Service", serviceCallList);
            var childService = new MockLifetimeAwareService("Child Service", serviceCallList);

            info.ParentAppContainer.LifetimeManager.Register(parentService);
            info.ChildAppContainer.LifetimeManager.Register(childService);
            await info.ParentAppContainer.Activate();

            Assert.Equal(new string[] { "Parent Service - Activate", "Child Service - Activate" }, serviceCallList);
        }

        [Fact]
        public async void DeactivatingAppContainer_DeactivatesChildBeforeParent()
        {
            var info = CreateAppContainerWithChild();
            var serviceCallList = new List<string>();
            var parentService = new MockLifetimeAwareService("Parent Service", serviceCallList);
            var childService = new MockLifetimeAwareService("Child Service", serviceCallList);

            info.ParentAppContainer.LifetimeManager.Register(parentService);
            info.ChildAppContainer.LifetimeManager.Register(childService);
            await info.ParentAppContainer.Deactivate();

            Assert.Equal(new string[] { "Child Service - Deactivate", "Parent Service - Deactivate" }, serviceCallList);
        }

        [Fact]
        public void DisposingRootAppContainer_ThrowsException()
        {
            var info = CreateAppContainer();

            var e = Assert.Throws<InvalidOperationException>(() => info.AppContainer.Dispose());
            Assert.Equal("You cannot dispose the root AppContainer.", e.Message);
        }

        [Fact]
        public void DisposingChildContainer_DisposesChildServiceProvider()
        {
            var info = CreateAppContainerWithChild();

            info.ChildAppContainer.Dispose();

            Assert.True(info.ChildServiceProvider.IsDisposed);
        }

        [Fact]
        public async void DisposingChildContainer_DoesNotReceiveFurtherActivations()
        {
            var info = CreateAppContainerWithChild();
            var serviceCallList = new List<string>();
            var service = new MockLifetimeAwareService("Child Service", serviceCallList);

            info.ChildAppContainer.LifetimeManager.Register(service);
            info.ChildAppContainer.Dispose();

            serviceCallList.Clear();
            await info.ParentAppContainer.Activate();

            Assert.Equal(new string[] { }, serviceCallList);
        }

        [Fact]
        public async void DisposingChildContainer_DoesNotReceiveFurtherDeactivations()
        {
            var info = CreateAppContainerWithChild();
            var serviceCallList = new List<string>();
            var service = new MockLifetimeAwareService("Child Service", serviceCallList);

            info.ChildAppContainer.LifetimeManager.Register(service);
            info.ChildAppContainer.Dispose();

            serviceCallList.Clear();
            await info.ParentAppContainer.Deactivate();

            Assert.Equal(new string[] { }, serviceCallList);
        }

        // *** Helper Methods ***

        private AppContainerWithChildInfo CreateAppContainerWithChild()
        {
            var childServiceProvider = GetDefaultServiceProviderWithoutChild();
            var parentServiceProvider = GetDefaultServiceProvider(childServiceProvider);
            var rootServiceProvider = GetDefaultServiceProvider(parentServiceProvider);

            var parentAppContainer = new AppContainerFactory(rootServiceProvider).CreateAppContainer();
            var childAppContainer = new AppContainerFactory(parentServiceProvider).CreateAppContainer();

            return new AppContainerWithChildInfo()
            {
                ChildAppContainer = childAppContainer,
                ParentAppContainer = parentAppContainer,
                ChildServiceProvider = childServiceProvider,
                ParentServiceProvider = parentServiceProvider
            };
        }

        private AppContainerInfo CreateAppContainer()
        {
            var serviceProvider = GetDefaultServiceProviderWithoutChild();
            var rootServiceProvider = GetDefaultServiceProvider(serviceProvider);

            var appContainerFactory = new AppContainerFactory(rootServiceProvider);
            var appContainer = appContainerFactory.CreateAppContainer();

            return new AppContainerInfo()
            {
                AppContainer = appContainer,
                ServiceProvider = serviceProvider
            };
        }

        private MockServiceProvider GetDefaultServiceProvider()
        {
            var childServiceProvider = GetDefaultServiceProviderWithoutChild();
            var parentServiceProvider = GetDefaultServiceProvider(childServiceProvider);

            return parentServiceProvider;
        }

        private MockServiceProvider GetDefaultServiceProvider(MockServiceProvider childServiceProvider)
        {
            return GetDefaultServiceProviderWithoutChild()
                        .With<IServiceScopeFactory>(new MockServiceScopeFactory(childServiceProvider));
        }

        private MockServiceProvider GetDefaultServiceProviderWithoutChild()
        {
            return new MockServiceProvider()
                        .WithInjector<IAppContainer>(new MockServiceInjector<IAppContainer>())
                        .WithInjector<ILifetimeManager>(new MockServiceInjector<ILifetimeManager>());
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

        // *** Private Classes ***

        private class AppContainerInfo
        {
            public IAppContainer AppContainer;
            public MockServiceProvider ServiceProvider;
        }

        private class AppContainerWithChildInfo
        {
            public IAppContainer ChildAppContainer;
            public IAppContainer ParentAppContainer;

            public MockServiceProvider ChildServiceProvider;
            public MockServiceProvider ParentServiceProvider;
        }
    }
}

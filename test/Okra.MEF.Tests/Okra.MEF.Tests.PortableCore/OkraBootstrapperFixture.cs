using Microsoft.Extensions.DependencyInjection;
using Okra.Lifetime;
using Okra.Builder;
using Okra.MEF.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Okra.DependencyInjection;

namespace Okra.MEF.Tests
{
    public class OkraBootstrapperFixture
    {
        // *** Tests ***

        [Fact]
        public async void Activate_ActivatesRootAppContainer()
        {
            TestableBootstrapper bootstrapper = new TestableBootstrapper();
            bootstrapper.Initialize();

            Assert.Equal(0, bootstrapper.RootAppContainer.ActivateCount);
            Assert.Equal(0, bootstrapper.RootAppContainer.DeactivateCount);

            await bootstrapper.Activate();

            Assert.Equal(1, bootstrapper.RootAppContainer.ActivateCount);
            Assert.Equal(0, bootstrapper.RootAppContainer.DeactivateCount);
        }

        [Fact]
        public void Activate_ThrowsException_IfNotInitialized()
        {
            TestableBootstrapper bootstrapper = new TestableBootstrapper();

            var e = Assert.Throws<InvalidOperationException>(() => { bootstrapper.Activate(); });
            Assert.Equal("The bootstrapper must be initialized before performing this operation.", e.Message);
        }

        [Fact]
        public async void Deactivate_DeactivatesRootAppContainer()
        {
            TestableBootstrapper bootstrapper = new TestableBootstrapper();
            bootstrapper.Initialize();

            Assert.Equal(0, bootstrapper.RootAppContainer.ActivateCount);
            Assert.Equal(0, bootstrapper.RootAppContainer.DeactivateCount);

            await bootstrapper.Deactivate();

            Assert.Equal(0, bootstrapper.RootAppContainer.ActivateCount);
            Assert.Equal(1, bootstrapper.RootAppContainer.DeactivateCount);
        }

        [Fact]
        public void Deactivate_ThrowsException_IfNotInitialized()
        {
            TestableBootstrapper bootstrapper = new TestableBootstrapper();

            var e = Assert.Throws<InvalidOperationException>(() => { bootstrapper.Deactivate(); });
            Assert.Equal("The bootstrapper must be initialized before performing this operation.", e.Message);
        }

        [Fact]
        public void Launch_CallsLaunchPipelineWithSpecifiedArguments()
        {
            TestableBootstrapper bootstrapper = new TestableBootstrapper();
            bootstrapper.Initialize();

            var appLaunchArgs1 = new MockAppLaunchRequest();
            var appLaunchArgs2 = new MockAppLaunchRequest();
            bootstrapper.Launch(appLaunchArgs1);
            bootstrapper.Launch(appLaunchArgs2);

            Assert.Equal(2, bootstrapper.AppLaunchCalls.Count);
            Assert.Equal(appLaunchArgs1, bootstrapper.AppLaunchCalls[0].LaunchRequest);
            Assert.Equal(appLaunchArgs2, bootstrapper.AppLaunchCalls[1].LaunchRequest);
        }

        [Fact]
        public void Launch_ThrowsException_IfNotInitialized()
        {
            TestableBootstrapper bootstrapper = new TestableBootstrapper();

            var e = Assert.Throws<InvalidOperationException>(() => { bootstrapper.Launch(new MockAppLaunchRequest()); });
            Assert.Equal("The bootstrapper must be initialized before performing this operation.", e.Message);
        }

        [Fact]
        public void Launch_ThrowsException_IfLaunchArgumentsAreNull()
        {
            TestableBootstrapper bootstrapper = new TestableBootstrapper();
            bootstrapper.Initialize();

            var e = Assert.Throws<ArgumentNullException>(() => { bootstrapper.Launch(null); });
            Assert.Equal("appLaunchRequest", e.ParamName);
        }

        [Fact]
        public void Initialize_CallsConfigureServicesThenConfigure()
        {
            TestableBootstrapper bootstrapper = new TestableBootstrapper();

            bootstrapper.Initialize();

            Assert.Equal(2, bootstrapper.MethodCalls.Count);
            Assert.Equal("ConfigureServices", bootstrapper.MethodCalls[0].Item1);
            Assert.Equal("Configure", bootstrapper.MethodCalls[1].Item1);
        }

        [Fact]
        public void Initialize_CallsConfigureServices_WithValidServiceCollection()
        {
            TestableBootstrapper bootstrapper = new TestableBootstrapper();

            bootstrapper.Initialize();

            object[] args = bootstrapper.MethodCalls.Where(m => m.Item1 == "ConfigureServices").FirstOrDefault().Item2;
            IServiceCollection serviceCollection = args[0] as IServiceCollection;

            Assert.NotNull(serviceCollection);
        }

        [Fact]
        public void Initialize_CallsConfigure_WithOkraAppBuilder()
        {
            TestableBootstrapper bootstrapper = new TestableBootstrapper();

            bootstrapper.Initialize();

            object[] args = bootstrapper.MethodCalls.Where(m => m.Item1 == "Configure").FirstOrDefault().Item2;
            IOkraAppBuilder appBuilder = args[0] as IOkraAppBuilder;

            Assert.NotNull(appBuilder);
        }

        [Fact]
        public void Initialize_CallsConfigure_OkraAppBuilderHasCorrectApplicationServices()
        {
            TestableBootstrapper bootstrapper = new TestableBootstrapper();

            bootstrapper.Initialize();

            object[] args = bootstrapper.MethodCalls.Where(m => m.Item1 == "Configure").FirstOrDefault().Item2;
            IOkraAppBuilder appBuilder = args[0] as IOkraAppBuilder;

            Assert.NotNull(appBuilder.ApplicationServices);
        }

        // *** Test Classes ***

        private class TestableBootstrapper : OkraBootstrapper
        {
            public List<AppLaunchContext> AppLaunchCalls = new List<AppLaunchContext>();
            public List<Tuple<string, object[]>> MethodCalls = new List<Tuple<string, object[]>>();
            public MockAppContainer RootAppContainer = new MockAppContainer();

            protected override void Configure(IOkraAppBuilder app)
            {
                MethodCalls.Add(Tuple.Create("Configure", new object[] { app }));

                app.Use(next =>
                {
                    return context =>
                    {
                        AppLaunchCalls.Add(context);
                        return next(context);
                    };
                });
            }

            protected override void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton<IAppContainerFactory>(new MockAppContainerFactory(RootAppContainer));

                MethodCalls.Add(Tuple.Create("ConfigureServices", new object[] { services }));
            }
        }

        private class MockAppLaunchRequest : IAppLaunchRequest
        {
        }

        private class MockAppContainerFactory : IAppContainerFactory
        {
            private readonly IAppContainer _rootContainer;

            public MockAppContainerFactory(IAppContainer rootContainer)
            {
                _rootContainer = rootContainer;
            }

            public IAppContainer CreateAppContainer()
            {
                return _rootContainer;
            }
        }

        private class MockAppContainer : IAppContainer
        {
            public int ActivateCount;
            public int DeactivateCount;

            public ILifetimeManager LifetimeManager
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IAppContainer Parent
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public IServiceProvider Services
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public async Task Activate()
            {
                await Task.Yield();
                ActivateCount++;
            }

            public IAppContainer CreateChildContainer()
            {
                throw new NotImplementedException();
            }

            public async Task Deactivate()
            {
                await Task.Yield();
                DeactivateCount++;
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}

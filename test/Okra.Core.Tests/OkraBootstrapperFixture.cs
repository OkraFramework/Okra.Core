using Microsoft.Extensions.DependencyInjection;
using Okra.Lifetime;
using Okra.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Okra.DependencyInjection;
using Okra.Tests.Mocks;

namespace Okra.Tests
{
    public class OkraBootstrapperFixture
    {
        [Fact]
        public void Initialize_SetsApplicationServices()
        {
            TestableBootstrapper bootstrapper = new TestableBootstrapper();

            bootstrapper.Initialize();
            
            Assert.NotNull(bootstrapper.ApplicationServices);
            Assert.Equal(bootstrapper.ServiceProvider, bootstrapper.ApplicationServices);
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
            Assert.Equal(bootstrapper.OkraAppBuilder, appBuilder);
        }
        
        [Fact] 
        public void ApplicationServices_ThrowsException_IfNotInitialized() 
        { 
            TestableBootstrapper bootstrapper = new TestableBootstrapper(); 
            
            var e = Assert.Throws<InvalidOperationException>(() => { var s = bootstrapper.ApplicationServices; }); 
            Assert.Equal("The bootstrapper must be initialized before performing this operation.", e.Message); 
         } 


        // *** Test Classes ***

        private class TestableBootstrapper : OkraBootstrapper
        {
            public List<Tuple<string, object[]>> MethodCalls = new List<Tuple<string, object[]>>();
            public MockOkraAppBuilder OkraAppBuilder = new MockOkraAppBuilder();
            public MockServiceProvider ServiceProvider = new MockServiceProvider();

            protected override void Configure(IOkraAppBuilder app)
            {
                MethodCalls.Add(Tuple.Create("Configure", new object[] { app }));
            }

            protected override IServiceProvider ConfigureServices(IServiceCollection services)
            {
                MethodCalls.Add(Tuple.Create("ConfigureServices", new object[] { services }));

                ServiceProvider.With<IOkraAppBuilder>(OkraAppBuilder);
                return ServiceProvider;
            }
        }
        
        private class MockOkraAppBuilder : IOkraAppBuilder
        {
            public IServiceProvider ApplicationServices
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
            public AppLaunchDelegate Build()
            {
                throw new NotImplementedException();
            }
            public IOkraAppBuilder Use(Func<AppLaunchDelegate, AppLaunchDelegate> middleware)
            {
                throw new NotImplementedException();
            }
        }
    }
}

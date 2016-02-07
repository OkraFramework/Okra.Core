using Okra.Builder;
using Okra.DependencyInjection;
using Okra.MEF.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Okra.MEF.Tests
{
    public class OkraBootstrapperFixture
    {
        // *** Tests ***

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
            Assert.IsType<MefServiceProvider>(appBuilder.ApplicationServices);
        }

        // *** Test Classes ***

        private class TestableBootstrapper : OkraBootstrapper
        {
            public List<Tuple<string, object[]>> MethodCalls = new List<Tuple<string, object[]>>();

            protected override void Configure(IOkraAppBuilder app)
            {
                MethodCalls.Add(Tuple.Create("Configure", new object[] { app }));
            }

            protected override void ConfigureServices(IServiceCollection services)
            {
                MethodCalls.Add(Tuple.Create("ConfigureServices", new object[] { services }));
            }
        }
    }
}

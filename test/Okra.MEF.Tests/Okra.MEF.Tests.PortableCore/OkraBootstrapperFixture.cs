using Microsoft.Extensions.DependencyInjection;
using Okra.Activation;
using Okra.Builder;
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
        public void Activate_CallsActivationPipelineWithSpecifiedArguments()
        {
            TestableBootstrapper bootstrapper = new TestableBootstrapper();
            bootstrapper.Initialize();

            var activationArgs1 = new MockAppActivationRequest();
            var activationArgs2 = new MockAppActivationRequest();
            bootstrapper.Activate(activationArgs1);
            bootstrapper.Activate(activationArgs2);

            Assert.Equal(2, bootstrapper.ActivationCalls.Count);
            Assert.Equal(activationArgs1, bootstrapper.ActivationCalls[0].ActivationRequest);
            Assert.Equal(activationArgs2, bootstrapper.ActivationCalls[1].ActivationRequest);
        }

        [Fact]
        public void Activate_ThrowsException_IfNotInitialized()
        {
            TestableBootstrapper bootstrapper = new TestableBootstrapper();

            var e = Assert.Throws<InvalidOperationException>(() => { bootstrapper.Activate(new MockAppActivationRequest()); });
            Assert.Equal("The bootstrapper must be initialized before performing this operation.", e.Message);
        }

        [Fact]
        public void Activate_ThrowsException_IfActivationArgumentsAreNull()
        {
            TestableBootstrapper bootstrapper = new TestableBootstrapper();
            bootstrapper.Initialize();

            var e = Assert.Throws<ArgumentNullException>(() => { bootstrapper.Activate(null); });
            Assert.Equal("activationRequest", e.ParamName);
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
            public List<AppActivationContext> ActivationCalls = new List<AppActivationContext>();
            public List<Tuple<string, object[]>> MethodCalls = new List<Tuple<string, object[]>>();

            protected override void Configure(IOkraAppBuilder app)
            {
                MethodCalls.Add(Tuple.Create("Configure", new object[] { app }));

                app.Use(next =>
                {
                    return context =>
                    {
                        ActivationCalls.Add(context);
                        return next(context);
                    };
                });
            }

            protected override void ConfigureServices(IServiceCollection services)
            {
                MethodCalls.Add(Tuple.Create("ConfigureServices", new object[] { services }));
            }
        }

        private class MockAppActivationRequest : IAppActivationRequest
        {
        }
    }
}

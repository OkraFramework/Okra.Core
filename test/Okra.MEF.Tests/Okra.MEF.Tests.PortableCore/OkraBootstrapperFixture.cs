using Okra.DependencyInjection;
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

        // *** Test Classes ***

        private class TestableBootstrapper : OkraBootstrapper
        {
            public List<Tuple<string, object[]>> MethodCalls = new List<Tuple<string, object[]>>();

            protected override void Configure()
            {
                MethodCalls.Add(Tuple.Create("Configure", new object[] { }));
            }

            protected override void ConfigureServices(IServiceCollection services)
            {
                MethodCalls.Add(Tuple.Create("ConfigureServices", new object[] { services }));
            }
        }
    }
}

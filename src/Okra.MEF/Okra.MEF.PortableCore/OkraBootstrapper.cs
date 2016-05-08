using Microsoft.Extensions.DependencyInjection;
using Okra.Lifetime;
using Okra.Builder;
using Okra.MEF.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.MEF
{
    public abstract class OkraBootstrapper : IOkraBootstrapper
    {
        // *** Fields ***

        private AppLaunchDelegate _appLaunchPipeline;

        // *** Methods ***

        public Task Launch(IAppLaunchRequest appLaunchRequest)
        {
            if (appLaunchRequest == null)
                throw new ArgumentNullException(nameof(appLaunchRequest));

            if (_appLaunchPipeline == null)
                throw new InvalidOperationException(Properties.Errors.Exception_InvalidOperation_BootstrapperNotInitialized);

            AppLaunchContext appLaunchContext = new MefAppLaunchContext(appLaunchRequest);
            return _appLaunchPipeline(appLaunchContext);
        }

        public void Initialize()
        {
            // Initialize MEF and compose the bootstrapper

            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            // Create the OkraAppBuilder and configure the application

            OkraAppBuilder appBuilder = new OkraAppBuilder(serviceProvider);
            Configure(appBuilder);
            _appLaunchPipeline = appBuilder.Build();
        }

        // *** Protected Methods ***

        protected virtual void Configure(IOkraAppBuilder app)
        {
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }

        // *** Private sub-classes ***

        private class MefAppLaunchContext : AppLaunchContext
        {
            public MefAppLaunchContext(IAppLaunchRequest launchRequest)
            {
                this.LaunchRequest = launchRequest;
            }

            public override IAppLaunchRequest LaunchRequest
            {
                get;
            }
        }
    }
}

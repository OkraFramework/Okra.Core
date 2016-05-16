using Microsoft.Extensions.DependencyInjection;
using Okra.Lifetime;
using Okra.Builder;
using Okra.MEF.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.DependencyInjection;

namespace Okra.MEF
{
    public abstract class OkraBootstrapper : IOkraBootstrapper
    {
        // *** Fields ***

        private IAppContainer _appContainer;
        private AppLaunchDelegate _appLaunchPipeline;

        // *** Methods ***

        public Task Activate()
        {
            if (!IsInitialized)
                throw new InvalidOperationException(Properties.Errors.Exception_InvalidOperation_BootstrapperNotInitialized);

            return _appContainer.Activate();
        }

        public Task Deactivate()
        {
            if (!IsInitialized)
                throw new InvalidOperationException(Properties.Errors.Exception_InvalidOperation_BootstrapperNotInitialized);

            return _appContainer.Deactivate();
        }

        public Task Launch(IAppLaunchRequest appLaunchRequest)
        {
            if (appLaunchRequest == null)
                throw new ArgumentNullException(nameof(appLaunchRequest));

            if (!IsInitialized)
                throw new InvalidOperationException(Properties.Errors.Exception_InvalidOperation_BootstrapperNotInitialized);

            AppLaunchContext appLaunchContext = new MefAppLaunchContext(appLaunchRequest);
            return _appLaunchPipeline(appLaunchContext);
        }

        public void Initialize()
        {
            // Initialize MEF and compose the bootstrapper

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var appContainerFactory = serviceProvider.GetRequiredService<IAppContainerFactory>();
            _appContainer = appContainerFactory.CreateAppContainer();

            // Create the OkraAppBuilder and configure the application

            OkraAppBuilder appBuilder = new OkraAppBuilder(_appContainer.Services);
            Configure(appBuilder);
            _appLaunchPipeline = appBuilder.Build();
        }

        private bool IsInitialized
        {
            get
            {
                return _appLaunchPipeline != null;
            }
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

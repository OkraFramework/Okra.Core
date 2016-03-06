using Microsoft.Extensions.DependencyInjection;
using Okra.Activation;
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

        private ActivationDelegate _activationPipeline;

        // *** Methods ***

        public Task Activate(IAppActivationRequest activationRequest)
        {
            if (activationRequest == null)
                throw new ArgumentNullException(nameof(activationRequest));

            if (_activationPipeline == null)
                throw new InvalidOperationException(Properties.Errors.Exception_InvalidOperation_BootstrapperNotInitialized);

            AppActivationContext activationContext = new MefAppActivationContext(activationRequest);
            return _activationPipeline(activationContext);
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
            _activationPipeline = appBuilder.Build();
        }

        // *** Protected Methods ***

        protected virtual void Configure(IOkraAppBuilder app)
        {
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }

        // *** Private sub-classes ***

        private class MefAppActivationContext : AppActivationContext
        {
            public MefAppActivationContext(IAppActivationRequest activationRequest)
            {
                this.ActivationRequest = activationRequest;
            }

            public override IAppActivationRequest ActivationRequest
            {
                get;
            }
        }
    }
}

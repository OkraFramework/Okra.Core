using Microsoft.Extensions.DependencyInjection;
using Okra.Lifetime;
using Okra.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.DependencyInjection;
using Okra.Core;

namespace Okra
{
    public abstract class OkraBootstrapper : IOkraBootstrapper
    {
        private IServiceProvider _applicationServices;
        
        // *** Properties ***
        
        public IServiceProvider ApplicationServices
        {
            get
            {
                if (_applicationServices == null)
                    throw new InvalidOperationException(Resources.Exception_InvalidOperation_BootstrapperNotInitialized);
                    
                return _applicationServices;
            }
        }
        
        // *** Methods ***

        public void Initialize()
        {
            // Initialize MEF and compose the bootstrapper

            var serviceCollection = new ServiceCollection();
            _applicationServices = ConfigureServices(serviceCollection);

            // Get the IOkraAppBuilder and configure the application

            IOkraAppBuilder appBuilder = ApplicationServices.GetRequiredService<IOkraAppBuilder>();
            Configure(appBuilder);
        }

        // *** Protected Methods ***

        protected virtual void Configure(IOkraAppBuilder app)
        {
        }

        protected abstract IServiceProvider ConfigureServices(IServiceCollection services);

        // *** Private Sub-classes ***

        private class ServiceCollection : List<ServiceDescriptor>, IServiceCollection
        {
        }
    }
}

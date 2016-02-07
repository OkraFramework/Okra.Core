using Okra.Builder;
using Okra.DependencyInjection;
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
        // *** Methods ***

        public void Activate(object args)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            // Initialize MEF and compose the bootstrapper

            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            MefServiceProvider serviceProvider = new MefServiceProvider();

            // Create the OkraAppBuilder and configure the application

            OkraAppBuilder appBuilder = new OkraAppBuilder(serviceProvider);
            Configure(appBuilder);
        }

        // *** Protected Methods ***

        protected virtual void Configure(IOkraAppBuilder app)
        {
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }
    }
}

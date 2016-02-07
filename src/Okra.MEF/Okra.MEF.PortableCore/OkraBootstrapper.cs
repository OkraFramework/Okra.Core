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
            // TODO : MefServiceProvider serviceProvider = new MefServiceProvider(serviceCollection);

            Configure();
        }

        // *** Protected Methods ***

        protected virtual void Configure()
        {
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.DependencyInjection
{
    public class AppContainerFactory : IAppContainerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public AppContainerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IAppContainer CreateAppContainer()
        {
            return new AppContainer(_serviceProvider);
        }
    }
}

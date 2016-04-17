using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.State;
using Microsoft.Extensions.DependencyInjection;

namespace Okra.DependencyInjection
{
    public class AppContainer : IAppContainer
    {
        public AppContainer(IServiceProvider serviceProvider)
        {
            this.Services = serviceProvider;
            this.State = new StateService();

            serviceProvider.InjectService<IAppContainer>(this);
            serviceProvider.InjectService<IStateService>(this.State);
        }

        public IServiceProvider Services
        {
            get;
        }

        public IStateService State
        {
            get;
        }

        public IAppContainer CreateChildContainer()
        {
            // TODO : Make sure child services are disposed!
            // TODO : Ensure State inherits from parent

            var serviceScopeFactory = Services.GetRequiredService<IServiceScopeFactory>();
            var serviceScope = serviceScopeFactory.CreateScope();
            var childServices = serviceScope.ServiceProvider;

            return new AppContainer(childServices);
        }
    }
}

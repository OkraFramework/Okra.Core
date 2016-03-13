using Microsoft.Extensions.DependencyInjection;
using Okra.Navigation;
using Okra.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.DependencyInjection
{
    public static class MvvmCoreServiceCollectionExtensions
    {
        public static IMvvmCoreBuilder AddMvvmCore(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // TODO : These should be scoped!!!!
            services.AddSingleton<INavigationManager, NavigationManager>();
            services.AddSingleton<IRouteBuilder, RouteBuilder>();
            services.AddSingleton<IViewRouter, ViewRouterProxy>();

            return new MvvmCoreBuilder(services);
        }
    }
}

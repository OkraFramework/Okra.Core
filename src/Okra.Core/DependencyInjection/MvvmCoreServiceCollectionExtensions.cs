﻿using Microsoft.Extensions.DependencyInjection;
using Okra.Builder;
using Okra.Lifetime;
using Okra.Navigation;
using Okra.Routing;
using Okra.State;
using System;

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
            services.AddSingleton<IOkraAppBuilder, OkraAppBuilder>();

            services.AddScoped<IAppContainerFactory, AppContainerFactory>();

            services.AddInjected<IAppContainer>();
            services.AddInjected<IStateService>();
            services.AddInjected<ILifetimeManager>();

            return new MvvmCoreBuilder(services);
        }
    }
}

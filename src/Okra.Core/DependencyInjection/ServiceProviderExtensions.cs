using System;
using Microsoft.Extensions.DependencyInjection;

namespace Okra.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static void InjectService<T>(this IServiceProvider serviceProvider, T service)
        {
            var serviceInjector = serviceProvider.GetRequiredService<IServiceInjector<T>>();
            serviceInjector.Service = service;
        }

        public static T GetInjectedService<T>(this IServiceProvider serviceProvider)
        {
            var serviceInjector = serviceProvider.GetRequiredService<IServiceInjector<T>>();
            return serviceInjector.Service;
        }
    }
}

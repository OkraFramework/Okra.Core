using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInjected<T>(this IServiceCollection services) where T : class
        {
            services.AddScoped<IServiceInjector<T>, ServiceInjector<T>>();
            services.AddScoped<T>(p => p.GetInjectedService<T>());

            return services;
        }
    }
}

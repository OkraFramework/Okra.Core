using Microsoft.Extensions.DependencyInjection;

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

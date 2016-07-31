using Microsoft.Extensions.DependencyInjection;
using System;

namespace Okra.DependencyInjection
{
    internal class MvvmCoreBuilder : IMvvmCoreBuilder
    {
        public MvvmCoreBuilder(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}

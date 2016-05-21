using Microsoft.Extensions.DependencyInjection;
using Okra.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.DependencyInjection
{
    public static class MvvmServiceCollectionExtensions
    {
        public static IMvvmBuilder AddMvvm(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var builder = services.AddMvvmCore();

            builder.Services.AddTransient<WindowAppHost, WindowAppHost>();
            builder.Services.AddTransient<DefaultAppShell, DefaultAppShell>();

            return new MvvmBuilder(builder.Services);
        }
    }
}

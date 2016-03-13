using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.DependencyInjection
{
    internal class MvvmBuilder : IMvvmBuilder
    {
        public MvvmBuilder(IServiceCollection services)
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

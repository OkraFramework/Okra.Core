using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.MEF.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Okra.MEF.Tests.DependencyInjection
{
    public class DependencyInjectionSpecificationTests : Microsoft.Extensions.DependencyInjection.Specification.DependencyInjectionSpecificationTests
    {
        protected override IServiceProvider CreateServiceProvider(IServiceCollection serviceCollection)
        {
            return serviceCollection.BuildServiceProvider();
        }
    }
}

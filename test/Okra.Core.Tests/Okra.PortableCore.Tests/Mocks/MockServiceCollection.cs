using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Tests.Mocks
{
    public class MockServiceCollection : List<ServiceDescriptor>, IServiceCollection
    {
    }
}

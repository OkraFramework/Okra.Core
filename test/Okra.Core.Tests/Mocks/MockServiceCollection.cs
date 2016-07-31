using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Okra.Tests.Mocks
{
    public class MockServiceCollection : List<ServiceDescriptor>, IServiceCollection
    {
    }
}

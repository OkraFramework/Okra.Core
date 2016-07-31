using Microsoft.Extensions.DependencyInjection;
using System;

namespace Okra.Tests.Mocks
{
    public class MockServiceScopeFactory : IServiceScopeFactory
    {
        private readonly IServiceProvider _childServiceProvider;

        public MockServiceScopeFactory(IServiceProvider childServiceProvider)
        {
            _childServiceProvider = childServiceProvider;
        }

        public IServiceScope CreateScope()
        {
            return new MockServiceScope(_childServiceProvider);
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

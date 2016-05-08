using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Tests.Mocks
{
    public class MockServiceScope : IServiceScope
    {
        private readonly IServiceProvider _childServiceProvider;

        public MockServiceScope(IServiceProvider childServiceProvider)
        {
            _childServiceProvider = childServiceProvider;
        }

        public bool IsDisposed
        {
            get;
            private set;
        }

        public IServiceProvider ServiceProvider
        {
            get
            {
                return _childServiceProvider;
            }
        }

        public void Dispose()
        {
            if (_childServiceProvider is IDisposable)
            {
                ((IDisposable)_childServiceProvider).Dispose();
            }

            this.IsDisposed = true;
        }
    }
}

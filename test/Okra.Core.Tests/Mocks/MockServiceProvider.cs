using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Tests.Mocks
{
    internal class MockServiceProvider : IServiceProvider, IDisposable
    {
        Dictionary<Type, object> _serviceDictionary = new Dictionary<Type, object>();

        public MockServiceProvider()
        {
        }

        public bool IsDisposed
        {
            get;
            private set;
        }

        public MockServiceProvider With<T>(T service)
        {
            _serviceDictionary[typeof(T)] = service;
            return this;
        }

        public object GetService(Type serviceType)
        {
            return _serviceDictionary[serviceType];
        }

        public void Dispose()
        {
            this.IsDisposed = true;
        }
    }
}

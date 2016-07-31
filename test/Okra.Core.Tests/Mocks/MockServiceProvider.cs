using Okra.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Okra.Tests.Mocks
{
    internal class MockServiceProvider : IServiceProvider, IDisposable
    {
        Dictionary<Type, Func<object>> _serviceDictionary = new Dictionary<Type, Func<object>>();

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
            _serviceDictionary[typeof(T)] = () => service ;
            return this;
        }

        public MockServiceProvider WithInjector<T>(IServiceInjector<T> serviceInjector)
        {
            _serviceDictionary[typeof(IServiceInjector<T>)] = () => serviceInjector;
            _serviceDictionary[typeof(T)] = () => serviceInjector.Service;
            return this;
        }

        public object GetService(Type serviceType)
        {
            return _serviceDictionary[serviceType]();
        }

        public void Dispose()
        {
            this.IsDisposed = true;
        }
    }
}

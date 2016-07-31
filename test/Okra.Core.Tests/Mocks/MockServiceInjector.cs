using Okra.DependencyInjection;
using System;

namespace Okra.Tests.Mocks
{
    public class MockServiceInjector<T> : IServiceInjector<T>
    {
        private T _service;

        public bool HasValue
        {
            get
            {
                return _service != null;
            }
        }

        public T Service
        {
            get
            {
                if (_service == null)
                    throw new InvalidOperationException();

                return _service;
            }
            set
            {
                if (_service != null)
                    throw new InvalidOperationException();

                _service = value;
            }
        }
    }
}

using Okra.Helpers;
using System;

namespace Okra.DependencyInjection
{
    public class ServiceInjector<T> : IServiceInjector<T>
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
                    throw new InvalidOperationException(string.Format(ResourceHelper.GetErrorResource("Exception_InvalidOperation_ServiceNotInjected"), typeof(T).Name));

                return _service;
            }

            set
            {
                if (_service != null)
                    throw new InvalidOperationException(string.Format(ResourceHelper.GetErrorResource("Exception_InvalidOperation_ServiceAlreadyInjected"), typeof(T).Name));

                _service = value;
            }
        }
    }
}

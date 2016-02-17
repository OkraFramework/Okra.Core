using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Tests.Mocks
{
    internal class MockServiceProvider : IServiceProvider
    {
        Dictionary<Type, object> _serviceDictionary = new Dictionary<Type, object>();

        public MockServiceProvider(params object[] services)
        {
            foreach (var service in services)
                _serviceDictionary.Add(service.GetType(), service);
        }

        public object GetService(Type serviceType)
        {
            return _serviceDictionary[serviceType];
        }
    }
}

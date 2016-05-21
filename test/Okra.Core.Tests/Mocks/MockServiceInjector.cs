using Okra.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Tests.Mocks
{
    public class MockServiceInjector<T> : IServiceInjector<T>
    {
        public T Service
        {
            get;
            set;
        }
    }
}

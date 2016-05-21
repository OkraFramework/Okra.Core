using Okra.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Tests.Mocks
{
    internal class MockStateService : IStateService
    {
        public T GetState<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void SetState<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public bool TryGetState<T>(string key, out T value)
        {
            throw new NotImplementedException();
        }
    }
}

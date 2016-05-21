using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.State
{
    public interface IStateService
    {
        T GetState<T>(string key);
        bool TryGetState<T>(string key, out T value);
        void SetState<T>(string key, T value);
    }
}

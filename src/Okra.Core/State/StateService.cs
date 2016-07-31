using System.Collections.Generic;

namespace Okra.State
{
    public class StateService : IStateService
    {
        private readonly Dictionary<string, object> _stateDictionary = new Dictionary<string, object>();

        public T GetState<T>(string key)
        {
            return (T)_stateDictionary[key];
        }

        public bool TryGetState<T>(string key, out T value)
        {
            object obj;
            
            if (_stateDictionary.TryGetValue(key, out obj))
            {
                value = (T)obj;
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public void SetState<T>(string key, T value)
        {
            _stateDictionary[key] = value;
        }
    }
}

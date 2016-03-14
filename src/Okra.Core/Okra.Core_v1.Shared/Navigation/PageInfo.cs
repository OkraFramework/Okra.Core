using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Okra.Helpers;

namespace Okra.Navigation
{
    [DataContract]
    public class PageInfo
    {
        // *** Fields ***

        [DataMember]
        private string _pageName;

        [DataMember]
        private StateData _arguments;

        [DataMember]
        private Dictionary<string, StateData> _stateDictionary = new Dictionary<string, StateData>();

        // *** Events ***

        public event StateChangedEventHandler StateChanged;

        // *** Constructors ***

        public PageInfo(string pageName, object arguments)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), nameof(pageName));

            // Set properties

            _pageName = pageName;
            _arguments = StateData.Create(arguments);
        }

        // *** Properties ***

        public string PageName
        {
            get
            {
                return _pageName;
            }
        }

        // *** Methods ***

        public T GetArguments<T>()
        {
            return _arguments.GetData<T>();
        }

        public T GetState<T>(string key)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(key))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), nameof(key));

            // Get value from dictionary

            return (T)_stateDictionary[key].GetData<T>();
        }

        public bool TryGetState<T>(string key, out T value)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(key))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), nameof(key));

            // Get value from dictionary

            StateData stateData;
            bool success = _stateDictionary.TryGetValue(key, out stateData);

            if (success)
                value = stateData.GetData<T>();
            else
                value = default(T);

            return success;
        }

        public void SetState<T>(string key, T value)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(key))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), nameof(key));

            // Set value into dictionary

            _stateDictionary[key] = StateData.Create<T>(value);

            // Raise events

            OnStateChanged(key);
        }

        // *** Protected Methods ***

        protected void OnStateChanged(string stateKey)
        {
            StateChangedEventHandler hander = StateChanged;

            if (hander != null)
                hander(this, new StateChangedEventArgs(stateKey));
        }

        // *** Overriden Base Methods ***

        public override string ToString() => _pageName;

        // *** Private Sub-classes ***

        [DataContract]
        private struct StateData
        {
            // *** Fields ***

            private object _data;
            private Type _dataType;
            private byte[] _rawData;

            // *** Properties ***

            [DataMember]
            private byte[] RawData
            {
                get
                {
                    if (_rawData == null && _data != null)
                        _rawData = SerializationHelper.SerializeToArray(_data, _dataType);

                    return _rawData;
                }
                set
                {
                    _rawData = value;
                }
            }

            // *** Methods ***

            public T GetData<T>()
            {
                if (_data == null)
                {
                    if (_rawData != null)
                    {
                        _data = SerializationHelper.DeserializeFromArray(_rawData, typeof(T));
                    }
                    else
                    {
                        _data = default(T);
                    }

                    _dataType = typeof(T);
                }

                return (T)_data;
            }

            // *** Static Methods ***

            public static StateData Create(object value)
            {
                return new StateData()
                {
                    _data = value,
                    _dataType = value != null ? value.GetType() : typeof(void)
                };
            }

            public static StateData Create<T>(object value)
            {
                return new StateData()
                {
                    _data = value,
                    _dataType = typeof(T)
                };
            }
        }
    }
}

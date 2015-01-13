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
        private string pageName;

        [DataMember]
        private StateData arguments;

        [DataMember]
        private Dictionary<string, StateData> stateDictionary = new Dictionary<string, StateData>();

        // *** Events ***

        public event StateChangedEventHandler StateChanged;

        // *** Constructors ***

        public PageInfo(string pageName, object arguments)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "pageName");

            // Set properties

            this.pageName = pageName;
            this.arguments = StateData.Create(arguments);
        }

        // *** Properties ***

        public string PageName
        {
            get
            {
                return pageName;
            }
        }

        // *** Methods ***

        public T GetArguments<T>()
        {
            return arguments.GetData<T>();
        }

        public T GetState<T>(string key)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(key))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "key");

            // Get value from dictionary

            return (T)stateDictionary[key].GetData<T>();
        }

        public bool TryGetState<T>(string key, out T value)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(key))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "key");

            // Get value from dictionary

            StateData stateData;
            bool success = stateDictionary.TryGetValue(key, out stateData);

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
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "key");

            // Set value into dictionary

            stateDictionary[key] = StateData.Create<T>(value);

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

        public override string ToString()
        {
            return pageName;
        }

        // *** Private Sub-classes ***

        [DataContract]
        private struct StateData
        {
            // *** Fields ***

            private object data;
            private Type dataType;
            private byte[] rawData;

            // *** Properties ***

            [DataMember]
            private byte[] RawData
            {
                get
                {
                    if (rawData == null && data != null)
                        rawData = SerializationHelper.SerializeToArray(data, dataType);

                    return rawData;
                }
                set
                {
                    rawData = value;
                }
            }

            // *** Methods ***

            public T GetData<T>()
            {
                if (data == null)
                {
                    if (rawData != null)
                    {
                        data = SerializationHelper.DeserializeFromArray(rawData, typeof(T));
                    }
                    else
                    {
                        data = default(T);
                    }

                    dataType = typeof(T);
                }

                return (T)data;
            }

            // *** Static Methods ***

            public static StateData Create(object value)
            {
                return new StateData()
                {
                    data = value,
                    dataType = value != null ? value.GetType() : typeof(void)
                };
            }

            public static StateData Create<T>(object value)
            {
                return new StateData()
                    {
                        data = value,
                        dataType = typeof(T)
                    };
            }
        }
    }
}

using System;
using System.IO;
using System.Runtime.Serialization;

namespace Okra.Helpers
{
    internal static class SerializationHelper
    {
        // *** Methods ***

        public static object DeserializeFromArray(byte[] data, Type type)
        {
            using (var stream = new MemoryStream(data))
            {
                var serializer = new DataContractSerializer(type);
                return serializer.ReadObject(stream);
            }
        }

        public static T DeserializeFromArray<T>(byte[] data)
        {
            return (T)DeserializeFromArray(data, typeof(T));
        }

        public static byte[] SerializeToArray(object value, Type type)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(type);
                serializer.WriteObject(stream, value);
                return stream.ToArray();
            }
        }

        public static byte[] SerializeToArray<T>(T value)
        {
            return SerializeToArray(value, typeof(T));
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Tests.Helpers
{
    public static class SerializationHelper
    {
        public static T DeserializeFromArray<T>(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                return (T)serializer.ReadObject(stream);
            }
        }

        public static byte[] SerializeToArray<T>(T value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(stream, value);
                return stream.ToArray();
            }
        }
    }
}

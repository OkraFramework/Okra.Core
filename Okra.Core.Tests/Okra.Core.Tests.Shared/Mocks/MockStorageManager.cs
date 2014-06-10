using Okra.Services;
using Okra.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Okra.Tests.Mocks
{
    public class MockStorageManager : IStorageManager
    {
        // *** Fields ***

        private Dictionary<string, byte[]> storageDictionary = new Dictionary<string, byte[]>();

        // *** IStorageManager Methods ***

        public async Task<T> RetrieveAsync<T>(StorageFile file)
        {
            await Task.Yield();

            if (storageDictionary.ContainsKey(file.Path))
                return SerializationHelper.DeserializeFromArray<T>(storageDictionary[file.Path]);
            else
                return default(T);
        }

        public async Task<T> RetrieveAsync<T>(StorageFolder folder, string name)
        {
            await Task.Yield();

            if (storageDictionary.ContainsKey(folder.Path + @"\" + name))
                return SerializationHelper.DeserializeFromArray<T>(storageDictionary[folder.Path + @"\" + name]);
            else
                return default(T);
        }

        public async Task StoreAsync<T>(StorageFile file, T value)
        {
            await Task.Yield();
            byte[] data = SerializationHelper.SerializeToArray(value);
            storageDictionary[file.Path] = data;
        }

        public async Task StoreAsync<T>(StorageFolder folder, string name, T value)
        {
            await Task.Yield();
            byte[] data = SerializationHelper.SerializeToArray(value);
            storageDictionary[folder.Path + @"\" + name] = data;
        }
    }
}

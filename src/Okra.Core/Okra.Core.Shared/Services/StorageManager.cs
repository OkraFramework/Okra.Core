using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Okra.Helpers;
using Windows.Storage;

namespace Okra.Services
{
    public class StorageManager : IStorageManager
    {
        // *** Methods ***

        public Task<T> RetrieveAsync<T>(StorageFile file)
        {
            // Validate parameters

            if (file == null)
                throw new ArgumentNullException("file");

            // Call the async method

            return RetrieveAsyncInternal<T>(file);
        }

        private async Task<T> RetrieveAsyncInternal<T>(StorageFile file)
        {
            // Open the file from the file stream

            using (Stream fileStream = await file.OpenStreamForReadAsync())
            {
                // Copy the file to a MemoryStream (as we can do this async)

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await fileStream.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    return (T)serializer.ReadObject(memoryStream);
                }
            }
        }

        public Task<T> RetrieveAsync<T>(StorageFolder folder, string name)
        {
            // Validate parameters

            if (folder == null)
                throw new ArgumentNullException("folder");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "name");

            // Call the async method

            return RetrieveAsyncInternal<T>(folder, name);
        }

        private async Task<T> RetrieveAsyncInternal<T>(StorageFolder folder, string name)
        {
            // Open the file, if it doesn't exist then return default, otherwise pass on
            // NB : As of Windows 8.1 we can ask for a 'null' when a file is not present, however this is not available in WinPRT

#if WINDOWS_APP || WINDOWS_UAP
            StorageFile file = await folder.TryGetItemAsync(name) as StorageFile;

            if (file != null)
                return await RetrieveAsync<T>(file);
            else
                return default(T);
#elif WINDOWS_PHONE_APP
            try
            {
                StorageFile file = await folder.GetFileAsync(name);
                return await RetrieveAsync<T>(file);
            }
            catch (FileNotFoundException)
            {
                return default(T);
            }
#endif
        }

        public Task StoreAsync<T>(StorageFile file, T value)
        {
            // Validate parameters

            if (file == null)
                throw new ArgumentNullException("file");

            // Call the async method

            return StoreAsyncInternal<T>(file, value);
        }

        private async Task StoreAsyncInternal<T>(StorageFile file, T value)
        {
            // Write the object to a MemoryStream using the DataContractSerializer
            // NB: Do this so that,
            //        (i)  We store the state of the object at this point in case it changes before we open the file
            //        (ii) DataContractSerializer doesn't provide async methods for writing to storage
            // TODO : Alternatively we could perform this directly on the file stream and call 'await fileStream.FlushAsync()' (will this ever block?)

            using (MemoryStream dataStream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(dataStream, value);

                // Save the data to the file stream

                using (Stream fileStream = await file.OpenStreamForWriteAsync())
                {
                    dataStream.Seek(0, SeekOrigin.Begin);
                    await dataStream.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }
            }
        }

        public Task StoreAsync<T>(StorageFolder folder, string name, T value)
        {
            // Validate parameters

            if (folder == null)
                throw new ArgumentNullException("folder");

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "name");

            // Call the async method

            return StoreAsyncInternal<T>(folder, name, value);
        }

        public async Task StoreAsyncInternal<T>(StorageFolder folder, string name, T value)
        {
            // Create the new file, overwriting the existing data, then pass on

            StorageFile file = await folder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
            await StoreAsync<T>(file, value);
        }
    }
}

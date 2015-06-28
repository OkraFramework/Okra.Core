using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Okra.Helpers;

#if NETFX_CORE
using Windows.Storage;
#else
using PCLStorage;
#endif

namespace Okra.Services
{
    public class StorageManager : IStorageManager
    {
        // *** Methods ***
#if NETFX_CORE
        public Task<T> RetrieveAsync<T>(StorageFile file)
#else
        public Task<T> RetrieveAsync<T>(IFile file)
#endif
        {
            // Validate parameters

            if (file == null)
                throw new ArgumentNullException(nameof(file));

            // Call the async method

            return RetrieveAsyncInternal<T>(file);
        }

#if NETFX_CORE
        public async Task<T> RetrieveAsyncInternal<T>(StorageFile file)
#else
        public async Task<T> RetrieveAsyncInternal<T>(IFile file)
#endif
        {
            // Open the file from the file stream

#if NETFX_CORE
            using (Stream fileStream = await file.OpenStreamForReadAsync().ConfigureAwait(false))
#else
            using (Stream fileStream = await file.OpenAsync(FileAccess.Read).ConfigureAwait(false))
#endif
            {
                // Copy the file to a MemoryStream (as we can do this async)

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await fileStream.CopyToAsync(memoryStream).ConfigureAwait(false);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    return (T)serializer.ReadObject(memoryStream);
                }
            }
        }

#if NETFX_CORE
        public Task<T> RetrieveAsync<T>(StorageFolder folder, string name)
#else
        public Task<T> RetrieveAsync<T>(IFolder folder, string name)
#endif
        {
            // Validate parameters

            if (folder == null)
                throw new ArgumentNullException(nameof(folder));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), nameof(name));

            // Call the async method

            return RetrieveAsyncInternal<T>(folder, name);
        }

#if NETFX_CORE
        private async Task<T> RetrieveAsyncInternal<T>(StorageFolder folder, string name)
#else
        private async Task<T> RetrieveAsyncInternal<T>(IFolder folder, string name)
#endif
        {
            // Open the file, if it doesn't exist then return default, otherwise pass on
            // NB : As of Windows 8.1 we can ask for a 'null' when a file is not present, however this is not available in WinPRT

#if WINDOWS_APP || WINDOWS_UAP
            StorageFile file = await folder.TryGetItemAsync(name).AsTask().ConfigureAwait(false) as StorageFile;

            if (file != null)
                return await RetrieveAsync<T>(file).ConfigureAwait(false);
            else
                return default(T);
#elif WINDOWS_PHONE_APP
            try
            {
                StorageFile file = await folder.GetFileAsync(name).AsTask().ConfigureAwait(false);
                return await RetrieveAsync<T>(file).ConfigureAwait(false);
            }
            catch (FileNotFoundException)
            {
                return default(T);
            }
#else
            //if (await folder.CheckExistsAsync(name).ConfigureAwait(false) != ExistenceCheckResult.FileExists)
            //    return default(T);

            var file = await folder.GetFileAsync(name).ConfigureAwait(false);
            if (file != null)
                return await RetrieveAsync<T>(file).ConfigureAwait(false);
            else
                return default(T);
#endif
        }

#if NETFX_CORE
        public Task StoreAsync<T>(StorageFile file, T value)
#else
        public Task StoreAsync<T>(IFile file, T value)
#endif
        {
            // Validate parameters

            if (file == null)
                throw new ArgumentNullException(nameof(file));

            // Call the async method

            return StoreAsyncInternal<T>(file, value);
        }

#if NETFX_CORE
        private async Task StoreAsyncInternal<T>(StorageFile file, T value)
#else
        private async Task StoreAsyncInternal<T>(IFile file, T value)
#endif
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

#if NETFX_CORE
                using (Stream fileStream = await file.OpenStreamForWriteAsync().ConfigureAwait(false))
#else
                using (Stream fileStream = await file.OpenAsync(FileAccess.ReadAndWrite).ConfigureAwait(false))
#endif
                {
                    dataStream.Seek(0, SeekOrigin.Begin);
                    await dataStream.CopyToAsync(fileStream).ConfigureAwait(false);
                    await fileStream.FlushAsync().ConfigureAwait(false);
                }
            }
        }

#if NETFX_CORE
        public Task StoreAsync<T>(StorageFolder folder, string name, T value)
#else
        public Task StoreAsync<T>(IFolder folder, string name, T value)
#endif
        {
            // Validate parameters

            if (folder == null)
                throw new ArgumentNullException(nameof(folder));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), nameof(name));

            // Call the async method

            return StoreAsyncInternal<T>(folder, name, value);
        }

#if NETFX_CORE
        public async Task StoreAsyncInternal<T>(StorageFolder folder, string name, T value)
#else
        public async Task StoreAsyncInternal<T>(IFolder folder, string name, T value)
#endif
        {
            // Create the new file, overwriting the existing data, then pass on

            var file = await folder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting).
#if NETFX_CORE
                AsTask().
#endif
                ConfigureAwait(false);
            await StoreAsync<T>(file, value).ConfigureAwait(false);
        }
    }
}

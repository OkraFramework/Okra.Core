using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Okra.Services
{
    public interface IStorageManager
    {
        // *** Methods ***

        Task<T> RetrieveAsync<T>(StorageFile file);
        Task<T> RetrieveAsync<T>(StorageFolder folder, string name);
        Task StoreAsync<T>(StorageFile file, T value);
        Task StoreAsync<T>(StorageFolder folder, string name, T value);
    }
}

using PCLStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Okra.Services
{
    public interface IStorageManager
    {
        // *** Methods ***

        Task<T> RetrieveAsync<T>(IFile file);
        Task<T> RetrieveAsync<T>(IFolder folder, string name);
        Task StoreAsync<T>(IFile file, T value);
        Task StoreAsync<T>(IFolder folder, string name, T value);
    }
}

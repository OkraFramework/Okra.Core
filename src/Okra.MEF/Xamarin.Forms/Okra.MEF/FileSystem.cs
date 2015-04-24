using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Composition;

namespace Okra
{
    /// <summary>
    /// Exports a shared wrapper instance for IFileSystem.
    /// </summary>
    [Shared]
    [Export(typeof(PCLStorage.IFileSystem))]
    internal class FileSystem : PCLStorage.IFileSystem
    {
        public Task<PCLStorage.IFile> GetFileFromPathAsync(string path, CancellationToken cancellationToken)
        {
            return PCLStorage.FileSystem.Current.GetFileFromPathAsync(path, cancellationToken);
        }

        public Task<PCLStorage.IFolder> GetFolderFromPathAsync(string path, CancellationToken cancellationToken)
        {
            return PCLStorage.FileSystem.Current.GetFolderFromPathAsync(path, cancellationToken);
        }

        public PCLStorage.IFolder LocalStorage
        {
            get { return PCLStorage.FileSystem.Current.LocalStorage; }
        }

        public PCLStorage.IFolder RoamingStorage
        {
            get { return PCLStorage.FileSystem.Current.RoamingStorage; }
        }
    }
}

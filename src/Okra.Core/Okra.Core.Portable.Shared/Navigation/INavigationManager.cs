using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    public interface INavigationManager : INavigationBase
    {
        // *** Properties ***

        string HomePageName { get; set; }
        NavigationStorageType NavigationStorageType { get; set; }

#if !NETFX_CORE && !CODE_ANALYSIS
        PCLStorage.IFileSystem FileSystem { get; set; }
#endif
        // *** Methods ***

        Task<bool> RestoreNavigationStack();
    }
}

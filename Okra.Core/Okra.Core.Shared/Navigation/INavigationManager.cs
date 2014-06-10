using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Okra.Navigation
{
    public interface INavigationManager : INavigationBase
    {
        // *** Properties ***

        string HomePageName { get; set; }
        NavigationStorageType NavigationStorageType { get; set; }

        // *** Methods ***

        Task<bool> RestoreNavigationStack();
    }
}

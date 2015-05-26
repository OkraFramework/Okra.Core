using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Okra.Helpers;
using Okra.Services;

#if NETFX_CORE
using Windows.Storage;
#else
using PCLStorage;
#endif

namespace Okra.Navigation
{
    public class NavigationManager : NavigationBase, INavigationManager, ILifetimeAware
    {
        // *** Constants ***

        private const string STORAGE_FILENAME = "Okra_Navigation_NavigationManager.xml";

        // *** Fields ***

        private readonly INavigationTarget _navigationTarget;
        private readonly IStorageManager _storageManager;
#if !NETFX_CORE
        private readonly IFileSystem _fileSystem;
#endif

        private string _homePageName = SpecialPageNames.Home;
        private NavigationStorageType _navigationStorageType = NavigationStorageType.None;

        // *** Constructors ***

        public NavigationManager(INavigationTarget navigationTarget, IViewFactory viewFactory, ILifetimeManager lifetimeManager, IStorageManager storageManager
#if !NETFX_CORE
            , IFileSystem fileSystem
#endif
            )
            : this(navigationTarget, viewFactory, lifetimeManager, storageManager
#if !NETFX_CORE
                , fileSystem
#endif
                , new NavigationStackWithHome())
        {
        }

        protected NavigationManager(INavigationTarget navigationTarget, IViewFactory viewFactory, ILifetimeManager lifetimeManager, IStorageManager storageManager
#if !NETFX_CORE
, IFileSystem fileSystem
#endif
            , INavigationStack navigationStack)
            : base(viewFactory, navigationStack)
        {
            if (lifetimeManager == null)
                throw new ArgumentNullException("lifetimeManager");

            if (storageManager == null)
                throw new ArgumentNullException("storageManager");

            _storageManager = storageManager;

#if !NETFX_CORE
            if (fileSystem == null)
                throw new ArgumentNullException("fileSystem");

            _fileSystem = fileSystem;
#endif

            // Use a default INavigationTarget if not specified

            if (navigationTarget != null)
                _navigationTarget = navigationTarget;
            else
#if NETFX_CORE
                _navigationTarget = new WindowNavigationTarget();
#else
                _navigationTarget = new NavigationViewNavigationTarget();
#endif

            // Register with the LifetimeManager

            lifetimeManager.Register(this);
        }

        // *** Properties ***

        public string HomePageName
        {
            get
            {
                return _homePageName;
            }
            set
            {
                // Validate parameters

                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "HomePageName");

                // Set the property

                _homePageName = value;
            }
        }

        public NavigationStorageType NavigationStorageType
        {
            get
            {
                return _navigationStorageType;
            }
            set
            {
                if (!Enum.IsDefined(typeof(NavigationStorageType), value))
                    throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_SpecifiedEnumIsNotDefined"));

                _navigationStorageType = value;
            }
        }

        // *** Protected Properties ***

        protected INavigationTarget NavigationTarget
        {
            get
            {
                return _navigationTarget;
            }
        }

        // *** Methods ***

        public async Task<bool> RestoreNavigationStack()
        {
            // Retrieve a navigation stack from storage unless,
            //    (1) The NavigationStorageType is 'None'
            //    (2) Cannot find the navigation stack in storage

            NavigationState restoredState = null;

            try
            {
                switch (NavigationStorageType)
                {
                    case Navigation.NavigationStorageType.Local:
#if NETFX_CORE
                        restoredState = await _storageManager.RetrieveAsync<NavigationState>(ApplicationData.Current.LocalFolder, STORAGE_FILENAME);
#else
                        restoredState = await _storageManager.RetrieveAsync<NavigationState>(_fileSystem.LocalStorage, STORAGE_FILENAME);//.ConfigureAwait(false);
#endif
                        break;
                    case Navigation.NavigationStorageType.Roaming:
#if NETFX_CORE
                        restoredState = await _storageManager.RetrieveAsync<NavigationState>(ApplicationData.Current.RoamingFolder, STORAGE_FILENAME);
#else
                        restoredState = await _storageManager.RetrieveAsync<NavigationState>(_fileSystem.RoamingStorage, STORAGE_FILENAME);//.ConfigureAwait(false);
#endif                        
                        break;
                }
            }
            catch (SerializationException)
            {
            }

            // If a navigation stack is available, then restore this

            if (restoredState != null)
            {
                RestoreState(restoredState);
                //await BeginInvokeOnMainThreadAsync(() => RestoreState(restoredState)).ConfigureAwait(false);

                // Return true to signal success

                return true;
            }

            // Otherwise navigate to the home page and return false

            else
            {
                this.NavigateTo(HomePageName);
                //await BeginInvokeOnMainThreadAsync(() => this.NavigateTo(HomePageName)).ConfigureAwait(false);

                return false;
            }
        }

        //private Task BeginInvokeOnMainThreadAsync(Action action)
        //{
        //    TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
        //    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
        //    {
        //        try
        //        {
        //            action();
        //            tcs.SetResult(null);
        //        }
        //        catch (Exception ex)
        //        {
        //            tcs.SetException(ex);
        //        }
        //    });
        //    return tcs.Task;
        //}

        // *** ILifetimeAware Methods ***

        public Task OnResuming()
        {
            return Task.FromResult(true);
        }

        public async Task OnSuspending()
        {
            // Store the current navigation stack to the relevant place

            switch (NavigationStorageType)
            {
                case Navigation.NavigationStorageType.Local:
#if NETFX_CORE
                    await StoreNavigationStack(ApplicationData.Current.LocalFolder).ConfigureAwait(false);
#else
                    await StoreNavigationStack(_fileSystem.LocalStorage).ConfigureAwait(false);
#endif                    
                    break;
                case Navigation.NavigationStorageType.Roaming:
#if NETFX_CORE
                    await StoreNavigationStack(ApplicationData.Current.RoamingFolder).ConfigureAwait(false);
#else
                    await StoreNavigationStack(_fileSystem.RoamingStorage).ConfigureAwait(false);
#endif
                    break;
            }
        }

        // *** Overriden Base Methods ***

        protected override void DisplayPage(object page)
        {
            // Navigate to the relevant page

            _navigationTarget.NavigateTo(page, this);
        }

        // *** Private Methods ***

#if NETFX_CORE
        private Task StoreNavigationStack(StorageFolder folder)
#else
        private Task StoreNavigationStack(IFolder folder)
#endif
        {
            // Create an object for storage of the navigation state

            NavigationState state = StoreState();

            // Store the state using the IStorageManager

            return _storageManager.StoreAsync(folder, STORAGE_FILENAME, state);
        }
    }
}

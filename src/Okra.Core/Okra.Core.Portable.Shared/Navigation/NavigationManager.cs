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

        private readonly INavigationTarget navigationTarget;
        private readonly IStorageManager storageManager;

        private string homePageName = SpecialPageNames.Home;
        private NavigationStorageType navigationStorageType = NavigationStorageType.None;

        // *** Constructors ***

        public NavigationManager(INavigationTarget navigationTarget, IViewFactory viewFactory, ILifetimeManager lifetimeManager, IStorageManager storageManager)
            : this(navigationTarget, viewFactory, lifetimeManager, storageManager, new NavigationStackWithHome())
        {
        }

        protected NavigationManager(INavigationTarget navigationTarget, IViewFactory viewFactory, ILifetimeManager lifetimeManager, IStorageManager storageManager, INavigationStack navigationStack)
            : base(viewFactory, navigationStack)
        {
            if (lifetimeManager == null)
                throw new ArgumentNullException("lifetimeManager");

            if (storageManager == null)
                throw new ArgumentNullException("storageManager");

            this.storageManager = storageManager;

            // Use a default INavigationTarget if not specified

            if (navigationTarget != null)
                this.navigationTarget = navigationTarget;
            else
#if NETFX_CORE
                this.navigationTarget = new WindowNavigationTarget();
#else
                this.navigationTarget = new NavigationViewNavigationTarget();
#endif

            // Register with the LifetimeManager

            lifetimeManager.Register(this);
        }

        // *** Imported Properties ***

#if !NETFX_CORE
        public IFileSystem FileSystem { get; set; }
#endif   

        // *** Properties ***

        public string HomePageName
        {
            get
            {
                return homePageName;
            }
            set
            {
                // Validate parameters

                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "HomePageName");

                // Set the property

                homePageName = value;
            }
        }

        public NavigationStorageType NavigationStorageType
        {
            get
            {
                return navigationStorageType;
            }
            set
            {
                if (!Enum.IsDefined(typeof(NavigationStorageType), value))
                    throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_SpecifiedEnumIsNotDefined"));

                navigationStorageType = value;
            }
        }

        // *** Protected Properties ***

        protected INavigationTarget NavigationTarget
        {
            get
            {
                return navigationTarget;
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
                        restoredState = await storageManager.RetrieveAsync<NavigationState>(ApplicationData.Current.LocalFolder, STORAGE_FILENAME);
#else
                        restoredState = await storageManager.RetrieveAsync<NavigationState>(FileSystem.LocalStorage, STORAGE_FILENAME);//.ConfigureAwait(false);
#endif
                        break;
                    case Navigation.NavigationStorageType.Roaming:
#if NETFX_CORE
                        restoredState = await storageManager.RetrieveAsync<NavigationState>(ApplicationData.Current.RoamingFolder, STORAGE_FILENAME);
#else
                        restoredState = await storageManager.RetrieveAsync<NavigationState>(FileSystem.RoamingStorage, STORAGE_FILENAME);//.ConfigureAwait(false);
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
                    await StoreNavigationStack(FileSystem.LocalStorage).ConfigureAwait(false);
#endif                    
                    break;
                case Navigation.NavigationStorageType.Roaming:
#if NETFX_CORE
                    await StoreNavigationStack(ApplicationData.Current.RoamingFolder).ConfigureAwait(false);
#else
                    await StoreNavigationStack(FileSystem.RoamingStorage).ConfigureAwait(false);
#endif
                    break;
            }
        }

        // *** Overriden Base Methods ***

        protected override void DisplayPage(object page)
        {
            // Navigate to the relevant page

            navigationTarget.NavigateTo(page, this);
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

            return storageManager.StoreAsync(folder, STORAGE_FILENAME, state);
        }
    }
}

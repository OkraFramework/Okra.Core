using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Okra.Helpers;
using Okra.Services;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using System.Reflection;
using Windows.UI.Xaml.Navigation;

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
            this.storageManager = storageManager;

            // Use a default INavigationTarget if not specified

            if (navigationTarget != null)
                this.navigationTarget = navigationTarget;
            else
                this.navigationTarget = new WindowNavigationTarget();

            // Register with the LifetimeManager

            lifetimeManager.Register(this);
        }

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
                        restoredState = await storageManager.RetrieveAsync<NavigationState>(ApplicationData.Current.LocalFolder, STORAGE_FILENAME);
                        break;
                    case Navigation.NavigationStorageType.Roaming:
                        restoredState = await storageManager.RetrieveAsync<NavigationState>(ApplicationData.Current.RoamingFolder, STORAGE_FILENAME);
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

                // Return true to signal success

                return true;
            }

            // Otherwise navigate to the home page and return false

            else
            {
                this.NavigateTo(HomePageName);

                return false;
            }
        }

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
                    await StoreNavigationStack(ApplicationData.Current.LocalFolder);
                    break;
                case Navigation.NavigationStorageType.Roaming:
                    await StoreNavigationStack(ApplicationData.Current.RoamingFolder);
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

        private Task StoreNavigationStack(StorageFolder folder)
        {
            // Create an object for storage of the navigation state

            NavigationState state = StoreState();
            
            // Store the state using the IStorageManager

            return storageManager.StoreAsync(folder, STORAGE_FILENAME, state);
        }
    }
}

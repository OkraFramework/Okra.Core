using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Okra.Navigation;
using Windows.UI.Xaml;
using Okra.Services;
using Windows.ApplicationModel;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.ApplicationModel.Activation;
using Okra.Tests.Helpers;
using System.Runtime.Serialization;
using Windows.UI.Xaml.Navigation;
using Okra.Tests.Mocks;

namespace Okra.Tests.Navigation
{
    [TestClass]
    public class NavigationManagerFixture
    {
        // *** Constructor Tests ***

        [TestMethod]
        public void Constructor_Exception_NullViewFactory()
        {
            INavigationTarget navigationTarget = new MockNavigationTarget();
            IViewFactory viewFactory = null;
            ILifetimeManager lifetimeManager = new MockLifetimeManager();
            IStorageManager storageManager = new MockStorageManager();

            Assert.ThrowsException<ArgumentNullException>(() => new NavigationManager(navigationTarget, viewFactory, lifetimeManager, storageManager));
        }

        [TestMethod]
        public void Constructor_Exception_NullLifetimeManager()
        {
            INavigationTarget navigationTarget = new MockNavigationTarget();
            IViewFactory viewFactory = MockViewFactory.WithPageAndViewModel;
            ILifetimeManager lifetimeManager = null;
            IStorageManager storageManager = new MockStorageManager();

            Assert.ThrowsException<ArgumentNullException>(() => new NavigationManager(navigationTarget, viewFactory, lifetimeManager, storageManager));
        }

        [TestMethod]
        public void Constructor_Exception_NullStorageManager()
        {
            INavigationTarget navigationTarget = new MockNavigationTarget();
            IViewFactory viewFactory = MockViewFactory.WithPageAndViewModel;
            ILifetimeManager lifetimeManager = new MockLifetimeManager();
            IStorageManager storageManager = null;

            Assert.ThrowsException<ArgumentNullException>(() => new NavigationManager(navigationTarget, viewFactory, lifetimeManager, storageManager));
        }

        // *** Property Tests ***

        [TestMethod]
        public void HomePageName_DefaultsToSpecialPageName()
        {
            INavigationManager navigationManager = CreateNavigationManager();

            Assert.AreEqual(SpecialPageNames.Home, navigationManager.HomePageName);
        }

        [TestMethod]
        public void HomePage_CanSetValue()
        {
            INavigationManager navigationManager = CreateNavigationManager();

            navigationManager.HomePageName = "Test Home Page";

            Assert.AreEqual("Test Home Page", navigationManager.HomePageName);
        }

        [TestMethod]
        public void HomePage_ThrowsException_IfHomePageNameIsNull()
        {
            INavigationManager navigationManager = CreateNavigationManager();

            Assert.ThrowsException<ArgumentException>(() => navigationManager.HomePageName = null);
        }

        [TestMethod]
        public void HomePage_ThrowsException_IfHomePageNameIsEmpty()
        {
            INavigationManager navigationManager = CreateNavigationManager();

            Assert.ThrowsException<ArgumentException>(() => navigationManager.HomePageName = "");
        }

        [TestMethod]
        public void NavigationStack_DefaultsToNavigationStackWithHome()
        {
            INavigationBase navigationManager = new NavigationManager(new MockNavigationTarget(), MockViewFactory.WithPageAndViewModel, new MockLifetimeManager(), new MockStorageManager());

            Assert.AreEqual(typeof(NavigationStackWithHome), navigationManager.NavigationStack.GetType());
        }

        [TestMethod]
        public void NavigationStorageType_DefaultsToNone()
        {
            INavigationManager navigationManager = CreateNavigationManager();

            Assert.AreEqual(NavigationStorageType.None, navigationManager.NavigationStorageType);
        }

        [TestMethod]
        public void NavigationStorageType_CanSetValue()
        {
            INavigationManager navigationManager = CreateNavigationManager();

            navigationManager.NavigationStorageType = NavigationStorageType.Local;

            Assert.AreEqual(NavigationStorageType.Local, navigationManager.NavigationStorageType);
        }

        [TestMethod]
        public void NavigationStorageType_Exception_InvalidEnum()
        {
            INavigationManager navigationManager = CreateNavigationManager();

            Assert.ThrowsException<ArgumentException>(() => navigationManager.NavigationStorageType = (NavigationStorageType)100);
        }

        [TestMethod]
        public void NavigationTarget_SetViaConstructor()
        {
            MockNavigationTarget navigationTarget = new MockNavigationTarget();
            TestableNavigationManager navigationManager = CreateNavigationManager(navigationTarget: navigationTarget);

            Assert.AreEqual(navigationTarget, navigationManager.NavigationTarget);
        }

        [TestMethod]
        public void NavigationTarget_DefaultsToWindowNavigationTarget()
        {
            TestableNavigationManager navigationManager = CreateNavigationManager(navigationTargetIsNull: true);

            Assert.IsInstanceOfType(navigationManager.NavigationTarget, typeof(WindowNavigationTarget));
        }

        // *** Method Tests ***

        [TestMethod]
        public void DisplayPage_PassesPageToNavigationTarget()
        {
            MockNavigationTarget navigationTarget = new MockNavigationTarget();
            TestableNavigationManager navigationManager = CreateNavigationManager(navigationTarget: navigationTarget);

            object page = new object();

            navigationManager.DisplayPage(page);

            CollectionAssert.AreEqual(new object[] { page }, navigationTarget.NavigateToCalls.Select(c=>c.Item1).ToArray());
        }

        [TestMethod]
        public void DisplayPage_PassesNavigationManagerToNavigationTarget()
        {
            MockNavigationTarget navigationTarget = new MockNavigationTarget();
            TestableNavigationManager navigationManager = CreateNavigationManager(navigationTarget: navigationTarget);

            object page = new object();

            navigationManager.DisplayPage(page);

            CollectionAssert.AreEqual(new INavigationBase[] { navigationManager }, navigationTarget.NavigateToCalls.Select(c => c.Item2).ToArray());
        }
                
        [TestMethod]
        public async Task RestoreNavigationStack_NavigatesToHomePageIfNoPreviousNavigationStack()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationManager navigationManager = CreateNavigationManager(navigationStack: navigationStack);
            navigationManager.NavigationStorageType = NavigationStorageType.Local;

            bool success = await navigationManager.RestoreNavigationStack();

            Assert.AreEqual(false, success);
            string[] pageNames = navigationStack.Select(page => page.PageName).ToArray();
            CollectionAssert.AreEqual(new string[] { "Home" }, pageNames);
        }

        [TestMethod]
        public async Task RestoreNavigationStack_NavigatesToHomePageIfNavigationStorageTypeIsNone()
        {
            IStorageManager storageManager = new MockStorageManager();

            // --- First Instance ---

            {
                MockNavigationStack navigationStack = new MockNavigationStack();
                MockLifetimeManager lifetimeManager = new MockLifetimeManager();
                INavigationManager navigationManager = CreateNavigationManager(navigationStack: navigationStack, lifetimeManager: lifetimeManager, storageManager: storageManager);
                navigationManager.NavigationStorageType = NavigationStorageType.None;

                // Navigate to some pages

                navigationStack.Add(new PageInfo("Page 1", null));
                navigationStack.Add(new PageInfo("Page 2", null));

                // Suspend the application

                lifetimeManager.Suspend();
            }

            // --- Second Instance ---

            {
                MockNavigationStack navigationStack = new MockNavigationStack();
                INavigationManager navigationManager = CreateNavigationManager(navigationStack: navigationStack, storageManager: storageManager);
                navigationManager.NavigationStorageType = NavigationStorageType.None;

                // Restore the navigation stack

                bool success = await navigationManager.RestoreNavigationStack();

                // Assert that the current page is restored from storage

                Assert.AreEqual(false, success);
                string[] pageNames = navigationStack.Select(page => page.PageName).ToArray();
                CollectionAssert.AreEqual(new string[] { "Home" }, pageNames);
            }
        }

        [TestMethod]
        public async Task RestoreNavigationStack_RestoresPreviousNavigationStackViaLocalStorage()
        {
            IStorageManager storageManager = new MockStorageManager();

            // --- First Instance ---

            {
                MockNavigationStack navigationStack = new MockNavigationStack();
                MockLifetimeManager lifetimeManager = new MockLifetimeManager();
                INavigationManager navigationManager = CreateNavigationManager(navigationStack: navigationStack, lifetimeManager: lifetimeManager, storageManager: storageManager);
                navigationManager.NavigationStorageType = NavigationStorageType.Local;

                // Navigate to some pages

                navigationStack.Add(new PageInfo("Page 1", null));
                navigationStack.Add(new PageInfo("Page 2", null));

                // Suspend the application

                lifetimeManager.Suspend();
            }

            // --- Second Instance ---

            {
                MockNavigationStack navigationStack = new MockNavigationStack();
                INavigationManager navigationManager = CreateNavigationManager(navigationStack: navigationStack, storageManager: storageManager);
                navigationManager.NavigationStorageType = NavigationStorageType.Local;

                // Restore the navigation stack

                bool success = await navigationManager.RestoreNavigationStack();
                
                // Assert that the current page is restored from storage

                Assert.AreEqual(true, success);
                string[] pageNames = navigationStack.Select(page => page.PageName).ToArray();
                CollectionAssert.AreEqual(new string[] { "Page 1", "Page 2" }, pageNames);
            }
        }

        [TestMethod]
        public async Task RestoreNavigationStack_RestoresPreviousNavigationStackViaRoamingStorage()
        {
            IStorageManager storageManager = new MockStorageManager();

            // --- First Instance ---

            {
                MockNavigationStack navigationStack = new MockNavigationStack();
                MockLifetimeManager lifetimeManager = new MockLifetimeManager();
                INavigationManager navigationManager = CreateNavigationManager(navigationStack: navigationStack, lifetimeManager: lifetimeManager, storageManager: storageManager);
                navigationManager.NavigationStorageType = NavigationStorageType.Roaming;

                // Navigate to some pages

                navigationStack.Add(new PageInfo("Page 1", null));
                navigationStack.Add(new PageInfo("Page 2", null));

                // Suspend the application

                lifetimeManager.Suspend();
            }

            // --- Second Instance ---

            {
                MockNavigationStack navigationStack = new MockNavigationStack();
                INavigationManager navigationManager = CreateNavigationManager(navigationStack: navigationStack, storageManager: storageManager);
                navigationManager.NavigationStorageType = NavigationStorageType.Roaming;

                // Restore the navigation stack

                bool success = await navigationManager.RestoreNavigationStack();

                // Assert that the current page is restored from storage

                Assert.AreEqual(true, success);
                string[] pageNames = navigationStack.Select(page => page.PageName).ToArray();
                CollectionAssert.AreEqual(new string[] { "Page 1", "Page 2" }, pageNames);
            }
        }

        [TestMethod]
        public async Task RestoreNavigationStack_NavigatesToHomePageIfStateFileIsCorrupt()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            MockStorageManager storageManager = new MockStorageManager();
            INavigationManager navigationManager = CreateNavigationManager(navigationStack: navigationStack, storageManager: storageManager);
            navigationManager.NavigationStorageType = NavigationStorageType.Local;

            InvalidNavigationState state = new InvalidNavigationState();
            await storageManager.StoreAsync<InvalidNavigationState>(ApplicationData.Current.LocalFolder, "Okra_Navigation_NavigationManager.xml", state);

            bool success = await navigationManager.RestoreNavigationStack();

            Assert.AreEqual(false, success);
            string[] pageNames = navigationStack.Select(page => page.PageName).ToArray();
            CollectionAssert.AreEqual(new string[] { "Home" }, pageNames);
        }

        // *** Behavior Tests ***

        [TestMethod]
        public void Constructor_RegistersWithLifetimeManager()
        {
            MockLifetimeManager lifetimeManager = new MockLifetimeManager();
            INavigationManager navigationManager = CreateNavigationManager(lifetimeManager: lifetimeManager);

            CollectionAssert.Contains(lifetimeManager.RegisteredServices, navigationManager);
        }

        // *** Private Methods ***

        private TestableNavigationManager CreateNavigationManager(INavigationTarget navigationTarget = null, IViewFactory viewFactory = null, INavigationStack navigationStack = null, ILifetimeManager lifetimeManager = null, IStorageManager storageManager = null, bool navigationTargetIsNull = false)
        {
            if (navigationTarget == null && !navigationTargetIsNull)
                navigationTarget = new MockNavigationTarget();

            if (viewFactory == null)
                viewFactory = MockViewFactory.WithPageAndViewModel;

            if (navigationStack == null)
                navigationStack = new MockNavigationStack();

            if (lifetimeManager == null)
                lifetimeManager = new MockLifetimeManager();

            if (storageManager == null)
                storageManager = new MockStorageManager();

            TestableNavigationManager navigationManager = new TestableNavigationManager(navigationTarget, viewFactory, navigationStack, lifetimeManager, storageManager);

            return navigationManager;
        }

        // *** Private Sub-classes ***

        private class TestableNavigationManager : NavigationManager
        {
            // *** Constructors ***

            public TestableNavigationManager(INavigationTarget navigationTarget, IViewFactory viewFactory, INavigationStack navigationStack, ILifetimeManager lifetimeManager, IStorageManager storageManager)
                : base(navigationTarget, viewFactory, lifetimeManager, storageManager, navigationStack)
            {
            }

            // *** Properties ***

            public new INavigationTarget NavigationTarget
            {
                get
                {
                    return base.NavigationTarget;
                }
            }

            // *** Methods ***

            public new void DisplayPage(object page)
            {
                base.DisplayPage(page);
            }
        }

        [DataContract]
        public class InvalidNavigationState
        {
            // *** Constructors ***

            public InvalidNavigationState()
            {
                SomeProperty = "Some Value";
            }

            // *** Properties ***

            [DataMember]
            public string SomeProperty
            {
                get;
                private set;
            }
        }
    }
}
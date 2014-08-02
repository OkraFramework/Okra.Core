using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Okra.DataTransfer;
using Okra.Navigation;
using Okra.Services;
using Okra.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.UI.Xaml.Navigation;

namespace Okra.Tests.DataTransfer
{
    [TestClass]
    public class ShareTargetManagerFixture
    {
        // *** Constructor Tests ***

        [TestMethod]
        public void Constructor_RegistersWithActivationManager()
        {
            MockActivationManager activationManager = new MockActivationManager();
            ShareTargetManager shareTargetManager = CreateShareTargetManager(activationManager: activationManager);

            CollectionAssert.Contains(activationManager.RegisteredServices, shareTargetManager);
        }

        // *** Property Tests ***

        [TestMethod]
        public void ShareTargetPageName_IsInitiallySpecialPageName()
        {
            ShareTargetManager shareTargetManager = CreateShareTargetManager(setShareTargetPageName: false);

            Assert.AreEqual(SpecialPageNames.ShareTarget, shareTargetManager.ShareTargetPageName);
        }

        [TestMethod]
        public void ShareTargetPageName_CanSetValue()
        {
            ShareTargetManager shareTargetManager = CreateShareTargetManager();

            shareTargetManager.ShareTargetPageName = "MyShareTargetPage";

            Assert.AreEqual("MyShareTargetPage", shareTargetManager.ShareTargetPageName);
        }

        [TestMethod]
        public void ShareTargetPageName_Exception_CannotSetToNull()
        {
            ShareTargetManager shareTargetManager = CreateShareTargetManager();

            Assert.ThrowsException<ArgumentException>(() => shareTargetManager.ShareTargetPageName = null);
        }

        [TestMethod]
        public void ShareTargetPageName_Exception_CannotSetToEmptyString()
        {
            ShareTargetManager shareTargetManager = CreateShareTargetManager();

            Assert.ThrowsException<ArgumentException>(() => shareTargetManager.ShareTargetPageName = "");
        }

        // *** Method Tests ***

        [TestMethod]
        public async Task Activate_ReturnsTrueIfActivationKindIsShareTarget()
        {
            ShareTargetManager shareTargetManager = CreateShareTargetManager();

            // Activate the application

            bool result = await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            // Check the result

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public async Task Activate_ReturnsFalseIfActivationKindIsLaunch()
        {
            ShareTargetManager shareTargetManager = CreateShareTargetManager();

            // Activate the application

            bool result = await shareTargetManager.Activate(new MockActivatedEventArgs() { Kind = ActivationKind.Launch });

            // Check the result

            Assert.AreEqual(false, result);
        }
        
        [TestMethod]
        public async Task Activate_DisplaysThePage_WithCorrectName()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            string[] navigatedPageNames = shareTargetManager.DisplayedViews
                                                .Select(viewLifetimeContext => viewLifetimeContext.PageName).ToArray();

            CollectionAssert.AreEqual(new string[] { "ShareTarget" }, navigatedPageNames);
        }

        [TestMethod]
        public async Task Activate_DisplaysThePage_WithNullNavigationContext()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            INavigationContext[] navigatedNavigationContexts = shareTargetManager.DisplayedViews
                                                .Select(viewLifetimeContext => viewLifetimeContext.NavigationContext).ToArray();

            CollectionAssert.AreEqual(new INavigationContext[] { null }, navigatedNavigationContexts);
        }

        [TestMethod]
        public async Task Activate_DoesNotNavigateIfActivationKindIsLaunch()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();

            await shareTargetManager.Activate(new MockActivatedEventArgs() { Kind = ActivationKind.Launch });

            Assert.AreEqual(0, shareTargetManager.DisplayedViews.Count);
        }
        
        [TestMethod]
        public async Task Activate_CallsNavigatedTo_OnView()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            MockViewLifetimeContext viewLifetimeContext = shareTargetManager.DisplayedViews.First();
            MockPageElement pageView = viewLifetimeContext.View as MockPageElement;
            CollectionAssert.AreEqual(new string[] { "NavigatedTo(New)" }, pageView.NavigationEvents);
        }

        [TestMethod]
        public async Task Activate_CallsNavigatedTo_OnViewModel()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            MockViewLifetimeContext viewLifetimeContext = shareTargetManager.DisplayedViews.First();
            MockPageElement pageViewModel = viewLifetimeContext.ViewModel as MockPageElement;
            CollectionAssert.AreEqual(new string[] { "NavigatedTo(New)" }, pageViewModel.NavigationEvents);
        }

        [TestMethod]
        public async Task Activate_CallsActivate_OnView()
        {
            IViewFactory viewFactory = new MockShareTargetViewFactory();
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager(viewFactory: viewFactory);
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            MockViewLifetimeContext viewLifetimeContext = shareTargetManager.DisplayedViews.First();
            MockShareTargetPageElement pageView = viewLifetimeContext.View as MockShareTargetPageElement;
            Assert.AreEqual(1, pageView.ActivateEvents.Count);
        }

        [TestMethod]
        public async Task Activate_CallsActivate_OnViewModel()
        {
            IViewFactory viewFactory = new MockShareTargetViewFactory();
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager(viewFactory: viewFactory);
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            MockViewLifetimeContext viewLifetimeContext = shareTargetManager.DisplayedViews.First();
            MockShareTargetPageElement pageViewModel = viewLifetimeContext.ViewModel as MockShareTargetPageElement;
            Assert.AreEqual(1, pageViewModel.ActivateEvents.Count);
        }

        [TestMethod]
        public async Task Activate_CallsActivateWithShareOperation_OnView()
        {
            IViewFactory viewFactory = new MockShareTargetViewFactory();
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager(viewFactory: viewFactory);
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            IShareTargetActivatedEventArgs activatedEventArgs = new MockShareTargetActivatedEventArgs();
            await shareTargetManager.Activate(activatedEventArgs);

            MockViewLifetimeContext viewLifetimeContext = shareTargetManager.DisplayedViews.First();
            MockShareTargetPageElement pageView = viewLifetimeContext.View as MockShareTargetPageElement;
            Assert.IsInstanceOfType(pageView.ActivateEvents[0], typeof(MockShareOperation));
            Assert.AreEqual(activatedEventArgs, ((MockShareOperation)pageView.ActivateEvents[0]).ActivatedEventArgs);
        }

        [TestMethod]
        public async Task Activate_CallsActivateWithShareOperation_OnViewModel()
        {
            IViewFactory viewFactory = new MockShareTargetViewFactory();
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager(viewFactory: viewFactory);
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            IShareTargetActivatedEventArgs activatedEventArgs = new MockShareTargetActivatedEventArgs();
            await shareTargetManager.Activate(activatedEventArgs);

            MockViewLifetimeContext viewLifetimeContext = shareTargetManager.DisplayedViews.First();
            MockShareTargetPageElement pageViewModel = viewLifetimeContext.ViewModel as MockShareTargetPageElement;
            Assert.IsInstanceOfType(pageViewModel.ActivateEvents[0], typeof(MockShareOperation));
            Assert.AreEqual(activatedEventArgs, ((MockShareOperation)pageViewModel.ActivateEvents[0]).ActivatedEventArgs);
        }

        // *** Behaviour Tests ***

        [TestMethod]
        public async Task ClosingWindow_DisposesPage()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            MockViewLifetimeContext viewLifetimeContext = shareTargetManager.DisplayedViews.First();
            shareTargetManager.OnWindowClosing(viewLifetimeContext);

            Assert.AreEqual(true, viewLifetimeContext.IsDisposed);
        }

        [TestMethod]
        public async Task ClosingWindow_CallsNavigatingFrom_OnView()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            MockViewLifetimeContext viewLifetimeContext = shareTargetManager.DisplayedViews.First();
            MockPageElement pageView = viewLifetimeContext.View as MockPageElement;
            pageView.NavigationEvents.Clear();

            shareTargetManager.OnWindowClosing(viewLifetimeContext);

            CollectionAssert.AreEqual(new string[] { "NavigatingFrom(Back)" }, pageView.NavigationEvents);
        }

        [TestMethod]
        public async Task ClosingWindow_CallsNavigatingFrom_OnViewModel()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            MockViewLifetimeContext viewLifetimeContext = shareTargetManager.DisplayedViews.First();
            MockPageElement pageViewModel = viewLifetimeContext.ViewModel as MockPageElement;
            pageViewModel.NavigationEvents.Clear();

            shareTargetManager.OnWindowClosing(viewLifetimeContext);


            CollectionAssert.AreEqual(new string[] { "NavigatingFrom(Back)" }, pageViewModel.NavigationEvents);
        }

        // *** Private Methods ***

        private TestableShareTargetManager CreateShareTargetManager(MockActivationManager activationManager = null, IViewFactory viewFactory = null, bool setShareTargetPageName = true)
        {
            if (activationManager == null)
                activationManager = new MockActivationManager();

            if (viewFactory == null)
                viewFactory = new MockViewFactory();

            TestableShareTargetManager shareTargetManager = new TestableShareTargetManager(activationManager, viewFactory);

            if (setShareTargetPageName)
                shareTargetManager.ShareTargetPageName = "ShareTarget";

            return shareTargetManager;
        }

        // *** Private sub-classes ***

        private class TestableShareTargetManager : ShareTargetManager
        {
            // *** Fields ***

            public readonly List<MockViewLifetimeContext> DisplayedViews = new List<MockViewLifetimeContext>();

            // *** Constructors ***

            public TestableShareTargetManager(IActivationManager activationManager, IViewFactory viewFactory)
                : base(activationManager, viewFactory)
            {
            }

            // *** Methods ***

            public new void OnWindowClosing(IViewLifetimeContext viewLifetimeContext)
            {
                base.OnWindowClosing(viewLifetimeContext);
            }

            // *** Overriden base methods ***

            protected override void DisplayPage(IViewLifetimeContext viewLifetimeContext)
            {
                DisplayedViews.Add(viewLifetimeContext as MockViewLifetimeContext);
            }

            protected override IShareOperation WrapShareOperation(IShareTargetActivatedEventArgs shareTargetEventArgs)
            {
                return new MockShareOperation(shareTargetEventArgs);
            }
        }

        private class MockViewFactory : IViewFactory
        {
            // *** Methods ***

            public IViewLifetimeContext CreateView(string name, INavigationContext context)
            {
                switch (name)
                {
                    case "ShareTarget":
                        return new MockViewLifetimeContext("ShareTarget", "ViewModel", context);
                    default:
                        throw new InvalidOperationException();
                }
            }

            public bool IsViewDefined(string name)
            {
                return new string[] { "Page 1", "Page 2" }.Contains(name);
            }
        }

        private class MockShareTargetViewFactory : IViewFactory
        {
            // *** Methods ***

            public IViewLifetimeContext CreateView(string name, INavigationContext context)
            {
                switch (name)
                {
                    case "ShareTarget":
                        return new MockViewLifetimeContext("ShareTarget", "ViewModel", context, pageType: typeof(MockShareTargetPageElement), viewModelType: typeof(MockShareTargetPageElement));
                    default:
                        throw new InvalidOperationException();
                }
            }

            public bool IsViewDefined(string name)
            {
                return new string[] { "Page 1", "Page 2" }.Contains(name);
            }
        }

        private class MockViewLifetimeContext : IViewLifetimeContext
        {
            // *** Constructors ***

            public MockViewLifetimeContext(string pageName, string viewModelName, INavigationContext navigationContext, Type pageType = null, Type viewModelType = null)
            {
                this.PageName = pageName;
                this.NavigationContext = navigationContext;

                if (pageName != null)
                    View = Activator.CreateInstance(pageType ?? typeof(MockPageElement));

                if (viewModelName != null)
                    ViewModel = Activator.CreateInstance(viewModelType ?? typeof(MockPageElement));
            }

            // *** Properties ***

            public object View { get; set; }
            public object ViewModel { get; set; }

            public string PageName { get; set; }
            public INavigationContext NavigationContext { get; set; }
            public bool IsDisposed { get; set; }

            // *** Methods ***

            public void Dispose()
            {
                this.IsDisposed = true;
            }
        }

        private class MockPageElement : INavigationAware
        {
            // *** Fields ***

            public List<string> NavigationEvents = new List<string>();

            // *** Methods ***

            public void NavigatedTo(PageNavigationMode navigationMode)
            {
                NavigationEvents.Add(string.Format("NavigatedTo({0})", navigationMode));
            }

            public void NavigatingFrom(PageNavigationMode navigationMode)
            {
                NavigationEvents.Add(string.Format("NavigatingFrom({0})", navigationMode));
            }
        }

        private class MockShareTargetPageElement : IShareTarget
        {
            // *** Fields ***

            public List<IShareOperation> ActivateEvents = new List<IShareOperation>();

            // *** Methods ***

            public void Activate(IShareOperation shareOperation)
            {
                ActivateEvents.Add(shareOperation);
            }
        }

        private class MockShareTargetActivatedEventArgs : MockActivatedEventArgs, IShareTargetActivatedEventArgs
        {
            // *** Constructors ***

            public MockShareTargetActivatedEventArgs()
            {
                base.Kind = ActivationKind.ShareTarget;
                base.PreviousExecutionState = ApplicationExecutionState.Terminated;
            }

            // *** Propertes ***

            public ShareOperation ShareOperation
            {
                get;
                set;
            }
        }

        private class MockShareOperation : IShareOperation
        {
            // *** Fields ***

            public IShareTargetActivatedEventArgs ActivatedEventArgs;

            // *** Constructors ***

            public MockShareOperation(IShareTargetActivatedEventArgs activatedEventArgs)
            {
                this.ActivatedEventArgs = activatedEventArgs;
            }

            // *** Properties ***

            public DataPackageView Data
            {
                get { throw new NotImplementedException(); }
            }

            public string QuickLinkId
            {
                get { throw new NotImplementedException(); }
            }

            // *** Methods ***

            public void RemoveThisQuickLink()
            {
                throw new NotImplementedException();
            }

            public void ReportCompleted()
            {
                throw new NotImplementedException();
            }

            public void ReportCompleted(QuickLink quicklink)
            {
                throw new NotImplementedException();
            }

            public void ReportDataRetrieved()
            {
                throw new NotImplementedException();
            }

            public void ReportError(string value)
            {
                throw new NotImplementedException();
            }

            public void ReportStarted()
            {
                throw new NotImplementedException();
            }

            public void ReportSubmittedBackgroundTask()
            {
                throw new NotImplementedException();
            }
        }
    }
}

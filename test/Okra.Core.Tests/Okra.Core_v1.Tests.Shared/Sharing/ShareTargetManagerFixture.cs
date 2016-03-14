using Okra.Sharing;
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
using Xunit;

namespace Okra.Tests.Sharing
{
    public class ShareTargetManagerFixture
    {
        // *** Constructor Tests ***

        [Fact]
        public void Constructor_RegistersWithActivationManager()
        {
            MockActivationManager activationManager = new MockActivationManager();
            ShareTargetManager shareTargetManager = CreateShareTargetManager(activationManager: activationManager);

            Assert.Contains(shareTargetManager, activationManager.RegisteredServices);
        }

        [Fact]
        public void Constructor_ThrowsException_IfActivationManagerIsNull()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new ShareTargetManager(null, new MockViewFactory()));

            Assert.Equal("Value cannot be null.\r\nParameter name: activationManager", e.Message);
            Assert.Equal("activationManager", e.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsException_IfViewFactoryIsNull()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new ShareTargetManager(new MockActivationManager(), null));

            Assert.Equal("Value cannot be null.\r\nParameter name: viewFactory", e.Message);
            Assert.Equal("viewFactory", e.ParamName);
        }

        // *** Property Tests ***

        [Fact]
        public void ShareTargetPageName_IsInitiallySpecialPageName()
        {
            ShareTargetManager shareTargetManager = CreateShareTargetManager(setShareTargetPageName: false);

            Assert.Equal(SpecialPageNames.ShareTarget, shareTargetManager.ShareTargetPageName);
        }

        [Fact]
        public void ShareTargetPageName_CanSetValue()
        {
            ShareTargetManager shareTargetManager = CreateShareTargetManager();

            shareTargetManager.ShareTargetPageName = "MyShareTargetPage";

            Assert.Equal("MyShareTargetPage", shareTargetManager.ShareTargetPageName);
        }

        [Fact]
        public void ShareTargetPageName_Exception_CannotSetToNull()
        {
            ShareTargetManager shareTargetManager = CreateShareTargetManager();

            var e = Assert.Throws<ArgumentException>(() => shareTargetManager.ShareTargetPageName = null);

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: ShareTargetPageName", e.Message);
            Assert.Equal("ShareTargetPageName", e.ParamName);
        }

        [Fact]
        public void ShareTargetPageName_Exception_CannotSetToEmptyString()
        {
            ShareTargetManager shareTargetManager = CreateShareTargetManager();

            var e = Assert.Throws<ArgumentException>(() => shareTargetManager.ShareTargetPageName = "");

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: ShareTargetPageName", e.Message);
            Assert.Equal("ShareTargetPageName", e.ParamName);
        }

        // *** Method Tests ***

        [Fact]
        public async Task Activate_ReturnsTrueIfActivationKindIsShareTarget()
        {
            ShareTargetManager shareTargetManager = CreateShareTargetManager();

            // Activate the application

            bool result = await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            // Check the result

            Assert.Equal(true, result);
        }

        [Fact]
        public async Task Activate_ReturnsFalseIfActivationKindIsLaunch()
        {
            ShareTargetManager shareTargetManager = CreateShareTargetManager();

            // Activate the application

            bool result = await shareTargetManager.Activate(new MockActivatedEventArgs() { Kind = ActivationKind.Launch });

            // Check the result

            Assert.Equal(false, result);
        }

        [Fact]
        public async Task Activate_DisplaysThePage_WithCorrectName()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            string[] navigatedPageNames = shareTargetManager.DisplayedViews
                                                .Select(viewLifetimeContext => viewLifetimeContext.PageName).ToArray();

            Assert.Equal(new string[] { "ShareTarget" }, navigatedPageNames);
        }

        [Fact]
        public async Task Activate_DisplaysThePage_WithNullReturningNavigationContext()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            INavigationBase[] navigatedNavigationContexts = shareTargetManager.DisplayedViews
                                                .Select(viewLifetimeContext => viewLifetimeContext.NavigationContext.GetCurrent()).ToArray();

            Assert.Equal(new INavigationBase[] { null }, navigatedNavigationContexts);
        }

        [Fact]
        public async Task Activate_DoesNotNavigateIfActivationKindIsLaunch()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();

            await shareTargetManager.Activate(new MockActivatedEventArgs() { Kind = ActivationKind.Launch });

            Assert.Equal(0, shareTargetManager.DisplayedViews.Count);
        }

        [Fact]
        public async Task Activate_CallsNavigatedTo_OnView()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            MockViewLifetimeContext viewLifetimeContext = shareTargetManager.DisplayedViews.First();
            MockPageElement pageView = viewLifetimeContext.View as MockPageElement;
            Assert.Equal(new string[] { "NavigatedTo(New)" }, pageView.NavigationEvents);
        }

        [Fact]
        public async Task Activate_CallsNavigatedTo_OnViewModel()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            MockViewLifetimeContext viewLifetimeContext = shareTargetManager.DisplayedViews.First();
            MockPageElement pageViewModel = viewLifetimeContext.ViewModel as MockPageElement;
            Assert.Equal(new string[] { "NavigatedTo(New)" }, pageViewModel.NavigationEvents);
        }

        [Fact]
        public async Task Activate_CallsActivate_OnView()
        {
            IViewFactory viewFactory = new MockShareTargetViewFactory();
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager(viewFactory: viewFactory);
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            MockViewLifetimeContext viewLifetimeContext = shareTargetManager.DisplayedViews.First();
            MockShareTargetPageElement pageView = viewLifetimeContext.View as MockShareTargetPageElement;
            Assert.Equal(1, pageView.ActivateEvents.Count);
        }

        [Fact]
        public async Task Activate_CallsActivate_OnViewModel()
        {
            IViewFactory viewFactory = new MockShareTargetViewFactory();
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager(viewFactory: viewFactory);
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            MockViewLifetimeContext viewLifetimeContext = shareTargetManager.DisplayedViews.First();
            MockShareTargetPageElement pageViewModel = viewLifetimeContext.ViewModel as MockShareTargetPageElement;
            Assert.Equal(1, pageViewModel.ActivateEvents.Count);
        }

        [Fact]
        public async Task Activate_CallsActivateWithShareOperation_OnView()
        {
            IViewFactory viewFactory = new MockShareTargetViewFactory();
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager(viewFactory: viewFactory);
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            IShareTargetActivatedEventArgs activatedEventArgs = new MockShareTargetActivatedEventArgs();
            await shareTargetManager.Activate(activatedEventArgs);

            MockViewLifetimeContext viewLifetimeContext = shareTargetManager.DisplayedViews.First();
            MockShareTargetPageElement pageView = viewLifetimeContext.View as MockShareTargetPageElement;
            Assert.IsAssignableFrom(typeof(MockShareOperation), pageView.ActivateEvents[0]);
            Assert.Equal(activatedEventArgs, ((MockShareOperation)pageView.ActivateEvents[0]).ActivatedEventArgs);
        }

        [Fact]
        public async Task Activate_CallsActivateWithShareOperation_OnViewModel()
        {
            IViewFactory viewFactory = new MockShareTargetViewFactory();
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager(viewFactory: viewFactory);
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            IShareTargetActivatedEventArgs activatedEventArgs = new MockShareTargetActivatedEventArgs();
            await shareTargetManager.Activate(activatedEventArgs);

            MockViewLifetimeContext viewLifetimeContext = shareTargetManager.DisplayedViews.First();
            MockShareTargetPageElement pageViewModel = viewLifetimeContext.ViewModel as MockShareTargetPageElement;
            Assert.IsAssignableFrom(typeof(MockShareOperation), pageViewModel.ActivateEvents[0]);
            Assert.Equal(activatedEventArgs, ((MockShareOperation)pageViewModel.ActivateEvents[0]).ActivatedEventArgs);
        }

        [Fact]
        public async void Activate_ThrowsException_IfEventArgsIsNull()
        {
            ShareTargetManager shareTargetManager = CreateShareTargetManager();

            var e = await Assert.ThrowsAsync<ArgumentNullException>(() => shareTargetManager.Activate(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: activatedEventArgs", e.Message);
            Assert.Equal("activatedEventArgs", e.ParamName);
        }

        [Fact]
        public void DisplayPage_ThrowsException_IfViewLifetimeContextIsNull()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();

            var e = Assert.Throws<ArgumentNullException>(() => shareTargetManager.DisplayPageDirect(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: viewLifetimeContext", e.Message);
            Assert.Equal("viewLifetimeContext", e.ParamName);
        }

        [Fact]
        public void OnWindowClosing_ThrowsException_IfViewLifetimeContextIsNull()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();

            var e = Assert.Throws<ArgumentNullException>(() => shareTargetManager.OnWindowClosing(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: viewLifetimeContext", e.Message);
            Assert.Equal("viewLifetimeContext", e.ParamName);
        }

        [Fact]
        public void WrapShareOperation_ThrowsException_IfEventArgsIsNull()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();

            var e = Assert.Throws<ArgumentNullException>(() => shareTargetManager.WrapShareOperationDirect(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: shareTargetEventArgs", e.Message);
            Assert.Equal("shareTargetEventArgs", e.ParamName);
        }

        // *** Behaviour Tests ***

        [Fact]
        public async Task ClosingWindow_DisposesPage()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            MockViewLifetimeContext viewLifetimeContext = shareTargetManager.DisplayedViews.First();
            shareTargetManager.OnWindowClosing(viewLifetimeContext);

            Assert.Equal(true, viewLifetimeContext.IsDisposed);
        }

        [Fact]
        public async Task ClosingWindow_CallsNavigatingFrom_OnView()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            MockViewLifetimeContext viewLifetimeContext = shareTargetManager.DisplayedViews.First();
            MockPageElement pageView = viewLifetimeContext.View as MockPageElement;
            pageView.NavigationEvents.Clear();

            shareTargetManager.OnWindowClosing(viewLifetimeContext);

            Assert.Equal(new string[] { "NavigatingFrom(Back)" }, pageView.NavigationEvents);
        }

        [Fact]
        public async Task ClosingWindow_CallsNavigatingFrom_OnViewModel()
        {
            TestableShareTargetManager shareTargetManager = CreateShareTargetManager();
            shareTargetManager.ShareTargetPageName = "ShareTarget";

            await shareTargetManager.Activate(new MockShareTargetActivatedEventArgs());

            MockViewLifetimeContext viewLifetimeContext = shareTargetManager.DisplayedViews.First();
            MockPageElement pageViewModel = viewLifetimeContext.ViewModel as MockPageElement;
            pageViewModel.NavigationEvents.Clear();

            shareTargetManager.OnWindowClosing(viewLifetimeContext);


            Assert.Equal(new string[] { "NavigatingFrom(Back)" }, pageViewModel.NavigationEvents);
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

            public void DisplayPageDirect(IViewLifetimeContext viewLifetimeContext)
            {
                base.DisplayPage(viewLifetimeContext);
            }

            public new void OnWindowClosing(IViewLifetimeContext viewLifetimeContext)
            {
                base.OnWindowClosing(viewLifetimeContext);
            }

            public void WrapShareOperationDirect(IShareTargetActivatedEventArgs shareTargetEventArgs)
            {
                base.WrapShareOperation(shareTargetEventArgs);
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

            public ISharePackageView Data
            {
                get { throw new NotImplementedException(); }
            }

            // *** Methods ***

            public void ReportCompleted()
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

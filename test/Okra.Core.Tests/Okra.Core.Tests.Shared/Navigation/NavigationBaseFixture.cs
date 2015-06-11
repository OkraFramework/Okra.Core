using Okra.Navigation;
using Okra.Tests.Helpers;
using Okra.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Xunit;

namespace Okra.Tests.Navigation
{
    public class NavigationBaseFixture
    {
        // *** Constructor Tests ***

        [Fact]
        public void Constructor_Exception_NullViewFactory()
        {
            INavigationStack navigationStack = new MockNavigationStack();

            Assert.Throws<ArgumentNullException>(() => new TestableNavigationBase(null, navigationStack));
        }

        [Fact]
        public void Constructor_Exception_NullNavigationStack()
        {
            IViewFactory viewFactory = MockViewFactory.WithPageAndViewModel;

            Assert.Throws<ArgumentNullException>(() => new TestableNavigationBase(viewFactory, null));
        }

        // *** Property Test ***

        [Fact]
        public void NavigationStack_SetByConstructor()
        {
            INavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationBase = new TestableNavigationBase(MockViewFactory.WithPageAndViewModel, navigationStack);

            Assert.Equal(navigationStack, navigationBase.NavigationStack);
        }

        [Fact]
        public void NavigationStack_DefaultsToNavigationStack()
        {
            INavigationBase navigationBase = new TestableNavigationBase(MockViewFactory.WithPageAndViewModel);

            Assert.Equal(typeof(NavigationStack), navigationBase.NavigationStack.GetType());
        }

        // *** NavigationStack Event Tests ***


        [Fact]
        public void WhenNavigationStack_PropertyChanged_CurrentPage_NullPageCallsDisplayPageWithNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            TestableNavigationBase navigationBase = CreateNavigationBase(navigationStack: navigationStack);

            navigationStack.SetCurrentPage(null);

            Assert.Equal(new PageInfo[] { null }, navigationBase.DisplayPageCalls);
        }

        [Fact]
        public void WhenNavigationStack_PropertyChanged_CurrentPage_DisplaysSpecifiedPage_WithViewModel()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            PageInfo pageInfo = new PageInfo("Page 1", null);
            TestableNavigationBase navigationBase = CreateNavigationBase(navigationStack: navigationStack);

            navigationStack.SetCurrentPage(pageInfo);

            string[] pageNames = navigationBase.DisplayPageCalls.Cast<MockPage>().Select(page => page.PageName).ToArray();
            Assert.Equal(new string[] { "Page 1" }, pageNames);
        }

        [Fact]
        public void WhenNavigationStack_PropertyChanged_CurrentPage_DisplaysSpecifiedPage_WithoutViewModel()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            IViewFactory viewFactory = MockViewFactory.WithPageOnly;
            PageInfo pageInfo = new PageInfo("Page 1", null);
            TestableNavigationBase navigationBase = CreateNavigationBase(viewFactory: viewFactory, navigationStack: navigationStack);

            navigationStack.SetCurrentPage(pageInfo);

            string[] pageNames = navigationBase.DisplayPageCalls.Cast<MockPage>().Select(page => page.PageName).ToArray();
            Assert.Equal(new string[] { "Page 1" }, pageNames);
        }

        [Fact]
        public void WhenNavigationStack_PropertyChanged_CurrentPage_ActivatesViewModel()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            IViewFactory viewFactory = MockViewFactory.WithActivatable;
            PageInfo pageInfo = new PageInfo("Page 1", null);
            TestableNavigationBase navigationBase = CreateNavigationBase(viewFactory: viewFactory, navigationStack: navigationStack);

            navigationStack.SetCurrentPage(pageInfo);

            MockPage currentPage = (MockPage)navigationBase.DisplayPageCalls.Last();
            MockViewModel_Activatable currentViewModel = (MockViewModel_Activatable)currentPage.DataContext;

            Assert.Equal(new[] { pageInfo }, currentViewModel.ActivationCalls);
        }

        [Fact]
        public void WhenNavigationStack_PropertyChanged_CurrentPage_PassesNavigationContextToViewFactory()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            PageInfo pageInfo = new PageInfo("Page 1", null);
            TestableNavigationBase navigationBase = CreateNavigationBase(navigationStack: navigationStack);

            navigationStack.SetCurrentPage(pageInfo);

            MockPage currentPage = (MockPage)navigationBase.DisplayPageCalls.Last();
            INavigationContext navigationContext = currentPage.NavigationContext;

            Assert.NotNull(navigationContext);
            Assert.Equal(navigationBase, navigationContext.GetCurrent());
        }

        [Fact]
        public void WhenNavigationStack_PageDisposed_DisposesCurrentViewModel()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            PageInfo pageInfo = new PageInfo("Page 1", null);
            TestableNavigationBase navigationBase = CreateNavigationBase(navigationStack: navigationStack);

            navigationStack.SetCurrentPage(pageInfo);
            MockPage currentPage = (MockPage)navigationBase.DisplayPageCalls.Last();
            MockViewModel currentViewModel = (MockViewModel)currentPage.DataContext;

            navigationStack.RaisePageDisposed(pageInfo, PageNavigationMode.Back);

            Assert.Equal(true, currentViewModel.IsDisposed);
        }

        [Fact]
        public void WhenNavigationStack_PageDisposed_DisposesCurrentPage()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            PageInfo pageInfo = new PageInfo("Page 1", null);
            TestableNavigationBase navigationBase = CreateNavigationBase(navigationStack: navigationStack);

            navigationStack.SetCurrentPage(pageInfo);
            MockPage currentPage = (MockPage)navigationBase.DisplayPageCalls.Last();

            navigationStack.RaisePageDisposed(pageInfo, PageNavigationMode.Back);

            Assert.Equal(true, currentPage.IsDisposed);
        }

        [Fact]
        public void WhenNavigationStack_NavigatingFrom_CallsNavigatedToOnPage()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            IViewFactory viewFactory = MockViewFactory.WithNavigationAware;
            TestableNavigationBase navigationBase = CreateNavigationBase(viewFactory: viewFactory, navigationStack: navigationStack);

            PageInfo page = new PageInfo("Page 1", null);
            navigationStack.SetCurrentPage(page);
            navigationStack.RaiseNavigatingFrom(page, PageNavigationMode.Forward);

            MockPage_NavigationAware currentPage = (MockPage_NavigationAware)navigationBase.DisplayPageCalls.Last();

            Assert.Equal(new string[] { "NavigatingFrom(Forward)" }, currentPage.NavigationEvents);
        }

        [Fact]
        public void WhenNavigationStack_NavigatingFrom_CallsNavigatedToOnViewModel()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            IViewFactory viewFactory = MockViewFactory.WithNavigationAware;
            TestableNavigationBase navigationBase = CreateNavigationBase(viewFactory: viewFactory, navigationStack: navigationStack);

            PageInfo page = new PageInfo("Page 1", null);
            navigationStack.SetCurrentPage(page);
            navigationStack.RaiseNavigatingFrom(page, PageNavigationMode.Forward);

            MockPage currentPage = (MockPage)navigationBase.DisplayPageCalls.Last();
            MockViewModel_NavigationAware currentViewModel = (MockViewModel_NavigationAware)currentPage.DataContext;

            Assert.Equal(new string[] { "NavigatingFrom(Forward)" }, currentViewModel.NavigationEvents);
        }

        [Fact]
        public void WhenNavigationStack_NavigatedTo_CallsNavigatedToOnPage()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            IViewFactory viewFactory = MockViewFactory.WithNavigationAware;
            TestableNavigationBase navigationBase = CreateNavigationBase(viewFactory: viewFactory, navigationStack: navigationStack);

            PageInfo page = new PageInfo("Page 1", null);
            navigationStack.SetCurrentPage(page);
            navigationStack.RaiseNavigatedTo(page, PageNavigationMode.Forward);

            MockPage_NavigationAware currentPage = (MockPage_NavigationAware)navigationBase.DisplayPageCalls.Last();

            Assert.Equal(new string[] { "NavigatedTo(Forward)" }, currentPage.NavigationEvents);
        }

        [Fact]
        public void WhenNavigationStack_NavigatedTo_CallsNavigatedToOnViewModel()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            IViewFactory viewFactory = MockViewFactory.WithNavigationAware;
            TestableNavigationBase navigationBase = CreateNavigationBase(viewFactory: viewFactory, navigationStack: navigationStack);

            PageInfo page = new PageInfo("Page 1", null);
            navigationStack.SetCurrentPage(page);
            navigationStack.RaiseNavigatedTo(page, PageNavigationMode.Forward);

            MockPage currentPage = (MockPage)navigationBase.DisplayPageCalls.Last();
            MockViewModel_NavigationAware currentViewModel = (MockViewModel_NavigationAware)currentPage.DataContext;

            Assert.Equal(new string[] { "NavigatedTo(Forward)" }, currentViewModel.NavigationEvents);
        }

        // *** Method Tests ***

        [Fact]
        public void CanNavigateTo_ReturnsTrue_PageExistsWithSpecifiedName()
        {
            INavigationBase navigationBase = CreateNavigationBase();

            bool canNavigateTo = navigationBase.CanNavigateTo("Page 1");

            Assert.Equal(true, canNavigateTo);
        }

        [Fact]
        public void CanNavigateTo_ReturnsFalse_NoPageWithSpecifiedName()
        {
            IViewFactory viewFactory = MockViewFactory.NoPageDefined;
            INavigationBase navigationBase = CreateNavigationBase(viewFactory: viewFactory);

            bool canNavigateTo = navigationBase.CanNavigateTo("Page X");

            Assert.Equal(false, canNavigateTo);
        }

        [Fact]
        public void CanNavigateTo_Exception_NullPageName()
        {
            INavigationBase navigationBase = CreateNavigationBase();

            Assert.Throws<ArgumentException>(() => navigationBase.CanNavigateTo(null));
        }

        [Fact]
        public void CanNavigateTo_Exception_EmptyPageName()
        {
            INavigationBase navigationBase = CreateNavigationBase();

            Assert.Throws<ArgumentException>(() => navigationBase.CanNavigateTo(""));
        }

        [Fact]
        public void GetPageElements_ReturnsEmptyArrayIfNotCached()
        {
            TestableNavigationBase navigationBase = CreateNavigationBase();
            PageInfo pageInfo = new PageInfo("Page 1", null);

            object[] pageElements = navigationBase.GetPageElements(pageInfo).ToArray();

            Assert.Equal(new object[] { }, pageElements);
        }

        [Fact]
        public void GetPageElements_ReturnsViewModelThenPageIfBothPresent()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            TestableNavigationBase navigationBase = CreateNavigationBase(navigationStack: navigationStack);
            PageInfo pageInfo = new PageInfo("Page 1", null);

            navigationStack.SetCurrentPage(pageInfo);

            MockPage currentPage = navigationBase.DisplayPageCalls.Cast<MockPage>().LastOrDefault();
            MockViewModel currentViewModel = (MockViewModel)currentPage.DataContext;

            Assert.Equal(new object[] { currentViewModel, currentPage }, navigationBase.GetPageElements(pageInfo).ToList());
        }

        [Fact]
        public void GetPageElements_ReturnsPageIfNoViewModelPresent()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            IViewFactory viewFactory = MockViewFactory.WithPageOnly;
            TestableNavigationBase navigationBase = CreateNavigationBase(viewFactory: viewFactory, navigationStack: navigationStack);
            PageInfo pageInfo = new PageInfo("Page 1", null);

            navigationStack.SetCurrentPage(pageInfo);

            MockPage currentPage = navigationBase.DisplayPageCalls.Cast<MockPage>().LastOrDefault();

            Assert.Equal(new object[] { currentPage }, navigationBase.GetPageElements(pageInfo).ToList());
        }

        [Fact]
        public void GetPageElements_Exception_NullPageInfo()
        {
            INavigationBase navigationBase = CreateNavigationBase();

            Assert.Throws<ArgumentNullException>(() => navigationBase.GetPageElements(null));
        }

        [Fact]
        public void StoreRestoreState_PersistsNavigationStack()
        {
            byte[] persistedData;

            // StoreState

            {
                MockNavigationStack navigationStack = new MockNavigationStack()
                {
                    new PageInfo("Page 1", null),
                    new PageInfo("Page 2", null)
                };

                TestableNavigationBase navigationBase = CreateNavigationBase(navigationStack: navigationStack);

                NavigationState state = navigationBase.StoreState();
                persistedData = SerializationHelper.SerializeToArray<NavigationState>(state);
            }

            // Restore State

            {
                MockNavigationStack navigationStack = new MockNavigationStack();
                TestableNavigationBase navigationBase = CreateNavigationBase(navigationStack: navigationStack);

                NavigationState state = SerializationHelper.DeserializeFromArray<NavigationState>(persistedData);
                navigationBase.RestoreState(state);

                Assert.Equal(2, navigationBase.NavigationStack.Count);
                Assert.Equal("Page 1", navigationBase.NavigationStack[0].PageName);
                Assert.Equal("Page 2", navigationBase.NavigationStack[1].PageName);
            }
        }

        [Fact]
        public void StoreRestoreState_CallsNavigatedToOnPage()
        {
            byte[] persistedData;

            // StoreState

            {
                MockNavigationStack navigationStack = new MockNavigationStack()
                {
                    new PageInfo("Page 1", null),
                    new PageInfo("Page 2", null)
                };

                TestableNavigationBase navigationBase = CreateNavigationBase(navigationStack: navigationStack);

                NavigationState state = navigationBase.StoreState();
                persistedData = SerializationHelper.SerializeToArray<NavigationState>(state);
            }

            // Restore State

            {
                MockNavigationStack navigationStack = new MockNavigationStack();
                IViewFactory viewFactory = MockViewFactory.WithNavigationAware;
                TestableNavigationBase navigationBase = CreateNavigationBase(viewFactory: viewFactory, navigationStack: navigationStack);

                NavigationState state = SerializationHelper.DeserializeFromArray<NavigationState>(persistedData);
                navigationBase.RestoreState(state);

                MockPage_NavigationAware currentPage = (MockPage_NavigationAware)navigationBase.DisplayPageCalls.Last();
                Assert.Equal(new string[] { "NavigatedTo(Refresh)" }, currentPage.NavigationEvents);
            }
        }

        [Fact]
        public void StoreRestoreState_CallsNavigatedToOnViewModel()
        {
            byte[] persistedData;

            // StoreState

            {
                MockNavigationStack navigationStack = new MockNavigationStack()
                {
                    new PageInfo("Page 1", null),
                    new PageInfo("Page 2", null)
                };

                TestableNavigationBase navigationBase = CreateNavigationBase(navigationStack: navigationStack);

                NavigationState state = navigationBase.StoreState();
                persistedData = SerializationHelper.SerializeToArray<NavigationState>(state);
            }

            // Restore State

            {
                MockNavigationStack navigationStack = new MockNavigationStack();
                IViewFactory viewFactory = MockViewFactory.WithNavigationAware;
                TestableNavigationBase navigationBase = CreateNavigationBase(viewFactory: viewFactory, navigationStack: navigationStack);

                NavigationState state = SerializationHelper.DeserializeFromArray<NavigationState>(persistedData);
                navigationBase.RestoreState(state);

                MockPage currentPage = (MockPage)navigationBase.DisplayPageCalls.Last();
                MockViewModel_NavigationAware currentViewModel = (MockViewModel_NavigationAware)currentPage.DataContext;
                Assert.Equal(new string[] { "NavigatedTo(Refresh)" }, currentViewModel.NavigationEvents);
            }
        }

        [Fact]
        public void RestoreState_Exception_NullState()
        {
            TestableNavigationBase navigationBase = CreateNavigationBase();

            Assert.Throws<ArgumentNullException>(() => navigationBase.RestoreState(null));
        }

        // *** Private Methods ***

        private TestableNavigationBase CreateNavigationBase(IViewFactory viewFactory = null, INavigationStack navigationStack = null)
        {
            if (viewFactory == null)
                viewFactory = MockViewFactory.WithPageAndViewModel;

            if (navigationStack == null)
                navigationStack = new MockNavigationStack();

            TestableNavigationBase navigationBase = new TestableNavigationBase(viewFactory, navigationStack);

            return navigationBase;
        }

        // *** Private Sub-classes ***

        private class TestableNavigationBase : NavigationBase
        {
            // *** Fields ***

            public readonly List<object> DisplayPageCalls = new List<object>();

            // *** Constructors ***

            public TestableNavigationBase(IViewFactory viewFactory)
                : base(viewFactory)
            {
            }

            public TestableNavigationBase(IViewFactory viewFactory, INavigationStack navigationStack)
                : base(viewFactory, navigationStack)
            {
            }

            // *** Methods ***

            protected override void DisplayPage(object page)
            {
                DisplayPageCalls.Add(page);
            }

            public new void RestoreState(NavigationState state)
            {
                base.RestoreState(state);
            }

            public new NavigationState StoreState()
            {
                return base.StoreState();
            }
        }

        private class MockNavigationStack : List<PageInfo>, INavigationStack
        {
            public event EventHandler<PageNavigationEventArgs> NavigatingFrom;
            public event EventHandler<PageNavigationEventArgs> NavigatedTo;
            public event EventHandler<PageNavigationEventArgs> PageDisposed;
            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            public bool CanGoBack
            {
                get;
                set;
            }

            public PageInfo CurrentPage
            {
                get;
                private set;
            }

            public void GoBack()
            {
                throw new NotImplementedException();
            }

            public void GoBackTo(PageInfo page)
            {
                throw new NotImplementedException();
            }

            public void NavigateTo(PageInfo page)
            {
                throw new NotImplementedException();
            }

            public void Push(IEnumerable<PageInfo> pages)
            {
                base.AddRange(pages);
                SetCurrentPage(pages.Last());
                RaiseNavigatedTo(pages.Last(), PageNavigationMode.Forward);
            }

            // *** Mock Helper Methods ***

            public void RaisePropertyChanged<T>(Expression<Func<T>> property)
            {
                string propertyName = ((MemberExpression)property.Body).Member.Name;

                if (PropertyChanged != null)
                    PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }

            public void RaiseNavigatedTo(PageInfo page, PageNavigationMode navigationMode)
            {
                if (NavigatedTo != null)
                    NavigatedTo(this, new PageNavigationEventArgs(page, navigationMode));
            }

            public void RaiseNavigatingFrom(PageInfo page, PageNavigationMode navigationMode)
            {
                if (NavigatingFrom != null)
                    NavigatingFrom(this, new PageNavigationEventArgs(page, navigationMode));
            }

            public void RaisePageDisposed(PageInfo page, PageNavigationMode navigationMode)
            {
                if (PageDisposed != null)
                    PageDisposed(this, new PageNavigationEventArgs(page, navigationMode));
            }

            public void SetCurrentPage(PageInfo pageInfo)
            {
                CurrentPage = pageInfo;
                RaisePropertyChanged(() => this.CurrentPage);
            }
        }
    }
}

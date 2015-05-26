using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
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

namespace Okra.Tests.Navigation
{
    [TestClass]
    public class NavigationBaseFixture
    {
        // *** Constructor Tests ***

        [TestMethod]
        public void Constructor_Exception_NullViewFactory()
        {
            INavigationStack navigationStack = new MockNavigationStack();

            Assert.ThrowsException<ArgumentNullException>(() => new TestableNavigationBase(null, navigationStack));
        }

        [TestMethod]
        public void Constructor_Exception_NullNavigationStack()
        {
            IViewFactory viewFactory = MockViewFactory.WithPageAndViewModel;

            Assert.ThrowsException<ArgumentNullException>(() => new TestableNavigationBase(viewFactory, null));
        }

        // *** Property Test ***

        [TestMethod]
        public void NavigationStack_SetByConstructor()
        {
            INavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationBase = new TestableNavigationBase(MockViewFactory.WithPageAndViewModel, navigationStack);

            Assert.AreEqual(navigationStack, navigationBase.NavigationStack);
        }

        [TestMethod]
        public void NavigationStack_DefaultsToNavigationStack()
        {
            INavigationBase navigationBase = new TestableNavigationBase(MockViewFactory.WithPageAndViewModel);

            Assert.AreEqual(typeof(NavigationStack), navigationBase.NavigationStack.GetType());
        }

        // *** NavigationStack Event Tests ***


        [TestMethod]
        public void WhenNavigationStack_PropertyChanged_CurrentPage_NullPageCallsDisplayPageWithNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            TestableNavigationBase navigationBase = CreateNavigationBase(navigationStack: navigationStack);

            navigationStack.SetCurrentPage(null);

            CollectionAssert.AreEqual(new PageInfo[] { null }, navigationBase.DisplayPageCalls);
        }

        [TestMethod]
        public void WhenNavigationStack_PropertyChanged_CurrentPage_DisplaysSpecifiedPage_WithViewModel()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            PageInfo pageInfo = new PageInfo("Page 1", null);
            TestableNavigationBase navigationBase = CreateNavigationBase(navigationStack: navigationStack);

            navigationStack.SetCurrentPage(pageInfo);

            string[] pageNames = navigationBase.DisplayPageCalls.Cast<MockPage>().Select(page => page.PageName).ToArray();
            CollectionAssert.AreEqual(new string[] { "Page 1" }, pageNames);
        }

        [TestMethod]
        public void WhenNavigationStack_PropertyChanged_CurrentPage_DisplaysSpecifiedPage_WithoutViewModel()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            IViewFactory viewFactory = MockViewFactory.WithPageOnly;
            PageInfo pageInfo = new PageInfo("Page 1", null);
            TestableNavigationBase navigationBase = CreateNavigationBase(viewFactory: viewFactory, navigationStack: navigationStack);

            navigationStack.SetCurrentPage(pageInfo);

            string[] pageNames = navigationBase.DisplayPageCalls.Cast<MockPage>().Select(page => page.PageName).ToArray();
            CollectionAssert.AreEqual(new string[] { "Page 1" }, pageNames);
        }

        [TestMethod]
        public void WhenNavigationStack_PropertyChanged_CurrentPage_ActivatesViewModel()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            IViewFactory viewFactory = MockViewFactory.WithActivatable;
            PageInfo pageInfo = new PageInfo("Page 1", null);
            TestableNavigationBase navigationBase = CreateNavigationBase(viewFactory: viewFactory, navigationStack: navigationStack);

            navigationStack.SetCurrentPage(pageInfo);

            MockPage currentPage = (MockPage)navigationBase.DisplayPageCalls.Last();
            MockViewModel_Activatable currentViewModel = (MockViewModel_Activatable)currentPage.DataContext;

            CollectionAssert.AreEqual(new[] { pageInfo }, currentViewModel.ActivationCalls);
        }

        [TestMethod]
        public void WhenNavigationStack_PropertyChanged_CurrentPage_PassesNavigationContextToViewFactory()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            PageInfo pageInfo = new PageInfo("Page 1", null);
            TestableNavigationBase navigationBase = CreateNavigationBase(navigationStack: navigationStack);

            navigationStack.SetCurrentPage(pageInfo);

            MockPage currentPage = (MockPage)navigationBase.DisplayPageCalls.Last();
            INavigationContext navigationContext = currentPage.NavigationContext;

            Assert.IsNotNull(navigationContext);
            Assert.AreEqual(navigationBase, navigationContext.GetCurrent());
        }

        [TestMethod]
        public void WhenNavigationStack_PageDisposed_DisposesCurrentViewModel()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            PageInfo pageInfo = new PageInfo("Page 1", null);
            TestableNavigationBase navigationBase = CreateNavigationBase(navigationStack: navigationStack);

            navigationStack.SetCurrentPage(pageInfo);
            MockPage currentPage = (MockPage)navigationBase.DisplayPageCalls.Last();
            MockViewModel currentViewModel = (MockViewModel)currentPage.DataContext;

            navigationStack.RaisePageDisposed(pageInfo, PageNavigationMode.Back);

            Assert.AreEqual(true, currentViewModel.IsDisposed);
        }

        [TestMethod]
        public void WhenNavigationStack_PageDisposed_DisposesCurrentPage()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            PageInfo pageInfo = new PageInfo("Page 1", null);
            TestableNavigationBase navigationBase = CreateNavigationBase(navigationStack: navigationStack);

            navigationStack.SetCurrentPage(pageInfo);
            MockPage currentPage = (MockPage)navigationBase.DisplayPageCalls.Last();

            navigationStack.RaisePageDisposed(pageInfo, PageNavigationMode.Back);

            Assert.AreEqual(true, currentPage.IsDisposed);
        }

        [TestMethod]
        public void WhenNavigationStack_NavigatingFrom_CallsNavigatedToOnPage()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            IViewFactory viewFactory = MockViewFactory.WithNavigationAware;
            TestableNavigationBase navigationBase = CreateNavigationBase(viewFactory: viewFactory, navigationStack: navigationStack);

            PageInfo page = new PageInfo("Page 1", null);
            navigationStack.SetCurrentPage(page);
            navigationStack.RaiseNavigatingFrom(page, PageNavigationMode.Forward);

            MockPage_NavigationAware currentPage = (MockPage_NavigationAware)navigationBase.DisplayPageCalls.Last();

            CollectionAssert.AreEqual(new string[] { "NavigatingFrom(Forward)" }, currentPage.NavigationEvents);
        }

        [TestMethod]
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

            CollectionAssert.AreEqual(new string[] { "NavigatingFrom(Forward)" }, currentViewModel.NavigationEvents);
        }

        [TestMethod]
        public void WhenNavigationStack_NavigatedTo_CallsNavigatedToOnPage()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            IViewFactory viewFactory = MockViewFactory.WithNavigationAware;
            TestableNavigationBase navigationBase = CreateNavigationBase(viewFactory: viewFactory, navigationStack: navigationStack);

            PageInfo page = new PageInfo("Page 1", null);
            navigationStack.SetCurrentPage(page);
            navigationStack.RaiseNavigatedTo(page, PageNavigationMode.Forward);

            MockPage_NavigationAware currentPage = (MockPage_NavigationAware)navigationBase.DisplayPageCalls.Last();

            CollectionAssert.AreEqual(new string[] { "NavigatedTo(Forward)" }, currentPage.NavigationEvents);
        }

        [TestMethod]
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

            CollectionAssert.AreEqual(new string[] { "NavigatedTo(Forward)" }, currentViewModel.NavigationEvents);
        }

        // *** Method Tests ***

        [TestMethod]
        public void CanNavigateTo_ReturnsTrue_PageExistsWithSpecifiedName()
        {
            INavigationBase navigationBase = CreateNavigationBase();

            bool canNavigateTo = navigationBase.CanNavigateTo("Page 1");

            Assert.AreEqual(true, canNavigateTo);
        }

        [TestMethod]
        public void CanNavigateTo_ReturnsFalse_NoPageWithSpecifiedName()
        {
            IViewFactory viewFactory = MockViewFactory.NoPageDefined;
            INavigationBase navigationBase = CreateNavigationBase(viewFactory: viewFactory);

            bool canNavigateTo = navigationBase.CanNavigateTo("Page X");

            Assert.AreEqual(false, canNavigateTo);
        }

        [TestMethod]
        public void CanNavigateTo_Exception_NullPageName()
        {
            INavigationBase navigationBase = CreateNavigationBase();

            Assert.ThrowsException<ArgumentException>(() => navigationBase.CanNavigateTo(null));
        }

        [TestMethod]
        public void CanNavigateTo_Exception_EmptyPageName()
        {
            INavigationBase navigationBase = CreateNavigationBase();

            Assert.ThrowsException<ArgumentException>(() => navigationBase.CanNavigateTo(""));
        }

        [TestMethod]
        public void GetPageElements_ReturnsEmptyArrayIfNotCached()
        {
            TestableNavigationBase navigationBase = CreateNavigationBase();
            PageInfo pageInfo = new PageInfo("Page 1", null);

            object[] pageElements = navigationBase.GetPageElements(pageInfo).ToArray();

            CollectionAssert.AreEqual(new object[] { }, pageElements);
        }

        [TestMethod]
        public void GetPageElements_ReturnsViewModelThenPageIfBothPresent()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            TestableNavigationBase navigationBase = CreateNavigationBase(navigationStack: navigationStack);
            PageInfo pageInfo = new PageInfo("Page 1", null);

            navigationStack.SetCurrentPage(pageInfo);

            MockPage currentPage = navigationBase.DisplayPageCalls.Cast<MockPage>().LastOrDefault();
            MockViewModel currentViewModel = (MockViewModel)currentPage.DataContext;

            CollectionAssert.AreEqual(new object[] { currentViewModel, currentPage }, navigationBase.GetPageElements(pageInfo).ToList());
        }

        [TestMethod]
        public void GetPageElements_ReturnsPageIfNoViewModelPresent()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            IViewFactory viewFactory = MockViewFactory.WithPageOnly;
            TestableNavigationBase navigationBase = CreateNavigationBase(viewFactory: viewFactory, navigationStack: navigationStack);
            PageInfo pageInfo = new PageInfo("Page 1", null);

            navigationStack.SetCurrentPage(pageInfo);

            MockPage currentPage = navigationBase.DisplayPageCalls.Cast<MockPage>().LastOrDefault();

            CollectionAssert.AreEqual(new object[] { currentPage }, navigationBase.GetPageElements(pageInfo).ToList());
        }

        [TestMethod]
        public void GetPageElements_Exception_NullPageInfo()
        {
            INavigationBase navigationBase = CreateNavigationBase();

            Assert.ThrowsException<ArgumentNullException>(() => navigationBase.GetPageElements(null));
        }

        [TestMethod]
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

                Assert.AreEqual(2, navigationBase.NavigationStack.Count);
                Assert.AreEqual("Page 1", navigationBase.NavigationStack[0].PageName);
                Assert.AreEqual("Page 2", navigationBase.NavigationStack[1].PageName);
            }
        }

        [TestMethod]
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
                CollectionAssert.AreEqual(new string[] { "NavigatedTo(Refresh)" }, currentPage.NavigationEvents);
            }
        }

        [TestMethod]
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
                CollectionAssert.AreEqual(new string[] { "NavigatedTo(Refresh)" }, currentViewModel.NavigationEvents);
            }
        }

        [TestMethod]
        public void RestoreState_Exception_NullState()
        {
            TestableNavigationBase navigationBase = CreateNavigationBase();

            Assert.ThrowsException<ArgumentNullException>(() => navigationBase.RestoreState(null));
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

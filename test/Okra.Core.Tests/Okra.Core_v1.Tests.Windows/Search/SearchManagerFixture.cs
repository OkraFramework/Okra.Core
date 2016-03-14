using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Navigation;
using Okra.Search;
using Okra.Tests.Mocks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Search;
using Okra.Services;
using Xunit;

namespace Okra.Tests.Search
{
    public class SearchManagerFixture
    {
        // *** Constructor Tests ***

        [Fact]
        public void Constructor_RegistersWithActivationManager()
        {
            MockActivationManager activationManager = new MockActivationManager();
            SearchManager searchManager = CreateSearchMananger(activationManager: activationManager);

            Assert.Contains(searchManager, activationManager.RegisteredServices);
        }

        [Fact]
        public void Constructor_ThrowsException_IfNavigationManagerIsNull()
        {
            MockActivationManager activationManager = new MockActivationManager();

            var e = Assert.Throws<ArgumentNullException>(() => new SearchManager(null, activationManager));

            Assert.Equal("Value cannot be null.\r\nParameter name: navigationManager", e.Message);
            Assert.Equal("navigationManager", e.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsException_IfActivationManagerIsNull()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();

            var e = Assert.Throws<ArgumentNullException>(() => new SearchManager(navigationManager, null));

            Assert.Equal("Value cannot be null.\r\nParameter name: activationManager", e.Message);
            Assert.Equal("activationManager", e.ParamName);
        }

        // *** Property Tests ***

        [Fact]
        public void SearchPageName_IsInitiallySpecialPageName()
        {
            SearchManager searchManager = CreateSearchMananger(setSearchPageName: false);

            Assert.Equal(SpecialPageNames.Search, searchManager.SearchPageName);
        }

        [Fact]
        public void SearchPageName_CanSetValue()
        {
            SearchManager searchManager = CreateSearchMananger();

            searchManager.SearchPageName = "MySearchPage";

            Assert.Equal("MySearchPage", searchManager.SearchPageName);
        }

        [Fact]
        public void SearchPageName_Exception_CannotSetToNull()
        {
            SearchManager searchManager = CreateSearchMananger();

            var e = Assert.Throws<ArgumentException>(() => searchManager.SearchPageName = null);

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: SearchPageName", e.Message);
            Assert.Equal("SearchPageName", e.ParamName);
        }

        [Fact]
        public void SearchPageName_Exception_CannotSetToEmptyString()
        {
            SearchManager searchManager = CreateSearchMananger();

            var e = Assert.Throws<ArgumentException>(() => searchManager.SearchPageName = "");

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: SearchPageName", e.Message);
            Assert.Equal("SearchPageName", e.ParamName);
        }

        [Fact]
        public void SearchPageName_Exception_CannotSetValueOnceActivated()
        {
            MockActivationManager activationManager = new MockActivationManager();
            SearchManager searchManager = CreateSearchMananger(activationManager: activationManager);

            activationManager.RaiseActivatedEvent(new MockActivatedEventArgs());

            var e = Assert.Throws<InvalidOperationException>(() => searchManager.SearchPageName = "MySearchPage");

            Assert.Equal("The 'SearchPageName' property cannot be set after the application has been activated.", e.Message);
        }

        // *** Method Tests ***

        [Fact]
        public async Task Activate_ReturnsTrueIfActivationKindIsSearch()
        {
            SearchManager searchManager = CreateSearchMananger();

            // Activate the application

            bool result = await searchManager.Activate(new MockSearchActivatedEventArgs() { QueryText = "MyQuery", Language = "en-GB" });

            // Check the result

            Assert.Equal(true, result);
        }

        [Fact]
        public async Task Activate_ReturnsFalseIfActivationKindIsNotLaunch()
        {
            SearchManager searchManager = CreateSearchMananger();

            // Activate the application

            bool result = await searchManager.Activate(new MockActivatedEventArgs() { Kind = ActivationKind.Launch });

            // Check the result

            Assert.Equal(false, result);
        }

        [Fact]
        public async Task Activate_NavigatesToSearchPageIfPreviousExecutionRunning()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            SearchManager searchManager = CreateSearchMananger(navigationManager: navigationManager);

            await searchManager.Activate(new MockSearchActivatedEventArgs() { QueryText = "MyQuery", Language = "en-GB", PreviousExecutionState = ApplicationExecutionState.Running });

            Assert.Equal<Tuple<string, object>>(new[] { Tuple.Create("Search", (object)null) }, navigationManager.NavigatedPages);
        }

        [Fact]
        public async Task Activate_NavigatesToSearchPageIfPreviousExecutionSuspended()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            SearchManager searchManager = CreateSearchMananger(navigationManager: navigationManager);

            await searchManager.Activate(new MockSearchActivatedEventArgs() { QueryText = "MyQuery", Language = "en-GB", PreviousExecutionState = ApplicationExecutionState.Suspended });

            Assert.Equal<Tuple<string, object>>(new[] { Tuple.Create("Search", (object)null) }, navigationManager.NavigatedPages);
        }

        [Fact]
        public async Task Activate_NavigatesToSearchPageWithRestoredNavigationIfPreviousExecutionTerminated()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            SearchManager searchManager = CreateSearchMananger(navigationManager: navigationManager);

            await searchManager.Activate(new MockSearchActivatedEventArgs() { QueryText = "MyQuery", Language = "en-GB", PreviousExecutionState = ApplicationExecutionState.Terminated });

            Assert.Equal<Tuple<string, object>>(new[] { Tuple.Create("[Restored Pages]", (object)null), Tuple.Create("Search", (object)null) }, navigationManager.NavigatedPages);
        }

        [Fact]
        public async Task Activate_NavigatesToSearchPageAfterHomePageIfPreviousExecutionClosedByUser()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            SearchManager searchManager = CreateSearchMananger(navigationManager: navigationManager);

            await searchManager.Activate(new MockSearchActivatedEventArgs() { QueryText = "MyQuery", Language = "en-GB", PreviousExecutionState = ApplicationExecutionState.ClosedByUser });

            Assert.Equal<Tuple<string, object>>(new[] { Tuple.Create("Home", (object)null), Tuple.Create("Search", (object)null) }, navigationManager.NavigatedPages);
        }

        [Fact]
        public async Task Activate_NavigatesToSearchPageAfterHomePageIfPreviousExecutionNotRunning()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            SearchManager searchManager = CreateSearchMananger(navigationManager: navigationManager);

            await searchManager.Activate(new MockSearchActivatedEventArgs() { QueryText = "MyQuery", Language = "en-GB", PreviousExecutionState = ApplicationExecutionState.NotRunning });

            Assert.Equal<Tuple<string, object>>(new[] { Tuple.Create("Home", (object)null), Tuple.Create("Search", (object)null) }, navigationManager.NavigatedPages);
        }

        [Fact]
        public async Task Activate_EmptySearch_NoNavigationIfPreviousExecutionRunning()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            SearchManager searchManager = CreateSearchMananger(navigationManager: navigationManager);

            await searchManager.Activate(new MockSearchActivatedEventArgs() { QueryText = "", Language = "en-GB", PreviousExecutionState = ApplicationExecutionState.Running });

            Assert.Equal<Tuple<string, object>>(new Tuple<string, object>[] { }, navigationManager.NavigatedPages);
        }

        [Fact]
        public async Task Activate_EmptySearch_NoNavigationIfPreviousExecutionSuspended()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            SearchManager searchManager = CreateSearchMananger(navigationManager: navigationManager);

            await searchManager.Activate(new MockSearchActivatedEventArgs() { QueryText = "", Language = "en-GB", PreviousExecutionState = ApplicationExecutionState.Suspended });

            Assert.Equal<Tuple<string, object>>(new Tuple<string, object>[] { }, navigationManager.NavigatedPages);
        }

        [Fact]
        public async Task Activate_EmptySearch_RestoresNavigationIfPreviousExecutionTerminated()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            SearchManager searchManager = CreateSearchMananger(navigationManager: navigationManager);

            await searchManager.Activate(new MockSearchActivatedEventArgs() { QueryText = "", Language = "en-GB", PreviousExecutionState = ApplicationExecutionState.Terminated });

            Assert.Equal<Tuple<string, object>>(new[] { Tuple.Create("[Restored Pages]", (object)null) }, navigationManager.NavigatedPages);
        }

        [Fact]
        public async Task Activate_EmptySearch_NavigatesToHomePageIfPreviousExecutionClosedByUser()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            SearchManager searchManager = CreateSearchMananger(navigationManager: navigationManager);

            await searchManager.Activate(new MockSearchActivatedEventArgs() { QueryText = "", Language = "en-GB", PreviousExecutionState = ApplicationExecutionState.ClosedByUser });

            Assert.Equal<Tuple<string, object>>(new[] { Tuple.Create("Home", (object)null) }, navigationManager.NavigatedPages);
        }

        [Fact]
        public async Task Activate_EmptySearch_NavigatesToHomePageIfPreviousExecutionNotRunning()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            SearchManager searchManager = CreateSearchMananger(navigationManager: navigationManager);

            await searchManager.Activate(new MockSearchActivatedEventArgs() { QueryText = "", Language = "en-GB", PreviousExecutionState = ApplicationExecutionState.NotRunning });

            Assert.Equal<Tuple<string, object>>(new[] { Tuple.Create("Home", (object)null) }, navigationManager.NavigatedPages);
        }

        [Fact]
        public async Task Activate_DoesNotNavigateIfAlreadyShowingSearchPage()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();
            SearchManager searchManager = CreateSearchMananger(navigationManager: navigationManager);

            navigationManager.NavigateTo("Search");

            await searchManager.Activate(new MockSearchActivatedEventArgs() { QueryText = "MyQuery", Language = "en-GB", PreviousExecutionState = ApplicationExecutionState.Running });

            Assert.Equal<Tuple<string, object>>(new Tuple<string, object>[] { Tuple.Create("Search", (object)null) }, navigationManager.NavigatedPages);
        }

        [Fact]
        public async Task Activate_CallsPerformQueryOnAllElements()
        {
            MockNavigationManager navigationManager = new MockNavigationManager(_ => new object[] { new MockSearchPageElement(), new MockSearchPageElement() });
            SearchManager searchManager = CreateSearchMananger(navigationManager: navigationManager);

            await searchManager.Activate(new MockSearchActivatedEventArgs() { QueryText = "MyQuery", Language = "en-GB" });

            PageInfo searchPage = navigationManager.NavigationStack.CurrentPage;
            IList<object> searchElements = navigationManager.GetPageElements(searchPage).ToList();

            Assert.Equal<Tuple<string, string>>(new[] { Tuple.Create("MyQuery", "en-GB") }, ((MockSearchPageElement)searchElements[1]).Queries);
            Assert.Equal<Tuple<string, string>>(new[] { Tuple.Create("MyQuery", "en-GB") }, ((MockSearchPageElement)searchElements[0]).Queries);
        }

        [Fact]
        public async Task Activate_CallsPerformQueryOnlyOnSearchPageImplementors()
        {
            MockNavigationManager navigationManager = new MockNavigationManager(_ => new object[] { new MockSearchPageElement(), new MockPageElement() });
            SearchManager searchManager = CreateSearchMananger(navigationManager: navigationManager);

            await searchManager.Activate(new MockSearchActivatedEventArgs() { QueryText = "MyQuery", Language = "en-GB" });

            PageInfo searchPage = navigationManager.NavigationStack.CurrentPage;
            IList<object> searchElements = navigationManager.GetPageElements(searchPage).ToList();

            Assert.Equal<Tuple<string, string>>(new[] { Tuple.Create("MyQuery", "en-GB") }, ((MockSearchPageElement)searchElements[0]).Queries);
        }

        [Fact]
        public async Task Activate_SuccessfulEvenWhenNoSearchPageImplementors()
        {
            MockNavigationManager navigationManager = new MockNavigationManager(_ => new object[] { new MockPageElement(), new MockPageElement() });
            SearchManager searchManager = CreateSearchMananger(navigationManager: navigationManager);

            await searchManager.Activate(new MockSearchActivatedEventArgs() { QueryText = "MyQuery", Language = "en-GB" });
        }

        [Fact]
        public async Task Activate_DoesNotCallPerformQueryIfSameQueryAsVisible()
        {
            MockNavigationManager navigationManager = new MockNavigationManager(_ => new object[] { new MockSearchPageElement(), new MockSearchPageElement() });
            SearchManager searchManager = CreateSearchMananger(navigationManager: navigationManager);

            await searchManager.Activate(new MockSearchActivatedEventArgs() { QueryText = "MyQuery", Language = "en-GB" });

            PageInfo searchPage = navigationManager.NavigationStack.CurrentPage;
            IList<object> searchElements = navigationManager.GetPageElements(searchPage).ToList();

            Assert.Equal<Tuple<string, string>>(new[] { Tuple.Create("MyQuery", "en-GB") }, ((MockSearchPageElement)searchElements[1]).Queries);
            Assert.Equal<Tuple<string, string>>(new[] { Tuple.Create("MyQuery", "en-GB") }, ((MockSearchPageElement)searchElements[0]).Queries);
        }

        [Fact]
        public async Task Activate_DoesNotNavigateIfActivationKindIsNotSearch()
        {
            MockNavigationManager navigationManager = new MockNavigationManager() { CanRestoreNavigationStack = true };
            SearchManager searchManager = CreateSearchMananger(navigationManager: navigationManager);

            // Activate the application

            await searchManager.Activate(new MockActivatedEventArgs() { Kind = ActivationKind.Launch });

            // Assert that no pages were navigated

            Assert.Equal(new string[] { }, navigationManager.NavigatedPages.Select(t => t.Item1).ToArray());
        }

        [Fact]
        public async void Activate_ThrowsException_IfEventArgsIsNull()
        {
            SearchManager searchManager = CreateSearchMananger();

            var e = await Assert.ThrowsAsync<ArgumentNullException>(() => searchManager.Activate(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: activatedEventArgs", e.Message);
            Assert.Equal("activatedEventArgs", e.ParamName);
        }

        [Fact]
        public void OnActivationManagerActivated_ThrowsException_IfEventArgsIsNull()
        {
            TestableSearchManager searchManager = CreateSearchMananger();

            var e = Assert.Throws<ArgumentNullException>(() => searchManager.CallOnActivationManagerActivated(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: e", e.Message);
            Assert.Equal("e", e.ParamName);
        }

        [Fact]
        public void OnQuerySubmitted_ThrowsException_IfEventArgsIsNull()
        {
            TestableSearchManager searchManager = CreateSearchMananger();

            var e = Assert.Throws<ArgumentNullException>(() => searchManager.CallOnQuerySubmitted(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: args", e.Message);
            Assert.Equal("args", e.ParamName);
        }

        // *** Behaviour Tests ***

        [Fact]
        public void BeforeFirstActivation_DoesNotCallRegisterQuerySubmitted()
        {
            MockActivationManager activationManager = new MockActivationManager();
            TestableSearchManager searchManager = CreateSearchMananger(activationManager: activationManager);

            Assert.Equal(0, searchManager.RegisterQuerySubmittedCount);
        }

        [Fact]
        public void OnFirstActivation_CallsRegisterQuerySubmitted()
        {
            MockActivationManager activationManager = new MockActivationManager();
            TestableSearchManager searchManager = CreateSearchMananger(activationManager: activationManager);

            activationManager.RaiseActivatedEvent(new MockActivatedEventArgs());

            Assert.Equal(1, searchManager.RegisterQuerySubmittedCount);
        }

        [Fact]
        public void OnMultipleActivations_CallsRegisterQuerySubmittedOnlyOnce()
        {
            MockActivationManager activationManager = new MockActivationManager();
            TestableSearchManager searchManager = CreateSearchMananger(activationManager: activationManager);

            activationManager.RaiseActivatedEvent(new MockActivatedEventArgs());
            activationManager.RaiseActivatedEvent(new MockActivatedEventArgs());

            Assert.Equal(1, searchManager.RegisterQuerySubmittedCount);
        }

        // *** Private Methods ***

        private TestableSearchManager CreateSearchMananger(MockNavigationManager navigationManager = null, MockActivationManager activationManager = null, bool setSearchPageName = true)
        {
            if (navigationManager == null)
                navigationManager = new MockNavigationManager();

            if (activationManager == null)
                activationManager = new MockActivationManager();

            TestableSearchManager searchManager = new TestableSearchManager(navigationManager, activationManager);

            if (setSearchPageName)
                searchManager.SearchPageName = "Search";

            return searchManager;
        }

        // *** Private sub-classes ***

        private class TestableSearchManager : SearchManager
        {
            // *** Fields ***

            public int RegisterQuerySubmittedCount;

            // *** Constructors ***

            public TestableSearchManager(INavigationManager navigationManager, IActivationManager activationManager)
                : base(navigationManager, activationManager)
            {
            }

            // *** Methods ***

            public void CallOnActivationManagerActivated(IActivatedEventArgs e)
            {
                base.OnActivationManagerActivated(this, e);
            }

            public void CallOnQuerySubmitted(SearchPaneQuerySubmittedEventArgs args)
            {
                base.OnQuerySubmitted(null, args);
            }

            // *** Overriden base methods ***

            protected override void RegisterQuerySubmitted()
            {
                RegisterQuerySubmittedCount++;
            }
        }

        private class MockPageElement
        {
        }

        private class MockSearchPageElement : MockPageElement, ISearchPage
        {
            // *** Fields ***

            public IList<Tuple<string, string>> Queries = new List<Tuple<string, string>>();

            // *** Methods ***

            public void PerformQuery(string queryText, string language)
            {
                Queries.Add(Tuple.Create(queryText, language));
            }
        }

        private class MockSearchActivatedEventArgs : MockActivatedEventArgs, ISearchActivatedEventArgs
        {
            // *** Constructors ***

            public MockSearchActivatedEventArgs()
            {
                base.Kind = ActivationKind.Search;
                base.PreviousExecutionState = ApplicationExecutionState.Terminated;
            }

            // *** Propertes ***

            public string Language
            {
                get;
                set;
            }

            public string QueryText
            {
                get;
                set;
            }
        }
    }
}

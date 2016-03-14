using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Navigation;

namespace Okra.Tests.Mocks
{
    public class MockNavigationManager : INavigationManager
    {
        // *** Fields ***

        public Func<PageInfo, IEnumerable<object>> pageElementFunc;
        public Dictionary<PageInfo, IEnumerable<object>> pageElementCache = new Dictionary<PageInfo, IEnumerable<object>>();
        public bool CanRestoreNavigationStack = false;

        // *** Constructors ***

        public MockNavigationManager(Func<PageInfo, IEnumerable<object>> pageElementFunc = null)
        {
            this.NavigationStack = new MockNavigationStack();
            this.pageElementFunc = pageElementFunc;

            this.HomePageName = "Home";
        }

        // *** Properties ***

        public string HomePageName
        {
            get;
            set;
        }

        public IList<Tuple<string, object>> NavigatedPages
        {
            get
            {
                return NavigationStack.Select(e => Tuple.Create(e.PageName, e.GetArguments<object>())).ToList();
            }
        }

        public INavigationStack NavigationStack
        {
            get;
            private set;
        }

        public NavigationStorageType NavigationStorageType
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        // *** Methods ***

        public bool CanNavigateTo(string pageName)
        {
            return true;
        }

        public IEnumerable<object> GetPageElements(PageInfo page)
        {
            // Return a cached value if available

            IEnumerable<object> pageElements;

            if (!pageElementCache.TryGetValue(page, out pageElements))
            {
                // Otherwise create a new set of page elements

                if (pageElementFunc != null)
                    pageElements = pageElementFunc(page);
                else
                    pageElements = new object[] { };

                // And add to the cache

                pageElementCache.Add(page, pageElements);
            }

            return pageElements;
        }

        public Task<bool> RestoreNavigationStack()
        {
            if (CanRestoreNavigationStack)
                this.NavigateTo("[Restored Pages]");

            return Task.FromResult(CanRestoreNavigationStack);
        }

        // *** Mock Methods ***

        public void RaiseNavigatedTo(PageNavigationEventArgs eventArgs)
        {
            MockNavigationStack navigationStack = (MockNavigationStack)this.NavigationStack;
            navigationStack.RaiseNavigatedTo(eventArgs);
        }
    }
}

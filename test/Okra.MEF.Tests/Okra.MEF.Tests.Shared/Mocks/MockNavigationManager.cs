using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Navigation;

namespace Okra.MEF.Tests.Mocks
{
    public class MockNavigationManager : INavigationManager
    {
        // *** Fields ***

        private readonly Func<string, PageInfo> pageEntryCreator;

        public IList<Tuple<string, object>> NavigatedPages = new List<Tuple<string, object>>();
        public bool CanRestoreNavigationStack = false;
        
        // *** Constructors ***

        public MockNavigationManager()
            :this(pageName => new PageInfo(pageName, null))
        {
        }

        public MockNavigationManager(Func<string, PageInfo> pageEntryCreator)
        {
            this.pageEntryCreator = pageEntryCreator;

            this.HomePageName = "Home";
        }

        // *** Properties ***
        
        public string HomePageName
        {
            get;
            set;
        }

        public INavigationStack NavigationStack
        {
            get { throw new NotImplementedException(); }
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
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetPageElements(PageInfo page)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RestoreNavigationStack()
        {
            if (CanRestoreNavigationStack)
                this.NavigateTo("[Restored Pages]");

            return Task.FromResult(CanRestoreNavigationStack);
        }
    }
}

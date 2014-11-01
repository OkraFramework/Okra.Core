using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okra.Tests.Mocks
{
    public class MockNavigationBase : INavigationBase
    {
        // *** Constants ***

        public const string MOCKPAGE_NAME = "Okra.Tests.Mocks.MockNavigationBase+MockPage";
        
        // *** Constructors ***

        public MockNavigationBase(INavigationStack navigationStack = null)
        {
            if (navigationStack == null)
                navigationStack = new MockNavigationStack();

            this.NavigationStack = navigationStack;
        }

        // *** Properties ***
        
        public INavigationStack NavigationStack
        {
            get;
            private set;
        }

        // *** Methods ***

        public bool CanNavigateTo(string pageName)
        {
            return (pageName == "Page 1" || pageName == "Page 2" || pageName == MOCKPAGE_NAME);
        }

        public IEnumerable<object> GetPageElements(PageInfo page)
        {
            throw new NotImplementedException();
        }

        // *** Sub-classes ***

        public class MockPage
        {
        }
    }
}

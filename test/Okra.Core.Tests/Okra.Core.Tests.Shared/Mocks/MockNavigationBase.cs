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

        // *** Events ***

        public event EventHandler CanGoBackChanged;
        public event EventHandler<PageNavigationEventArgs> NavigatingFrom;
        public event EventHandler<PageNavigationEventArgs> NavigatedTo;

        // *** Constructors ***

        public MockNavigationBase(INavigationStack navigationStack = null)
        {
            if (navigationStack == null)
                navigationStack = new MockNavigationStack();

            this.NavigationStack = navigationStack;
        }

        // *** Properties ***

        public bool CanGoBack
        {
            get { throw new NotImplementedException(); }
        }

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

        // *** Mock Methods ***

        public void RaiseCanGoBackChanged()
        {
            if (CanGoBackChanged != null)
                CanGoBackChanged(this, new EventArgs());
        }

        public void RaiseNavigatedTo(PageNavigationEventArgs eventArgs)
        {
            if (NavigatedTo != null)
                NavigatedTo(this, eventArgs);
        }

        public void RaiseNavigatingFrom(PageNavigationEventArgs eventArgs)
        {
            if (NavigatingFrom != null)
                NavigatingFrom(this, eventArgs);
        }

        // *** Sub-classes ***

        public class MockPage
        {
        }
    }
}

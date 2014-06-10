using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Navigation;

namespace Okra.Tests.Mocks
{
    public class MockPage
    {
        // *** Properties ***

        public bool IsDisposed { get; set; }
        public string PageName { get; set; }
        public object DataContext { get; set; }
        public INavigationContext NavigationContext { get; set; }
    }

    public class MockPage_NavigationAware : MockPage, INavigationAware
    {
        // *** Fields ***

        public List<string> NavigationEvents = new List<string>();

        // *** Methods ***

        public void NavigatedTo(NavigationMode navigationMode)
        {
            NavigationEvents.Add(string.Format("NavigatedTo({0})", navigationMode));
        }

        public void NavigatingFrom(NavigationMode navigationMode)
        {
            NavigationEvents.Add(string.Format("NavigatingFrom({0})", navigationMode));
        }
    }
}

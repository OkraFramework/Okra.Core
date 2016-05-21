using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Navigation;

namespace Okra.Tests.Mocks
{
    public class MockViewModel
    {
        public string Name { get; set; }
        public bool IsDisposed { get; set; }
    }

    public class MockViewModel_Activatable : MockViewModel, IActivatable
    {
        // *** Properties ***

        public List<PageInfo> ActivationCalls = new List<PageInfo>();
        public List<PageInfo> SaveSateCalls = new List<PageInfo>();

        // *** Methods ***

        public void Activate(PageInfo pageInfo)
        {
            ActivationCalls.Add(pageInfo);
        }

        public void SaveState(PageInfo pageInfo)
        {
            SaveSateCalls.Add(pageInfo);
        }
    }

    public class MockViewModel_NavigationAware : MockViewModel, INavigationAware
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
}

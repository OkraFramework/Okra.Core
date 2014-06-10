using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okra.Tests.Mocks
{
    public class MockViewLifetimeContext : IViewLifetimeContext
    {
        // *** Constructors ***

        public MockViewLifetimeContext(string pageName, string viewModelName, INavigationContext navigationContext = null, Type pageType = null, Type viewModelType = null)
        {
            if (pageName != null)
            {
                MockPage page = (MockPage)Activator.CreateInstance(pageType ?? typeof(MockPage));
                page.PageName = pageName;
                page.NavigationContext = navigationContext;

                View = page;
            }

            if (viewModelName != null)
            {
                MockViewModel viewModel = (MockViewModel)Activator.CreateInstance(viewModelType ?? typeof(MockViewModel));
                viewModel.Name = viewModelName;

                ViewModel = viewModel;
            }

            if (pageName != null && viewModelName != null)
                ((MockPage)View).DataContext = ViewModel;
        }

        // *** Properties ***

        public object View { get; set; }
        public object ViewModel { get; set; }

        // *** Methods ***

        public void Dispose()
        {
            if (View != null)
                ((MockPage)View).IsDisposed = true;

            if (ViewModel != null)
                ((MockViewModel)ViewModel).IsDisposed = true;
        }
    }
}

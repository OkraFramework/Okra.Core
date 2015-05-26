using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Okra.Navigation
{
    public class NavigationViewNavigationTarget : INavigationTarget
    {
        // *** Fields ***

        private INavigationBase _navigationManager;

        // *** Methods ***

        public void NavigateTo(object page, INavigationBase navigationManager)
        {
            _navigationManager = navigationManager;

            SetWindowContent(page);
        }

        protected virtual async void SetWindowContent(object page)
        {
            var nextPage = page as Page;
            if (nextPage == null)
                throw new ArgumentException("Only objects of type 'Page' can be presented.", "page");

            var navigationView = Application.Current.MainPage as NavigationView;
            if (navigationView == null)
                // MainPage must be assigned from the Application constructor.
                throw new InvalidOperationException("NavigationView missing as main page.");

            // Set the content to display

            await navigationView.ShowAsync(nextPage);
        }
    }
}

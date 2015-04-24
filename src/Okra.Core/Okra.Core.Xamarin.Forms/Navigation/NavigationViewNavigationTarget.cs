using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Okra.Navigation
{
    public class NavigationViewNavigationTarget : INavigationTarget
    {
        // *** Fields ***

        private INavigationBase navigationManager;

        // *** Methods ***

        public void NavigateTo(object page, INavigationBase navigationManager)
        {
            this.navigationManager = navigationManager;

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

            var previousPage = navigationView.Navigation.NavigationStack.LastOrDefault();

            if (!navigationManager.NavigationStack.CanGoBack &&
                (navigationView.Navigation.NavigationStack.FirstOrDefault() == nextPage))
            {
                await navigationView.Navigation.PopAsync();
                return;
            }

            await navigationView.Navigation.PushAsync(nextPage);

            // Remove first empty page after navigating to home page for the first time.
            if (!navigationManager.NavigationStack.CanGoBack && navigationView.Navigation.NavigationStack.Count == 2)
            {
                navigationView.Navigation.RemovePage(previousPage);
            }
            // We only keep the home page and the current page in the navigation stack of Xamarin Forms.
            else if (previousPage != null && navigationView.Navigation.NavigationStack.Count > 2)
            {
                navigationView.Navigation.RemovePage(previousPage);
            }
        }
    }
}

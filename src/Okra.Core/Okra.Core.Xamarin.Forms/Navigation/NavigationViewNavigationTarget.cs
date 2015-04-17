using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Okra.Navigation
{
    public class NavigationViewNavigationTarget : INavigationTarget
    {
        // *** Fields ***

        private NavigationView navigationView;
        private INavigationBase navigationManager;

        // *** Methods ***

        public async void NavigateTo(object page, INavigationBase navigationManager)
        {
            this.navigationManager = navigationManager;

            SetWindowContent(page);
        }

        protected virtual async void SetWindowContent(object page)
        {
            var nextPage = page as Page;
            if (nextPage == null)
                throw new ArgumentException("Only objects of type 'Page' can be presented.", "page");

            // If the content host has not been created then create this

            if (navigationView == null)
            {
                navigationView = new NavigationView(navigationManager);
            }

            // Ensure that the window content is set to the content host

            if (Application.Current.MainPage != navigationView)
                Application.Current.MainPage = navigationView;

            // Set the content to display

            var previousPage = navigationView.Navigation.NavigationStack.LastOrDefault();

            if (!navigationManager.NavigationStack.CanGoBack &&
                (navigationView.Navigation.NavigationStack.FirstOrDefault() == nextPage))
            {
                await navigationView.Navigation.PopAsync();
                return;
            }

            await navigationView.Navigation.PushAsync(nextPage);

            // We only keep the home page and the current page in the navigation stack of Xamarin Forms.
            if (previousPage != null && navigationView.Navigation.NavigationStack.Count > 2)
            {
                navigationView.Navigation.RemovePage(previousPage);
            }
        }
    }
}

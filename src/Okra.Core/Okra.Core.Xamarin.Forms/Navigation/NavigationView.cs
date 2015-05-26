using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Okra.Navigation
{
    /// <summary>
    /// Class NavigationView.
    /// </summary>
    public class NavigationView : NavigationPage
    {
        private static readonly Page s_backPage = new Page();
        private static readonly Page s_emptyPage = new Page();
        private Page _homePage;

        /// <summary>
        /// The current page property name
        /// </summary>
        private const string CURRENT_PAGE_PROPERTY_NAME = "CurrentPage";

        private readonly INavigationBase _navigationManager;

        public NavigationView(INavigationBase navigationManager)
            : base(s_emptyPage)
        {
            _navigationManager = navigationManager;
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();

            //if (navigationManager.NavigationStack.CanGoBack)
            //{
            //    navigationManager.NavigationStack.GoBack();
            //    return true;
            //}

            //return false;
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == CURRENT_PAGE_PROPERTY_NAME)
            {
                if ((CurrentPage == s_backPage) && _navigationManager.NavigationStack.CanGoBack)
                {
                    _navigationManager.GoBack();
                }
            }

            base.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// 
        /// Initial stack:
        /// <EmptyPage>
        /// 
        /// Home page stack:
        /// <HomePage>
        /// 
        /// Second page stack:
        /// <HomePage> <--> <BackPage> <--> <SecondPage>
        /// 
        /// Third page stack:
        /// <HomePage> <--> <BackPage> <--> <ThirdPage>
        /// 
        /// </summary>
        /// <remarks>
        /// HomePage is kept in stack, since exception is thrown (at least on WP8) 
        /// when trying to push that page again even after being removed from stack.
        /// 
        /// BackPage is inserted before every page navigated to in order to be able 
        /// to detect back navigation from software back button in Android and iOS.
        /// </remarks>
        /// <param name="nextPage">The page to navigate to.</param>
        /// <returns>A task representing the asynchronous show operation.</returns>
        internal async Task ShowAsync(Page nextPage)
        {
            var firstPage = Navigation.NavigationStack.First();
            var secondPage = Navigation.NavigationStack.LastOrDefault();

            // Here we handle initial navigation when EmptyPage is to be displayed.
            if ((Navigation.NavigationStack.Count == 1) && (firstPage == s_emptyPage))
            {
                // HomePage instance is normally set, but might not on 
                // app resume when at the end of the navigation stack.
                if (_navigationManager.NavigationStack.CurrentPage.PageName == SpecialPageNames.Home)
                {
                    _homePage = nextPage;
                }

                await Navigation.PushAsync(nextPage);

                // Make sure BackPage is inserted before last page if we should be able 
                // to navigate back, in order for software back button to be displayed.
                if (_navigationManager.NavigationStack.CanGoBack && (firstPage != s_backPage))
                {
                    Navigation.InsertPageBefore(s_backPage, CurrentPage);
                }

                // Remove first dummy page (EmpptyPage).
                Navigation.RemovePage(firstPage);
                return;
            }

            if ((nextPage == _homePage) && !_navigationManager.NavigationStack.CanGoBack)
            {
                await Navigation.PopAsync();
                return;
            }
            else
            {
                await Navigation.PushAsync(nextPage);
            }

            // Make sure BackPage is inserted before last page if we should be able 
            // to navigate back, in order for software back button to be displayed.
            if (_navigationManager.NavigationStack.CanGoBack && (!Navigation.NavigationStack.Contains(s_backPage)))
            {
                Navigation.InsertPageBefore(s_backPage, CurrentPage);
            }

            // HomePage is kept in stack, since exception is thrown (at least on WP8) when trying 
            // to push that page again (upon back navigation) even after being removed from stack.
            if ((secondPage != null) && (secondPage != s_backPage) && (secondPage != _homePage))
            {
                Navigation.RemovePage(secondPage);
            }

            // If navigation stack is empty, remove first page if it is a back 
            // page, in order for next back navigation to exit application.
            if (!_navigationManager.NavigationStack.CanGoBack && (firstPage == s_backPage))
            {
                Navigation.RemovePage(s_backPage);
            }
        }
    }
}

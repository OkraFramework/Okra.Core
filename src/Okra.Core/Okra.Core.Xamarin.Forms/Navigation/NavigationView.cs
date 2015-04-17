using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace Okra.Navigation
{
    /// <summary>
    /// Class NavigationView.
    /// </summary>
    public class NavigationView : NavigationPage
    {
        private readonly INavigationBase navigationManager;
      
        public NavigationView(INavigationBase navigationManager)
        {
            this.navigationManager = navigationManager;
        }

        protected override bool OnBackButtonPressed()
        {
            if (navigationManager.NavigationStack.CanGoBack)
            {
                navigationManager.NavigationStack.GoBack();
                return true;
            }

            return false; // base.OnBackButtonPressed();
        }
    }
}

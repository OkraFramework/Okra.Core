using Okra.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    public static class NavigationBaseEx
    {
        public static void GoBack(this INavigationBase navigationBase)
        {
            // Validate Parameters

            if (!navigationBase.NavigationStack.CanGoBack)
                throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotGoBackWithEmptyBackStack"));

            // Delegate to the INavigationStack

            navigationBase.NavigationStack.GoBack();
        }

        public static void NavigateTo(this INavigationBase navigationBase, string pageName)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "pageName");

            if (!navigationBase.CanNavigateTo(pageName))
                throw new InvalidOperationException(string.Format(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotNavigateAsPageIsNotFound"), pageName));

            // Delegate to the INavigationStack

            navigationBase.NavigationStack.NavigateTo(pageName);
        }

        public static void NavigateTo(this INavigationBase navigationBase, string pageName, object arguments)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "pageName");

            if (!navigationBase.CanNavigateTo(pageName))
                throw new InvalidOperationException(string.Format(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotNavigateAsPageIsNotFound"), pageName));

            // Delegate to the INavigationStack

            navigationBase.NavigationStack.NavigateTo(pageName, arguments);
        }

        public static void NavigateTo(this INavigationBase navigationManager, Type pageName)
        {
            // Validate Parameters

            if (pageName == null)
                throw new ArgumentNullException("pageName");

            // Delegate to the INavigationManager

            navigationManager.NavigateTo(PageName.FromType(pageName));
        }

        public static void NavigateTo(this INavigationBase navigationManager, Type pageName, object arguments)
        {
            // Validate Parameters

            if (pageName == null)
                throw new ArgumentNullException("pageName");

            // Delegate to the INavigationManager

            navigationManager.NavigateTo(PageName.FromType(pageName), arguments);
        }
    }
}

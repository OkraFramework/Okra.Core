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

            if (navigationBase == null)
                throw new ArgumentNullException("navigationBase");

            if (!navigationBase.NavigationStack.CanGoBack)
                throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotGoBackWithEmptyBackStack"));

            // Delegate to the INavigationStack

            navigationBase.NavigationStack.GoBack();
        }

        public static void NavigateTo(this INavigationBase navigationBase, string pageName)
        {
            // Validate Parameters

            if (navigationBase == null)
                throw new ArgumentNullException("navigationBase");

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

            if (navigationBase == null)
                throw new ArgumentNullException("navigationBase");

            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "pageName");

            if (!navigationBase.CanNavigateTo(pageName))
                throw new InvalidOperationException(string.Format(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotNavigateAsPageIsNotFound"), pageName));

            // Delegate to the INavigationStack

            navigationBase.NavigationStack.NavigateTo(pageName, arguments);
        }

        public static void NavigateTo(this INavigationBase navigationBase, Type pageName)
        {
            // Validate Parameters

            if (navigationBase == null)
                throw new ArgumentNullException("navigationBase");

            if (pageName == null)
                throw new ArgumentNullException("pageName");

            string pageNameString = PageName.FromType(pageName);

            if (!navigationBase.CanNavigateTo(pageNameString))
                throw new InvalidOperationException(string.Format(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotNavigateAsPageIsNotFound"), pageNameString));

            // Delegate to the INavigationManager

            navigationBase.NavigationStack.NavigateTo(pageNameString);
        }

        public static void NavigateTo(this INavigationBase navigationBase, Type pageName, object arguments)
        {
            // Validate Parameters

            if (navigationBase == null)
                throw new ArgumentNullException("navigationBase");

            if (pageName == null)
                throw new ArgumentNullException("pageName");

            string pageNameString = PageName.FromType(pageName);

            if (!navigationBase.CanNavigateTo(pageNameString))
                throw new InvalidOperationException(string.Format(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotNavigateAsPageIsNotFound"), pageNameString));


            // Delegate to the INavigationManager

            navigationBase.NavigationStack.NavigateTo(pageNameString, arguments);
        }
    }
}

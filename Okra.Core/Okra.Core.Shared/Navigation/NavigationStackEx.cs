using Okra.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    public static class NavigationStackEx
    {
        public static void NavigateTo(this INavigationStack navigationStack, string pageName)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "pageName");

            // Delegate to the INavigationStack

            PageInfo navigationEntry = new PageInfo(pageName, null);
            navigationStack.NavigateTo(navigationEntry);
        }

        public static void NavigateTo(this INavigationStack navigationStack, string pageName, object arguments)
        {
            // Validate Parameters

            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "pageName");

            // Delegate to the INavigationStack

            PageInfo navigationEntry = new PageInfo(pageName, arguments);
            navigationStack.NavigateTo(navigationEntry);
        }

        public static void NavigateTo(this INavigationStack navigationStack, Type pageName)
        {
            // Validate Parameters

            if (pageName == null)
                throw new ArgumentNullException("pageName");

            // Convert the page type to a string and delegate this

            navigationStack.NavigateTo(PageName.FromType(pageName));
        }

        public static void NavigateTo(this INavigationStack navigationStack, Type pageName, object arguments)
        {
            // Validate Parameters

            if (pageName == null)
                throw new ArgumentNullException("pageName");

            // Convert the page type to a string and delegate this

            navigationStack.NavigateTo(PageName.FromType(pageName), arguments);
        }
    }
}

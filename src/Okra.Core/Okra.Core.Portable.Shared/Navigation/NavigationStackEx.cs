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
        public static void GoBackTo(this INavigationStack navigationStack, string pageName)
        {
            // Validate Parameters

            if (navigationStack == null)
                throw new ArgumentNullException(nameof(navigationStack));

            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), nameof(pageName));

            // Delegate to the INavigationStack

            PageInfo page = navigationStack.Last(p => p.PageName == pageName);
            navigationStack.GoBackTo(page);
        }

        public static void NavigateTo(this INavigationStack navigationStack, string pageName)
        {
            // Validate Parameters

            if (navigationStack == null)
                throw new ArgumentNullException(nameof(navigationStack));

            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), nameof(pageName));

            // Delegate to the INavigationStack

            PageInfo navigationEntry = new PageInfo(pageName, null);
            navigationStack.NavigateTo(navigationEntry);
        }

        public static void NavigateTo(this INavigationStack navigationStack, string pageName, object arguments)
        {
            // Validate Parameters

            if (navigationStack == null)
                throw new ArgumentNullException(nameof(navigationStack));

            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), nameof(pageName));

            // Delegate to the INavigationStack

            PageInfo navigationEntry = new PageInfo(pageName, arguments);
            navigationStack.NavigateTo(navigationEntry);
        }

        public static void NavigateTo(this INavigationStack navigationStack, Type pageName)
        {
            // Validate Parameters

            if (navigationStack == null)
                throw new ArgumentNullException(nameof(navigationStack));

            if (pageName == null)
                throw new ArgumentNullException(nameof(pageName));

            // Convert the page type to a string and delegate this

            PageInfo navigationEntry = new PageInfo(PageName.FromType(pageName), null);
            navigationStack.NavigateTo(navigationEntry);
        }

        public static void NavigateTo(this INavigationStack navigationStack, Type pageName, object arguments)
        {
            // Validate Parameters

            if (navigationStack == null)
                throw new ArgumentNullException(nameof(navigationStack));

            if (pageName == null)
                throw new ArgumentNullException(nameof(pageName));

            // Convert the page type to a string and delegate this

            PageInfo navigationEntry = new PageInfo(PageName.FromType(pageName), arguments);
            navigationStack.NavigateTo(navigationEntry);
        }
    }
}

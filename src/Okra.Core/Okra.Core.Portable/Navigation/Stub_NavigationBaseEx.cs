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
            throw new NotImplementedException();
        }

        public static void NavigateTo(this INavigationBase navigationBase, string pageName)
        {
            throw new NotImplementedException();
        }

        public static void NavigateTo(this INavigationBase navigationBase, string pageName, object arguments)
        {
            throw new NotImplementedException();
        }

        public static void NavigateTo(this INavigationBase navigationManager, Type pageName)
        {
            throw new NotImplementedException();
        }

        public static void NavigateTo(this INavigationBase navigationManager, Type pageName, object arguments)
        {
            throw new NotImplementedException();
        }
    }
}

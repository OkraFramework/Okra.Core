using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Okra.Navigation
{
    public static class NavigationCommands
    {
        // *** Methods ***

        public static ICommand GetGoBackCommand(this INavigationBase navigationBase)
        {
            throw new NotImplementedException();
        }

        public static ICommand GetNavigateToCommand(this INavigationBase navigationBase, string pageName, object arguments = null)
        {
            throw new NotImplementedException();
        }
    }
}

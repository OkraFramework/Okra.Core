using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

#if WINDOWS_APP
using Windows.UI.ApplicationSettings;
#endif

namespace Okra.Navigation
{
    public static class NavigationCommands
    {
        // *** Methods ***

        public static ICommand GetGoBackCommand(this INavigationBase navigationManager)
        {
            return new GoBackCommand(navigationManager);
        }

        public static ICommand GetNavigateToCommand(this INavigationBase navigationManager, string pageName, object arguments = null)
        {
            return new NavigateToCommand(navigationManager, pageName, arguments);
        }

#if WINDOWS_APP
        public static SettingsCommand GetNavigateToSettingsCommand(this INavigationBase navigationManager, string label, string pageName, object arguments = null)
        {
            NavigateToState state = new NavigateToState(navigationManager, pageName, arguments);

            return new SettingsCommand(state, label, NavigateToUICommand_Invoked);
        }
#endif

        // *** ICommand Implementations ***

        private class GoBackCommand : ICommand
        {
            // *** Fields ***

            private readonly INavigationBase navigationManager;

            // *** Events ***

            public event EventHandler CanExecuteChanged
            {
                // Simply use this as an alias for the INavigationBase.OnCanGoBackChanged event

                add
                {
                    navigationManager.CanGoBackChanged += value;
                }
                remove
                {
                    navigationManager.CanGoBackChanged -= value;
                }
            }

            // *** Constructors ***

            public GoBackCommand(INavigationBase navigationManager)
            {
                this.navigationManager = navigationManager;
            }

            // *** Methods ***

            public bool CanExecute(object parameter)
            {
                return navigationManager.CanGoBack;
            }

            public void Execute(object parameter)
            {
                if (navigationManager.CanGoBack)
                    navigationManager.GoBack();
            }
        }

        private class NavigateToCommand : ICommand
        {
            // *** Fields ***

            private readonly INavigationBase navigationManager;
            private readonly string pageName;
            private readonly object arguments;

            // *** Events ***

            public event EventHandler CanExecuteChanged;

            // *** Constructors ***

            public NavigateToCommand(INavigationBase navigationManager, string pageName, object arguments)
            {
                this.navigationManager = navigationManager;
                this.pageName = pageName;
                this.arguments = arguments;
            }

            // *** Methods ***

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                navigationManager.NavigateTo(pageName, arguments);
            }
        }

        // *** IUICommand State Classes ***

        private class NavigateToState
        {
            // *** Fields ***

            public readonly INavigationBase NavigationManager;
            public readonly string PageName;
            public readonly object Arguments;

            // *** Constructors ***

            public NavigateToState(INavigationBase navigationManager, string pageName, object arguments)
            {
                this.NavigationManager = navigationManager;
                this.PageName = pageName;
                this.Arguments = arguments;
            }
        }

        // *** IUICommand Implementations ***

        private static void NavigateToUICommand_Invoked(IUICommand command)
        {
            NavigateToState state = (NavigateToState)command.Id;

            state.NavigationManager.NavigateTo(state.PageName, state.Arguments);
        }
    }
}

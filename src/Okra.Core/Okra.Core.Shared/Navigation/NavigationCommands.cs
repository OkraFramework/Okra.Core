using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Popups;
using Okra.Helpers;

#if WINDOWS_APP || WINDOWS_UAP
using Windows.UI.ApplicationSettings;
#endif

namespace Okra.Navigation
{
    public static class NavigationCommands
    {
        // *** Methods ***

        public static ICommand GetGoBackCommand(this INavigationBase navigationBase)
        {
            // Validate Parameters

            if (navigationBase == null)
                throw new ArgumentNullException("navigationBase");

            // Return the command

            return new GoBackCommand(navigationBase);
        }

        public static ICommand GetNavigateToCommand(this INavigationBase navigationBase, string pageName, object arguments = null)
        {
            // Validate Parameters

            if (navigationBase == null)
                throw new ArgumentNullException("navigationBase");

            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "pageName");

            // Return the command

            return new NavigateToCommand(navigationBase, pageName, arguments);
        }

#if WINDOWS_APP || WINDOWS_UAP
        public static SettingsCommand GetNavigateToSettingsCommand(this INavigationBase navigationBase, string label, string pageName, object arguments = null)
        {
            // Validate Parameters

            if (navigationBase == null)
                throw new ArgumentNullException("navigationBase");

            if (string.IsNullOrEmpty(label))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "label");

            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "pageName");

            // Return the command

            NavigateToState state = new NavigateToState(navigationBase, pageName, arguments);

            return new SettingsCommand(state, label, NavigateToUICommand_Invoked);
        }
#endif

        // *** ICommand Implementations ***

        private class GoBackCommand : ICommand
        {
            // *** Fields ***

            private readonly INavigationBase navigationManager;

            // *** Events ***

            public event EventHandler CanExecuteChanged;

            // *** Constructors ***

            public GoBackCommand(INavigationBase navigationManager)
            {
                this.navigationManager = navigationManager;

                navigationManager.NavigationStack.PropertyChanged += NavigationStack_PropertyChanged;
            }

            // *** Methods ***

            public bool CanExecute(object parameter)
            {
                return navigationManager.NavigationStack.CanGoBack;
            }

            public void Execute(object parameter)
            {
                if (navigationManager.NavigationStack.CanGoBack)
                    navigationManager.NavigationStack.GoBack();
            }

            // *** Private Methods ***

            private void NavigationStack_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "CanGoBack")
                {
                    OnCanExecuteChanged();
                }
            }

            private void OnCanExecuteChanged()
            {
                EventHandler eventHandler = CanExecuteChanged;

                if (eventHandler != null)
                    eventHandler(this, new EventArgs());
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

            protected void OnCanExecuteChanged()
            {
                EventHandler eventHandler = CanExecuteChanged;

                if (eventHandler != null)
                    eventHandler(this, new EventArgs());
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

using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Okra.MEF.WinPhone
{
    public partial class App : Application
    {
        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            PhoneApplicationService.Current.Launching += Application_Launching;
            PhoneApplicationService.Current.Activated += Application_Activated;
            PhoneApplicationService.Current.Deactivated += Application_Deactivated;
            PhoneApplicationService.Current.Closing += Application_Closing;

        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, Microsoft.Phone.Shell.ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }
    }
}

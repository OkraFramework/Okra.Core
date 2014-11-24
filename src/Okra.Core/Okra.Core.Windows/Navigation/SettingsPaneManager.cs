using Okra.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Okra.Navigation
{
    public class SettingsPaneManager : NavigationBase, ISettingsPaneManager
    {
        // *** Fields ***

        private SettingsPaneHost settingsPaneHost;
        private bool isUnloading = false;

        // *** Events ***

        public event EventHandler FlyoutClosed;
        public event EventHandler FlyoutOpened;

        // *** Constructors ***

        public SettingsPaneManager(IViewFactory viewFactory)
            : base(viewFactory)
        {
        }

        protected SettingsPaneManager(IViewFactory viewFactory, INavigationStack navigationStack)
            : base(viewFactory, navigationStack)
        {
        }

        // *** Methods ***

        public void ShowSettingsPane()
        {
            SettingsPane.Show();
        }

        // *** Protected Methods ***
        
        protected void OnSettingsPaneBackClick(object sender, BackClickEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            e.Handled = true;
            this.GoBack();
        }

        protected void OnSettingsFlyoutUnloaded(object sender, object e)
        {
            // Raise the FlyoutClosed event

            OnFlyoutClosed();

            // Remove all navigation entries from the stack
            // NB: Set the 'isUnloading' flag to stop reopening the system settings pane
            // TODO : Add some way to indicate to VMs that they are closing - IClosingAware?

            isUnloading = false;
            NavigationStack.Clear();
            isUnloading = false;
        }

        protected void OnSettingsFlyoutLoaded(object sender, object e)
        {
            // Raise the FlyoutOpened event

            OnFlyoutOpened();
        }

        protected override void DisplayPage(object page)
        {
            // If we are responding to unloading of the settings pane, then just ignore this

            if (isUnloading)
                return;

            // If the page is null then close the flyout and show the system settings pane

            if (page == null)
            {
                if (settingsPaneHost != null)
                    settingsPaneHost.Hide();

                SettingsPane.Show();
            }

            // Otherwise navigate the flyout to the specified page

            else
            {
                // Lazy create the settings flyout

                if (settingsPaneHost == null)
                {
                    settingsPaneHost = new SettingsPaneHost();

                    settingsPaneHost.BackClick += OnSettingsPaneBackClick;
                    settingsPaneHost.Loaded += OnSettingsFlyoutLoaded;
                    settingsPaneHost.Unloaded += OnSettingsFlyoutUnloaded;
                }

                // Set the content for the settings flyout

                settingsPaneHost.Content = page;
                
                // Show the settings flyout

                ShowSettingsFlyout(settingsPaneHost);
            }
        }

        protected virtual void OnFlyoutClosed()
        {
            EventHandler eventHandler = FlyoutClosed;

            if (eventHandler != null)
                eventHandler(this, EventArgs.Empty);
        }

        protected virtual void OnFlyoutOpened()
        {
            EventHandler eventHandler = FlyoutOpened;

            if (eventHandler != null)
                eventHandler(this, EventArgs.Empty);
        }

        protected virtual void ShowSettingsFlyout(SettingsFlyout settingsFlyout)
        {
            if (settingsFlyout == null)
                throw new ArgumentNullException("settingsFlyout");

            // Show the settings flyout
            // NB: Call 'ShowIndependent()' rather than 'Show()' as we handle displaying the system settings pane as required

            settingsFlyout.ShowIndependent();
        }
    }
}

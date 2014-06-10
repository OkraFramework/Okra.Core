using Okra.Helpers;
using Okra.UI;
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

        private SettingsFlyout settingsFlyout;

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
            e.Handled = true;
            this.GoBack();
        }

        protected void OnSettingsFlyoutUnloaded(object sender, object e)
        {
            // Raise the FlyoutClosed event

            OnFlyoutClosed();

            // Remove all navigation entries from the stack
            // TODO : Add some way to indicate to VMs that they are closing - IClosingAware?

            NavigationStack.Clear();
        }

        protected void OnSettingsFlyoutLoaded(object sender, object e)
        {
            // Raise the FlyoutOpened event

            OnFlyoutOpened();
        }

        protected override void DisplayPage(object page)
        {
            // If the page is null then close the flyout and show the system settings pane

            if (page == null)
            {
                if (settingsFlyout != null)
                    settingsFlyout.Hide();

                SettingsPane.Show();
            }

            // Otherwise navigate the flyout to the specified page

            else
            {
                // Lazy create the settings flyout

                if (settingsFlyout == null)
                {
                    settingsFlyout = new SettingsFlyout();

                    settingsFlyout.BackClick += OnSettingsPaneBackClick;
                    settingsFlyout.Loaded += OnSettingsFlyoutLoaded;
                    settingsFlyout.Unloaded += OnSettingsFlyoutUnloaded;
                }

                // Set the content for the settings flyout

                settingsFlyout.Content = page;

                // Style the settings flyout based upon properties specified by the page

                if (page is DependencyObject)
                {
                    settingsFlyout.Title = SettingsPaneInfo.GetTitle((DependencyObject)page);
                    settingsFlyout.IconSource = SettingsPaneInfo.GetIconSource((DependencyObject)page);

                    Brush headerBackground = SettingsPaneInfo.GetHeaderBackground((DependencyObject)page);

                    if (headerBackground != null)
                        settingsFlyout.HeaderBackground = headerBackground;
                    else
                        settingsFlyout.ClearValue(SettingsFlyout.HeaderBackgroundProperty);

                    Brush headerForeground = SettingsPaneInfo.GetHeaderForeground((DependencyObject)page);

                    if (headerForeground != null)
                        settingsFlyout.HeaderForeground = headerForeground;
                    else
                        settingsFlyout.ClearValue(SettingsFlyout.HeaderForegroundProperty);
                }

                // Show the settings flyout

                ShowSettingsFlyout(settingsFlyout);
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
            // Show the settings flyout
            // NB: Call 'ShowIndependent()' rather than 'Show()' as we handle displaying the system settings pane as required

            settingsFlyout.ShowIndependent();
        }
    }
}

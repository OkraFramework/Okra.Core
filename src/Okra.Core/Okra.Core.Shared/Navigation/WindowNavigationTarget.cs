using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Okra.Navigation
{
    public class WindowNavigationTarget : INavigationTarget
    {
        // *** Fields ***

        private bool eventHandlersRegistered;
        private ContentControl contentHost;
        private INavigationBase navigationManager;

        // *** Methods ***

        public void NavigateTo(object page, INavigationBase navigationManager)
        {
            this.navigationManager = navigationManager;

            if (!eventHandlersRegistered)
            {
                RegisterEventHandlers();
                eventHandlersRegistered = true;
            }

            SetWindowContent(page);
        }

        // *** Protected Methods ***

        protected virtual void RegisterEventHandlers()
        {
#if WINDOWS_PHONE_APP
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
#endif

#if WINDOWS_APP
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += Window_AcceleratorKeyActivated;
            Window.Current.CoreWindow.PointerPressed += Window_PointerPressed;
#endif
        }

        protected virtual void SetWindowContent(object page)
        {
            // If the content host has not been created then create this

            if (contentHost == null)
            {
                contentHost = new ContentControl()
                {
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Stretch
                };
            }

            // Ensure that the window content is set to the content host

            if (Window.Current.Content != contentHost)
                Window.Current.Content = contentHost;

            // Set the content to display

            contentHost.Content = page;
        }

        // *** Event Handlers ***

#if WINDOWS_PHONE_APP
        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            if (navigationManager != null && navigationManager.NavigationStack.CanGoBack)
            {
                e.Handled = true;
                navigationManager.NavigationStack.GoBack();
            }
        }
#endif

#if WINDOWS_APP

        protected void Window_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            VirtualKey key = args.VirtualKey;
            CoreAcceleratorKeyEventType eventType = args.EventType;

            // Only continue processing if we are pressing the left, right, back or forward keys

            if ((eventType == CoreAcceleratorKeyEventType.KeyDown || eventType == CoreAcceleratorKeyEventType.SystemKeyDown) &&
                (key == VirtualKey.Left || key == VirtualKey.Right || key == VirtualKey.GoBack || key == VirtualKey.GoForward))
            {
                // Get the current modifier state

                CoreWindow coreWindow = Window.Current.CoreWindow;
                bool menuKey = (coreWindow.GetKeyState(VirtualKey.Menu) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
                bool controlKey = (coreWindow.GetKeyState(VirtualKey.Control) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
                bool shiftKey = (coreWindow.GetKeyState(VirtualKey.Shift) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
                bool noModifiers = !menuKey && !controlKey && !shiftKey;
                bool onlyAlt = menuKey && !controlKey && !shiftKey;

                // Handle going back via keyboard (Alt+Left or GoBack)

                if ((key == VirtualKey.Left && onlyAlt) || (key== VirtualKey.GoBack && noModifiers))
                {
                    args.Handled = true;
                    GoBack();
                }

                // Handle going forward via keyboard (Alt+Right or GoForward)

                if ((key == VirtualKey.Right && onlyAlt) || (key == VirtualKey.GoForward && noModifiers))
                {
                    args.Handled = true;
                    GoForward();
                }
            }
        }

        protected void Window_PointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            PointerPointProperties properties = args.CurrentPoint.Properties;

            // Ignore button chords with the left, right, and middle buttons
            if (properties.IsLeftButtonPressed || properties.IsRightButtonPressed ||
                properties.IsMiddleButtonPressed) return;

            // If back or foward are pressed (but not both) navigate appropriately
            bool backPressed = properties.IsXButton1Pressed;
            bool forwardPressed = properties.IsXButton2Pressed;

            if (backPressed && !forwardPressed)
            {
                args.Handled = true;
                GoBack();
            }

            if (!backPressed && forwardPressed)
            {
                args.Handled = true;
                GoForward();
            }
        }

#endif

        // *** Private Methods ***

        private void GoBack()
        {
            if (navigationManager != null && navigationManager.NavigationStack.CanGoBack)
                navigationManager.NavigationStack.GoBack();
        }

        private void GoForward()
        {
            // Currently not implemented in navigation stack so ignore these events
        }
    }
}

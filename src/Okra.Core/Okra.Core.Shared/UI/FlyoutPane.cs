using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;

namespace Okra.UI
{
    public class FlyoutPane
    {
        // *** Fields ***

        private Popup popup;
        private ContentControl contentControl;

        private readonly FlyoutEdge flyoutEdge;
        private readonly bool isLightDismissEnabled;

        // *** Events ***

        public event EventHandler Closed;
        public event EventHandler Opened;

        // *** Constructors ***

        public FlyoutPane(FlyoutEdge flyoutEdge = FlyoutEdge.Right, bool isLightDismissEnabled = false)
        {
            this.flyoutEdge = flyoutEdge;
            this.isLightDismissEnabled = isLightDismissEnabled;
        }

        // *** Properties ***

        public FlyoutEdge FlyoutEdge
        {
            get
            {
                return flyoutEdge;
            }
        }

        public bool IsLightDismissEnabled
        {
            get
            {
                return isLightDismissEnabled;
            }
        }

        public bool IsOpen
        {
            get
            {
                if (popup == null)
                    return false;
                else
                    return popup.IsOpen;
            }
        }

        // *** Methods ***

        public void Close()
        {
            // Hide the popup

            popup.IsOpen = false;
        }

        public void Show(object content)
        {
            // Create the popup if required

            if (popup == null)
                CreatePopup();

            // Update the content control

            contentControl.Content = content;
            contentControl.Measure(new Size(double.PositiveInfinity, contentControl.Height));

            // Position the flyout on the screen

            SetFlyoutPosition();

            // Ensure that the popup is displayed

            popup.IsOpen = true;
        }

        // *** Protected Methods ***

        protected virtual void OnClosed()
        {
            EventHandler eventHandler = Closed;

            if (eventHandler != null)
                eventHandler(this, EventArgs.Empty);
        }

        protected virtual void OnOpened()
        {
            EventHandler eventHandler = Opened;

            if (eventHandler != null)
                eventHandler(this, EventArgs.Empty);
        }

        // *** Private Methods ***

        private void CreatePopup()
        {
            // Create the transitions

            TransitionCollection childTransitions = new TransitionCollection();
            childTransitions.Add(new PaneThemeTransition() { Edge =  GetPopupTransitionEdge() });

            // Create a new Popup to display the content

            popup = new Popup()
                {
                    IsLightDismissEnabled = isLightDismissEnabled,
                    ChildTransitions = childTransitions
                };

            // Register for the Opened and Closed events

            popup.Opened += Popup_Opened;
            popup.Closed += Popup_Closed;

            // Create a content control to display the content

            contentControl = new ContentControl()
                {
                    Height = Window.Current.Bounds.Height,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Stretch
                };

            popup.Child = contentControl;
        }

        private EdgeTransitionLocation GetPopupTransitionEdge()
        {
            switch (flyoutEdge)
            {
                case FlyoutEdge.Left:
                    return EdgeTransitionLocation.Left;
                case FlyoutEdge.Right:
                    return EdgeTransitionLocation.Right;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void SetFlyoutPosition()
        {
            // Position the flyout on the screen

            double width = contentControl.DesiredSize.Width;
            double left = flyoutEdge == FlyoutEdge.Left ? 0.0 : Window.Current.Bounds.Width - width;

            Canvas.SetTop(popup, 0);
            Canvas.SetLeft(popup, left);

            // Make sure that the flyout is always the height of the screen

            contentControl.Height = Window.Current.Bounds.Height;
        }

        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            // Reset the flyout position every time the window size changes

            SetFlyoutPosition();
        }

        private void Popup_Closed(object sender, object e)
        {
            // Unsubscribe from the Window.SizeChanged event

            Window.Current.SizeChanged -= Window_SizeChanged;

            // Clear the content control (so the garbage collector can dispose of it)

            contentControl.Content = null;

            // Raise the 'Closed' event

            OnClosed();
        }

        private void Popup_Opened(object sender, object e)
        {
            // Register to the Window.SizeChanged event

            Window.Current.SizeChanged += Window_SizeChanged;

            // Raise the 'Opened' event

            OnOpened();
        }
    }
}

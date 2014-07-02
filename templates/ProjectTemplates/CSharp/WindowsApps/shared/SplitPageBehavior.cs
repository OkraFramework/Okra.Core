using Microsoft.Xaml.Interactivity;
using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace $safeprojectname$.Common
{
    /// <summary>
    /// Behavior that handles split page logic
    /// </summary>
    public class SplitPageBehavior : DependencyObject, IBehavior
    {
        // *** Dependency Properties ***

        public static readonly DependencyProperty MaximisedVisualStateProperty = DependencyProperty.Register("MaximisedVisualState", typeof(string), typeof(SplitPageBehavior), new PropertyMetadata(null));
        public static readonly DependencyProperty MinimisedVisualStateProperty = DependencyProperty.Register("MinimisedVisualState", typeof(string), typeof(SplitPageBehavior), new PropertyMetadata(null));
        public static readonly DependencyProperty MinimisedDetailVisualStateProperty = DependencyProperty.Register("MinimisedDetailVisualState", typeof(string), typeof(SplitPageBehavior), new PropertyMetadata(null));
        public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register("SelectedObject", typeof(object), typeof(SplitPageBehavior), new PropertyMetadata(null, PropertyChanged_SelectedObject));
        public static readonly DependencyProperty SplitWidthProperty = DependencyProperty.Register("SplitWidth", typeof(double), typeof(SplitPageBehavior), new PropertyMetadata(768.0));

        // *** Fields ***

        private DependencyObject associatedObject;
        private bool isMinimised;

        // *** Properties ***

        public string MaximisedVisualState
        {
            get { return (string)GetValue(MaximisedVisualStateProperty); }
            set { SetValue(MaximisedVisualStateProperty, value); }
        }

        public string MinimisedVisualState
        {
            get { return (string)GetValue(MinimisedVisualStateProperty); }
            set { SetValue(MinimisedVisualStateProperty, value); }
        }

        public string MinimisedDetailVisualState
        {
            get { return (string)GetValue(MinimisedDetailVisualStateProperty); }
            set { SetValue(MinimisedDetailVisualStateProperty, value); }
        }

        public object SelectedObject
        {
            get { return (object)GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }

        public double SplitWidth
        {
            get { return (double)GetValue(SplitWidthProperty); }
            set { SetValue(SplitWidthProperty, value); }
        }

        public DependencyObject AssociatedObject
        {
            get
            {
                return this.associatedObject;
            }
        }

        // *** Methods ***

        public void Attach(DependencyObject associatedObject)
        {
            if (associatedObject == this.associatedObject || DesignMode.DesignModeEnabled)
                return;

            if (this.associatedObject != null)
                throw new InvalidOperationException();

            this.associatedObject = associatedObject;
            RegisterEvent();
        }

        public void Detach()
        {
            UnregisterEvent();
            this.associatedObject = null;
        }

        // *** Private Methods ***

        private void RegisterEvent()
        {
            if (AssociatedObject is FrameworkElement)
                ((FrameworkElement)AssociatedObject).SizeChanged += AssociatedObject_SizeChanged;
        }

        private void UnregisterEvent()
        {
            if (AssociatedObject is FrameworkElement)
                ((FrameworkElement)AssociatedObject).SizeChanged -= AssociatedObject_SizeChanged;
        }

        private void InvalidateVisualState()
        {
            if (!(AssociatedObject is FrameworkElement))
                return;

            Control control = VisualStateUtilities.FindNearestStatefulControl((FrameworkElement)AssociatedObject);

            if (control != null)
            {
                string stateName = DetermineVisualState();
                VisualStateManager.GoToState(control, stateName, true);
            }
        }

        private string DetermineVisualState()
        {
            if (isMinimised)
                if (SelectedObject != null)
                    return MinimisedDetailVisualState;
                else
                    return MinimisedVisualState;
            else
                return MaximisedVisualState;
        }

        private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            bool oldIsMinimised = isMinimised;
            isMinimised = e.NewSize.Width < SplitWidth;

            if (oldIsMinimised != isMinimised)
                InvalidateVisualState();
        }

        // *** Private Static Methods ***

        private static void PropertyChanged_SelectedObject(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((e.OldValue == null && e.NewValue != null) || (e.OldValue != null && e.NewValue == null))
                ((SplitPageBehavior)d).InvalidateVisualState();
        }
    }
}

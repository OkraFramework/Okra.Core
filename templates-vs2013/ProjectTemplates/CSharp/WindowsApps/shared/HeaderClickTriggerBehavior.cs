using Microsoft.Xaml.Interactivity;
using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;

namespace $safeprojectname$.Common
{
    /// <summary>
    /// Behavior trigger that fires upon clicking of buttons in a control's header section
    /// </summary>
    [ContentProperty(Name = "Actions")]
    public class HeaderClickTriggerBehavior : DependencyObject, IBehavior
    {
        // *** Dependency Properties ***

        public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register("Actions", typeof(ActionCollection), typeof(HeaderClickTriggerBehavior), new PropertyMetadata(null));

        // *** Fields ***

        private DependencyObject associatedObject;

        // *** Properties ***

        public ActionCollection Actions
        {
            get
            {
                ActionCollection value = (ActionCollection)base.GetValue(HeaderClickTriggerBehavior.ActionsProperty);
                if (value == null)
                {
                    value = new ActionCollection();
                    base.SetValue(HeaderClickTriggerBehavior.ActionsProperty, value);
                }
                return value;
            }
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
            if (AssociatedObject is ButtonBase)
                ((ButtonBase)AssociatedObject).Click += AssociatedObject_Click;
        }

        private void UnregisterEvent()
        {
            if (AssociatedObject is ButtonBase)
                ((ButtonBase)AssociatedObject).Click -= AssociatedObject_Click;
        }

        void AssociatedObject_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement)
            {
                object headerItem = ((FrameworkElement)sender).DataContext;
                Interaction.ExecuteActions(AssociatedObject, this.Actions, headerItem);
            }
        }
    }
}

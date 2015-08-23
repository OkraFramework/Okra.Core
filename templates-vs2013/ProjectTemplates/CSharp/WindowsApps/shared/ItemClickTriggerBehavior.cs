using Microsoft.Xaml.Interactivity;
using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace $safeprojectname$.Common
{
    /// <summary>
    /// Behavior trigger that fires upon clicking of items in a ListViewBase derived control
    /// </summary>
    [ContentProperty(Name = "Actions")]
    public class ItemClickTriggerBehavior : DependencyObject, IBehavior
    {
        // *** Dependency Properties ***

        public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register("Actions", typeof(ActionCollection), typeof(ItemClickTriggerBehavior), new PropertyMetadata(null));

        // *** Fields ***

        private DependencyObject associatedObject;

        // *** Properties ***

        public ActionCollection Actions
        {
            get
            {
                ActionCollection value = (ActionCollection)base.GetValue(ItemClickTriggerBehavior.ActionsProperty);
                if (value == null)
                {
                    value = new ActionCollection();
                    base.SetValue(ItemClickTriggerBehavior.ActionsProperty, value);
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
            if (AssociatedObject is ListViewBase)
                ((ListViewBase)AssociatedObject).ItemClick += AssociatedObject_ItemClick;
        }

        private void UnregisterEvent()
        {
            if (AssociatedObject is ListViewBase)
                ((ListViewBase)AssociatedObject).ItemClick -= AssociatedObject_ItemClick;
        }

        private void AssociatedObject_ItemClick(object sender, ItemClickEventArgs e)
        {
            Interaction.ExecuteActions(AssociatedObject, this.Actions, e.ClickedItem);
        }
    }
}

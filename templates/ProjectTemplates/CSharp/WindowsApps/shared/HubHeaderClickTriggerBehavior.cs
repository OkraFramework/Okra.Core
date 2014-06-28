using Microsoft.Xaml.Interactivity;
using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace $safeprojectname$.Common
{
    /// <summary>
    /// Behavior trigger that fires upon clicking of a Hub control's section header
    /// </summary>
    [ContentProperty(Name = "Actions")]
    public class HubHeaderClickTriggerBehavior : DependencyObject, IBehavior
    {
        // *** Dependency Properties ***

        public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register("Actions", typeof(ActionCollection), typeof(HubHeaderClickTriggerBehavior), new PropertyMetadata(null));

        // *** Fields ***

        private DependencyObject associatedObject;

        // *** Properties ***

        public ActionCollection Actions
        {
            get
            {
                ActionCollection value = (ActionCollection)base.GetValue(HubHeaderClickTriggerBehavior.ActionsProperty);
                if (value == null)
                {
                    value = new ActionCollection();
                    base.SetValue(HubHeaderClickTriggerBehavior.ActionsProperty, value);
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
            if (AssociatedObject is Hub)
                ((Hub)AssociatedObject).SectionHeaderClick += AssociatedObject_SectionHeaderClick;
        }

        private void UnregisterEvent()
        {
            if (AssociatedObject is Hub)
                ((Hub)AssociatedObject).SectionHeaderClick -= AssociatedObject_SectionHeaderClick;
        }

        void AssociatedObject_SectionHeaderClick(object sender, HubSectionHeaderClickEventArgs e)
        {
            object sectionHeaderItem = e.Section.DataContext;
            Interaction.ExecuteActions(AssociatedObject, this.Actions, sectionHeaderItem);
        }
    }
}

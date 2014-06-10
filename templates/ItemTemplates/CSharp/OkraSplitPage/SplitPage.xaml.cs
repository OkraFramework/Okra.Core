using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel;

// The Split Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234234

namespace $rootnamespace$
{
    /// <summary>
    /// A page that displays a group title, a list of items within the group, and details for the
    /// currently selected item.
    /// </summary>
    [PageExport("$fileinputname$")]
    public sealed partial class $safeitemname$ : $safeprojectname$.Common.LayoutAwarePage
    {
        public $safeitemname$()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // When the page is loaded then attach to the view-model for property changed events

            if (DataContext is  $safeitemname$ViewModel)
                (($safeitemname$ViewModel)DataContext).PropertyChanged += ViewModel_PropertyChanged;
        }

        #region Logical page navigation

        // Visual state management typically reflects the four application view states directly
        // (full screen landscape and portrait plus snapped and filled views.)  The split page is
        // designed so that the snapped and portrait view states each have two distinct sub-states:
        // either the item list or the details are displayed, but not both at the same time.
        //
        // This is all implemented with a single physical page that can represent two logical
        // pages.  The code below achieves this goal without making the user aware of the
        // distinction.

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // When the 'SelectedItem' property of the view-model changes then update the visual state

            if (e.PropertyName == "SelectedItem")
                this.InvalidateVisualState();
        }

        /// <summary>
        /// Invoked to determine the name of the visual state that corresponds to an application
        /// view state.
        /// </summary>
        /// <param name="viewState">The view state for which the question is being posed.</param>
        /// <returns>The name of the desired visual state.  This is the same as the name of the
        /// view state except when there is a selected item in portrait and snapped views where
        /// this additional logical page is represented by adding a suffix of _Detail.</returns>
        protected override string DetermineVisualState(ApplicationViewState viewState)
        {
            $safeitemname$ViewModel viewModel = (($safeitemname$ViewModel)DataContext);

            viewModel.UsingLogicalPageNavigation = viewState == ApplicationViewState.FullScreenPortrait || viewState == ApplicationViewState.Snapped;
            bool logicalItemView = viewModel.UsingLogicalPageNavigation && viewModel.SelectedItem != null;

            // Determine visual states for landscape layouts based not on the view state, but
            // on the width of the window.  This page has one layout that is appropriate for
            // 1366 virtual pixels or wider, and another for narrower displays or when a snapped
            // application reduces the horizontal space available to less than 1366.
            if (viewState == ApplicationViewState.Filled ||
                viewState == ApplicationViewState.FullScreenLandscape)
            {
                var windowWidth = Window.Current.Bounds.Width;
                if (windowWidth >= 1366) return "FullScreenLandscapeOrWide";
                return "FilledOrNarrow";
            }

            // When in portrait or snapped start with the default visual state name, then add a
            // suffix when viewing details instead of the list
            var defaultStateName = base.DetermineVisualState(viewState);
            return logicalItemView ? defaultStateName + "_Detail" : defaultStateName;
        }

        #endregion
    }
}

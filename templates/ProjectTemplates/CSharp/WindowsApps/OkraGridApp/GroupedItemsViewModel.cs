using $safeprojectname$.Common;
using $safeprojectname$.Data;
using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace $safeprojectname$
{
    /// <summary>
    /// A view model for displaying a grouped collection of items.
    /// </summary>
    [ViewModelExport("Home")]
    public sealed partial class GroupedItemsViewModel : ViewModelBase
    {
        private IEnumerable<SampleDataGroup> groups;

        [ImportingConstructor]
        public GroupedItemsViewModel(INavigationContext navigationContext)
            : base(navigationContext)
        {
            Initialize();
        }

        public IEnumerable<SampleDataGroup> Groups
        {
            get
            {
                return groups;
            }
            protected set
            {
                SetProperty(ref groups, value);
            }
        }

        /// <summary>
        /// Initializes the page with content.
        /// </summary>
        private async void Initialize()
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var sampleDataGroups = await SampleDataSource.GetGroupsAsync();
            this.Groups = sampleDataGroups;
        }

        ///// <summary>
        ///// Invoked when a group header is clicked.
        ///// </summary>
        ///// <param name="sender">The Button used as a group header for the selected group.</param>
        ///// <param name="e">Event data that describes how the click was initiated.</param>
        //void Header_Click(object sender, RoutedEventArgs e)
        //{
        //    // Determine what group the Button instance represents
        //    var group = (sender as FrameworkElement).DataContext;

        //    // Navigate to the appropriate destination page, configuring the new page
        //    // by passing required information as a navigation parameter
        //    this.Frame.Navigate(typeof(GroupDetailPage), ((SampleDataGroup)group).UniqueId);
        //}

        ///// <summary>
        ///// Invoked when an item within a group is clicked.
        ///// </summary>
        ///// <param name="sender">The GridView (or ListView when the application is snapped)
        ///// displaying the item clicked.</param>
        ///// <param name="e">Event data that describes the item clicked.</param>
        //void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //    // Navigate to the appropriate destination page, configuring the new page
        //    // by passing required information as a navigation parameter
        //    var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
        //    this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        //}
    }
}
using Okra.Navigation;
using $safeprojectname$.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace $safeprojectname$.Pages.Home
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    [PageExport("Home")]
    public sealed partial class HomePage : $safeprojectname$.Common.LayoutAwarePage
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when an item within a group is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            // NB: This code-behind is required due to the current lack of behaviour support

            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            ((HomeViewModel)DataContext).NavigateToItemDetail(itemId);
        }
    }
}

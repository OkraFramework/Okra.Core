using $safeprojectname$.Common;
using $safeprojectname$.Data;
using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace $safeprojectname$
{
    /// <summary>
    /// A view model for displaying details for a single item within a group.
    /// </summary>
    [ViewModelExport("ItemDetail")]
    public sealed partial class ItemDetailViewModel : ViewModelBase
    {
        //private NavigationHelper navigationHelper;
        //private ObservableDictionary defaultViewModel = new ObservableDictionary();

        ///// <summary>
        ///// NavigationHelper is used on each page to aid in navigation and 
        ///// process lifetime management
        ///// </summary>
        //public NavigationHelper NavigationHelper
        //{
        //    get { return this.navigationHelper; }
        //}

        ///// <summary>
        ///// This can be changed to a strongly typed view model.
        ///// </summary>
        //public ObservableDictionary DefaultViewModel
        //{
        //    get { return this.defaultViewModel; }
        //}

        //public ItemDetailPage()
        //{
        //    this.InitializeComponent();
        //    this.navigationHelper = new NavigationHelper(this);
        //    this.navigationHelper.LoadState += navigationHelper_LoadState;
        //}

        ///// <summary>
        ///// Populates the page with content passed during navigation.  Any saved state is also
        ///// provided when recreating a page from a prior session.
        ///// </summary>
        ///// <param name="sender">
        ///// The source of the event; typically <see cref="NavigationHelper"/>
        ///// </param>
        ///// <param name="e">Event data that provides both the navigation parameter passed to
        ///// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        ///// a dictionary of state preserved by this page during an earlier
        ///// session.  The state will be null the first time a page is visited.</param>
        //private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        //{
        //    // TODO: Create an appropriate data model for your problem domain to replace the sample data
        //    var item = await SampleDataSource.GetItemAsync((String)e.NavigationParameter);
        //    this.DefaultViewModel["Item"] = item;
        //}
    }
}
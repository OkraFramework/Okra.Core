using $safeprojectname$.Common;
using $safeprojectname$.Data;
using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace $safeprojectname$
{
    /// <summary>
    /// A view model for displaying a collection of item previews.  In the Split App this page
    /// is used to display and select one of the available groups.
    /// </summary>
    [ViewModelExport("Home")]
    public class ItemsViewModel : ViewModelBase
    {
        private IEnumerable<SampleDataGroup> items;

        [ImportingConstructor]
        public ItemsViewModel(INavigationContext navigationContext)
            : base(navigationContext)
        {
            Initialize();
        }

        public IEnumerable<SampleDataGroup> Items
        {
            get
            {
                return items;
            }
            protected set
            {
                SetProperty(ref items, value);
            }
        }

        /// <summary>
        /// Initializes the page with content.
        /// </summary>
        private async void Initialize()
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var sampleDataGroups = await SampleDataSource.GetGroupsAsync();
            this.Items = sampleDataGroups;
        }

        public void NavigateToSplitView(object sender, SampleDataGroup group)
        {
            NavigationManager.NavigateTo("Split", group.UniqueId);
        }
    }
}

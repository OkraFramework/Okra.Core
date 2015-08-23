using $safeprojectname$.Common;
using $safeprojectname$.Data;
using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Hub Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=321224

namespace $safeprojectname$
{
    /// <summary>
    /// A view model for displaying a grouped collection of items.
    /// </summary>
    [ViewModelExport("Home")]
    public class HubViewModel : ViewModelBase
    {
        private SampleDataGroup section3Items;

        [ImportingConstructor]
        public HubViewModel(INavigationContext navigationContext)
            : base(navigationContext)
        {
            Initialize();
        }

        public SampleDataGroup Section3Items
        {
            get
            {
                return section3Items;
            }
            protected set
            {
                SetProperty(ref section3Items, value);
            }
        }

        /// <summary>
        /// Initializes the page with content.
        /// </summary>
        private async void Initialize()
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-4");
            this.Section3Items = sampleDataGroup;
        }

        public void NavigateToItemDetail(object sender, SampleDataItem item)
        {
            NavigationManager.NavigateTo("Item", item.UniqueId);
        }

        public void NavigateToSectionDetail(object sender, SampleDataGroup group)
        {
            NavigationManager.NavigateTo("Section", group.UniqueId);
        }
    }
}

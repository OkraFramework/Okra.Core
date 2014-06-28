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

        public void NavigateToItemDetail(object sender, SampleDataItem item)
        {
            NavigationManager.NavigateTo("ItemDetail", item.UniqueId);
        }

        public void NavigateToGroupDetail(object sender, SampleDataGroup group)
        {
            NavigationManager.NavigateTo("GroupDetail", group.UniqueId);
        }
    }
}
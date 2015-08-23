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

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace $safeprojectname$
{
    /// <summary>
    /// A view model for displaying an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    [ViewModelExport("GroupDetail")]
    public class GroupDetailViewModel : ViewModelBase, IActivatable
    {
        private SampleDataGroup group;
        private IEnumerable<SampleDataItem> items;

        [ImportingConstructor]
        public GroupDetailViewModel(INavigationContext navigationContext)
            : base(navigationContext)
        {
        }

        public SampleDataGroup Group
        {
            get
            {
                return group;
            }
            protected set
            {
                SetProperty(ref group, value);
            }
        }

        public IEnumerable<SampleDataItem> Items
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
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="pageInfo">Information on the arguments and state passed to the page.</param>
        public async void Activate(PageInfo pageInfo)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data

            string groupId = pageInfo.GetArguments<string>();
            var group = await SampleDataSource.GetGroupAsync(groupId);
            this.Group = group;
            this.Items = group.Items;
        }

        /// <summary>
        /// Saves any state to be recreated in a future session.
        /// </summary>
        /// <param name="pageInfo">Object to store page state.</param>
        public void SaveState(PageInfo pageInfo)
        {
        }

        public void NavigateToItemDetail(object sender, SampleDataItem item)
        {
            NavigationManager.NavigateTo("ItemDetail", item.UniqueId);
        }
    }
}
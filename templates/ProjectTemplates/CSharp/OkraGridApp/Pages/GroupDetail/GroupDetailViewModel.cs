using Okra.Navigation;
using $safeprojectname$.Common;
using $safeprojectname$.Data;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Pages.GroupDetail
{
    /// <summary>
    /// A view model for displaying an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    [ViewModelExport("GroupDetail")]
    public class GroupDetailViewModel : ViewModelBase, IActivatable<string, string>
    {
        private SampleDataGroup group;
        private IEnumerable<SampleDataItem> items;

        protected GroupDetailViewModel()
        {
        }

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
        /// <param name="arguments">The parameter value passed when this page was initially requested.</param>
        /// <param name="state">State preserved by this page during an earlier session.  This will be null
        /// the first time a page is visited.</param>
        public void Activate(string arguments, string state)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var group = SampleDataSource.GetGroup(arguments);
            this.Group = group;
            this.Items = group.Items;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.
        /// </summary>
        /// <returns>The state to serialize.</returns>
        public string SaveState()
        {
            // No state is required to preserve the group detail page
            return null;
        }

        /// <summary>
        /// Navigates to the 'ItemDetail' page associated with the supplied unique-ID.
        /// </summary>
        /// <param name="uniqueId">The ID of the item to display.</param>
        public void NavigateToItemDetail(string uniqueId)
        {
            NavigationManager.NavigateTo("ItemDetail", uniqueId);
        }
    }
}

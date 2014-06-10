using Okra.Core;
using Okra.Navigation;
using $safeprojectname$.Common;
using $safeprojectname$.Data;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace $safeprojectname$.Pages.Home
{
    /// <summary>
    /// A view-model for displaying a grouped collection of items.
    /// </summary>
    [ViewModelExport("Home")]
    public class HomeViewModel : ViewModelBase
    {
        private IEnumerable<SampleDataGroup> groups;

        protected HomeViewModel()
        {
        }

        [ImportingConstructor]
        public HomeViewModel(INavigationContext navigationContext)
            : base(navigationContext)
        {
            Initialize();
            InitializeCommands();
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

        public ICommand NavigateToGroupDetailCommand { get; private set; }

        /// <summary>
        /// Initializes the page with content.
        /// </summary>
        private void Initialize()
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            Groups = SampleDataSource.GetGroups("AllGroups");
        }

        /// <summary>
        /// Initializes all commands with their associates actions.
        /// </summary>
        private void InitializeCommands()
        {
            NavigateToGroupDetailCommand = new DelegateCommand<string>(NavigateToGroupDetail);
        }

        /// <summary>
        /// Navigates to the 'GroupDetail' page associated with the supplied unique-ID.
        /// </summary>
        /// <param name="uniqueId">The ID of the item to display.</param>
        private void NavigateToGroupDetail(string uniqueId)
        {
            NavigationManager.NavigateTo("GroupDetail", uniqueId);
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

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
    /// A view-model for displaying a collection of item previews.
    /// </summary>
    [ViewModelExport("Home")]
    public class HomeViewModel : ViewModelBase
    {
        private IEnumerable<SampleDataGroup> items;

        protected HomeViewModel()
        {
        }

        [ImportingConstructor]
        public HomeViewModel(INavigationContext navigationContext)
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
        private void Initialize()
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            Items = SampleDataSource.GetGroups("AllGroups");
        }

        /// <summary>
        /// Navigates to the 'Split' page associated with the supplied unique-ID.
        /// </summary>
        /// <param name="uniqueId">The ID of the item to display.</param>
        public void NavigateToSplitPage(string uniqueId)
        {
            NavigationManager.NavigateTo("Split", uniqueId);
        }
    }
}

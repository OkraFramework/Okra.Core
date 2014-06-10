using Okra.Core;
using Okra.Navigation;
using $safeprojectname$.Common;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.ViewManagement;

namespace $rootnamespace$
{
    /// <summary>
    /// A view model for displaying a group title, a list of items within the group, and details for the
    /// currently selected item.
    /// </summary>
    [ViewModelExport("$fileinputname$")]
    public class $fileinputname$ViewModel : ViewModelBase, IActivatable<string, string>
    {
        private SampleDataGroup group;
        private IEnumerable<SampleDataItem> items;
        private SampleDataItem selectedItem;
        private bool usingLogicalPageNavigation;

        protected $fileinputname$ViewModel()
        {
        }

        [ImportingConstructor]
        public $fileinputname$ViewModel(INavigationContext navigationContext)
            : base(navigationContext)
        {
            InitializeCommands();
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

        public SampleDataItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                SetProperty(ref selectedItem, value);
            }
        }

        public bool UsingLogicalPageNavigation
        {
            get
            {
                return usingLogicalPageNavigation;
            }
            set
            {
                SetProperty(ref usingLogicalPageNavigation, value);
            }
        }

        /// <summary>
        /// A command that can be bound to UI to navigate backward in the navigation stack.
        /// </summary>
        public new ICommand GoBackCommand { get; private set; }

        private void InitializeCommands()
        {
            // Override the default ViewModelBase GoBackCommand implementation
            this.GoBackCommand = new DelegateCommand(GoBack);
        }

        private void GoBack()
        {
            // If we are using logical page navigation and an item is selected then just unselect the item.
            // Otherwise go back via the navigation manager.

            if (UsingLogicalPageNavigation && SelectedItem != null)
                SelectedItem = null;
            else
                NavigationManager.GoBack();
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
            // TODO: Assign a bindable group to this.Group
            // TODO: Assign a collection of bindable items to this.Items

            if (state == null)
            {
                // When this is a new page, select the first item automatically unless logical page
                // navigation is being used (see the logical page navigation #region below.)

                ApplicationViewState viewState = ApplicationView.Value;

                if (viewState == ApplicationViewState.FullScreenPortrait || viewState == ApplicationViewState.Snapped)
                    this.SelectedItem = null;
                else
                    this.SelectedItem = Items.FirstOrDefault();
            }
            else
            {
                // Restore the previously saved state associated with this page
                // TODO : Implement logic such as "this.SelectedItem = SampleDataSource.GetItem(state);"
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.
        /// </summary>
        /// <returns>The state to serialize.</returns>
        public string SaveState()
        {
            // Preserve the item ID in case the application is suspended or the page discarded.

            if (selectedItem != null)
                return selectedItem.UniqueId;
            else
                return null;
        }

        // TODO : Remove these placeholder data types and replace all references with those from your actual data model.

        public class SampleDataGroup
        {
            public string Title { get; set; }
        }

        public class SampleDataItem
        {
            public string UniqueId { get; private set; }

            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Content { get; set; }
        }
    }
}

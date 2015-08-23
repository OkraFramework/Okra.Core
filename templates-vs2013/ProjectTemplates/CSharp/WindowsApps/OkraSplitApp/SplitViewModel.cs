using $safeprojectname$.Common;
using $safeprojectname$.Data;
using Okra.Core;
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

// The Split Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234234

namespace $safeprojectname$
{
    /// <summary>
    /// A view model for displaying a group title, a list of items within the group, and details for
    /// the currently selected item.
    /// </summary>
    [ViewModelExport("Split")]
    public class SplitViewModel : ViewModelBase, IActivatable
    {
        private SampleDataGroup group;
        private IEnumerable<SampleDataItem> items;
        private SampleDataItem selectedItem;

        [ImportingConstructor]
        public SplitViewModel(INavigationContext navigationContext)
            : base(navigationContext)
        {
            ClearSelectionCommand = new DelegateCommand(ClearSelection);
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

        public ICommand ClearSelectionCommand
        {
            get;
            private set;
        }

        public void ClearSelection()
        {
            SelectedItem = null;
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

            // Restore the selected item

            string selectedItemId;

            if (pageInfo.TryGetState<string>("SelectedItem", out selectedItemId))
                SelectedItem = await SampleDataSource.GetItemAsync(selectedItemId);
            else
                SelectedItem = null;
        }

        /// <summary>
        /// Saves any state to be recreated in a future session.
        /// </summary>
        /// <param name="pageInfo">Object to store page state.</param>
        public void SaveState(PageInfo pageInfo)
        {
            if (SelectedItem != null)
                pageInfo.SetState<string>("SelectedItem", SelectedItem.UniqueId);
            else
                pageInfo.SetState<string>("SelectedItem", null);
        }
    }
}
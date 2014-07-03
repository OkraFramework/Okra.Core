using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using $safeprojectname$.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace $rootnamespace$
{
    /// <summary>
    /// A view model for displaying details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    [ViewModelExport("$fileinputname$")]
    public class $safeitemname$ : ViewModelBase, IActivatable
    {
        private SampleDataGroup group;
        private IEnumerable<SampleDataItem> items;
        private SampleDataItem selectedItem;

        [ImportingConstructor]
        public $safeitemname$(INavigationContext navigationContext)
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

        public SampleDataItem SelectedItem
        {
            get
            {
                return selectedItem;
            }
            protected set
            {
                SetProperty(ref selectedItem, value);
            }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="pageInfo">Information on the arguments and state passed to the page.</param>
        public async void Activate(PageInfo pageInfo)
        {
            string navigationParameter;
            pageInfo.TryGetState<string>("SelectedItem", out navigationParameter);

            // TODO: Assign a bindable group to this.Group
            // TODO: Assign a collection of bindable items to this.Items
            // TODO: Assign the selected item to this.SelectedItem
        }

        /// <summary>
        /// Saves any state to be recreated in a future session.
        /// </summary>
        /// <param name="pageInfo">Object to store page state.</param>
        public void SaveState(PageInfo pageInfo)
        {
            // Save the selected item to the "SelectedItem" state key of 'pageInfo'
        }

        // TODO : Replace references to placeholder classes with actual data objects

        public class SampleDataGroup {}
        public class SampleDataItem {}
    }
}

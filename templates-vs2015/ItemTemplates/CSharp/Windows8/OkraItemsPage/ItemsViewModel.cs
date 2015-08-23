using $safeprojectname$.Common;
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

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace $rootnamespace$
{
    /// <summary>
    /// A view model for displaying a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    [ViewModelExport("$fileinputname$")]
    public class $safeitemname$ : ViewModelBase, IActivatable
    {
        private IEnumerable<SampleDataItem> items;

        [ImportingConstructor]
        public $safeitemname$(INavigationContext navigationContext)
            : base(navigationContext)
        {
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
            // TODO: Assign a bindable collection of items to this.Items
        }

        /// <summary>
        /// Saves any state to be recreated in a future session.
        /// </summary>
        /// <param name="pageInfo">Object to store page state.</param>
        public void SaveState(PageInfo pageInfo)
        {
        }

        // TODO : Replace references to placeholder classes with actual data objects

        public class SampleDataItem {}
    }
}

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

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace $rootnamespace$
{
    /// <summary>
    /// A view model for displaying a grouped collection of items.
    /// </summary>
    [ViewModelExport("$fileinputname$")]
    public class $safeitemname$ : ViewModelBase, IActivatable
    {
        private IEnumerable<SampleDataGroup> groups;

        [ImportingConstructor]
        public $safeitemname$(INavigationContext navigationContext)
            : base(navigationContext)
        {
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
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="pageInfo">Information on the arguments and state passed to the page.</param>
        public async void Activate(PageInfo pageInfo)
        {
            // TODO: Assign a collection of bindable groups to this.Groups
        }

        /// <summary>
        /// Saves any state to be recreated in a future session.
        /// </summary>
        /// <param name="pageInfo">Object to store page state.</param>
        public void SaveState(PageInfo pageInfo)
        {
        }

        // TODO : Replace references to placeholder classes with actual data objects

        public class SampleDataGroup {}
    }
}

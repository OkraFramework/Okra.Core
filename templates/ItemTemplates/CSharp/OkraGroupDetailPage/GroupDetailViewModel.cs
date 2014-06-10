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

namespace $rootnamespace$
{
    /// <summary>
    /// A view-model for displaying a grouped collection of items.
    /// </summary>
    [ViewModelExport("$fileinputname$")]
    public class $fileinputname$ViewModel : ViewModelBase, IActivatable<string, string>
    {
        private SampleDataGroup group;
        private IEnumerable<SampleDataItem> items;

        protected $fileinputname$ViewModel()
        {
        }

        [ImportingConstructor]
        public $fileinputname$ViewModel(INavigationContext navigationContext)
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
            // TODO: Assign a bindable group to this.Group
            // TODO: Assign a collection of bindable items to this.Items
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

        // TODO : Remove these placeholder data types and replace all references with those from your actual data model.

        public class SampleDataItem
        {
            public string UniqueId { get; private set; }

            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Content { get; set; }
        }

        public class SampleDataGroup
        {
            public string UniqueId { get; private set; }

            public string Title { get; set; }
            public IEnumerable<SampleDataItem> TopItems { get; set; }
        }
    }
}

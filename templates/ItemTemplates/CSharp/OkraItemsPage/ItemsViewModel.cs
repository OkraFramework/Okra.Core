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
    /// A view-model for displaying a collection of item previews.
    /// </summary>
    [ViewModelExport("$fileinputname$")]
    public class $fileinputname$ViewModel : ViewModelBase
    {
        private IEnumerable<SampleDataItem> items;

        protected $fileinputname$ViewModel()
        {
        }

        [ImportingConstructor]
        public $fileinputname$ViewModel(INavigationContext navigationContext)
            : base(navigationContext)
        {
            Initialize();
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
        /// Initializes the page with content.
        /// </summary>
        private void Initialize()
        {
            // TODO: Assign a bindable collection of items to this.Items
        }

        // TODO : Remove these placeholder data types and replace all references with those from your actual data model.

        public class SampleDataItem
        {
            public string UniqueId { get; private set; }

            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Content { get; set; }
        }
    }
}

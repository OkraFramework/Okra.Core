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

namespace $safeprojectname$.Pages.Home
{
    /// <summary>
    /// A view-model for displaying a grouped collection of items.
    /// </summary>
    [ViewModelExport("Home")]
    public class HomeViewModel : ViewModelBase
    {
        protected HomeViewModel()
        {
        }

        [ImportingConstructor]
        public HomeViewModel(INavigationContext navigationContext)
            : base(navigationContext)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the page with content.
        /// </summary>
        private void Initialize()
        {
            // TODO: Create an appropriate data model for your problem domain
        }
    }
}

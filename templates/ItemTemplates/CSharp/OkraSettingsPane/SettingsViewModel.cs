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
    /// A basic view-model that provides characteristics common to most applications.
    /// </summary>
    [ViewModelExport("$fileinputname$")]
    public class $fileinputname$ViewModel : ViewModelBase
    {
        protected $fileinputname$ViewModel()
        {
        }

        [ImportingConstructor]
        public $fileinputname$ViewModel(INavigationContext navigationContext)
            : base(navigationContext)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes the page with content.
        /// </summary>
        private void Initialize()
        {
            // TODO: Initialize run-time data here
        }
    }
}

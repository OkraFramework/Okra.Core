using Okra.Core;
using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace $safeprojectname$.Common
{
    /// <summary>
    /// Typical implementation of a view-model base class. This includes,
    /// <list type="bullet">
    /// <item>
    /// <description>Property changed notification support.</description>
    /// </item>
    /// <item>
    /// <description>GoBack command.</description>
    /// </item>
    /// </list>
    /// </summary>
    public abstract class ViewModelBase : NotifyPropertyChangedBase
    {
        public ViewModelBase()
        {
        }

        public ViewModelBase(INavigationContext navigationContext)
        {
            this.NavigationManager = navigationContext.GetCurrent();

            InitializeCommands();
        }

        /// <summary>
        /// A command that can be bound to UI to navigate backward in the navigation stack.
        /// </summary>
        public ICommand GoBackCommand { get; private set; }

        /// <summary>
        /// An instance of the current navigation manager to perform any custom navigation.
        /// </summary>
        protected INavigationBase NavigationManager { get; private set; }

        private void InitializeCommands()
        {
            this.GoBackCommand = NavigationManager.GetGoBackCommand();
        }
    }
}

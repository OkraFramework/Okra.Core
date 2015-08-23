using Okra;
using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    public class AppBootstrapper : OkraBootstrapper
    {
        /// <summary>
        /// Perform general initialization of application services.
        /// </summary>
        protected override void SetupServices()
        {
            // Configure the navigation manager to preseve application state in local storage
            NavigationManager.NavigationStorageType = NavigationStorageType.Local;
        }
    }
}
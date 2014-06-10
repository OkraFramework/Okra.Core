using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    [Export(typeof(INavigationContext))]
    [Shared("page")]
    public class NavigationContextProxy : INavigationContext
    {
        // *** Fields ***

        private INavigationContext navigationContext;

        // *** Constructors ***

        public NavigationContextProxy()
        {
        }

        // *** Methods ***

        public INavigationBase GetCurrent()
        {
            return navigationContext.GetCurrent();
        }

        public void SetNavigationContext(INavigationContext navigationContext)
        {
            this.navigationContext = navigationContext;
        }
    }
}

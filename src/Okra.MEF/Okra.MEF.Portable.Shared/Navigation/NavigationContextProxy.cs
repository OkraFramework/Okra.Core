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

        private INavigationContext _navigationContext;

        // *** Constructors ***

        public NavigationContextProxy()
        {
        }

        // *** Methods ***

        public INavigationBase GetCurrent()
        {
            return _navigationContext.GetCurrent();
        }

        public void SetNavigationContext(INavigationContext navigationContext)
        {
            if (navigationContext == null)
                throw new ArgumentNullException(nameof(navigationContext));

            _navigationContext = navigationContext;
        }
    }
}

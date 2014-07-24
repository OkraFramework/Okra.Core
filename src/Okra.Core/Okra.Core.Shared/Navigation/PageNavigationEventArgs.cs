using Okra.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Okra.Navigation
{
    public class PageNavigationEventArgs : EventArgs
    {
        // *** Fields ***

        private readonly PageInfo page;
        private readonly PageNavigationMode navigationMode;

        // *** Constructors ***

        public PageNavigationEventArgs(PageInfo page, PageNavigationMode navigationMode)
        {
            // Validate arguments

            if (page == null)
                throw new ArgumentNullException("page");

            if (!Enum.IsDefined(typeof(PageNavigationMode), navigationMode))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_SpecifiedEnumIsNotDefined"), "navigationMode");

            // Set properties

            this.page = page;
            this.navigationMode = navigationMode;
        }

        // *** Properties ***

        public PageNavigationMode NavigationMode
        {
            get
            {
                return navigationMode;
            }
        }

        public PageInfo Page
        {
            get
            {
                return page;
            }
        }
    }
}

using Okra.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Okra.Navigation
{
    public class PageNavigationEventArgs : EventArgs
    {
        // *** Fields ***

        private readonly PageInfo _page;
        private readonly PageNavigationMode _navigationMode;

        // *** Constructors ***

        public PageNavigationEventArgs(PageInfo page, PageNavigationMode navigationMode)
        {
            // Validate arguments

            if (page == null)
                throw new ArgumentNullException(nameof(page));

            if (!Enum.IsDefined(typeof(PageNavigationMode), navigationMode))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_SpecifiedEnumIsNotDefined"), nameof(navigationMode));

            // Set properties

            _page = page;
            _navigationMode = navigationMode;
        }

        // *** Properties ***

        public PageNavigationMode NavigationMode
        {
            get
            {
                return _navigationMode;
            }
        }

        public PageInfo Page
        {
            get
            {
                return _page;
            }
        }
    }
}

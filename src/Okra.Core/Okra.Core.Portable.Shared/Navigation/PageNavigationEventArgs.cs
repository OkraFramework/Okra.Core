using Okra.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Okra.Navigation
{
    public class PageNavigationEventArgs : EventArgs
    {
        // *** Constructors ***

        public PageNavigationEventArgs(PageInfo page, PageNavigationMode navigationMode)
        {
            // Validate arguments

            if (page == null)
                throw new ArgumentNullException(nameof(page));

            if (!Enum.IsDefined(typeof(PageNavigationMode), navigationMode))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_SpecifiedEnumIsNotDefined"), nameof(navigationMode));

            // Set properties

            this.Page = page;
            this.NavigationMode = navigationMode;
        }

        // *** Properties ***

        public PageNavigationMode NavigationMode { get; }
        public PageInfo Page { get; }
    }
}

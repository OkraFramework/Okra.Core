using Okra.Helpers;
using Okra.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    public class PageEntry
    {
        // *** Constructors ***

        public PageEntry(string pageName, IStateService pageState)
        {
            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), nameof(pageName));

            if (pageState == null)
                throw new ArgumentNullException(nameof(pageState));

            this.PageName = pageName;
            this.PageState = pageState;
        }

        // *** Properties ***

        public string PageName { get; }
        public IStateService PageState { get; }

        // *** Methods ***

        public override string ToString() => PageName;
    }
}

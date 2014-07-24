using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    public class PageNavigationEventArgs : EventArgs
    {
        // *** Constructors ***

        public PageNavigationEventArgs(PageInfo page, PageNavigationMode navigationMode)
        {
            throw new NotImplementedException();
        }

        // *** Properties ***

        public PageNavigationMode NavigationMode
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public PageInfo Page
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}

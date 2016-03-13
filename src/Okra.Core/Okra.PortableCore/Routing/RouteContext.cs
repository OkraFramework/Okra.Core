using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Routing
{
    public class RouteContext
    {
        public RouteContext()
        {
        }

        public RouteContext(PageInfo page)
        {
            this.Page = page;
        }

        public PageInfo Page
        {
            get;
        }
    }
}

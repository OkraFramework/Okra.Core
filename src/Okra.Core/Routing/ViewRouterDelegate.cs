using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Routing
{
    public delegate Task<ViewInfo> ViewRouterDelegate(RouteContext context);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Routing
{
    public interface IRouteBuilder
    {
        IServiceProvider ApplicationServices { get; }

        ViewRouterDelegate Build();
        IRouteBuilder AddRouter(Func<ViewRouterDelegate, ViewRouterDelegate> router);
    }
}

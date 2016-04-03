using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Navigation;

namespace Okra.Routing
{
    public class ViewRouterProxy : IViewRouter
    {
        private readonly IRouteBuilder _routeBuilder;
        private readonly Lazy<ViewRouterDelegate> _router;

        public ViewRouterProxy(IRouteBuilder routeBuilder)
        {
            _routeBuilder = routeBuilder;
            _router = new Lazy<ViewRouterDelegate>(BuildRouter);
        }

        public Task<object> GetViewAsync(PageInfo pageInfo, IServiceProvider pageServices)
        {
            RouteContext context = new RouteContext(pageInfo, pageServices);
            return _router.Value(context);
        }

        private ViewRouterDelegate BuildRouter()
        {
            return _routeBuilder.Build();
        }
    }
}

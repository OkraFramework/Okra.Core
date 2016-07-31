using System;
using System.Threading.Tasks;

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

        public Task<ViewInfo> GetViewAsync(string pageName, IServiceProvider pageServices)
        {
            RouteContext context = new RouteContext(pageName, pageServices);
            return _router.Value(context);
        }

        private ViewRouterDelegate BuildRouter()
        {
            return _routeBuilder.Build();
        }
    }
}

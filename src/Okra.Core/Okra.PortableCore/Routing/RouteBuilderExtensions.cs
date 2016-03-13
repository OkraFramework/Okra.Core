using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Routing
{
    public static class RouteBuilderExtensions
    {
        public static IRouteBuilder AddRoute(this IRouteBuilder routeBuilder, ViewRouterDelegate route)
        {
            return routeBuilder.AddRouter(next =>
            {
                return async context =>
                {
                    var view = await route(context);

                    if (view != null)
                        return view;
                    else
                        return next(context);
                };
            });
        }

        public static IRouteBuilder MapRoute(this IRouteBuilder routeBuilder, string pageName, Type viewType)
        {
            return routeBuilder.AddRoute(context =>
            {
                if (context.Page.PageName == pageName)
                    return Task.FromResult<object>(routeBuilder.ApplicationServices.GetService(viewType));
                else
                    return Task.FromResult<object>(null);
            });
        }
    }
}

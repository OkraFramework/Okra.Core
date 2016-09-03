using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

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
                        return await next(context);
                };
            });
        }

        public static IRouteBuilder MapRoute(this IRouteBuilder routeBuilder, string pageName, Type viewType)
        {
            return routeBuilder.MapRoute(pageName, viewType, null);
        }

        public static IRouteBuilder MapRoute(this IRouteBuilder routeBuilder, string pageName, Type viewType, Type viewModelType)
        {
            return routeBuilder.AddRoute(context =>
            {
                if (context.PageName == pageName)
                {
                    var view = context.PageServices.GetRequiredService(viewType);
                    var viewModel = viewModelType != null ? context.PageServices.GetService(viewModelType) : null;
                    var viewInfo = new ViewInfo(view, viewModel);
                    return Task.FromResult(viewInfo);
                }
                else
                    return Task.FromResult<ViewInfo>(null);
            });
        }
    }
}

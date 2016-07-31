using System;

namespace Okra.Routing
{
    public interface IRouteBuilder
    {
        IServiceProvider ApplicationServices { get; }

        ViewRouterDelegate Build();
        IRouteBuilder AddRouter(Func<ViewRouterDelegate, ViewRouterDelegate> router);
    }
}

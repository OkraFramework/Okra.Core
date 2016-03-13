using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Routing
{
    public class RouteBuilder : IRouteBuilder
    {
        // *** Fields ***

        private readonly IServiceProvider _serviceProvider;
        private readonly List<Func<ViewRouterDelegate, ViewRouterDelegate>> _routerList = new List<Func<ViewRouterDelegate, ViewRouterDelegate>>();

        // *** Constructors ***

        public RouteBuilder(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            this._serviceProvider = serviceProvider;
        }

        public IServiceProvider ApplicationServices
        {
            get
            {
                return _serviceProvider;
            }
        }

        public ViewRouterDelegate Build()
        {
            ViewRouterDelegate activationDelegate = context => Task.FromResult<object>(null);

            foreach (var middleware in _routerList.Reverse<Func<ViewRouterDelegate, ViewRouterDelegate>>())
                activationDelegate = middleware(activationDelegate);

            return activationDelegate;
        }

        public IRouteBuilder AddRouter(Func<ViewRouterDelegate, ViewRouterDelegate> router)
        {
            if (router == null)
                throw new ArgumentNullException(nameof(router));

            _routerList.Add(router);

            return this;
        }
    }
}

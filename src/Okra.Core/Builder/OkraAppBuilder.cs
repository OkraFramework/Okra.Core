using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Okra.Builder
{
    public class OkraAppBuilder : IOkraAppBuilder
    {
        // *** Fields ***

        private readonly IServiceProvider _serviceProvider;
        private readonly List<Func<AppLaunchDelegate, AppLaunchDelegate>> _middlewareList = new List<Func<AppLaunchDelegate, AppLaunchDelegate>>();

        // *** Constructors ***

        public OkraAppBuilder(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            this._serviceProvider = serviceProvider;
        }

        // *** Properties ***

        public IServiceProvider ApplicationServices
        {
            get
            {
                return _serviceProvider;
            }
        }

        // *** Methods ***

        public AppLaunchDelegate Build()
        {
            AppLaunchDelegate appLaunchDelegate = context => Task.FromResult(false);

            foreach (var middleware in _middlewareList.Reverse<Func<AppLaunchDelegate, AppLaunchDelegate>>())
                appLaunchDelegate = middleware(appLaunchDelegate);

            return appLaunchDelegate;
        }

        public IOkraAppBuilder Use(Func<AppLaunchDelegate, AppLaunchDelegate> middleware)
        {
            if (middleware == null)
                throw new ArgumentNullException(nameof(middleware));

            _middlewareList.Add(middleware);

            return this;
        }
    }
}

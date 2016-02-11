using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Builder
{
    public class OkraAppBuilder : IOkraAppBuilder
    {
        // *** Fields ***

        private readonly IServiceProvider _serviceProvider;
        private readonly List<Func<ActivationDelegate, ActivationDelegate>> _middlewareList = new List<Func<ActivationDelegate, ActivationDelegate>>();

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

        public ActivationDelegate Build()
        {
            ActivationDelegate activationDelegate = context => Task.FromResult(false);

            foreach (var middleware in _middlewareList.Reverse<Func<ActivationDelegate, ActivationDelegate>>())
                activationDelegate = middleware(activationDelegate);

            return activationDelegate;
        }

        public IOkraAppBuilder Use(Func<ActivationDelegate, ActivationDelegate> middleware)
        {
            if (middleware == null)
                throw new ArgumentNullException(nameof(middleware));

            _middlewareList.Add(middleware);

            return this;
        }
    }
}

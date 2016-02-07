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

        public IOkraAppBuilder Use(Func<ActivationDelegate, ActivationDelegate> middleware)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Okra.Services
{
    public interface ILifetimeManager
    {
        // *** Methods ***

        void Register(ILifetimeAware service);
        void Unregister(ILifetimeAware service);
    }
}

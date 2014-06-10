using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Services
{
    public interface ILifetimeManager
    {
        // *** Methods ***

        void Register(ILifetimeAware service);
        void Unregister(ILifetimeAware service);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Lifetime
{
    public interface ILifetimeManager
    {
        void Register(ILifetimeAware service);
        void Unregister(ILifetimeAware service);
    }
}

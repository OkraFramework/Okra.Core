using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Lifetime
{
    public interface ILifetimeManagerSource
    {
        ILifetimeManager LifetimeManager { get; }

        Task Activate();
        Task Deactivate();
    }
}

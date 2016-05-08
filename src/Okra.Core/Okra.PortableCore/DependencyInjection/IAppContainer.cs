using Okra.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.DependencyInjection
{
    public interface IAppContainer : IDisposable
    {
        IAppContainer Parent { get; }
        IServiceProvider Services { get; }
        ILifetimeManager LifetimeManager { get; }

        Task Activate();
        IAppContainer CreateChildContainer();
        Task Deactivate();

    }
}

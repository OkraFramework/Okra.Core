using Okra.Lifetime;
using System;
using System.Threading.Tasks;

namespace Okra.DependencyInjection
{
    public interface IAppContainer : IDisposable
    {
        IAppContainer Parent { get; }
        IServiceProvider Services { get; }
        ILifetimeManager LifetimeManager { get; }

        Task Activate();
        Task Deactivate();
    }
}

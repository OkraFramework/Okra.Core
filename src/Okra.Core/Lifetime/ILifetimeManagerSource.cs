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

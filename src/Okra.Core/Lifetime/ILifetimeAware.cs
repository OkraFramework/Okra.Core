using System.Threading.Tasks;

namespace Okra.Lifetime
{
    public interface ILifetimeAware
    {
        Task Activate();
        Task Deactivate();
    }
}

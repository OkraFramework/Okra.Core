using Okra.Lifetime;
using System.Threading.Tasks;

namespace Okra.Builder
{
    public interface IMiddleware<TOptions>
    {
        void Configure(AppLaunchDelegate next, TOptions options);
        Task Invoke(AppLaunchContext context);
    }
}

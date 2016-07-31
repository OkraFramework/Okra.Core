using Microsoft.Extensions.DependencyInjection;

namespace Okra.DependencyInjection
{
    public interface IMvvmCoreBuilder
    {
        IServiceCollection Services { get; }
    }
}

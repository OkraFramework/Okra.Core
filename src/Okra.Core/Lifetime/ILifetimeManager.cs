namespace Okra.Lifetime
{
    public interface ILifetimeManager
    {
        void Register(ILifetimeAware service);
        void Unregister(ILifetimeAware service);
    }
}

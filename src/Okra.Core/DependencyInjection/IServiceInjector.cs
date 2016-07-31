namespace Okra.DependencyInjection
{
    public interface IServiceInjector<T>
    {
        bool HasValue { get; }
        T Service { get; set; }
    }
}

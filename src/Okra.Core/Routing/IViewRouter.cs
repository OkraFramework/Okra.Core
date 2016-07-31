using System;
using System.Threading.Tasks;

namespace Okra.Routing
{
    public interface IViewRouter
    {
        Task<ViewInfo> GetViewAsync(string pageName, IServiceProvider pageServices);
    }
}

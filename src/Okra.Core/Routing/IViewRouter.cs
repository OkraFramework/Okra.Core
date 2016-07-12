using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Routing
{
    public interface IViewRouter
    {
        Task<ViewInfo> GetViewAsync(string pageName, IServiceProvider pageServices);
    }
}

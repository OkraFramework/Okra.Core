using System.Threading.Tasks;

namespace Okra.Routing
{
    public delegate Task<ViewInfo> ViewRouterDelegate(RouteContext context);
}

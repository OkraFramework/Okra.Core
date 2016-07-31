using System;

namespace Okra.Routing
{
    public class RouteContext
    {
        public RouteContext()
        {
        }

        public RouteContext(string pageName, IServiceProvider pageServices)
        {
            this.PageName = pageName;
            this.PageServices = pageServices;
        }

        public string PageName
        {
            get;
        }

        public IServiceProvider PageServices
        {
            get;
        }
    }
}

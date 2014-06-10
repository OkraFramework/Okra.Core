using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    public interface IViewFactory
    {
        // *** Methods ***

        IViewLifetimeContext CreateView(string name, INavigationContext context);
        bool IsViewDefined(string name);
    }
}

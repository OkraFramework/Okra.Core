using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    public interface INavigationBase
    {
        // *** Properties ***

        INavigationStack NavigationStack { get; }

        // *** Methods ***

        bool CanNavigateTo(string pageName);
        IEnumerable<object> GetPageElements(PageInfo page);
    }
}

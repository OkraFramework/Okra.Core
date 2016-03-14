using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    internal class NavigationContext : INavigationContext
    {
        // *** Fields ***

        private readonly INavigationBase _current;

        // *** Constructors ***

        public NavigationContext(INavigationBase current)
        {
            _current = current;
        }

        // *** Methods ***

        public INavigationBase GetCurrent() => _current;
    }
}

using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okra.MEF.Tests.Mocks
{
    public class MockNavigationContext : INavigationContext
    {
        // *** Fields ***

        private readonly INavigationBase _current;

        // *** Constructors ***

        public MockNavigationContext(INavigationBase current)
        {
            _current = current;
        }

        // *** Methods ***

        public INavigationBase GetCurrent()
        {
            return _current;
        }
    }
}

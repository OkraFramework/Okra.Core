using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okra.MEF.Tests.Mocks
{
    public class MockNavigationContext : INavigationContext
    {
        // *** Fields ***

        private readonly INavigationBase current;

        // *** Constructors ***

        public MockNavigationContext(INavigationBase current)
        {
            this.current = current;
        }

        // *** Methods ***

        public INavigationBase GetCurrent()
        {
            return current;
        }
    }
}

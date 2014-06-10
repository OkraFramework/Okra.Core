using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okra.Tests.Mocks
{
    public class MockNavigationTarget : INavigationTarget
    {
        // *** Fields ***

        public List<object> NavigateToCalls = new List<object>();

        // *** Methods ***

        public void NavigateTo(object page)
        {
            NavigateToCalls.Add(page);
        }
    }
}

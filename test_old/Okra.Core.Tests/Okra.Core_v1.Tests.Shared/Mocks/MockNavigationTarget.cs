using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okra.Tests.Mocks
{
    public class MockNavigationTarget : INavigationTarget
    {
        // *** Fields ***

        public List<Tuple<object, INavigationBase>> NavigateToCalls = new List<Tuple<object, INavigationBase>>();

        // *** Methods ***

        public void NavigateTo(object page, INavigationBase navigationManager)
        {
            NavigateToCalls.Add(Tuple.Create(page, navigationManager));
        }
    }
}

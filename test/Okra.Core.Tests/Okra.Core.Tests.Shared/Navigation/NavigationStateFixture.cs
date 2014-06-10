using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Tests.Navigation
{
    [TestClass]
    public class NavigationStateFixture
    {
        // *** Property Tests ***

        [TestMethod]
        public void NavigationStack_IsEmptyCollection()
        {
            NavigationState state = new NavigationState();

            Assert.IsNotNull(state.NavigationStack);
            Assert.AreEqual(0, state.NavigationStack.Count);
        }
    }
}

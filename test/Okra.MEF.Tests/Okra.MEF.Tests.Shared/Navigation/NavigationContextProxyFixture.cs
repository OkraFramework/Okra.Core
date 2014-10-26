using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Okra.Navigation;
using Okra.MEF.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Okra.MEF.Tests.Navigation
{
    [TestClass]
    public class NavigationContextProxyFixture
    {
        [TestMethod]
        public void GetCurrent_ReturnsResultFromWrappedContext()
        {
            INavigationBase navigationBase = new MockNavigationManager();
            INavigationContext context = new MockNavigationContext(navigationBase);

            NavigationContextProxy proxy = new NavigationContextProxy();
            proxy.SetNavigationContext(context);

            Assert.AreEqual(navigationBase, proxy.GetCurrent());
        }

        [TestMethod]
        public void SetNavigationContext_ThrowsException_IfNavigationContextIsNull()
        {
            NavigationContextProxy proxy = new NavigationContextProxy();
            Assert.ThrowsException<ArgumentNullException>(() => proxy.SetNavigationContext(null));
        }
    }
}

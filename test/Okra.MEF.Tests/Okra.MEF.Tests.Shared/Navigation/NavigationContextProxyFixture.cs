using Okra.Navigation;
using Okra.MEF.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Okra.MEF.Tests.Navigation
{
    public class NavigationContextProxyFixture
    {
        [Fact]
        public void GetCurrent_ReturnsResultFromWrappedContext()
        {
            INavigationBase navigationBase = new MockNavigationManager();
            INavigationContext context = new MockNavigationContext(navigationBase);

            NavigationContextProxy proxy = new NavigationContextProxy();
            proxy.SetNavigationContext(context);

            Assert.Equal(navigationBase, proxy.GetCurrent());
        }

        [Fact]
        public void SetNavigationContext_ThrowsException_IfNavigationContextIsNull()
        {
            NavigationContextProxy proxy = new NavigationContextProxy();
            Assert.Throws<ArgumentNullException>(() => proxy.SetNavigationContext(null));
        }
    }
}

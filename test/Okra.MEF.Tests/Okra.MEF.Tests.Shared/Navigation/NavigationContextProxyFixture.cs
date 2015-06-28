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
            var e = Assert.Throws<ArgumentNullException>(() => proxy.SetNavigationContext(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: navigationContext", e.Message);
            Assert.Equal("navigationContext", e.ParamName);
        }
    }
}

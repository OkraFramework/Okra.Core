using Okra.Navigation;
using Okra.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Xunit;

namespace Okra.Tests.Navigation
{
    public class PageNavigationEventArgsFixture
    {
        // *** Constructor Tests ***

        [Fact]
        public void Constructor_SetsPageProperty()
        {
            PageInfo navigationEntry = new PageInfo("SamplePage", null);
            PageNavigationEventArgs eventArgs = new PageNavigationEventArgs(navigationEntry, PageNavigationMode.Forward);

            Assert.Equal(navigationEntry, eventArgs.Page);
        }

        [Fact]
        public void Constructor_SetsNavigationMode()
        {
            PageInfo navigationEntry = new PageInfo("SamplePage", null);
            PageNavigationEventArgs eventArgs = new PageNavigationEventArgs(navigationEntry, PageNavigationMode.Forward);

            Assert.Equal(PageNavigationMode.Forward, eventArgs.NavigationMode);
        }

        [Fact]
        public void Constructor_Exception_PageIsNull()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new PageNavigationEventArgs(null, PageNavigationMode.Forward));

            Assert.Equal("Value cannot be null.\r\nParameter name: page", e.Message);
            Assert.Equal("page", e.ParamName);
        }

        [Fact]
        public void Constructor_Exception_InvalidNavigationMode()
        {
            PageInfo navigationEntry = new PageInfo("SamplePage", null);

            var e = Assert.Throws<ArgumentException>(() => new PageNavigationEventArgs(navigationEntry, (PageNavigationMode)100));

            Assert.Equal("The argument contains an undefined enumeration value.\r\nParameter name: navigationMode", e.Message);
            Assert.Equal("navigationMode", e.ParamName);
        }
    }
}

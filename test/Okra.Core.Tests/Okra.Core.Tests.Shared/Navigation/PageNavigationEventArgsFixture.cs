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
            Assert.Throws<ArgumentNullException>(() =>
                {
                    PageNavigationEventArgs eventArgs = new PageNavigationEventArgs(null, PageNavigationMode.Forward);
                });
        }

        [Fact]
        public void Constructor_Exception_InvalidNavigationMode()
        {
            PageInfo navigationEntry = new PageInfo("SamplePage", null);

            Assert.Throws<ArgumentException>(() =>
            {
                PageNavigationEventArgs eventArgs = new PageNavigationEventArgs(navigationEntry, (PageNavigationMode)100);
            });
        }
    }
}

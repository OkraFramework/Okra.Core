using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Okra.Navigation;
using Okra.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Okra.Tests.Navigation
{
    [TestClass]
    public class PageNavigationEventArgsFixture
    {
        // *** Constructor Tests ***

        [TestMethod]
        public void Constructor_SetsPageProperty()
        {
            PageInfo navigationEntry = new PageInfo("SamplePage", null);
            PageNavigationEventArgs eventArgs = new PageNavigationEventArgs(navigationEntry, NavigationMode.Forward);
            
            Assert.AreEqual(navigationEntry, eventArgs.Page);
        }

        [TestMethod]
        public void Constructor_SetsNavigationMode()
        {
            PageInfo navigationEntry = new PageInfo("SamplePage", null);
            PageNavigationEventArgs eventArgs = new PageNavigationEventArgs(navigationEntry, NavigationMode.Forward);

            Assert.AreEqual(NavigationMode.Forward, eventArgs.NavigationMode);
        }

        [TestMethod]
        public void Constructor_Exception_PageIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                {
                    PageNavigationEventArgs eventArgs = new PageNavigationEventArgs(null, NavigationMode.Forward);
                });
        }

        [TestMethod]
        public void Constructor_Exception_InvalidNavigationMode()
        {
            PageInfo navigationEntry = new PageInfo("SamplePage", null);

            Assert.ThrowsException<ArgumentException>(() =>
            {
                PageNavigationEventArgs eventArgs = new PageNavigationEventArgs(navigationEntry, (NavigationMode)100);
            });
        }
    }
}

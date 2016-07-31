using Okra.Navigation;
using Okra.Tests.Mocks;
using System;
using Xunit;

namespace Okra.Tests.Navigation
{
    public class PageEntryFixture
    {
        [Fact]
        public void Constructor_SetsPageName()
        {
            var stateService = new MockStateService();
            var pageEntry = new PageEntry("Test Page", stateService);

            Assert.Equal("Test Page", pageEntry.PageName);
        }

        [Fact]
        public void Constructor_SetsStateService()
        {
            var stateService = new MockStateService();
            var pageEntry = new PageEntry("Test Page", stateService);

            Assert.Equal(stateService, pageEntry.PageState);
        }

        [Fact]
        public void Constructor_ThrowsException_WhenPageNameIsNull()
        {
            var e = Assert.Throws<ArgumentException>(() => new PageEntry(null, new MockStateService()));

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: pageName", e.Message);
            Assert.Equal("pageName", e.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsException_WhenPageNameIsEmpty()
        {
            var e = Assert.Throws<ArgumentException>(() => new PageEntry("", new MockStateService()));

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: pageName", e.Message);
            Assert.Equal("pageName", e.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsException_WhenPageStateIsNull()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new PageEntry("Test Page", null));

            Assert.Equal("Value cannot be null.\r\nParameter name: pageState", e.Message);
            Assert.Equal("pageState", e.ParamName);
        }

        [Fact]
        public void ToString_ReturnsReadableString()
        {
            var pageEntry = new PageEntry("Page Name", new MockStateService());

            string str = pageEntry.ToString();

            Assert.Equal("Page Name", str);
        }
    }
}

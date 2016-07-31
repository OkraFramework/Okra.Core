using Okra.Navigation;
using System;
using Xunit;

namespace Okra.Tests.Navigation
{
    public class PageInfoFixture
    {
        [Fact]
        public void Constructor_SetsPageName()
        {
            object args = new object();
            var pageInfo = new PageInfo("Test Page", args);

            Assert.Equal("Test Page", pageInfo.PageName);
        }

        [Fact]
        public void Constructor_SetsArguments()
        {
            object args = new object();
            var pageInfo = new PageInfo("Test Page", args);

            Assert.Equal(args, pageInfo.Arguments);
        }

        [Fact]
        public void Constructor_ThrowsException_WhenPageNameIsNull()
        {
            var e = Assert.Throws<ArgumentException>(() => new PageInfo(null, "Arguments"));

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: pageName", e.Message);
            Assert.Equal("pageName", e.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsException_WhenPageNameIsEmpty()
        {
            var e = Assert.Throws<ArgumentException>(() => new PageInfo("", "Arguments"));

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: pageName", e.Message);
            Assert.Equal("pageName", e.ParamName);
        }

        [Fact]
        public void ToString_ReturnsReadableString_IfArgumentsAreNull()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", null);

            string str = navigationEntry.ToString();

            Assert.Equal("Page Name", str);
        }

        [Fact]
        public void ToString_ReturnsReadableString_IfArgumentsAreString()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Page Arguments");

            string str = navigationEntry.ToString();

            Assert.Equal("Page Name(Page Arguments)", str);
        }
    }
}

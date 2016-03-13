using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}

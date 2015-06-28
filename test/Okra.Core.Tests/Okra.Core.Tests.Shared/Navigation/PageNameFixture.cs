using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Navigation;
using Xunit;

namespace Okra.Tests.Navigation
{
    public class PageNameFixture
    {
        // *** Static Method Tests ***

        [Fact]
        public void FromType_ReturnsFullName()
        {
            string pageName = PageName.FromType(typeof(PageNameFixture));

            Assert.Equal("Okra.Tests.Navigation.PageNameFixture", pageName);
        }

        [Fact]
        public void FromType_Exception_NullType()
        {
            var e = Assert.Throws<ArgumentNullException>(() => PageName.FromType(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: type", e.Message);
            Assert.Equal("type", e.ParamName);
        }
    }
}

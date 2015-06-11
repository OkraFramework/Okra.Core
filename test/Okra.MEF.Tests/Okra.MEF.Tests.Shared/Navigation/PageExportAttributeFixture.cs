using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Navigation;
using Xunit;

namespace Okra.MEF.Tests.Navigation
{
    public class PageExportAttributeFixture
    {
        // *** Constructor Tests ***

        [Fact]
        public void Constructor_SetsPageName_ByString()
        {
            PageExportAttribute attribute = new PageExportAttribute("Page X");

            Assert.Equal("Page X", attribute.PageName);
        }

        [Fact]
        public void Constructor_SetsPageName_ByType()
        {
            PageExportAttribute attribute = new PageExportAttribute(typeof(PageExportAttributeFixture));

            Assert.Equal("Okra.MEF.Tests.Navigation.PageExportAttributeFixture", attribute.PageName);
        }

        [Fact]
        public void Constructor_ThrowsException_IfPageNameIsNull()
        {
            Assert.Throws<ArgumentException>(() => new PageExportAttribute((string)null));
        }

        [Fact]
        public void Constructor_ThrowsException_IfPageNameIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new PageExportAttribute(""));
        }

        [Fact]
        public void Constructor_ThrowsException_IfPageTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new PageExportAttribute((Type)null));
        }

        // *** Property Tests ***

        [Fact]
        public void ContractName_IsCorrect()
        {
            PageExportAttribute attribute = new PageExportAttribute("Page X");

            Assert.Equal("OkraPage", attribute.ContractName);
        }

        [Fact]
        public void ContractType_IsObject()
        {
            PageExportAttribute attribute = new PageExportAttribute("Page X");

            Assert.Equal(typeof(object), attribute.ContractType);
        }
    }
}

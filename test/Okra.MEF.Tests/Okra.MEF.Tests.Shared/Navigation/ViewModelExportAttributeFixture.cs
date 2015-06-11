using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Navigation;
using Xunit;

namespace Okra.MEF.Tests.Navigation
{
    public class ViewModelExportAttributeFixture
    {
        // *** Constructor Tests ***

        [Fact]
        public void Constructor_SetsPageName_ByString()
        {
            ViewModelExportAttribute attribute = new ViewModelExportAttribute("Page X");

            Assert.Equal("Page X", attribute.PageName);
        }

        [Fact]
        public void Constructor_SetsPageName_ByType()
        {
            ViewModelExportAttribute attribute = new ViewModelExportAttribute(typeof(ViewModelExportAttributeFixture));

            Assert.Equal("Okra.MEF.Tests.Navigation.ViewModelExportAttributeFixture", attribute.PageName);
        }

        [Fact]
        public void Constructor_ThrowsException_IfPageNameIsNull()
        {
            Assert.Throws<ArgumentException>(() => new ViewModelExportAttribute((string)null));
        }

        [Fact]
        public void Constructor_ThrowsException_IfPageNameIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new ViewModelExportAttribute(""));
        }

        [Fact]
        public void Constructor_ThrowsException_IfPageTypeIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ViewModelExportAttribute((Type)null));
        }

        // *** Property Tests ***

        [Fact]
        public void ContractName_IsCorrect()
        {
            ViewModelExportAttribute attribute = new ViewModelExportAttribute("Page X");

            Assert.Equal("OkraViewModel", attribute.ContractName);
        }

        [Fact]
        public void ContractType_IsObject()
        {
            ViewModelExportAttribute attribute = new ViewModelExportAttribute("Page X");

            Assert.Equal(typeof(object), attribute.ContractType);
        }
    }
}

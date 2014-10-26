using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Navigation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Okra.MEF.Tests.Navigation
{
    [TestClass]
    public class PageExportAttributeFixture
    {
        // *** Constructor Tests ***

        [TestMethod]
        public void Constructor_SetsPageName_ByString()
        {
            PageExportAttribute attribute = new PageExportAttribute("Page X");

            Assert.AreEqual("Page X", attribute.PageName);
        }

        [TestMethod]
        public void Constructor_SetsPageName_ByType()
        {
            PageExportAttribute attribute = new PageExportAttribute(typeof(PageExportAttributeFixture));

            Assert.AreEqual("Okra.MEF.Tests.Navigation.PageExportAttributeFixture", attribute.PageName);
        }

        [TestMethod]
        public void Constructor_ThrowsException_IfPageNameIsNull()
        {
            Assert.ThrowsException<ArgumentException>(() => new PageExportAttribute((string)null));
        }

        [TestMethod]
        public void Constructor_ThrowsException_IfPageNameIsEmpty()
        {
            Assert.ThrowsException<ArgumentException>(() => new PageExportAttribute(""));
        }

        [TestMethod]
        public void Constructor_ThrowsException_IfPageTypeIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new PageExportAttribute((Type)null));
        }

        // *** Property Tests ***

        [TestMethod]
        public void ContractName_IsCorrect()
        {
            PageExportAttribute attribute = new PageExportAttribute("Page X");

            Assert.AreEqual("OkraPage", attribute.ContractName);
        }

        [TestMethod]
        public void ContractType_IsObject()
        {
            PageExportAttribute attribute = new PageExportAttribute("Page X");

            Assert.AreEqual(typeof(object), attribute.ContractType);
        }
    }
}

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
    public class ViewModelExportAttributeFixture
    {
        // *** Constructor Tests ***

        [TestMethod]
        public void Constructor_SetsPageName_ByString()
        {
            ViewModelExportAttribute attribute = new ViewModelExportAttribute("Page X");

            Assert.AreEqual("Page X", attribute.PageName);
        }

        [TestMethod]
        public void Constructor_SetsPageName_ByType()
        {
            ViewModelExportAttribute attribute = new ViewModelExportAttribute(typeof(ViewModelExportAttributeFixture));

            Assert.AreEqual("Okra.MEF.Tests.Navigation.ViewModelExportAttributeFixture", attribute.PageName);
        }

        [TestMethod]
        public void Constructor_ThrowsException_IfPageNameIsNull()
        {
            Assert.ThrowsException<ArgumentException>(() => new ViewModelExportAttribute((string)null));
        }

        [TestMethod]
        public void Constructor_ThrowsException_IfPageNameIsEmpty()
        {
            Assert.ThrowsException<ArgumentException>(() => new ViewModelExportAttribute(""));
        }

        [TestMethod]
        public void Constructor_ThrowsException_IfPageTypeIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ViewModelExportAttribute((Type)null));
        }

        // *** Property Tests ***

        [TestMethod]
        public void ContractName_IsCorrect()
        {
            ViewModelExportAttribute attribute = new ViewModelExportAttribute("Page X");

            Assert.AreEqual("OkraViewModel", attribute.ContractName);
        }

        [TestMethod]
        public void ContractType_IsObject()
        {
            ViewModelExportAttribute attribute = new ViewModelExportAttribute("Page X");

            Assert.AreEqual(typeof(object), attribute.ContractType);
        }
    }
}

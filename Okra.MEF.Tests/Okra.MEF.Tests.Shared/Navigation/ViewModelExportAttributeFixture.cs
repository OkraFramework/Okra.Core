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

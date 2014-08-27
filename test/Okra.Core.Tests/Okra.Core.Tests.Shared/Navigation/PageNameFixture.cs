using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Navigation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Okra.Tests.Navigation
{
    [TestClass]
    public class PageNameFixture
    {
        // *** Static Method Tests ***

        [TestMethod]
        public void FromType_ReturnsFullName()
        {
            string pageName = PageName.FromType(typeof(PageNameFixture));

            Assert.AreEqual("Okra.Tests.Navigation.PageNameFixture", pageName);
        }

        [TestMethod]
        public void FromType_Exception_NullType()
        {
            Assert.ThrowsException<ArgumentNullException>(() => PageName.FromType(null));
        }
    }
}

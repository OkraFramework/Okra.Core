using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Navigation;
using Okra.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Okra.Tests.Navigation
{
    [TestClass]
    public class NavigationStackExFixture
    {
        // *** Static Method Tests ***

        [TestMethod]
        public void GoBackTo_NavigatesBackToPage()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            navigationStack.NavigateTo("Page 1");
            navigationStack.NavigateTo("Page 2");
            navigationStack.NavigateTo("Page 3");
            navigationStack.NavigateTo("Page 4");

            navigationStack.GoBackTo("Page 2");

            CollectionAssert.AreEqual(new string[] { "Page 1", "Page 2" }, navigationStack.Select(e => e.PageName).ToList());
        }

        [TestMethod]
        public void GoBackTo_NavigatesBackToLastPageWithName()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            navigationStack.NavigateTo("Page 1");
            navigationStack.NavigateTo("Page 2");
            navigationStack.NavigateTo("Page 3");
            navigationStack.NavigateTo("Page 2");
            navigationStack.NavigateTo("Page 4");

            navigationStack.GoBackTo("Page 2");

            CollectionAssert.AreEqual(new string[] { "Page 1", "Page 2", "Page 3", "Page 2" }, navigationStack.Select(e => e.PageName).ToList());
        }

        [TestMethod]
        public void GoBackTo_Exception_NavigationStackIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => NavigationStackEx.GoBackTo(null, "Page 1"));
        }

        [TestMethod]
        public void GoBackTo_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            Assert.ThrowsException<ArgumentException>(() => navigationStack.GoBackTo((string)null));
        }

        [TestMethod]
        public void GoBackTo_Exception_IfPageNameIsEmpty()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            Assert.ThrowsException<ArgumentException>(() => navigationStack.GoBackTo(""));
        }

        [TestMethod]
        public void NavigateTo_WithPageName_NavigatesToPage()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            navigationStack.NavigateTo("Page 1");

            CollectionAssert.AreEqual(new string[] { "Page 1" }, navigationStack.Select(e => e.PageName).ToList());
            CollectionAssert.AreEqual(new object[] { null }, navigationStack.Select(e => e.GetArguments<string>()).ToList());
        }

        [TestMethod]
        public void NavigateTo_WithPageName_Exception_NavigationStackIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => NavigationStackEx.NavigateTo(null, "Page 1"));
        }

        [TestMethod]
        public void NavigateTo_WithPageName_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            Assert.ThrowsException<ArgumentException>(() => navigationStack.NavigateTo((string)null));
        }

        [TestMethod]
        public void NavigateTo_WithPageName_Exception_IfPageNameIsEmpty()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            Assert.ThrowsException<ArgumentException>(() => navigationStack.NavigateTo(""));
        }
        
        [TestMethod]
        public void NavigateTo_WithPageNameAndParameter_NavigatesToPage()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            navigationStack.NavigateTo("Page 1", "Parameter 1");

            CollectionAssert.AreEqual(new[] { "Page 1" }, navigationStack.Select(e => e.PageName).ToList());
            CollectionAssert.AreEqual(new object[] { "Parameter 1" }, navigationStack.Select(e => e.GetArguments<string>()).ToList());
        }

        [TestMethod]
        public void NavigateTo_WithPageNameAndParameter_Exception_NavigationStackIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => NavigationStackEx.NavigateTo(null, "Page 1", new object()));
        }

        [TestMethod]
        public void NavigateTo_WithPageNameAndParameter_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            Assert.ThrowsException<ArgumentException>(() => navigationStack.NavigateTo((string)null, "Parameter 1"));
        }

        [TestMethod]
        public void NavigateTo_WithPageNameAndParameter_Exception_IfPageNameIsEmpty()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            Assert.ThrowsException<ArgumentException>(() => navigationStack.NavigateTo("", "Parameter 1"));
        }
        
        [TestMethod]
        public void NavigateTo_WithType_NavigatesToPageWithFullTypeName()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            navigationStack.NavigateTo(typeof(MockNavigationBase.MockPage));

            CollectionAssert.AreEqual(new string[] { MockNavigationBase.MOCKPAGE_NAME }, navigationStack.Select(e => e.PageName).ToList());
            CollectionAssert.AreEqual(new object[] { null }, navigationStack.Select(e => e.GetArguments<string>()).ToList());
        }

        [TestMethod]
        public void NavigateTo_WithType_Exception_NavigationStackIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => NavigationStackEx.NavigateTo(null, typeof(MockPage)));
        }

        [TestMethod]
        public void NavigateTo_WithType_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            Assert.ThrowsException<ArgumentNullException>(() => navigationStack.NavigateTo((Type)null));
        }
        
        [TestMethod]
        public void NavigateTo_WithTypeAndParameter_NavigatesToPageWithFullTypeName()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            navigationStack.NavigateTo(typeof(MockNavigationBase.MockPage), "Parameter 1");

            CollectionAssert.AreEqual(new string[] { MockNavigationBase.MOCKPAGE_NAME }, navigationStack.Select(e => e.PageName).ToList());
            CollectionAssert.AreEqual(new object[] { "Parameter 1" }, navigationStack.Select(e => e.GetArguments<string>()).ToList());
        }

        [TestMethod]
        public void NavigateTo_WithTypeAndParameter_Exception_NavigationStackIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => NavigationStackEx.NavigateTo(null, typeof(MockPage), new object()));
        }

        [TestMethod]
        public void NavigateTo_WithTypeAndParameter_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            Assert.ThrowsException<ArgumentNullException>(() => navigationStack.NavigateTo((Type)null, "Parameter"));
        }
        
        // *** Sub-classes ***

        public class InvalidPage
        {
        }
    }
}

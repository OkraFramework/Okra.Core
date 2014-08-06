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
    public class NavigationBaseExFixture
    {
        // *** Static Method Tests ***

        [TestMethod]
        public void GoBack_CallsNavigationStack()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            navigationManager.GoBack();

            Assert.AreEqual(1, navigationStack.GoBackCallCount);
        }

        [TestMethod]
        public void GoBack_Exception_CannotGoBack()
        {
            MockNavigationStack navigationStack = new MockNavigationStack() { CanGoBack = false };
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.ThrowsException<InvalidOperationException>(() => navigationManager.GoBack());
        }

        [TestMethod]
        public void NavigateTo_WithPageName_CallsNavigationStack()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            navigationManager.NavigateTo("Page 1");

            CollectionAssert.AreEqual(new string[] { "Page 1" }, navigationStack.NavigateToCalls.Select(e => e.PageName).ToList());
            CollectionAssert.AreEqual(new object[] { null }, navigationStack.NavigateToCalls.Select(e => e.GetArguments<string>()).ToList());
        }

        [TestMethod]
        public void NavigateTo_WithPageName_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.ThrowsException<ArgumentException>(() => navigationManager.NavigateTo((string)null));
        }

        [TestMethod]
        public void NavigateTo_WithPageName_Exception_IfPageNameIsEmpty()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.ThrowsException<ArgumentException>(() => navigationManager.NavigateTo(""));
        }

        [TestMethod]
        public void NavigateTo_WithPageName_Exception_IfPageDoesNotExist()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.ThrowsException<InvalidOperationException>(() => navigationManager.NavigateTo("Page X"));
        }

        [TestMethod]
        public void NavigateTo_WithPageNameAndParameter_NavigatesToPage()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            navigationManager.NavigateTo("Page 1", "Parameter 1");

            CollectionAssert.AreEqual(new[] { "Page 1" }, navigationStack.NavigateToCalls.Select(e => e.PageName).ToList());
            CollectionAssert.AreEqual(new object[] { "Parameter 1" }, navigationStack.NavigateToCalls.Select(e => e.GetArguments<string>()).ToList());
        }

        [TestMethod]
        public void NavigateTo_WithPageNameAndParameter_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.ThrowsException<ArgumentException>(() => navigationManager.NavigateTo((string)null, "Parameter 1"));
        }

        [TestMethod]
        public void NavigateTo_WithPageNameAndParameter_Exception_IfPageNameIsEmpty()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.ThrowsException<ArgumentException>(() => navigationManager.NavigateTo("", "Parameter 1"));
        }

        [TestMethod]
        public void NavigateTo_WithPageNameAndParameter_Exception_IfPageDoesNotExist()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.ThrowsException<InvalidOperationException>(() => navigationManager.NavigateTo("Page X", "Parameter 1"));
        }

        [TestMethod]
        public void NavigateTo_WithType_NavigatesToPageWithFullTypeName()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            navigationManager.NavigateTo(typeof(MockNavigationBase.MockPage));

            CollectionAssert.AreEqual(new string[] { MockNavigationBase.MOCKPAGE_NAME }, navigationStack.NavigateToCalls.Select(e => e.PageName).ToList());
            CollectionAssert.AreEqual(new object[] { null }, navigationStack.NavigateToCalls.Select(e => e.GetArguments<string>()).ToList());
        }

        [TestMethod]
        public void NavigateTo_WithType_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.ThrowsException<ArgumentNullException>(() => navigationManager.NavigateTo((Type)null));
        }

        [TestMethod]
        public void NavigateTo_WithType_Exception_IfPageDoesNotExist()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.ThrowsException<InvalidOperationException>(() => navigationManager.NavigateTo(typeof(InvalidPage)));
        }

        [TestMethod]
        public void NavigateTo_WithTypeAndParameter_NavigatesToPageWithFullTypeName()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            navigationManager.NavigateTo(typeof(MockNavigationBase.MockPage), "Parameter 1");

            CollectionAssert.AreEqual(new string[] { MockNavigationBase.MOCKPAGE_NAME }, navigationStack.NavigateToCalls.Select(e => e.PageName).ToList());
            CollectionAssert.AreEqual(new object[] { "Parameter 1" }, navigationStack.NavigateToCalls.Select(e => e.GetArguments<string>()).ToList());
        }

        [TestMethod]
        public void NavigateTo_WithTypeAndParameter_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.ThrowsException<ArgumentNullException>(() => navigationManager.NavigateTo((Type)null, "Parameter"));
        }

        [TestMethod]
        public void NavigateTo_WithTypeAndParameter_Exception_IfPageDoesNotExist()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.ThrowsException<InvalidOperationException>(() => navigationManager.NavigateTo(typeof(InvalidPage), "Parameter"));
        }

        // *** Sub-classes ***

        private class MockNavigationStack : List<PageInfo>, INavigationStack
        {
            // *** Fields ***

            public int GoBackCallCount = 0;
            public List<PageInfo> NavigateToCalls = new List<PageInfo>();

            // *** Events ***

            public event EventHandler<PageNavigationEventArgs> NavigatingFrom;
            public event EventHandler<PageNavigationEventArgs> NavigatedTo;
            public event EventHandler<PageNavigationEventArgs> PageDisposed;
            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            // *** Constructors ***

            public MockNavigationStack()
            {
                CanGoBack = true;
            }

            // *** Properties ***

            public bool CanGoBack
            {
                get;
                set;
            }

            public PageInfo CurrentPage
            {
                get { throw new NotImplementedException(); }
            }

            // *** Methods ***

            public void GoBack()
            {
                GoBackCallCount++;
            }

            public void GoBackTo(PageInfo page)
            {
                throw new NotImplementedException();
            }

            public void NavigateTo(PageInfo page)
            {
                NavigateToCalls.Add(page);
            }

            public void Push(IEnumerable<PageInfo> pages)
            {
                throw new NotImplementedException();
            }

        }

        public class InvalidPage
        {
        }
    }
}

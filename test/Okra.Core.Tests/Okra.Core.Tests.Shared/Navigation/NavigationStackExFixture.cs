using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Navigation;
using Okra.Tests.Mocks;
using Xunit;

namespace Okra.Tests.Navigation
{
    public class NavigationStackExFixture
    {
        // *** Static Method Tests ***

        [Fact]
        public void GoBackTo_NavigatesBackToPage()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            navigationStack.NavigateTo("Page 1");
            navigationStack.NavigateTo("Page 2");
            navigationStack.NavigateTo("Page 3");
            navigationStack.NavigateTo("Page 4");

            navigationStack.GoBackTo("Page 2");

            Assert.Equal(new string[] { "Page 1", "Page 2" }, navigationStack.Select(e => e.PageName).ToList());
        }

        [Fact]
        public void GoBackTo_NavigatesBackToLastPageWithName()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            navigationStack.NavigateTo("Page 1");
            navigationStack.NavigateTo("Page 2");
            navigationStack.NavigateTo("Page 3");
            navigationStack.NavigateTo("Page 2");
            navigationStack.NavigateTo("Page 4");

            navigationStack.GoBackTo("Page 2");

            Assert.Equal(new string[] { "Page 1", "Page 2", "Page 3", "Page 2" }, navigationStack.Select(e => e.PageName).ToList());
        }

        [Fact]
        public void GoBackTo_Exception_NavigationStackIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => NavigationStackEx.GoBackTo(null, "Page 1"));
        }

        [Fact]
        public void GoBackTo_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            Assert.Throws<ArgumentException>(() => navigationStack.GoBackTo((string)null));
        }

        [Fact]
        public void GoBackTo_Exception_IfPageNameIsEmpty()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            Assert.Throws<ArgumentException>(() => navigationStack.GoBackTo(""));
        }

        [Fact]
        public void NavigateTo_WithPageName_NavigatesToPage()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            navigationStack.NavigateTo("Page 1");

            Assert.Equal(new string[] { "Page 1" }, navigationStack.Select(e => e.PageName).ToList());
            Assert.Equal(new object[] { null }, navigationStack.Select(e => e.GetArguments<string>()).ToList());
        }

        [Fact]
        public void NavigateTo_WithPageName_Exception_NavigationStackIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => NavigationStackEx.NavigateTo(null, "Page 1"));
        }

        [Fact]
        public void NavigateTo_WithPageName_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            Assert.Throws<ArgumentException>(() => navigationStack.NavigateTo((string)null));
        }

        [Fact]
        public void NavigateTo_WithPageName_Exception_IfPageNameIsEmpty()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            Assert.Throws<ArgumentException>(() => navigationStack.NavigateTo(""));
        }

        [Fact]
        public void NavigateTo_WithPageNameAndParameter_NavigatesToPage()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            navigationStack.NavigateTo("Page 1", "Parameter 1");

            Assert.Equal(new[] { "Page 1" }, navigationStack.Select(e => e.PageName).ToList());
            Assert.Equal(new object[] { "Parameter 1" }, navigationStack.Select(e => e.GetArguments<string>()).ToList());
        }

        [Fact]
        public void NavigateTo_WithPageNameAndParameter_Exception_NavigationStackIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => NavigationStackEx.NavigateTo(null, "Page 1", new object()));
        }

        [Fact]
        public void NavigateTo_WithPageNameAndParameter_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            Assert.Throws<ArgumentException>(() => navigationStack.NavigateTo((string)null, "Parameter 1"));
        }

        [Fact]
        public void NavigateTo_WithPageNameAndParameter_Exception_IfPageNameIsEmpty()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            Assert.Throws<ArgumentException>(() => navigationStack.NavigateTo("", "Parameter 1"));
        }

        [Fact]
        public void NavigateTo_WithType_NavigatesToPageWithFullTypeName()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            navigationStack.NavigateTo(typeof(MockNavigationBase.MockPage));

            Assert.Equal(new string[] { MockNavigationBase.MOCKPAGE_NAME }, navigationStack.Select(e => e.PageName).ToList());
            Assert.Equal(new object[] { null }, navigationStack.Select(e => e.GetArguments<string>()).ToList());
        }

        [Fact]
        public void NavigateTo_WithType_Exception_NavigationStackIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => NavigationStackEx.NavigateTo(null, typeof(MockPage)));
        }

        [Fact]
        public void NavigateTo_WithType_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            Assert.Throws<ArgumentNullException>(() => navigationStack.NavigateTo((Type)null));
        }

        [Fact]
        public void NavigateTo_WithTypeAndParameter_NavigatesToPageWithFullTypeName()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            navigationStack.NavigateTo(typeof(MockNavigationBase.MockPage), "Parameter 1");

            Assert.Equal(new string[] { MockNavigationBase.MOCKPAGE_NAME }, navigationStack.Select(e => e.PageName).ToList());
            Assert.Equal(new object[] { "Parameter 1" }, navigationStack.Select(e => e.GetArguments<string>()).ToList());
        }

        [Fact]
        public void NavigateTo_WithTypeAndParameter_Exception_NavigationStackIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => NavigationStackEx.NavigateTo(null, typeof(MockPage), new object()));
        }

        [Fact]
        public void NavigateTo_WithTypeAndParameter_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();

            Assert.Throws<ArgumentNullException>(() => navigationStack.NavigateTo((Type)null, "Parameter"));
        }

        // *** Sub-classes ***

        public class InvalidPage
        {
        }
    }
}

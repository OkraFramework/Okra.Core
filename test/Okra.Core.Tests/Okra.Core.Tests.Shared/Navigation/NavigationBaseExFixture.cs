using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Navigation;
using Okra.Tests.Mocks;
using System.ComponentModel;
using Xunit;

namespace Okra.Tests.Navigation
{
    public class NavigationBaseExFixture
    {
        // *** Static Method Tests ***

        [Fact]
        public void GoBack_CallsNavigationStack()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            navigationManager.GoBack();

            Assert.Equal(1, navigationStack.GoBackCallCount);
        }

        [Fact]
        public void GoBack_Exception_NullNavigationBase()
        {
            var e = Assert.Throws<ArgumentNullException>(() => NavigationBaseEx.GoBack(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: navigationBase", e.Message);
            Assert.Equal("navigationBase", e.ParamName);
        }

        [Fact]
        public void GoBack_Exception_CannotGoBack()
        {
            MockNavigationStack navigationStack = new MockNavigationStack() { CanGoBack = false };
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            var e = Assert.Throws<InvalidOperationException>(() => navigationManager.GoBack());

            Assert.Equal("You cannot navigate backwards as the back stack is empty.", e.Message);
        }

        [Fact]
        public void NavigateTo_WithPageName_CallsNavigationStack()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            navigationManager.NavigateTo("Page 1");

            Assert.Equal(new string[] { "Page 1" }, navigationStack.NavigateToCalls.Select(e => e.PageName).ToList());
            Assert.Equal(new object[] { null }, navigationStack.NavigateToCalls.Select(e => e.GetArguments<string>()).ToList());
        }

        [Fact]
        public void NavigateTo_WithPageName_Exception_NullNavigationBase()
        {
            INavigationBase navigationManager = new MockNavigationBase();

            var e = Assert.Throws<ArgumentNullException>(() => NavigationBaseEx.NavigateTo(null, "Page Name"));

            Assert.Equal("Value cannot be null.\r\nParameter name: navigationBase", e.Message);
            Assert.Equal("navigationBase", e.ParamName);
        }

        [Fact]
        public void NavigateTo_WithPageName_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            var e = Assert.Throws<ArgumentException>(() => navigationManager.NavigateTo((string)null));

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: pageName", e.Message);
            Assert.Equal("pageName", e.ParamName);
        }

        [Fact]
        public void NavigateTo_WithPageName_Exception_IfPageNameIsEmpty()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            var e = Assert.Throws<ArgumentException>(() => navigationManager.NavigateTo(""));

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: pageName", e.Message);
            Assert.Equal("pageName", e.ParamName);
        }

        [Fact]
        public void NavigateTo_WithPageName_Exception_IfPageDoesNotExist()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            var e = Assert.Throws<InvalidOperationException>(() => navigationManager.NavigateTo("Page X"));

            Assert.Equal("Cannot navigate as a page named 'Page X' does not exist.", e.Message);
        }

        [Fact]
        public void NavigateTo_WithPageNameAndParameter_NavigatesToPage()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            navigationManager.NavigateTo("Page 1", "Parameter 1");

            Assert.Equal(new[] { "Page 1" }, navigationStack.NavigateToCalls.Select(e => e.PageName).ToList());
            Assert.Equal(new object[] { "Parameter 1" }, navigationStack.NavigateToCalls.Select(e => e.GetArguments<string>()).ToList());
        }

        [Fact]
        public void NavigateTo_WithPageNameAndParameter_Exception_NullNavigationBase()
        {
            INavigationBase navigationManager = new MockNavigationBase();

            var e = Assert.Throws<ArgumentNullException>(() => NavigationBaseEx.NavigateTo(null, "Page Name", new object()));

            Assert.Equal("Value cannot be null.\r\nParameter name: navigationBase", e.Message);
            Assert.Equal("navigationBase", e.ParamName);
        }

        [Fact]
        public void NavigateTo_WithPageNameAndParameter_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            var e = Assert.Throws<ArgumentException>(() => navigationManager.NavigateTo((string)null, "Parameter 1"));

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: pageName", e.Message);
            Assert.Equal("pageName", e.ParamName);
        }

        [Fact]
        public void NavigateTo_WithPageNameAndParameter_Exception_IfPageNameIsEmpty()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            var e = Assert.Throws<ArgumentException>(() => navigationManager.NavigateTo("", "Parameter 1"));

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: pageName", e.Message);
            Assert.Equal("pageName", e.ParamName);
        }

        [Fact]
        public void NavigateTo_WithPageNameAndParameter_Exception_IfPageDoesNotExist()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            var e = Assert.Throws<InvalidOperationException>(() => navigationManager.NavigateTo("Page X", "Parameter 1"));

            Assert.Equal("Cannot navigate as a page named 'Page X' does not exist.", e.Message);
        }

        [Fact]
        public void NavigateTo_WithType_NavigatesToPageWithFullTypeName()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            navigationManager.NavigateTo(typeof(MockNavigationBase.MockPage));

            Assert.Equal(new string[] { MockNavigationBase.MOCKPAGE_NAME }, navigationStack.NavigateToCalls.Select(e => e.PageName).ToList());
            Assert.Equal(new object[] { null }, navigationStack.NavigateToCalls.Select(e => e.GetArguments<string>()).ToList());
        }

        [Fact]
        public void NavigateTo_WithType_Exception_NullNavigationBase()
        {
            INavigationBase navigationManager = new MockNavigationBase();

            var e = Assert.Throws<ArgumentNullException>(() => NavigationBaseEx.NavigateTo(null, typeof(MockPage)));

            Assert.Equal("Value cannot be null.\r\nParameter name: navigationBase", e.Message);
            Assert.Equal("navigationBase", e.ParamName);
        }

        [Fact]
        public void NavigateTo_WithType_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            var e = Assert.Throws<ArgumentNullException>(() => navigationManager.NavigateTo((Type)null));

            Assert.Equal("Value cannot be null.\r\nParameter name: pageName", e.Message);
            Assert.Equal("pageName", e.ParamName);
        }

        [Fact]
        public void NavigateTo_WithType_Exception_IfPageDoesNotExist()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            var e = Assert.Throws<InvalidOperationException>(() => navigationManager.NavigateTo(typeof(InvalidPage)));

            Assert.Equal("Cannot navigate as a page named 'Okra.Tests.Navigation.NavigationBaseExFixture+InvalidPage' does not exist.", e.Message);
        }

        [Fact]
        public void NavigateTo_WithTypeAndParameter_NavigatesToPageWithFullTypeName()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            navigationManager.NavigateTo(typeof(MockNavigationBase.MockPage), "Parameter 1");

            Assert.Equal(new string[] { MockNavigationBase.MOCKPAGE_NAME }, navigationStack.NavigateToCalls.Select(e => e.PageName).ToList());
            Assert.Equal(new object[] { "Parameter 1" }, navigationStack.NavigateToCalls.Select(e => e.GetArguments<string>()).ToList());
        }

        [Fact]
        public void NavigateTo_WithTypeAndParameter_Exception_NullNavigationBase()
        {
            INavigationBase navigationManager = new MockNavigationBase();

            var e = Assert.Throws<ArgumentNullException>(() => NavigationBaseEx.NavigateTo(null, typeof(MockPage), new object()));

            Assert.Equal("Value cannot be null.\r\nParameter name: navigationBase", e.Message);
            Assert.Equal("navigationBase", e.ParamName);
        }

        [Fact]
        public void NavigateTo_WithTypeAndParameter_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            var e = Assert.Throws<ArgumentNullException>(() => navigationManager.NavigateTo((Type)null, "Parameter"));

            Assert.Equal("Value cannot be null.\r\nParameter name: pageName", e.Message);
            Assert.Equal("pageName", e.ParamName);
        }

        [Fact]
        public void NavigateTo_WithTypeAndParameter_Exception_IfPageDoesNotExist()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            var e = Assert.Throws<InvalidOperationException>(() => navigationManager.NavigateTo(typeof(InvalidPage), "Parameter"));

            Assert.Equal("Cannot navigate as a page named 'Okra.Tests.Navigation.NavigationBaseExFixture+InvalidPage' does not exist.", e.Message);
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

            // *** Mock Methods ***

            public void RaiseNavigatedTo(PageNavigationEventArgs eventArgs)
            {
                if (NavigatedTo != null)
                    NavigatedTo(this, eventArgs);
            }

            public void RaiseNavigatingFrom(PageNavigationEventArgs eventArgs)
            {
                if (NavigatingFrom != null)
                    NavigatingFrom(this, eventArgs);
            }

            public void RaisePageDisposed(PageNavigationEventArgs eventArgs)
            {
                if (PageDisposed != null)
                    PageDisposed(this, eventArgs);
            }

            public void RaisePropertyChanged(PropertyChangedEventArgs eventArgs)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, eventArgs);
            }
        }

        public class InvalidPage
        {
        }
    }
}

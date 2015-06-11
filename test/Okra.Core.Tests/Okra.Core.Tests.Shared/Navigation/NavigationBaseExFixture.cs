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
            Assert.Throws<ArgumentNullException>(() => NavigationBaseEx.GoBack(null));
        }

        [Fact]
        public void GoBack_Exception_CannotGoBack()
        {
            MockNavigationStack navigationStack = new MockNavigationStack() { CanGoBack = false };
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.Throws<InvalidOperationException>(() => navigationManager.GoBack());
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

            Assert.Throws<ArgumentNullException>(() => NavigationBaseEx.NavigateTo(null, "Page Name"));
        }

        [Fact]
        public void NavigateTo_WithPageName_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.Throws<ArgumentException>(() => navigationManager.NavigateTo((string)null));
        }

        [Fact]
        public void NavigateTo_WithPageName_Exception_IfPageNameIsEmpty()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.Throws<ArgumentException>(() => navigationManager.NavigateTo(""));
        }

        [Fact]
        public void NavigateTo_WithPageName_Exception_IfPageDoesNotExist()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.Throws<InvalidOperationException>(() => navigationManager.NavigateTo("Page X"));
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

            Assert.Throws<ArgumentNullException>(() => NavigationBaseEx.NavigateTo(null, "Page Name", new object()));
        }

        [Fact]
        public void NavigateTo_WithPageNameAndParameter_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.Throws<ArgumentException>(() => navigationManager.NavigateTo((string)null, "Parameter 1"));
        }

        [Fact]
        public void NavigateTo_WithPageNameAndParameter_Exception_IfPageNameIsEmpty()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.Throws<ArgumentException>(() => navigationManager.NavigateTo("", "Parameter 1"));
        }

        [Fact]
        public void NavigateTo_WithPageNameAndParameter_Exception_IfPageDoesNotExist()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.Throws<InvalidOperationException>(() => navigationManager.NavigateTo("Page X", "Parameter 1"));
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

            Assert.Throws<ArgumentNullException>(() => NavigationBaseEx.NavigateTo(null, typeof(MockPage)));
        }

        [Fact]
        public void NavigateTo_WithType_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.Throws<ArgumentNullException>(() => navigationManager.NavigateTo((Type)null));
        }

        [Fact]
        public void NavigateTo_WithType_Exception_IfPageDoesNotExist()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.Throws<InvalidOperationException>(() => navigationManager.NavigateTo(typeof(InvalidPage)));
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

            Assert.Throws<ArgumentNullException>(() => NavigationBaseEx.NavigateTo(null, typeof(MockPage), new object()));
        }

        [Fact]
        public void NavigateTo_WithTypeAndParameter_Exception_IfPageNameIsNull()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.Throws<ArgumentNullException>(() => navigationManager.NavigateTo((Type)null, "Parameter"));
        }

        [Fact]
        public void NavigateTo_WithTypeAndParameter_Exception_IfPageDoesNotExist()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            INavigationBase navigationManager = new MockNavigationBase(navigationStack);

            Assert.Throws<InvalidOperationException>(() => navigationManager.NavigateTo(typeof(InvalidPage), "Parameter"));
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

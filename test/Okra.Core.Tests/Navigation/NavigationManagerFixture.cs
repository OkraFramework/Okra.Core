using Okra.Navigation;
using Okra.State;
using Okra.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Okra.Tests.Navigation
{
    public class NavigationManagerFixture
    {
        // *** Test Data ***

        public static PageInfo[][] SingleTestValues = { new [] { new PageInfo("First Page", null) },
                                                        new [] { new PageInfo("First Page", "First Arguments") } };

        public static PageInfo[][] DoubleTestValues = { new [] { new PageInfo("First Page", null), new PageInfo("Second Page", null) },
                                                        new [] { new PageInfo("First Page", "First Arguments"), new PageInfo("Second Page", "Second Arguments") } };

        public static PageInfo[][] TripleTestValues = { new [] { new PageInfo("First Page", null), new PageInfo("Second Page", null), new PageInfo("Third Page", null) },
                                                        new [] { new PageInfo("First Page", "First Arguments"), new PageInfo("Second Page", "Second Arguments"), new PageInfo("Third Page", "Third Arguments") } };

        // *** Tests ***

        [Fact]
        public void InitialNavigationStack_IsEmpty()
        {
            var navigationManager = new NavigationManager();

            Assert.NotNull(navigationManager);

            Assert.Equal(0, navigationManager.NavigationStack.Count);
            Assert.Equal(null, navigationManager.CurrentPage);

            Assert.False(navigationManager.CanGoBack);
            Assert.False(navigationManager.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Clear_ClearsNavigationStack(PageInfo page1, PageInfo page2)
        {
            var navigationManager = new NavigationManager();

            navigationManager.NavigateTo(page1);
            navigationManager.NavigateTo(page2);
            navigationManager.GoBack();

            AssertEx.PropertyChangedEvents(navigationManager, () => navigationManager.Clear());

            Assert.Equal(0, navigationManager.NavigationStack.Count);
            Assert.Equal(null, navigationManager.CurrentPage);

            Assert.False(navigationManager.CanGoBack);
            Assert.False(navigationManager.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void GoBack_ThrowsException_IfOnlyOnePageInBackStack(PageInfo page1, PageInfo page2)
        {
            var navigationManager = new NavigationManager();

            navigationManager.NavigateTo(page1);
            navigationManager.NavigateTo(page2);
            navigationManager.GoBack();

            var e = Assert.Throws<InvalidOperationException>(() => navigationManager.GoBack());
            Assert.Equal("You cannot navigate backwards as the back stack is empty.", e.Message);
        }

        [Theory]
        [MemberData(nameof(SingleTestValues))]
        public void GoForward_ThrowsException_IfNoPagesInForwardStack(PageInfo page1)
        {
            var navigationManager = new NavigationManager();

            navigationManager.NavigateTo(page1);

            var e = Assert.Throws<InvalidOperationException>(() => navigationManager.GoForward());
            Assert.Equal("You cannot navigate forwards as the forward stack is empty.", e.Message);
        }

        [Fact]
        public void NavigateTo_ThrowsException_IfPageIsNull()
        {
            var navigationManager = new NavigationManager();

            var e = Assert.Throws<ArgumentNullException>(() => navigationManager.NavigateTo(null));
            Assert.Equal("Value cannot be null.\r\nParameter name: page", e.Message);
            Assert.Equal("page", e.ParamName);
        }

        [Theory]
        [MemberData(nameof(SingleTestValues))]
        public void Navigate_ToPage_AddsOneItemToStack(PageInfo page1)
        {
            var navigationManager = new NavigationManager();

            AssertEx.PropertyChangedEvents(navigationManager, () => navigationManager.NavigateTo(page1));

            Assert.Equal(1, navigationManager.NavigationStack.Count);
            Assert.Equal(page1.PageName, navigationManager.NavigationStack[0].PageName);
            Assert.Equal(page1.Arguments, navigationManager.NavigationStack[0].PageState.GetState<string>(StateNames.PageArguments));

            Assert.Equal(navigationManager.NavigationStack[0], navigationManager.CurrentPage);

            Assert.False(navigationManager.CanGoBack);
            Assert.False(navigationManager.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Navigate_ToPage_ToPage_AddsTwoItemsToStack(PageInfo page1, PageInfo page2)
        {
            var navigationManager = new NavigationManager();

            navigationManager.NavigateTo(page1);

            AssertEx.PropertyChangedEvents(navigationManager, () => navigationManager.NavigateTo(page2));

            Assert.Equal(2, navigationManager.NavigationStack.Count);
            Assert.Equal(page1.PageName, navigationManager.NavigationStack[0].PageName);
            Assert.Equal(page2.PageName, navigationManager.NavigationStack[1].PageName);

            Assert.Equal(navigationManager.NavigationStack[1], navigationManager.CurrentPage);

            Assert.True(navigationManager.CanGoBack);
            Assert.False(navigationManager.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Navigate_ToPage_ToPage_Back_LeavesOneItemOnStack(PageInfo page1, PageInfo page2)
        {
            var navigationManager = new NavigationManager();

            navigationManager.NavigateTo(page1);
            navigationManager.NavigateTo(page2);

            AssertEx.PropertyChangedEvents(navigationManager, () => navigationManager.GoBack());

            Assert.Equal(1, navigationManager.NavigationStack.Count);
            Assert.Equal(page1.PageName, navigationManager.NavigationStack[0].PageName);
            Assert.Equal(page1.Arguments, navigationManager.NavigationStack[0].PageState.GetState<string>(StateNames.PageArguments));

            Assert.Equal(navigationManager.NavigationStack[0], navigationManager.CurrentPage);

            Assert.False(navigationManager.CanGoBack);
            Assert.True(navigationManager.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Navigate_ToPage_ToPage_Back_Forward_AddsItemBackOntoStack(PageInfo page1, PageInfo page2)
        {
            var navigationManager = new NavigationManager();

            navigationManager.NavigateTo(page1);
            navigationManager.NavigateTo(page2);
            navigationManager.GoBack();

            AssertEx.PropertyChangedEvents(navigationManager, () => navigationManager.GoForward());

            Assert.Equal(2, navigationManager.NavigationStack.Count);
            Assert.Equal(page1.PageName, navigationManager.NavigationStack[0].PageName);
            Assert.Equal(page1.Arguments, navigationManager.NavigationStack[0].PageState.GetState<string>(StateNames.PageArguments));
            Assert.Equal(page2.PageName, navigationManager.NavigationStack[1].PageName);
            Assert.Equal(page2.Arguments, navigationManager.NavigationStack[1].PageState.GetState<string>(StateNames.PageArguments));
            
            Assert.Equal(navigationManager.NavigationStack[1], navigationManager.CurrentPage);

            Assert.True(navigationManager.CanGoBack);
            Assert.False(navigationManager.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(TripleTestValues))]
        public void Navigate_ToPage_ToPage_ToPage_Back_Back_Forward_AddsItemBackOntoStack(PageInfo page1, PageInfo page2, PageInfo page3)
        {
            var navigationManager = new NavigationManager();

            navigationManager.NavigateTo(page1);
            navigationManager.NavigateTo(page2);
            navigationManager.NavigateTo(page3);
            navigationManager.GoBack();
            navigationManager.GoBack();

            AssertEx.PropertyChangedEvents(navigationManager, () => navigationManager.GoForward());

            Assert.Equal(2, navigationManager.NavigationStack.Count);
            Assert.Equal(page1.PageName, navigationManager.NavigationStack[0].PageName);
            Assert.Equal(page1.Arguments, navigationManager.NavigationStack[0].PageState.GetState<string>(StateNames.PageArguments));
            Assert.Equal(page2.PageName, navigationManager.NavigationStack[1].PageName);
            Assert.Equal(page2.Arguments, navigationManager.NavigationStack[1].PageState.GetState<string>(StateNames.PageArguments));

            Assert.Equal(navigationManager.NavigationStack[1], navigationManager.CurrentPage);

            Assert.True(navigationManager.CanGoBack);
            Assert.True(navigationManager.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(TripleTestValues))]
        public void Navigate_ToPage_ToPage_ToPage_Back_Back_Forward_Forward_AddsItemsBackOntoStack(PageInfo page1, PageInfo page2, PageInfo page3)
        {
            var navigationManager = new NavigationManager();

            navigationManager.NavigateTo(page1);
            navigationManager.NavigateTo(page2);
            navigationManager.NavigateTo(page3);
            navigationManager.GoBack();
            navigationManager.GoBack();
            navigationManager.GoForward();

            AssertEx.PropertyChangedEvents(navigationManager, () => navigationManager.GoForward());

            Assert.Equal(3, navigationManager.NavigationStack.Count);
            Assert.Equal(page1.PageName, navigationManager.NavigationStack[0].PageName);
            Assert.Equal(page1.Arguments, navigationManager.NavigationStack[0].PageState.GetState<string>(StateNames.PageArguments));
            Assert.Equal(page2.PageName, navigationManager.NavigationStack[1].PageName);
            Assert.Equal(page2.Arguments, navigationManager.NavigationStack[1].PageState.GetState<string>(StateNames.PageArguments));
            Assert.Equal(page3.PageName, navigationManager.NavigationStack[2].PageName);
            Assert.Equal(page3.Arguments, navigationManager.NavigationStack[2].PageState.GetState<string>(StateNames.PageArguments));

            Assert.Equal(navigationManager.NavigationStack[2], navigationManager.CurrentPage);

            Assert.True(navigationManager.CanGoBack);
            Assert.False(navigationManager.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(TripleTestValues))]
        public void Navigate_ToPage_ToPage_Back_ToPage_LeavesTwoItemOnStack(PageInfo page1, PageInfo page2, PageInfo page3)
        {
            var navigationManager = new NavigationManager();

            navigationManager.NavigateTo(page1);
            navigationManager.NavigateTo(page2);
            navigationManager.GoBack();

            AssertEx.PropertyChangedEvents(navigationManager, () => navigationManager.NavigateTo(page3));

            Assert.Equal(2, navigationManager.NavigationStack.Count);
            Assert.Equal(page1.PageName, navigationManager.NavigationStack[0].PageName);
            Assert.Equal(page1.Arguments, navigationManager.NavigationStack[0].PageState.GetState<string>(StateNames.PageArguments));
            Assert.Equal(page3.PageName, navigationManager.NavigationStack[1].PageName);
            Assert.Equal(page3.Arguments, navigationManager.NavigationStack[1].PageState.GetState<string>(StateNames.PageArguments));

            Assert.Equal(navigationManager.NavigationStack[1], navigationManager.CurrentPage);

            Assert.True(navigationManager.CanGoBack);
            Assert.False(navigationManager.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(TripleTestValues))]
        public void Navigate_ToPage_ToPage_ToPage_Back_ToPage_Back_Forward_AddsCorrectItemOntoStack(PageInfo page1, PageInfo page2, PageInfo page3)
        {
            var navigationManager = new NavigationManager();

            navigationManager.NavigateTo(page1);
            navigationManager.NavigateTo(page2);
            navigationManager.GoBack();
            navigationManager.NavigateTo(page3);
            navigationManager.GoBack();

            AssertEx.PropertyChangedEvents(navigationManager, () => navigationManager.GoForward());

            Assert.Equal(2, navigationManager.NavigationStack.Count);
            Assert.Equal(page1.PageName, navigationManager.NavigationStack[0].PageName);
            Assert.Equal(page1.Arguments, navigationManager.NavigationStack[0].PageState.GetState<string>(StateNames.PageArguments));
            Assert.Equal(page3.PageName, navigationManager.NavigationStack[1].PageName);
            Assert.Equal(page3.Arguments, navigationManager.NavigationStack[1].PageState.GetState<string>(StateNames.PageArguments));

            Assert.Equal(navigationManager.NavigationStack[1], navigationManager.CurrentPage);

            Assert.True(navigationManager.CanGoBack);
            Assert.False(navigationManager.CanGoForward);
        }
    }
}

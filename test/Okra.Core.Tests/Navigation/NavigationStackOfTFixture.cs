using Okra.Navigation;
using Okra.Tests.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Okra.Tests.Navigation
{
    public class NavigationStackOfTFixture
    {
        // *** Test Data ***

        public static object[][] SingleTestValues = { new object[] { 42 },
                                                      new object[] { "string" },
                                                      new object[] { new TestClass("string") } };

        public static object[][] DoubleTestValues = { new object[] { 42, 68 },
                                                      new object[] { "alpha", "beta" },
                                                      new object[] { new TestClass("alpha"), new TestClass("beta") } };

        public static object[][] TripleTestValues = { new object[] { 42, 68, 36 },
                                                      new object[] { "alpha", "beta", "gamma" },
                                                      new object[] { new TestClass("alpha"), new TestClass("beta"), new TestClass("gamma") } };

        // *** Tests ***

        [Theory]
        [MemberData(nameof(SingleTestValues))]
        public void InitialStack_IsEmpty_DefaultConstructor<T>(T defaultItem)
        {
            var stack = new NavigationStack<T>();

            Assert.NotNull(stack);

            Assert.Equal(0, stack.Count);
            Assert.Equal(0, stack.MinimumCount);
            Assert.Equal(default(T), stack.CurrentItem);

            Assert.False(stack.CanGoBack);
            Assert.False(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(SingleTestValues))]
        public void InitialStack_IsEmpty_AllowsEmptyStack<T>(T defaultItem)
        {
            var stack = new NavigationStack<T>(NavigationStackType.AllowEmptyStack);

            Assert.NotNull(stack);

            Assert.Equal(0, stack.Count);
            Assert.Equal(0, stack.MinimumCount);
            Assert.Equal(default(T), stack.CurrentItem);

            Assert.False(stack.CanGoBack);
            Assert.False(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(SingleTestValues))]
        public void InitialStack_IsEmpty_RequiresFirstItem<T>(T defaultItem)
        {
            var stack = new NavigationStack<T>(NavigationStackType.RequireFirstItem);

            Assert.NotNull(stack);

            Assert.Equal(0, stack.Count);
            Assert.Equal(1, stack.MinimumCount);
            Assert.Equal(default(T), stack.CurrentItem);

            Assert.False(stack.CanGoBack);
            Assert.False(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Clear_ClearsStackContent<T>(T item1, T item2)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);
            stack.GoBack();

            AssertEx.PropertyChangedEvents(stack, () => stack.Clear());

            Assert.Equal(0, stack.Count);
            Assert.Equal(default(T), stack.CurrentItem);

            Assert.False(stack.CanGoBack);
            Assert.False(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void GetEnumerator_Generic_EnumeratesPagesInStack<T>(T item1, T item2)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);

            T[] items = ((IEnumerable<T>)stack).ToArray();

            Assert.Equal(new T[] { item1, item2 }, items);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void GetEnumerator_NonGeneric_EnumeratesPagesInStack<T>(T item1, T item2)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);

            var items = new List<T>();

            foreach (T item in (IEnumerable)stack)
                items.Add(item);

            Assert.Equal(new T[] { item1, item2 }, items);
        }

        [Theory]
        [MemberData(nameof(SingleTestValues))]
        public void GoBack_ThrowsException_IfNoPagesInBackStack_AllowsEmptyStack<T>(T item)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item);
            stack.GoBack();

            var e = Assert.Throws<InvalidOperationException>(() => stack.GoBack());
            Assert.Equal("You cannot navigate backwards as the back stack is empty.", e.Message);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void GoBack_ThrowsException_IfNoPagesInBackStack_RequiresFirstItem<T>(T item1, T item2)
        {
            var stack = new NavigationStack<T>(NavigationStackType.RequireFirstItem);

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);
            stack.GoBack();

            var e = Assert.Throws<InvalidOperationException>(() => stack.GoBack());
            Assert.Equal("You cannot navigate backwards as the back stack is empty.", e.Message);
        }

        [Theory]
        [MemberData(nameof(SingleTestValues))]
        public void GoForward_ThrowsException_IfNoPagesInForwardStack<T>(T item)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item);

            var e = Assert.Throws<InvalidOperationException>(() => stack.GoForward());
            Assert.Equal("You cannot navigate forwards as the forward stack is empty.", e.Message);
        }

        [Theory]
        [MemberData(nameof(TripleTestValues))]
        public void GoBackTo_ThrowsException_IfPageNotInBackStack<T>(T item1, T item2, T item3)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);

            var e = Assert.Throws<InvalidOperationException>(() => stack.GoBackTo(item3));
            Assert.Equal($"The specified page '{item3}' does not exist in the navigation stack.", e.Message);
        }

        [Theory]
        [MemberData(nameof(TripleTestValues))]
        public void GoForwardTo_ThrowsException_IfPageNotInForwardStack<T>(T item1, T item2, T item3)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);
            stack.GoBack();
            stack.GoBack();

            var e = Assert.Throws<InvalidOperationException>(() => stack.GoForwardTo(item3));
            Assert.Equal($"The specified page '{item3}' does not exist in the navigation stack.", e.Message);
        }

        [Theory]
        [MemberData(nameof(SingleTestValues))]
        public void Navigate_ToPage_AddsOneItemToStack_AllowsEmptyStack<T>(T item)
        {
            var stack = new NavigationStack<T>();

            AssertEx.PropertyChangedEvents(stack, () => stack.NavigateTo(item));

            Assert.Equal(1, stack.Count);
            Assert.Equal(item, stack[0]);
            Assert.Equal(item, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.False(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(SingleTestValues))]
        public void Navigate_ToPage_AddsOneItemToStack_RequiresFirstItem<T>(T item)
        {
            var stack = new NavigationStack<T>(NavigationStackType.RequireFirstItem);

            AssertEx.PropertyChangedEvents(stack, () => stack.NavigateTo(item));

            Assert.Equal(1, stack.Count);
            Assert.Equal(item, stack[0]);
            Assert.Equal(item, stack.CurrentItem);

            Assert.False(stack.CanGoBack);
            Assert.False(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Navigate_ToPage_ToPage_AddsTwoItemsToStack_AllowsEmptyStack<T>(T item1, T item2)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);

            AssertEx.PropertyChangedEvents(stack, () => stack.NavigateTo(item2));

            Assert.Equal(2, stack.Count);
            Assert.Equal(item1, stack[0]);
            Assert.Equal(item2, stack[1]);
            Assert.Equal(item2, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.False(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Navigate_ToPage_ToPage_AddsTwoItemsToStack_RequiresFirstItem<T>(T item1, T item2)
        {
            var stack = new NavigationStack<T>(NavigationStackType.RequireFirstItem);

            stack.NavigateTo(item1);

            AssertEx.PropertyChangedEvents(stack, () => stack.NavigateTo(item2));

            Assert.Equal(2, stack.Count);
            Assert.Equal(item1, stack[0]);
            Assert.Equal(item2, stack[1]);
            Assert.Equal(item2, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.False(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(SingleTestValues))]
        public void Navigate_ToPage_Back_GivesEmptyStack<T>(T item)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item);

            AssertEx.PropertyChangedEvents(stack, () => stack.GoBack());

            Assert.Equal(0, stack.Count);
            Assert.Equal(default(T), stack.CurrentItem);

            Assert.False(stack.CanGoBack);
            Assert.True(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Navigate_ToPage_ToPage_Back_LeavesOneItemOnStack_AllowsEmptyStack<T>(T item1, T item2)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);

            AssertEx.PropertyChangedEvents(stack, () => stack.GoBack());

            Assert.Equal(1, stack.Count);
            Assert.Equal(item1, stack[0]);
            Assert.Equal(item1, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.True(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Navigate_ToPage_ToPage_Back_LeavesOneItemOnStack_RequiresFirstItem<T>(T item1, T item2)
        {
            var stack = new NavigationStack<T>(NavigationStackType.RequireFirstItem);

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);

            AssertEx.PropertyChangedEvents(stack, () => stack.GoBack());

            Assert.Equal(1, stack.Count);
            Assert.Equal(item1, stack[0]);
            Assert.Equal(item1, stack.CurrentItem);

            Assert.False(stack.CanGoBack);
            Assert.True(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(SingleTestValues))]
        public void Navigate_ToPage_Back_Forward_AddsItemBackOntoStack<T>(T item)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item);
            stack.GoBack();

            AssertEx.PropertyChangedEvents(stack, () => stack.GoForward());

            Assert.Equal(1, stack.Count);
            Assert.Equal(item, stack[0]);
            Assert.Equal(item, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.False(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Navigate_ToPage_ToPage_Back_Forward_AddsItemBackOntoStack<T>(T item1, T item2)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);
            stack.GoBack();

            AssertEx.PropertyChangedEvents(stack, () => stack.GoForward());

            Assert.Equal(2, stack.Count);
            Assert.Equal(item1, stack[0]);
            Assert.Equal(item2, stack[1]);
            Assert.Equal(item2, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.False(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Navigate_ToPage_ToPage_Back_Back_Forward_AddsItemBackOntoStack<T>(T item1, T item2)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);
            stack.GoBack();
            stack.GoBack();

            AssertEx.PropertyChangedEvents(stack, () => stack.GoForward());

            Assert.Equal(1, stack.Count);
            Assert.Equal(item1, stack[0]);
            Assert.Equal(item1, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.True(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Navigate_ToPage_ToPage_Back_Back_Forward_Forward_AddsItemsBackOntoStack<T>(T item1, T item2)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);
            stack.GoBack();
            stack.GoBack();
            stack.GoForward();

            AssertEx.PropertyChangedEvents(stack, () => stack.GoForward());

            Assert.Equal(2, stack.Count);
            Assert.Equal(item1, stack[0]);
            Assert.Equal(item2, stack[1]);
            Assert.Equal(item2, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.False(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Navigate_ToPage_Back_ToPage_LeavesOneItemOnStack<T>(T item1, T item2)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.GoBack();

            AssertEx.PropertyChangedEvents(stack, () => stack.NavigateTo(item2));

            Assert.Equal(1, stack.Count);
            Assert.Equal(item2, stack[0]);
            Assert.Equal(item2, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.False(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Navigate_ToPage_Back_ToPage_Back_Forward_AddsCorrectItemOntoStack<T>(T item1, T item2)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.GoBack();
            stack.NavigateTo(item2);
            stack.GoBack();

            AssertEx.PropertyChangedEvents(stack, () => stack.GoForward());

            Assert.Equal(1, stack.Count);
            Assert.Equal(item2, stack[0]);
            Assert.Equal(item2, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.False(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(TripleTestValues))]
        public void Navigate_ToPage_ToPage_ToPage_GoBackTo_GoesBackOnePage<T>(T item1, T item2, T item3)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);
            stack.NavigateTo(item3);

            AssertEx.PropertyChangedEvents(stack, () => stack.GoBackTo(item2));

            Assert.Equal(2, stack.Count);
            Assert.Equal(item1, stack[0]);
            Assert.Equal(item2, stack[1]);
            Assert.Equal(item2, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.True(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(TripleTestValues))]
        public void Navigate_ToPage_ToPage_ToPage_GoBackTo_GoesBackTwoPages<T>(T item1, T item2, T item3)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);
            stack.NavigateTo(item3);

            AssertEx.PropertyChangedEvents(stack, () => stack.GoBackTo(item1));

            Assert.Equal(1, stack.Count);
            Assert.Equal(item1, stack[0]);
            Assert.Equal(item1, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.True(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(TripleTestValues))]
        public void Navigate_ToPage_ToPage_ToPage_GoBackTo_GoBack_GoesBackToExpectedPage<T>(T item1, T item2, T item3)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);
            stack.NavigateTo(item3);
            stack.GoBackTo(item2);

            AssertEx.PropertyChangedEvents(stack, () => stack.GoBack());

            Assert.Equal(1, stack.Count);
            Assert.Equal(item1, stack[0]);
            Assert.Equal(item1, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.True(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(TripleTestValues))]
        public void Navigate_ToPage_ToPage_ToPage_GoBackTo_GoForward_GoesForwardToExpectedPage<T>(T item1, T item2, T item3)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);
            stack.NavigateTo(item3);
            stack.GoBackTo(item2);

            AssertEx.PropertyChangedEvents(stack, () => stack.GoForward());

            Assert.Equal(3, stack.Count);
            Assert.Equal(item1, stack[0]);
            Assert.Equal(item2, stack[1]);
            Assert.Equal(item3, stack[2]);
            Assert.Equal(item3, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.False(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Navigate_ToPage_ToPage_GoBack_GoBack_GoForwardTo_GoesForwardOnePage<T>(T item1, T item2)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);
            stack.GoBack();
            stack.GoBack();

            AssertEx.PropertyChangedEvents(stack, () => stack.GoForwardTo(item1));

            Assert.Equal(1, stack.Count);
            Assert.Equal(item1, stack[0]);
            Assert.Equal(item1, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.True(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Navigate_ToPage_ToPage_GoBack_GoBack_GoForwardTo_GoesForwardTwoPages<T>(T item1, T item2)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);
            stack.GoBack();
            stack.GoBack();

            AssertEx.PropertyChangedEvents(stack, () => stack.GoForwardTo(item2));

            Assert.Equal(2, stack.Count);
            Assert.Equal(item1, stack[0]);
            Assert.Equal(item2, stack[1]);
            Assert.Equal(item2, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.False(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Navigate_ToPage_ToPage_GoBack_GoBack_GoForwardTo_GoForward_GoesForwardToExpectedPage<T>(T item1, T item2)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);
            stack.GoBack();
            stack.GoBack();
            stack.GoForwardTo(item1);

            AssertEx.PropertyChangedEvents(stack, () => stack.GoForward());

            Assert.Equal(2, stack.Count);
            Assert.Equal(item1, stack[0]);
            Assert.Equal(item2, stack[1]);
            Assert.Equal(item2, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.False(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(TripleTestValues))]
        public void Navigate_ToPage_ToPage_ToPage_GoBack_GoBack_GoBack_GoForwardTo_GoBack_GoesBackToExpectedPage<T>(T item1, T item2, T item3)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);
            stack.NavigateTo(item3);
            stack.GoBack();
            stack.GoBack();
            stack.GoBack();
            stack.GoForwardTo(item2);

            AssertEx.PropertyChangedEvents(stack, () => stack.GoForward());

            Assert.Equal(3, stack.Count);
            Assert.Equal(item1, stack[0]);
            Assert.Equal(item2, stack[1]);
            Assert.Equal(item3, stack[2]);
            Assert.Equal(item3, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.False(stack.CanGoForward);
        }

        // *** Test Classes ***

        public class TestClass
        {
            public TestClass(string name)
            {
                Name = name;
            }

            public string Name { get; }
        }
    }
}

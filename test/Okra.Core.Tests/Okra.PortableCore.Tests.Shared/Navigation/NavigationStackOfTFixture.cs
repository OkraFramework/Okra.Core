using Okra.Navigation;
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
                                                      new object[] { new TestClass("alpha"), new TestClass("beta") },
                                                      new object[] { new TestClass("alpha"), new TestClass("beta") } };

        // *** Tests ***

        [Theory]
        [MemberData(nameof(SingleTestValues))]
        public void InitialStack_IsEmpty<T>(T defaultItem)
        {
            var stack = new NavigationStack<T>();

            Assert.NotNull(stack);

            Assert.Equal(0, stack.Count);
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

            AssertPropertyChangedEvents(stack, () => stack.Clear());

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
        public void GoBack_ThrowsException_IfNoPagesInBackStack<T>(T item)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item);
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
        [MemberData(nameof(SingleTestValues))]
        public void Navigate_ToPage_AddsOneItemToStack<T>(T item)
        {
            var stack = new NavigationStack<T>();

            AssertPropertyChangedEvents(stack, () => stack.NavigateTo(item));

            Assert.Equal(1, stack.Count);
            Assert.Equal(item, stack[0]);
            Assert.Equal(item, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.False(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Navigate_ToPage_ToPage_AddsTwoItemsToStack<T>(T item1, T item2)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);

            AssertPropertyChangedEvents(stack, () => stack.NavigateTo(item2));

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

            AssertPropertyChangedEvents(stack, () => stack.GoBack());

            Assert.Equal(0, stack.Count);
            Assert.Equal(default(T), stack.CurrentItem);

            Assert.False(stack.CanGoBack);
            Assert.True(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(DoubleTestValues))]
        public void Navigate_ToPage_ToPage_Back_LeavesOneItemOnStack<T>(T item1, T item2)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item1);
            stack.NavigateTo(item2);

            AssertPropertyChangedEvents(stack, () => stack.GoBack());

            Assert.Equal(1, stack.Count);
            Assert.Equal(item1, stack[0]);
            Assert.Equal(item1, stack.CurrentItem);

            Assert.True(stack.CanGoBack);
            Assert.True(stack.CanGoForward);
        }

        [Theory]
        [MemberData(nameof(SingleTestValues))]
        public void Navigate_ToPage_Back_Forward_AddsItemBackOntoStack<T>(T item)
        {
            var stack = new NavigationStack<T>();

            stack.NavigateTo(item);
            stack.GoBack();

            AssertPropertyChangedEvents(stack, () => stack.GoForward());

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

            AssertPropertyChangedEvents(stack, () => stack.GoForward());

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

            AssertPropertyChangedEvents(stack, () => stack.GoForward());

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

            AssertPropertyChangedEvents(stack, () => stack.GoForward());

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

            AssertPropertyChangedEvents(stack, () => stack.NavigateTo(item2));

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

            AssertPropertyChangedEvents(stack, () => stack.GoForward());

            Assert.Equal(1, stack.Count);
            Assert.Equal(item2, stack[0]);
            Assert.Equal(item2, stack.CurrentItem);

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

        // *** Private Methods ***

        private void AssertPropertyChangedEvents<T>(T obj, Action action) where T : INotifyPropertyChanged
        {
            TypeInfo typeInfo = typeof(T).GetTypeInfo();

            // Get current values for all properties

            var initialProperties = new Dictionary<PropertyInfo, object>();

            foreach (PropertyInfo property in typeInfo.DeclaredProperties)
            {
                if (property.GetIndexParameters().Length == 0)
                    initialProperties[property] = property.GetValue(obj);
            }

            // Perform the action, recording property changed events

            var changedPropertyNames = new List<string>();

            PropertyChangedEventHandler eventHandler = (sender, e) =>
            {
                Assert.Equal(obj, sender);
                changedPropertyNames.Add(e.PropertyName);
            };

            obj.PropertyChanged += eventHandler;
            action();
            obj.PropertyChanged -= eventHandler;

            // Check that property changed events were raised for all modified properties

            foreach (KeyValuePair<PropertyInfo, object> initialProperty in initialProperties)
            {
                if (!object.Equals(initialProperty.Key.GetValue(obj), initialProperty.Value))
                {
                    string propertyName = initialProperty.Key.Name;
                    Assert.True(changedPropertyNames.Contains(propertyName), $"Property '{propertyName}' did not raise a PropertyChanged event");
                    changedPropertyNames.Remove(propertyName);
                }
            }

            // Check that no unchanged properties raised PropertyChanged events

            if (changedPropertyNames.Count > 0)
                Assert.True(false, $"Unchanged Property '{changedPropertyNames[0]}' raised PropertyChanged event");
        }
    }
}

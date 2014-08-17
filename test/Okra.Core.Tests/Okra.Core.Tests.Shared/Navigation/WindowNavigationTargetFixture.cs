using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Okra.Navigation;
using Okra.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Core;

namespace Okra.Tests.Navigation
{
    [TestClass]
    public class WindowNavigationTargetFixture
    {
        [TestMethod]
        public void NavigateTo_SetsWindowContent()
        {
            TestableWindowNavigationTarget target = new TestableWindowNavigationTarget();
            MockNavigationBase navigationBase = new MockNavigationBase();

            object page = new object();
            target.NavigateTo(page, navigationBase);

            CollectionAssert.AreEqual(new object[] { page }, target.SetWindowContentCalls);
        }

        [TestMethod]
        public void NavigateTo_FirstCallRegistersEventHandlers()
        {
            TestableWindowNavigationTarget target = new TestableWindowNavigationTarget();
            MockNavigationBase navigationBase = new MockNavigationBase();
            object page = new object();

            Assert.AreEqual(0, target.RegisterEventHandlersCallCount);

            target.NavigateTo(page, navigationBase);

            Assert.AreEqual(1, target.RegisterEventHandlersCallCount);
        }

        [TestMethod]
        public void NavigateTo_RegisterEventHandlersIsOnlyCalledOnce()
        {
            TestableWindowNavigationTarget target = new TestableWindowNavigationTarget();
            MockNavigationBase navigationBase = new MockNavigationBase();
            object page = new object();

            Assert.AreEqual(0, target.RegisterEventHandlersCallCount);

            target.NavigateTo(page, navigationBase);
            target.NavigateTo(page, navigationBase);

            Assert.AreEqual(1, target.RegisterEventHandlersCallCount);
        }

        // *** Private classes ***

        public class TestableWindowNavigationTarget : WindowNavigationTarget
        {
            public int RegisterEventHandlersCallCount = 0;
            public List<object> SetWindowContentCalls = new List<object>();

            protected override void RegisterEventHandlers()
            {
                RegisterEventHandlersCallCount++;
            }

            protected override void SetWindowContent(object page)
            {
                SetWindowContentCalls.Add(page);
            }
        }
    }
}

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Okra.Navigation;
using Okra.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Text;

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

        // *** Private classes ***

        public class TestableWindowNavigationTarget : WindowNavigationTarget
        {
            public List<object> SetWindowContentCalls = new List<object>();

            protected override void SetWindowContent(object page)
            {
                SetWindowContentCalls.Add(page);
            }
        }
    }
}

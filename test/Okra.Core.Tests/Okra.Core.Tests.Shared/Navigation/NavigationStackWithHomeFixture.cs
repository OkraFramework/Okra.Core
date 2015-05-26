using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Okra.Navigation;
using Okra.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Okra.Tests.Navigation
{
    [TestClass]
    public class NavigationStackWithHomeFixture
    {
        // *** Property Tests ***

        [TestMethod]
        public void CanGoBack_IsFalseIfNoPagesNavigated()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            Assert.AreEqual(false, navigationStack.CanGoBack);
        }

        [TestMethod]
        public void CanGoBack_IsFalseIfOnePageNavigated()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.AreEqual(false, navigationStack.CanGoBack);
        }

        [TestMethod]
        public void CanGoBack_IsTrueIfTwoPagesNavigated()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.AreEqual(true, navigationStack.CanGoBack);
        }

        [TestMethod]
        public void CanGoBack_IsFalseIfTwoPageNavigatedThenBack()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.GoBack();

            Assert.AreEqual(false, navigationStack.CanGoBack);
        }

        // *** Method Tests ***

        [TestMethod]
        public void GoBack_ThrowsException_NoPageInBackStack()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            Assert.ThrowsException<InvalidOperationException>(() => navigationStack.GoBack());
        }

        [TestMethod]
        public void GoBack_ThrowsException_SinglePageInBackStack()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.ThrowsException<InvalidOperationException>(() => navigationStack.GoBack());
        }

        // *** Behavior Tests ***

        [TestMethod]
        public void PropertyChanged_CanGoBack_IsNotCalledWhenFirstPageNavigated()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.AreEqual(0, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CanGoBack_IsCalledWhenSecondPageNavigated()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.AreEqual(1, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CanGoBack_IsNotCalledWhenThirdPageNavigated()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.NavigateTo(new PageInfo("Page 3", null));

            Assert.AreEqual(0, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CanGoBack_IsNotCalledWhenThirdPageNavigatedThenBack()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.NavigateTo(new PageInfo("Page 3", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.GoBack();

            Assert.AreEqual(0, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CanGoBack_IsCalledWhenSecondPageNavigatedThenBack()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.GoBack();

            Assert.AreEqual(1, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CanGoBack_IsCalledWheNavigatingBackToFirstPage()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.NavigateTo(new PageInfo("Page 3", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.GoBackTo(navigationStack[0]);

            Assert.AreEqual(1, changedCount);
        }

        [TestMethod]
        public void PropertyChanged_CanGoBack_IsNotCalledWheNavigatingBackToSecondPage()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.NavigateTo(new PageInfo("Page 3", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.GoBackTo(navigationStack[1]);

            Assert.AreEqual(0, changedCount);
        }
    }
}

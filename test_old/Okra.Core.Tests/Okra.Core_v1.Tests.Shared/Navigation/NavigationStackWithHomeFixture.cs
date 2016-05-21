using Okra.Navigation;
using Okra.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xunit;

namespace Okra.Tests.Navigation
{
    public class NavigationStackWithHomeFixture
    {
        // *** Property Tests ***

        [Fact]
        public void CanGoBack_IsFalseIfNoPagesNavigated()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            Assert.Equal(false, navigationStack.CanGoBack);
        }

        [Fact]
        public void CanGoBack_IsFalseIfOnePageNavigated()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.Equal(false, navigationStack.CanGoBack);
        }

        [Fact]
        public void CanGoBack_IsTrueIfTwoPagesNavigated()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.Equal(true, navigationStack.CanGoBack);
        }

        [Fact]
        public void CanGoBack_IsFalseIfTwoPageNavigatedThenBack()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.GoBack();

            Assert.Equal(false, navigationStack.CanGoBack);
        }

        // *** Method Tests ***

        [Fact]
        public void GoBack_ThrowsException_NoPageInBackStack()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            var e = Assert.Throws<InvalidOperationException>(() => navigationStack.GoBack());

            Assert.Equal("You cannot navigate backwards as the back stack is empty.", e.Message);
        }

        [Fact]
        public void GoBack_ThrowsException_SinglePageInBackStack()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            var e = Assert.Throws<InvalidOperationException>(() => navigationStack.GoBack());

            Assert.Equal("You cannot navigate backwards as the back stack is empty.", e.Message);
        }

        // *** Behavior Tests ***

        [Fact]
        public void PropertyChanged_CanGoBack_IsNotCalledWhenFirstPageNavigated()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            Assert.Equal(0, changedCount);
        }

        [Fact]
        public void PropertyChanged_CanGoBack_IsCalledWhenSecondPageNavigated()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_CanGoBack_IsNotCalledWhenThirdPageNavigated()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.NavigateTo(new PageInfo("Page 3", null));

            Assert.Equal(0, changedCount);
        }

        [Fact]
        public void PropertyChanged_CanGoBack_IsNotCalledWhenThirdPageNavigatedThenBack()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.NavigateTo(new PageInfo("Page 3", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.GoBack();

            Assert.Equal(0, changedCount);
        }

        [Fact]
        public void PropertyChanged_CanGoBack_IsCalledWhenSecondPageNavigatedThenBack()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.GoBack();

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_CanGoBack_IsCalledWheNavigatingBackToFirstPage()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.NavigateTo(new PageInfo("Page 3", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.GoBackTo(navigationStack[0]);

            Assert.Equal(1, changedCount);
        }

        [Fact]
        public void PropertyChanged_CanGoBack_IsNotCalledWheNavigatingBackToSecondPage()
        {
            NavigationStackWithHome navigationStack = new NavigationStackWithHome();

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));
            navigationStack.NavigateTo(new PageInfo("Page 3", null));

            int changedCount = 0;
            navigationStack.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e) { if (e.PropertyName == "CanGoBack") changedCount++; };

            navigationStack.GoBackTo(navigationStack[1]);

            Assert.Equal(0, changedCount);
        }
    }
}

using Okra.Navigation;
using Okra.Tests.Helpers;
using Okra.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Xunit;

namespace Okra.Tests.Navigation
{
    public class SettingsPaneManagerFixture
    {
        // *** Constructor Tests ***

        [Fact]
        public void Constructor_WithViewFactory_ThrowsException_WhenViewFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new SettingsPaneManager(null));
        }

        [Fact]
        public void Constructor_WithViewFactoryAndNavigationStack_ThrowsException_WhenViewFactoryIsNull()
        {
            INavigationStack navigationStack = new MockNavigationStack();

            Assert.Throws<ArgumentNullException>(() => new TestableSettingsPaneManager(null, navigationStack));
        }

        [Fact]
        public void Constructor_WithViewFactoryAndNavigationStack_ThrowsException_WhenNavigationStackIsNull()
        {
            IViewFactory viewFactory = MockViewFactory.NoPageDefined;

            Assert.Throws<ArgumentNullException>(() => new TestableSettingsPaneManager(viewFactory, null));
        }

        // *** Property Tests ***

        [Fact]
        public void NavigationStack_DefaultsToNavigationStack()
        {
            ISettingsPaneManager settingsPaneManager = new SettingsPaneManager(MockViewFactory.WithPageAndViewModel);

            Assert.Equal(typeof(NavigationStack), settingsPaneManager.NavigationStack.GetType());
        }

        // *** Method Tests ***

        [UITestMethod]
        public void DisplayPage_CreatesNewSettingsFlyoutIfNotOpen()
        {
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager();

            object page = new object();
            settingsPaneManager.CallDisplayPage(page);

            Assert.Equal(1, settingsPaneManager.ShowSettingsFlyoutCalls.Count);
            Assert.NotNull(settingsPaneManager.ShowSettingsFlyoutCalls[0]);
        }

        [UITestMethod]
        public void DisplayPage_UsesSameSettingsFlyoutForMultipleCalls()
        {
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager();

            object page = new object();
            settingsPaneManager.CallDisplayPage(page);
            settingsPaneManager.CallDisplayPage(page);

            Assert.Equal(2, settingsPaneManager.ShowSettingsFlyoutCalls.Count);
            Assert.Equal(settingsPaneManager.ShowSettingsFlyoutCalls[0], settingsPaneManager.ShowSettingsFlyoutCalls[1]);
        }

        [UITestMethod]
        public void DisplayPage_SetsSettingsFlyoutHostContent()
        {
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager();

            UserControl page = new UserControl();
            settingsPaneManager.CallDisplayPage(page);

            SettingsFlyout flyout = settingsPaneManager.ShowSettingsFlyoutCalls.First();
            Assert.Equal(page, flyout.Content);
        }

        [UITestMethod]
        public void DisplayPage_WithSettingsFlyout_SetsSettingsFlyoutHostContent()
        {
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager();

            SettingsFlyout page = new SettingsFlyout();
            settingsPaneManager.CallDisplayPage(page);

            SettingsFlyout flyout = settingsPaneManager.ShowSettingsFlyoutCalls.First();
            Assert.Equal(page, flyout.Content);
        }

        [UITestMethod]
        public void DisplayPage_UsesDefaultSettingsFlyoutTemplate()
        {
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager();

            UserControl page = new UserControl();
            settingsPaneManager.CallDisplayPage(new SettingsFlyout());
            settingsPaneManager.CallDisplayPage(page);

            SettingsFlyout flyout = settingsPaneManager.ShowSettingsFlyoutCalls.First();
            Assert.Equal(null, flyout.Template);
        }

        [UITestMethod]
        public void DisplayPage_WithSettingsFlyout_UsesCustomTemplate()
        {
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager();

            SettingsFlyout page = new SettingsFlyout();
            settingsPaneManager.CallDisplayPage(new UserControl());
            settingsPaneManager.CallDisplayPage(page);

            SettingsFlyout flyout = settingsPaneManager.ShowSettingsFlyoutCalls.First();
            Assert.NotNull(flyout.Template);
        }

        [UITestMethod]
        public void DisplayPage_SetsSettingsFlyoutTitle()
        {
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager();

            UserControl page = new UserControl();
            SettingsPaneInfo.SetTitle(page, "Test Title");
            settingsPaneManager.CallDisplayPage(page);

            SettingsFlyout flyout = settingsPaneManager.ShowSettingsFlyoutCalls.First();
            Assert.Equal("Test Title", flyout.Title);
        }

        [UITestMethod]
        public void DisplayPage_SetsSettingsFlyoutWidth()
        {
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager();

            UserControl page = new UserControl();
            SettingsPaneInfo.SetWidth(page, 420);
            settingsPaneManager.CallDisplayPage(page);

            SettingsFlyout flyout = settingsPaneManager.ShowSettingsFlyoutCalls.First();
            Assert.Equal(420, flyout.Width);
        }

        [UITestMethod]
        public void DisplayPage_WithSettingsFlyout_SetsSettingsFlyoutHostWidth()
        {
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager();

            UserControl page1 = new UserControl();
            SettingsPaneInfo.SetWidth(page1, 420);
            settingsPaneManager.CallDisplayPage(page1);

            SettingsFlyout page2 = new SettingsFlyout() { Width = 500 };
            settingsPaneManager.CallDisplayPage(page2);

            SettingsFlyout flyout = settingsPaneManager.ShowSettingsFlyoutCalls.First();
            Assert.Equal(500, flyout.Width);
        }

        [UITestMethod]
        public void DisplayPage_SetsSettingsFlyoutIconSource()
        {
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager();
            ImageSource icon = new BitmapImage();

            UserControl page = new UserControl();
            SettingsPaneInfo.SetIconSource(page, icon);
            settingsPaneManager.CallDisplayPage(page);

            SettingsFlyout flyout = settingsPaneManager.ShowSettingsFlyoutCalls.First();
            Assert.Equal(icon, flyout.IconSource);
        }

        [UITestMethod]
        public void DisplayPage_SetsSettingsFlyoutHeaderBackground()
        {
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager();
            Brush brush = new SolidColorBrush();

            UserControl page = new UserControl();
            SettingsPaneInfo.SetHeaderBackground(page, brush);
            settingsPaneManager.CallDisplayPage(page);

            SettingsFlyout flyout = settingsPaneManager.ShowSettingsFlyoutCalls.First();
            Assert.Equal(brush, flyout.HeaderBackground);
        }

        [UITestMethod]
        public void DisplayPage_SetsSettingsFlyoutHeaderForeground()
        {
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager();
            Brush brush = new SolidColorBrush();

            UserControl page = new UserControl();
            SettingsPaneInfo.SetHeaderForeground(page, brush);
            settingsPaneManager.CallDisplayPage(page);

            SettingsFlyout flyout = settingsPaneManager.ShowSettingsFlyoutCalls.First();
            Assert.Equal(brush, flyout.HeaderForeground);
        }

        [UITestMethod]
        public void OnSettingsPaneBackClick_ThrowsException_IfEventArgsIsNull()
        {
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager();

            Assert.Throws<ArgumentNullException>(() => settingsPaneManager.CallOnSettingsPaneBackClick(null));
        }

        [UITestMethod]
        public void ShowSettingsFlyout_ThrowsException_IfEventArgsIsNull()
        {
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager();

            Assert.Throws<ArgumentNullException>(() => settingsPaneManager.CallShowSettingsFlyoutDirect(null));
        }

        // *** Behaviour Tests ***

        [UITestMethod]
        public void FlyoutClosed_ClearsNavigationStack()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager(navigationStack: navigationStack);

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            settingsPaneManager.CallOnSettingsFlyoutClosed();

            string[] pageNames = navigationStack.Select(page => page.PageName).ToArray();
            Assert.Equal(new string[] { }, pageNames);
        }

        [UITestMethod]
        public void FlyoutClosed_FiresFlyoutClosedEvent()
        {
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager();

            int flyoutClosedCount = 0;
            int flyoutOpenedCount = 0;
            settingsPaneManager.FlyoutClosed += (sender, e) => { flyoutClosedCount++; };
            settingsPaneManager.FlyoutOpened += (sender, e) => { flyoutOpenedCount++; };

            settingsPaneManager.CallOnSettingsFlyoutClosed();

            Assert.Equal(1, flyoutClosedCount);
            Assert.Equal(0, flyoutOpenedCount);
        }

        [UITestMethod]
        public void FlyoutOpened_FiresFlyoutOpenedEvent()
        {
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager();

            int flyoutClosedCount = 0;
            int flyoutOpenedCount = 0;
            settingsPaneManager.FlyoutClosed += (sender, e) => { flyoutClosedCount++; };
            settingsPaneManager.FlyoutOpened += (sender, e) => { flyoutOpenedCount++; };

            settingsPaneManager.CallOnSettingsFlyoutOpened();

            Assert.Equal(0, flyoutClosedCount);
            Assert.Equal(1, flyoutOpenedCount);
        }

        [UITestMethod]
        public void SettingsPaneBackClick_HandlesEvent()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager(navigationStack: navigationStack);

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            BackClickEventArgs e = new BackClickEventArgs() { Handled = false };
            settingsPaneManager.CallOnSettingsPaneBackClick(e);

            Assert.Equal(true, e.Handled);
        }

        [UITestMethod]
        public void SettingsPaneBackClick_GoesBackInNavigationStack()
        {
            MockNavigationStack navigationStack = new MockNavigationStack();
            TestableSettingsPaneManager settingsPaneManager = CreateSettingsPaneManager(navigationStack: navigationStack);

            navigationStack.NavigateTo(new PageInfo("Page 1", null));
            navigationStack.NavigateTo(new PageInfo("Page 2", null));

            BackClickEventArgs e = new BackClickEventArgs() { Handled = false };
            settingsPaneManager.CallOnSettingsPaneBackClick(e);

            string[] pageNames = navigationStack.Select(page => page.PageName).ToArray();
            Assert.Equal(new string[] { "Page 1" }, pageNames);
        }

        // *** Private Methods ***

        private TestableSettingsPaneManager CreateSettingsPaneManager(IViewFactory viewFactory = null, INavigationStack navigationStack = null)
        {
            if (viewFactory == null)
                viewFactory = MockViewFactory.WithPageAndViewModel;

            if (navigationStack == null)
                navigationStack = new MockNavigationStack();

            TestableSettingsPaneManager settingsPaneManager = new TestableSettingsPaneManager(viewFactory, navigationStack);

            return settingsPaneManager;
        }

        // *** Private Sub-classes ***

        private class TestableSettingsPaneManager : SettingsPaneManager
        {
            // *** Fields ***

            public List<SettingsFlyout> ShowSettingsFlyoutCalls = new List<SettingsFlyout>();

            // *** Constructors ***

            public TestableSettingsPaneManager(IViewFactory viewFactory, INavigationStack navigationStack)
                : base(viewFactory, navigationStack)
            {
            }

            // *** Properties ***

            public new INavigationStack NavigationStack
            {
                get
                {
                    return base.NavigationStack;
                }
            }

            // *** Methods ***

            public void CallOnSettingsFlyoutClosed()
            {
                base.OnSettingsFlyoutUnloaded(null, null);
            }

            public void CallOnSettingsFlyoutOpened()
            {
                base.OnSettingsFlyoutLoaded(null, null);
            }

            public void CallOnSettingsPaneBackClick(BackClickEventArgs e)
            {
                base.OnSettingsPaneBackClick(null, e);
            }

            public void CallShowSettingsFlyoutDirect(SettingsFlyout settingsFlyout)
            {
                base.ShowSettingsFlyout(settingsFlyout);
            }

            public void CallDisplayPage(object page)
            {
                base.DisplayPage(page);
            }

            protected override void ShowSettingsFlyout(SettingsFlyout settingsFlyout)
            {
                ShowSettingsFlyoutCalls.Add(settingsFlyout);
            }
        }
    }
}

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using UITestMethodAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer.UITestMethodAttribute;

namespace Okra.Tests.Navigation
{
    [TestClass]
    public class SettingsPaneInfoFixture
    {
        // *** Property Getter/Setter Tests ***

        [UITestMethod]
        public void HeaderBackground_DefaultIsNull()
        {
            DependencyObject obj = new Page();

            Brush brush = SettingsPaneInfo.GetHeaderBackground(obj);
            
            Assert.AreEqual(null, brush);
        }

        [UITestMethod]
        public void HeaderBackground_CanSetValue()
        {
            DependencyObject obj = new Page();
            Brush red = new SolidColorBrush(Colors.Red);

            SettingsPaneInfo.SetHeaderBackground(obj, red);
            Brush brush = SettingsPaneInfo.GetHeaderBackground(obj);

            Assert.AreEqual(red, brush);
        }

        [UITestMethod]
        public void HeaderForeground_DefaultIsNull()
        {
            DependencyObject obj = new Page();

            Brush brush = SettingsPaneInfo.GetHeaderForeground(obj);

            Assert.AreEqual(null, brush);
        }

        [UITestMethod]
        public void HeaderForeground_CanSetValue()
        {
            DependencyObject obj = new Page();
            Brush red = new SolidColorBrush(Colors.Red);

            SettingsPaneInfo.SetHeaderForeground(obj, red);
            Brush brush = SettingsPaneInfo.GetHeaderForeground(obj);

            Assert.AreEqual(red, brush);
        }

        [UITestMethod]
        public void IconSource_DefaultIsSmallLogo()
        {
            DependencyObject obj = new Page();

            ImageSource icon = SettingsPaneInfo.GetIconSource(obj);

            Assert.IsInstanceOfType(icon, typeof(BitmapImage));
            Assert.AreEqual("ms-appx:/Assets/SmallLogo.png", ((BitmapImage)icon).UriSource.AbsoluteUri);
        }

        [UITestMethod]
        public void IconSource_CanSet()
        {
            DependencyObject obj = new Page();

            SettingsPaneInfo.SetIconSource(obj, new BitmapImage(new Uri("ms-appx:/Test")));
            ImageSource icon = SettingsPaneInfo.GetIconSource(obj);

            Assert.IsInstanceOfType(icon, typeof(BitmapImage));
            Assert.AreEqual("ms-appx:/Test", ((BitmapImage)icon).UriSource.AbsoluteUri);
        }

        [UITestMethod]
        public void Title_DefaultIsEmptyString()
        {
            DependencyObject obj = new Page();

            string title = SettingsPaneInfo.GetTitle(obj);

            Assert.AreEqual("", title);
        }

        [UITestMethod]
        public void Title_CanSet()
        {
            DependencyObject obj = new Page();

            SettingsPaneInfo.SetTitle(obj, "Title");
            string title = SettingsPaneInfo.GetTitle(obj);

            Assert.AreEqual("Title", title);
        }
    }
}

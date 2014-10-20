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
        public void HeaderBackground_ThrowsException_IfGetterObjectIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SettingsPaneInfo.GetHeaderBackground(null));
        }

        [UITestMethod]
        public void HeaderBackground_ThrowsException_IfSetterObjectIsNull()
        {
            Brush red = new SolidColorBrush(Colors.Red);

            Assert.ThrowsException<ArgumentNullException>(() => SettingsPaneInfo.SetHeaderBackground(null, red));
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
        public void HeaderForeground_ThrowsException_IfGetterObjectIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SettingsPaneInfo.GetHeaderForeground(null));
        }

        [UITestMethod]
        public void HeaderForeground_ThrowsException_IfSetterObjectIsNull()
        {
            Brush red = new SolidColorBrush(Colors.Red);

            Assert.ThrowsException<ArgumentNullException>(() => SettingsPaneInfo.SetHeaderForeground(null, red));
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
        public void IconSource_ThrowsException_IfGetterObjectIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SettingsPaneInfo.GetIconSource(null));
        }

        [UITestMethod]
        public void IconSource_ThrowsException_IfSetterObjectIsNull()
        {
            ImageSource iconSource = new BitmapImage(new Uri("ms-appx:/Test"));

            Assert.ThrowsException<ArgumentNullException>(() => SettingsPaneInfo.SetIconSource(null, iconSource));
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

        [UITestMethod]
        public void Title_ThrowsException_IfGetterObjectIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SettingsPaneInfo.GetTitle(null));
        }

        [UITestMethod]
        public void Title_ThrowsException_IfSetterObjectIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SettingsPaneInfo.SetTitle(null, "Title"));
        }

        [UITestMethod]
        public void Width_DefaultIsStandardSettingsWidth()
        {
            DependencyObject obj = new Page();

            double width = SettingsPaneInfo.GetWidth(obj);

            Assert.AreEqual(346, width);
        }

        [UITestMethod]
        public void Width_CanSet()
        {
            DependencyObject obj = new Page();

            SettingsPaneInfo.SetWidth(obj, 500);
            double width = SettingsPaneInfo.GetWidth(obj);

            Assert.AreEqual(500, width);
        }

        [UITestMethod]
        public void Width_ThrowsException_IfGetterObjectIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SettingsPaneInfo.GetWidth(null));
        }

        [UITestMethod]
        public void Width_ThrowsException_IfSetterObjectIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SettingsPaneInfo.SetWidth(null, 500));
        }
    }
}

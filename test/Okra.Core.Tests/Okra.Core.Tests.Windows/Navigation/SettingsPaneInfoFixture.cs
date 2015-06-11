using Okra.Navigation;
using Okra.Tests.Helpers;
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
using Xunit;

namespace Okra.Tests.Navigation
{
    public class SettingsPaneInfoFixture
    {
        // *** Property Getter/Setter Tests ***

        [UITestMethod]
        public void HeaderBackground_DefaultIsNull()
        {
            DependencyObject obj = new Page();

            Brush brush = SettingsPaneInfo.GetHeaderBackground(obj);

            Assert.Equal(null, brush);
        }

        [UITestMethod]
        public void HeaderBackground_CanSetValue()
        {
            DependencyObject obj = new Page();
            Brush red = new SolidColorBrush(Colors.Red);

            SettingsPaneInfo.SetHeaderBackground(obj, red);
            Brush brush = SettingsPaneInfo.GetHeaderBackground(obj);

            Assert.Equal(red, brush);
        }

        [UITestMethod]
        public void HeaderBackground_ThrowsException_IfGetterObjectIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => SettingsPaneInfo.GetHeaderBackground(null));
        }

        [UITestMethod]
        public void HeaderBackground_ThrowsException_IfSetterObjectIsNull()
        {
            Brush red = new SolidColorBrush(Colors.Red);

            Assert.Throws<ArgumentNullException>(() => SettingsPaneInfo.SetHeaderBackground(null, red));
        }

        [UITestMethod]
        public void HeaderForeground_DefaultIsNull()
        {
            DependencyObject obj = new Page();

            Brush brush = SettingsPaneInfo.GetHeaderForeground(obj);

            Assert.Equal(null, brush);
        }

        [UITestMethod]
        public void HeaderForeground_CanSetValue()
        {
            DependencyObject obj = new Page();
            Brush red = new SolidColorBrush(Colors.Red);

            SettingsPaneInfo.SetHeaderForeground(obj, red);
            Brush brush = SettingsPaneInfo.GetHeaderForeground(obj);

            Assert.Equal(red, brush);
        }

        [UITestMethod]
        public void HeaderForeground_ThrowsException_IfGetterObjectIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => SettingsPaneInfo.GetHeaderForeground(null));
        }

        [UITestMethod]
        public void HeaderForeground_ThrowsException_IfSetterObjectIsNull()
        {
            Brush red = new SolidColorBrush(Colors.Red);

            Assert.Throws<ArgumentNullException>(() => SettingsPaneInfo.SetHeaderForeground(null, red));
        }

        [UITestMethod]
        public void IconSource_DefaultIsSmallLogo()
        {
            DependencyObject obj = new Page();

            ImageSource icon = SettingsPaneInfo.GetIconSource(obj);

            Assert.IsAssignableFrom(typeof(BitmapImage),icon);
            Assert.Equal("ms-appx:/Assets/SmallLogo.png", ((BitmapImage)icon).UriSource.AbsoluteUri);
        }

        [UITestMethod]
        public void IconSource_CanSet()
        {
            DependencyObject obj = new Page();

            SettingsPaneInfo.SetIconSource(obj, new BitmapImage(new Uri("ms-appx:/Test")));
            ImageSource icon = SettingsPaneInfo.GetIconSource(obj);

            Assert.IsAssignableFrom(typeof(BitmapImage),icon);
            Assert.Equal("ms-appx:/Test", ((BitmapImage)icon).UriSource.AbsoluteUri);
        }

        [UITestMethod]
        public void IconSource_ThrowsException_IfGetterObjectIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => SettingsPaneInfo.GetIconSource(null));
        }

        [UITestMethod]
        public void IconSource_ThrowsException_IfSetterObjectIsNull()
        {
            ImageSource iconSource = new BitmapImage(new Uri("ms-appx:/Test"));

            Assert.Throws<ArgumentNullException>(() => SettingsPaneInfo.SetIconSource(null, iconSource));
        }

        [UITestMethod]
        public void Title_DefaultIsEmptyString()
        {
            DependencyObject obj = new Page();

            string title = SettingsPaneInfo.GetTitle(obj);

            Assert.Equal("", title);
        }

        [UITestMethod]
        public void Title_CanSet()
        {
            DependencyObject obj = new Page();

            SettingsPaneInfo.SetTitle(obj, "Title");
            string title = SettingsPaneInfo.GetTitle(obj);

            Assert.Equal("Title", title);
        }

        [UITestMethod]
        public void Title_ThrowsException_IfGetterObjectIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => SettingsPaneInfo.GetTitle(null));
        }

        [UITestMethod]
        public void Title_ThrowsException_IfSetterObjectIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => SettingsPaneInfo.SetTitle(null, "Title"));
        }

        [UITestMethod]
        public void Width_DefaultIsStandardSettingsWidth()
        {
            DependencyObject obj = new Page();

            double width = SettingsPaneInfo.GetWidth(obj);

            Assert.Equal(346, width);
        }

        [UITestMethod]
        public void Width_CanSet()
        {
            DependencyObject obj = new Page();

            SettingsPaneInfo.SetWidth(obj, 500);
            double width = SettingsPaneInfo.GetWidth(obj);

            Assert.Equal(500, width);
        }

        [UITestMethod]
        public void Width_ThrowsException_IfGetterObjectIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => SettingsPaneInfo.GetWidth(null));
        }

        [UITestMethod]
        public void Width_ThrowsException_IfSetterObjectIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => SettingsPaneInfo.SetWidth(null, 500));
        }
    }
}

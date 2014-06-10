using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Okra.Navigation
{
    public class SettingsPaneInfo
    {
        // *** Attached Dependency Properties ***

        public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.RegisterAttached("HeaderBackground", typeof(Brush), typeof(SettingsPaneInfo), new PropertyMetadata(null));
        public static readonly DependencyProperty HeaderForegroundProperty = DependencyProperty.RegisterAttached("HeaderForeground", typeof(Brush), typeof(SettingsPaneInfo), new PropertyMetadata(null));
        public static readonly DependencyProperty IconSourceProperty = DependencyProperty.RegisterAttached("IconSource", typeof(ImageSource), typeof(SettingsPaneInfo), new PropertyMetadata(new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:/Assets/SmallLogo.png"))));
        public static readonly DependencyProperty TitleProperty = DependencyProperty.RegisterAttached("Title", typeof(string), typeof(SettingsPaneInfo), new PropertyMetadata(string.Empty));

        // *** Attached Dependency Property Getters/Setters ***

        public static Brush GetHeaderBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(HeaderBackgroundProperty);
        }

        public static void SetHeaderBackground(DependencyObject obj, Brush value)
        {
            obj.SetValue(HeaderBackgroundProperty, value);
        }

        public static Brush GetHeaderForeground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(HeaderForegroundProperty);
        }

        public static void SetHeaderForeground(DependencyObject obj, Brush value)
        {
            obj.SetValue(HeaderForegroundProperty, value);
        }

        public static ImageSource GetIconSource(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(IconSourceProperty);
        }

        public static void SetIconSource(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(IconSourceProperty, value);
        }

        public static string GetTitle(DependencyObject obj)
        {
            return (string)obj.GetValue(TitleProperty);
        }

        public static void SetTitle(DependencyObject obj, string value)
        {
            obj.SetValue(TitleProperty, value);
        }
    }
}

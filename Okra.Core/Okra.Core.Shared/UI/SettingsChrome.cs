using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Okra.UI
{
    [Obsolete("Okra will now use the system provided SettingsChrome. Remove SettingsChrome element and apply styles using SettingsPaneInfo")]
    public sealed class SettingsChrome : ContentControl
    {
        // *** Dependency Properties ***

        public static readonly DependencyProperty BackButtonCommandProperty = DependencyProperty.Register("BackButtonCommand", typeof(ICommand), typeof(SettingsChrome), new PropertyMetadata(null));
        public static readonly DependencyProperty HeaderBackgroundBrushProperty = DependencyProperty.Register("HeaderBackgroundBrush", typeof(Brush), typeof(SettingsChrome), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 0, 128, 240))));
        public static readonly DependencyProperty HeaderForegroundBrushProperty = DependencyProperty.Register("HeaderForegroundBrush", typeof(Brush), typeof(SettingsChrome), new PropertyMetadata(new SolidColorBrush(Colors.White)));
        public static readonly DependencyProperty LogoProperty = DependencyProperty.Register("Logo", typeof(ImageSource), typeof(SettingsChrome), new PropertyMetadata(null));
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(SettingsChrome), new PropertyMetadata("Settings"));

        // *** Constructors ***

        public SettingsChrome()
        {
            this.DefaultStyleKey = typeof(SettingsChrome);
        }

        // *** Properties **

        public ICommand BackButtonCommand
        {
            get
            {
                return (ICommand)GetValue(BackButtonCommandProperty);
            }
            set
            {
                SetValue(BackButtonCommandProperty, value);
            }
        }

        public Brush HeaderBackgroundBrush
        {
            get { return (Brush)GetValue(HeaderBackgroundBrushProperty); }
            set { SetValue(HeaderBackgroundBrushProperty, value); }
        }

        public Brush HeaderForegroundBrush
        {
            get { return (Brush)GetValue(HeaderForegroundBrushProperty); }
            set { SetValue(HeaderForegroundBrushProperty, value); }
        }

        public ImageSource Logo
        {
            get
            {
                return (ImageSource)GetValue(LogoProperty);
            }
            set
            {
                SetValue(LogoProperty, value);
            }
        }

        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }
    }
}

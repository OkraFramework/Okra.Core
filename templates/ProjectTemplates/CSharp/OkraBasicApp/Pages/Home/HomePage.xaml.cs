using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace $safeprojectname$.Pages.Home
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    [PageExport("Home")]
    public sealed partial class HomePage : $safeprojectname$.Common.LayoutAwarePage
    {
        public HomePage()
        {
            this.InitializeComponent();
        }
    }
}

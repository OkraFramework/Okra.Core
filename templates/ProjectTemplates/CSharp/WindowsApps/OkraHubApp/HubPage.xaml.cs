using $safeprojectname$.Common;
using $safeprojectname$.Data;
using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Composition;
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

// The Hub Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=321224

namespace $safeprojectname$
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    [PageExport("Home")]
    public sealed partial class HubPage : Page
    {
        public HubPage()
        {
            this.InitializeComponent();
        }
    }
}

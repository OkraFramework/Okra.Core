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

// The Section Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace $safeprojectname$
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    [PageExport("Section")]
    public sealed partial class SectionPage : Page
    {
        public SectionPage()
        {
            this.InitializeComponent();
        }
    }
}
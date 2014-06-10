using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Share Target Contract item template is documented at http://go.microsoft.com/fwlink/?LinkId=234241

namespace $rootnamespace$
{
    $wizardcomment$/// <summary>
    /// This page allows other applications to share content through this application.
    /// </summary>
    [PageExport(SpecialPageNames.ShareTarget)]
    public sealed partial class $safeitemname$ : $safeprojectname$.Common.LayoutAwarePage
    {
        public $safeitemname$()
        {
            this.InitializeComponent();
        }
    }
}

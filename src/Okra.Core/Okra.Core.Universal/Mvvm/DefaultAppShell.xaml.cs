using Okra.Navigation;
using Okra.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Okra.Mvvm
{
    public sealed partial class DefaultAppShell : Page
    {
        public DefaultAppShell(INavigationManager navigationManager, IViewRouter viewRouter)
        {
            this.NavigationManager = navigationManager;
            this.ViewManager = new ViewManager(navigationManager, viewRouter);

            this.InitializeComponent();
        }

        public INavigationManager NavigationManager
        {
            get;
        }

        public ViewManager ViewManager
        {
            get;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Okra.Mvvm
{
    public class WindowAppHost
    {
        // *** Constructors ***

        public WindowAppHost()
        {
        }

        // *** Methods ***

        public void SetShell(object shell)
        {
            Window.Current.Content = shell as UIElement;
            Window.Current.Activate();
        }
    }
}

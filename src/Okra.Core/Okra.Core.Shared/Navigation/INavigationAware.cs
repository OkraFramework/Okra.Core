using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Okra.Navigation
{
    public interface INavigationAware
    {
        void NavigatedTo(NavigationMode navigationMode);
        void NavigatingFrom(NavigationMode navigationMode);
    }
}

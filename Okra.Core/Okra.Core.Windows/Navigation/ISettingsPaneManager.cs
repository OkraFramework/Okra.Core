using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    public interface ISettingsPaneManager : INavigationBase
    {
        // *** Events ***

        event EventHandler FlyoutClosed;
        event EventHandler FlyoutOpened;

        // *** Methods ***

        void ShowSettingsPane();
    }
}

using Okra.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    public interface INavigationManager : INotifyPropertyChanged
    {
        // *** Properties ***

        bool CanGoBack { get; }
        PageInfo CurrentPage { get; }

        // *** Methods ***

        void Clear();
        void GoBack();
        void NavigateTo(PageInfo page);
    }
}

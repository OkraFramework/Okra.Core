using System.Collections.Generic;
using System.ComponentModel;

namespace Okra.Navigation
{
    public interface INavigationManager : INotifyPropertyChanged
    {
        // *** Properties ***

        bool CanGoBack { get; }
        bool CanGoForward { get; }
        PageEntry CurrentPage { get; }
        IReadOnlyList<PageEntry> NavigationStack { get; }

        // *** Methods ***

        void Clear();
        void GoBack();
        void GoForward();
        void NavigateTo(PageInfo page);
    }
}

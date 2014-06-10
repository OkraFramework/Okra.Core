using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Okra.Navigation
{
    public interface INavigationStack : IReadOnlyList<PageInfo>, INotifyPropertyChanged
    {
        // *** Events ***

        event EventHandler<PageNavigationEventArgs> NavigatingFrom;
        event EventHandler<PageNavigationEventArgs> NavigatedTo;
        event EventHandler<PageNavigationEventArgs> PageDisposed;

        // *** Properties ***

        bool CanGoBack { get; }
        PageInfo CurrentPage { get; }

        // *** Methods ***

        void Clear();
        void GoBack();
        void NavigateTo(PageInfo page);
        void Push(IEnumerable<PageInfo> pages);
    }
}

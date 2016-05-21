using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Okra.Tests.Mocks
{
    public class MockNavigationStack : List<PageInfo>, INavigationStack
    {
        // *** Events ***

        public event EventHandler<PageNavigationEventArgs> NavigatingFrom;
        public event EventHandler<PageNavigationEventArgs> NavigatedTo;
        public event EventHandler<PageNavigationEventArgs> PageDisposed;
        public event PropertyChangedEventHandler PropertyChanged;

        // *** Properties ***

        public bool CanGoBack
        {
            get
            {
                return Count > 0;
            }
        }

        public PageInfo CurrentPage
        {
            get
            {
                if (base.Count == 0)
                    return null;
                else
                    return base[base.Count - 1];
            }
        }

        // *** Methods ***

        public void GoBack()
        {
            base.RemoveAt(base.Count - 1);
        }

        public void GoBackTo(PageInfo page)
        {
            while (CurrentPage != page)
                GoBack();
        }

        public void NavigateTo(PageInfo page)
        {
            base.Add(page);
        }

        public void Push(IEnumerable<PageInfo> pages)
        {
            base.AddRange(pages);
        }

        // *** Mock Methods ***

        public void RaiseNavigatedTo(PageNavigationEventArgs eventArgs)
        {
            if (NavigatedTo != null)
                NavigatedTo(this, eventArgs);
        }

        public void RaiseNavigatingFrom(PageNavigationEventArgs eventArgs)
        {
            if (NavigatingFrom != null)
                NavigatingFrom(this, eventArgs);
        }

        public void RaisePageDisposed(PageNavigationEventArgs eventArgs)
        {
            if (PageDisposed != null)
                PageDisposed(this, eventArgs);
        }

        public void RaisePropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, eventArgs);
        }
    }
}

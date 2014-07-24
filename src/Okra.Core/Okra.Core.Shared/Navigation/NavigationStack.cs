using Okra.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Navigation;

namespace Okra.Navigation
{
    public class NavigationStack : INavigationStack, INotifyCollectionChanged
    {
        // *** Fields ***

        private readonly List<PageInfo> internalStack = new List<PageInfo>();

        // *** Events ***

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event EventHandler<PageNavigationEventArgs> PageDisposed;
        public event EventHandler<PageNavigationEventArgs> NavigatingFrom;
        public event EventHandler<PageNavigationEventArgs> NavigatedTo;
        public event PropertyChangedEventHandler PropertyChanged;

        // *** Constructors ***

        public NavigationStack()
        {
        }

        // *** Properties ***

        public PageInfo this[int index]
        {
            get
            {
                return internalStack[index];
            }
        }

        public virtual bool CanGoBack
        {
            get
            {
                return internalStack.Count > 0;
            }
        }

        public int Count
        {
            get
            {
                return internalStack.Count;
            }
        }

        public PageInfo CurrentPage
        {
            get
            {
                if (internalStack.Count == 0)
                    return null;
                else
                    return internalStack[internalStack.Count - 1];
            }
        }

        // *** Methods ***

        public void Clear()
        {
            // If the stack is already empty then just return (and don't raise any events)

            if (internalStack.Count == 0)
                return;

            // Call NavigatingFrom on the existing navigation entry

            OnNavigatingFrom(CurrentPage, PageNavigationMode.Back);

            // Clear the navigation stack

            internalStack.Clear();

            // Raise property changed events

            OnPropertyChanged("Count");
            OnPropertyChanged("CurrentPage");
            OnPropertyChanged("CanGoBack");

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public IEnumerator<PageInfo> GetEnumerator()
        {
            return internalStack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return internalStack.GetEnumerator();
        }

        public void GoBack()
        {
            // Check that we can go back

            if (!CanGoBack)
                throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotGoBackWithEmptyBackStack"));

            // Call NavigatingFrom on the existing navigation entry

            PageInfo oldPage = CurrentPage;
            OnNavigatingFrom(oldPage, PageNavigationMode.Back);

            // Pop the last page from the navigation stack

            PageInfo page = internalStack[internalStack.Count - 1];
            internalStack.RemoveAt(internalStack.Count - 1);

            // Raise property changed events

            OnPropertyChanged("Count");
            OnPropertyChanged("CurrentPage");
            if (!CanGoBack)
                OnPropertyChanged("CanGoBack");

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, page, internalStack.Count));

            if (CurrentPage != null)
                OnNavigatedTo(CurrentPage, PageNavigationMode.Back);

            if (oldPage != null)
                OnPageDisposed(oldPage, PageNavigationMode.Back);
        }

        public void NavigateTo(PageInfo page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            Push(new PageInfo[] { page });
        }

        public void Push(IEnumerable<PageInfo> pages)
        {
            if (pages == null)
                throw new ArgumentNullException("pages");

            // If there are no pages to push then just return (and don't raise any events)

            if (pages.Count() == 0)
                return;

            // Call NavigatingFrom on the existing navigation entry (if one exists)

            if (CurrentPage != null)
                OnNavigatingFrom(CurrentPage, PageNavigationMode.New);

            // Get the initial state

            int insertionPosition = Count;
            bool oldCanGoBack = CanGoBack;

            // Add the pages to the stack

            internalStack.AddRange(pages);

            // Raise events

            OnPropertyChanged("Count");
            OnPropertyChanged("CurrentPage");
            if (CanGoBack != oldCanGoBack)
                OnPropertyChanged("CanGoBack");

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, pages.ToList(), insertionPosition));

            OnNavigatedTo(CurrentPage, PageNavigationMode.New);
        }

        // *** Protected Methods ***

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            NotifyCollectionChangedEventHandler eventHandler = CollectionChanged;

            if (eventHandler != null)
                eventHandler(this, args);
        }

        protected virtual void OnNavigatingFrom(PageInfo page, PageNavigationMode navigationMode)
        {
            EventHandler<PageNavigationEventArgs> eventHandler = NavigatingFrom;

            if (eventHandler != null)
                eventHandler(this, new PageNavigationEventArgs(page, navigationMode));
        }

        protected virtual void OnNavigatedTo(PageInfo page, PageNavigationMode navigationMode)
        {
            EventHandler<PageNavigationEventArgs> eventHandler = NavigatedTo;

            if (eventHandler != null)
                eventHandler(this, new PageNavigationEventArgs(page, navigationMode));
        }

        protected virtual void OnPageDisposed(PageInfo page, PageNavigationMode navigationMode)
        {
            EventHandler<PageNavigationEventArgs> eventHandler = PageDisposed;

            if (eventHandler != null)
                eventHandler(this, new PageNavigationEventArgs(page, navigationMode));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler eventHandler = PropertyChanged;

            if (eventHandler != null)
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using Okra.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Okra.Navigation
{
    public class NavigationStack : INavigationStack, INotifyCollectionChanged
    {
        // *** Fields ***

        private readonly List<PageInfo> _internalStack = new List<PageInfo>();

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
                return _internalStack[index];
            }
        }

        public virtual bool CanGoBack
        {
            get
            {
                return _internalStack.Count > 0;
            }
        }

        public int Count
        {
            get
            {
                return _internalStack.Count;
            }
        }

        public PageInfo CurrentPage
        {
            get
            {
                if (_internalStack.Count == 0)
                    return null;
                else
                    return _internalStack[_internalStack.Count - 1];
            }
        }

        // *** Methods ***

        public void Clear()
        {
            // If the stack is already empty then just return (and don't raise any events)

            if (_internalStack.Count == 0)
                return;

            // Call NavigatingFrom on the existing navigation entry

            OnNavigatingFrom(CurrentPage, PageNavigationMode.Back);

            // Clear the navigation stack

            _internalStack.Clear();

            // Raise property changed events

            OnPropertyChanged("Count");
            OnPropertyChanged("CurrentPage");
            OnPropertyChanged("CanGoBack");

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public IEnumerator<PageInfo> GetEnumerator()
        {
            return _internalStack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _internalStack.GetEnumerator();
        }

        public void GoBack()
        {
            // Check that we can go back

            if (!CanGoBack)
                throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotGoBackWithEmptyBackStack"));

            // Call into internal method

            GoBackToInternal(Count - 1);
        }

        public void GoBackTo(PageInfo page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            // Check that the specified page exists in the navigation stack

            int pageIndex = _internalStack.IndexOf(page);

            if (pageIndex == -1)
                throw new InvalidOperationException(string.Format(ResourceHelper.GetErrorResource("Exception_InvalidOperation_SpecifiedPageDoesNotExistInNavigationStack"), page.PageName));

            // Call into internal method

            GoBackToInternal(pageIndex + 1);
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

            if (pages.Contains(null))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_EnumerableContainsNullPage"));

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

            _internalStack.AddRange(pages);

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
            if (args == null)
                throw new ArgumentNullException("args");

            NotifyCollectionChangedEventHandler eventHandler = CollectionChanged;

            if (eventHandler != null)
                eventHandler(this, args);
        }

        protected virtual void OnNavigatingFrom(PageInfo page, PageNavigationMode navigationMode)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            if (!Enum.IsDefined(typeof(PageNavigationMode), navigationMode))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_SpecifiedEnumIsNotDefined"), "navigationMode");

            EventHandler<PageNavigationEventArgs> eventHandler = NavigatingFrom;

            if (eventHandler != null)
                eventHandler(this, new PageNavigationEventArgs(page, navigationMode));
        }

        protected virtual void OnNavigatedTo(PageInfo page, PageNavigationMode navigationMode)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            if (!Enum.IsDefined(typeof(PageNavigationMode), navigationMode))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_SpecifiedEnumIsNotDefined"), "navigationMode");

            EventHandler<PageNavigationEventArgs> eventHandler = NavigatedTo;

            if (eventHandler != null)
                eventHandler(this, new PageNavigationEventArgs(page, navigationMode));
        }

        protected virtual void OnPageDisposed(PageInfo page, PageNavigationMode navigationMode)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            if (!Enum.IsDefined(typeof(PageNavigationMode), navigationMode))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_SpecifiedEnumIsNotDefined"), "navigationMode");

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

        // *** Private Methods ***

        private void GoBackToInternal(int removedPageIndex)
        {
            int removedPageCount = _internalStack.Count - removedPageIndex;
            IList<PageInfo> removedPages = _internalStack.GetRange(removedPageIndex, removedPageCount);

            // Call NavigatingFrom on the existing navigation entry

            OnNavigatingFrom(CurrentPage, PageNavigationMode.Back);

            // Remove the pages from the navigation stack

            _internalStack.RemoveRange(removedPageIndex, removedPageCount);

            // Raise property changed events

            OnPropertyChanged("Count");
            OnPropertyChanged("CurrentPage");
            if (!CanGoBack)
                OnPropertyChanged("CanGoBack");

            if (removedPageCount == 1)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedPages[0], _internalStack.Count));
            else
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            if (CurrentPage != null)
                OnNavigatedTo(CurrentPage, PageNavigationMode.Back);

            foreach (PageInfo removedPage in removedPages)
                OnPageDisposed(removedPage, PageNavigationMode.Back);
        }
    }
}

using Okra.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Okra.Navigation
{
    public class NavigationStack<T> : IReadOnlyList<T>, INotifyPropertyChanged
    {
        // *** Fields ***

        private readonly NavigationStackType _navigationStackType;
        private readonly List<T> _internalStack = new List<T>();
        private readonly List<T> _forwardStack = new List<T>();

        // *** Constructors ***

        public NavigationStack()
            : this(NavigationStackType.AllowEmptyStack)
        {
        }

        public NavigationStack(NavigationStackType navigationStackType)
        {
            _navigationStackType = navigationStackType;
        }

        // *** Events ***

        public event PropertyChangedEventHandler PropertyChanged;

        // *** Properties ***

        public T this[int index]
        {
            get
            {
                return _internalStack[index];
            }
        }

        public bool CanGoBack => _internalStack.Count > MinimumCount;

        public bool CanGoForward => _forwardStack.Count > 0;

        public int Count => _internalStack.Count;

        public T CurrentItem => _internalStack.Count == 0 ? default(T) : _internalStack[_internalStack.Count - 1];

        public int MinimumCount => _navigationStackType == NavigationStackType.RequireFirstItem ? 1 : 0;

        // *** Methods ***

        public void Clear()
        {
            bool hadItems = _internalStack.Count > 0;
            bool couldGoBack = CanGoBack;
            bool couldGoForward = CanGoForward;

            _internalStack.Clear();
            _forwardStack.Clear();

            if (hadItems) OnPropertyChanged(nameof(Count));
            if (hadItems) OnPropertyChanged(nameof(CurrentItem));
            if (couldGoBack) OnPropertyChanged(nameof(CanGoBack));
            if (couldGoForward) OnPropertyChanged(nameof(CanGoForward));
        }

        public void GoBack()
        {
            if (!CanGoBack)
                throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotGoBackWithEmptyBackStack"));

            bool couldGoForward = CanGoForward;

            T item = Pop(_internalStack);
            _forwardStack.Add(item);

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(CurrentItem));
            if (!CanGoBack) OnPropertyChanged(nameof(CanGoBack));
            if (!couldGoForward) OnPropertyChanged(nameof(CanGoForward));
        }

        public void GoBackTo(T item)
        {
            if (!_internalStack.Contains(item))
                throw new InvalidOperationException(string.Format(ResourceHelper.GetErrorResource("Exception_InvalidOperation_SpecifiedPageDoesNotExistInNavigationStack"), item));

            bool couldGoForward = CanGoForward;

            while (!object.Equals(item, CurrentItem))
            {
                T poppedItem = Pop(_internalStack);
                _forwardStack.Add(poppedItem);
            }

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(CurrentItem));
            if (!CanGoBack) OnPropertyChanged(nameof(CanGoBack));
            if (!couldGoForward) OnPropertyChanged(nameof(CanGoForward));
        }

        public void GoForward()
        {
            if (!CanGoForward)
                throw new InvalidOperationException(ResourceHelper.GetErrorResource("Exception_InvalidOperation_CannotGoForwardWithEmptyForwardStack"));

            bool couldGoBack = CanGoBack;

            T item = Pop(_forwardStack);
            _internalStack.Add(item);

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(CurrentItem));
            if (!couldGoBack) OnPropertyChanged(nameof(CanGoBack));
            if (!CanGoForward) OnPropertyChanged(nameof(CanGoForward));
        }

        public void GoForwardTo(T item)
        {
            if (!_forwardStack.Contains(item))
                throw new InvalidOperationException(string.Format(ResourceHelper.GetErrorResource("Exception_InvalidOperation_SpecifiedPageDoesNotExistInNavigationStack"), item));

            bool couldGoBack = CanGoBack;

            while (!object.Equals(item, CurrentItem))
            {
                T poppedItem = Pop(_forwardStack);
                _internalStack.Add(poppedItem);
            }

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(CurrentItem));
            if (!couldGoBack) OnPropertyChanged(nameof(CanGoBack));
            if (!CanGoForward) OnPropertyChanged(nameof(CanGoForward));
        }

        public void NavigateTo(T item)
        {
            bool couldGoBack = CanGoBack;
            bool couldGoForward = CanGoForward;

            _internalStack.Add(item);
            _forwardStack.Clear();

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(nameof(CurrentItem));
            if (!couldGoBack && CanGoBack) OnPropertyChanged(nameof(CanGoBack));
            if (couldGoForward) OnPropertyChanged(nameof(CanGoForward));
        }

        // *** Enumerators ***

        public IEnumerator<T> GetEnumerator() => _internalStack.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _internalStack.GetEnumerator();

        // *** Protected Methods ***

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // *** Private Methods ***

        private T Pop(List<T> list)
        {
            int index = list.Count - 1;
            T item = list[index];
            list.RemoveAt(index);
            return item;
        }
    }
}

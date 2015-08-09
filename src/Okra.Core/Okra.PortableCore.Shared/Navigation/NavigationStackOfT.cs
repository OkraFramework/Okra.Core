using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Okra.Navigation
{
    public class NavigationStack<T> : IReadOnlyList<T>, INotifyPropertyChanged
    {
        // *** Fields ***

        private readonly List<T> _internalStack = new List<T>();
        private readonly List<T> _forwardStack = new List<T>();

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

        public bool CanGoBack => _internalStack.Count > 0;

        public bool CanGoForward => _forwardStack.Count > 0;

        public int Count => _internalStack.Count;

        public T CurrentItem => _internalStack.Count == 0 ? default(T) : _internalStack[_internalStack.Count - 1];

        // *** Methods ***

        public void Clear()
        {
            _internalStack.Clear();
            _forwardStack.Clear();
        }

        public void GoBack()
        {
            T item = Pop(_internalStack);
            _forwardStack.Add(item);
        }

        public void GoForward()
        {
            T item = Pop(_forwardStack);
            _internalStack.Add(item);
        }

        public void NavigateTo(T item)
        {
            _internalStack.Add(item);
            _forwardStack.Clear();
        }

        // *** Enumerators ***

        public IEnumerator<T> GetEnumerator() => _internalStack.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _internalStack.GetEnumerator();

        // *** Protected Methods ***

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // *** Private Methods ***

        public T Pop(List<T> list)
        {
            int index = list.Count - 1;
            T item = list[index];
            list.RemoveAt(index);
            return item;
        }
    }
}

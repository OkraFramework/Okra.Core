using Okra.State;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Okra.Navigation
{
    public class NavigationManager : INavigationManager, INotifyPropertyChanged
    {
        // *** Fields ***

        private readonly NavigationStack<PageEntry> _navigationStack = new NavigationStack<PageEntry>(NavigationStackType.RequireFirstItem);

        public event PropertyChangedEventHandler PropertyChanged;

        // *** Constructors ***

        public NavigationManager()
        {
            _navigationStack.PropertyChanged += NavigationStack_PropertyChanged;
        }

        // *** Properties ***

        public bool CanGoBack
        {
            get
            {
                return _navigationStack.CanGoBack;
            }
        }

        public PageEntry CurrentPage
        {
            get
            {
                return _navigationStack.CurrentItem;
            }
        }

        public bool CanGoForward
        {
            get
            {
                return _navigationStack.CanGoForward;
            }
        }

        public IReadOnlyList<PageEntry> NavigationStack
        {
            get
            {
                return _navigationStack;
            }
        }

        // *** Methods ***

        public void Clear()
        {
            _navigationStack.Clear();
        }

        public void GoBack()
        {
            _navigationStack.GoBack();
        }

        public void GoForward()
        {
            _navigationStack.GoForward();
        }

        public void NavigateTo(PageInfo page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            var pageState = new StateService();
            pageState.SetState(StateNames.PageArguments, page.Arguments);
            var pageEntry = new PageEntry(page.PageName, pageState);

            _navigationStack.NavigateTo(pageEntry);
        }

        // *** Protected Methods ***

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // *** Private Methods ***

        private void NavigationStack_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(NavigationStack<PageInfo>.CurrentItem):
                    OnPropertyChanged(nameof(CurrentPage));
                    break;
                case nameof(NavigationStack<PageInfo>.CanGoBack):
                    OnPropertyChanged(nameof(CanGoBack));
                    break;
                case nameof(NavigationStack<PageInfo>.CanGoForward):
                    OnPropertyChanged(nameof(CanGoForward));
                    break;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    public class NavigationManager : INavigationManager, INotifyPropertyChanged
    {
        private readonly NavigationStack<PageInfo> _navigationStack = new NavigationStack<PageInfo>();

        public event PropertyChangedEventHandler PropertyChanged;

        public NavigationManager()
        {
            _navigationStack.PropertyChanged += NavigationStack_PropertyChanged;
        }

        public bool CanGoBack
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public PageInfo CurrentPage
        {
            get
            {
                return _navigationStack.CurrentItem;
            }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void NavigateTo(PageInfo page)
        {
            _navigationStack.NavigateTo(page);
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
            }
        }
    }
}

﻿using Microsoft.Extensions.DependencyInjection;
using Okra.DependencyInjection;
using Okra.Navigation;
using Okra.Routing;
using Okra.State;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Mvvm
{
    public class ViewManager : INotifyPropertyChanged
    {
        private readonly INavigationManager _navigationManager;
        private readonly IViewRouter _viewRouter;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private object _currentView;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewManager(INavigationManager navigationManager, IViewRouter viewRouter, IServiceScopeFactory serviceScopeFactory)
        {
            this._navigationManager = navigationManager;
            this._viewRouter = viewRouter;
            this._serviceScopeFactory = serviceScopeFactory;

            navigationManager.PropertyChanged += NavigationManager_PropertyChanged;
        }

        public object CurrentView
        {
            get
            {
                return _currentView;
            }
            set
            {
                _currentView = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(CurrentView)));
            }
        }

        private async void NavigationManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(INavigationManager.CurrentPage))
            {
                var page = _navigationManager.CurrentPage;

                // TODO : Must dispose of page scope!
                var pageScope = _serviceScopeFactory.CreateScope();
                var pageServices = pageScope.ServiceProvider;

                pageServices.InjectService<IStateService>(page.PageState);

                var view = await _viewRouter.GetViewAsync(page.PageName, pageServices);
                this.CurrentView = view;
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);
    }
}

using Okra.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Okra.Navigation
{
    public abstract class NavigationBase : INavigationBase
    {
        // *** Fields ***

        private readonly IViewFactory _viewFactory;
        private readonly NavigationContext _navigationContext;

        private readonly Dictionary<PageInfo, PageDetails> _pageCache = new Dictionary<PageInfo, PageDetails>();

        private bool _restoringState = false;

        // *** Constructors ***

        public NavigationBase(IViewFactory viewFactory)
            : this(viewFactory, new NavigationStack())
        {
        }

        public NavigationBase(IViewFactory viewFactory, INavigationStack navigationStack)
        {
            if (viewFactory == null)
                throw new ArgumentNullException(nameof(viewFactory));

            if (navigationStack == null)
                throw new ArgumentNullException(nameof(navigationStack));

            _viewFactory = viewFactory;
            this.NavigationStack = navigationStack;

            _navigationContext = new NavigationContext(this);

            navigationStack.NavigatedTo += navigationStack_NavigatedTo;
            navigationStack.NavigatingFrom += navigationStack_NavigatingFrom;
            navigationStack.PageDisposed += navigationStack_PageDisposed;
            navigationStack.PropertyChanged += navigationStack_PropertyChanged;
        }

        // *** Properties ***

        public INavigationStack NavigationStack { get; }

        // *** Methods ***

        public bool CanNavigateTo(string pageName)
        {
            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), nameof(pageName));

            // Query the underlying view factory to see if the page exists

            return _viewFactory.IsViewDefined(pageName);
        }

        public IEnumerable<object> GetPageElements(PageInfo page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            // Get the cached page if present, otherwise return an empty array

            PageDetails pageDetails;

            if (!_pageCache.TryGetValue(page, out pageDetails))
                return new object[] { };

            // Return in the following order,
            //   (1) The view model (if present)
            //   (2) The view

            IViewLifetimeContext viewLifetimeContext = pageDetails.ViewLifetimeContext;

            if (viewLifetimeContext.ViewModel != null)
                return new object[] { viewLifetimeContext.ViewModel, viewLifetimeContext.View };
            else
                return new object[] { viewLifetimeContext.View };
        }

        // *** Protected Methods ***        

        protected abstract void DisplayPage(object page);

        protected void RestoreState(NavigationState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            // Restore the navigation stack
            // NB: Flag that we are restoring state so we can pass a NavigationMode.Refresh to restored pages

            _restoringState = true;

            try
            {
                NavigationStack.Push(state.NavigationStack);
            }
            finally
            {
                _restoringState = false;
            }
        }

        protected NavigationState StoreState()
        {
            // Create an object for storage of the navigation state

            NavigationState state = new NavigationState();

            // Enumerate all NavigationEntries in the navigation stack

            foreach (PageInfo pageInfo in NavigationStack)
            {
                // If the view model is IActivatable then use this to save the page state
                // NB: First check that the view has been created - this may still have state from a previous instance

                PageDetails pageDetails;

                if (_pageCache.TryGetValue(pageInfo, out pageDetails))
                {
                    object viewModel = pageDetails.ViewLifetimeContext.ViewModel;

                    if (viewModel is IActivatable)
                        ((IActivatable)viewModel).SaveState(pageInfo);
                }

                // Add to the navigation state

                state.NavigationStack.Add(pageInfo);
            }

            // Return the result

            return state;
        }

        // *** Private Methods ***

        private void CallNavigatedTo(PageInfo pageInfo, PageNavigationMode navigationMode)
        {
            // Call NavigatedTo on all page elements

            foreach (object element in GetPageElements(pageInfo))
            {
                if (element is INavigationAware)
                    ((INavigationAware)element).NavigatedTo(navigationMode);
            }
        }

        private void CallNavigatingFrom(PageInfo pageInfo, PageNavigationMode navigationMode)
        {
            // Call NavigatingFrom on all page elements

            foreach (object element in GetPageElements(pageInfo))
            {
                if (element is INavigationAware)
                    ((INavigationAware)element).NavigatingFrom(navigationMode);
            }
        }

        private PageDetails CreatePage(PageInfo pageInfo)
        {
            // Create the View

            IViewLifetimeContext viewLifetimeContext = _viewFactory.CreateView(pageInfo.PageName, _navigationContext);

            // Activate the view model if it implements IActivatable

            object viewModel = viewLifetimeContext.ViewModel;

            if (viewModel is IActivatable)
            {
                ((IActivatable)viewModel).Activate(pageInfo);
            }

            // Add the page details to the cache and return

            PageDetails pageDetails = new PageDetails(viewLifetimeContext);
            _pageCache.Add(pageInfo, pageDetails);

            return pageDetails;
        }

        private void DisposePage(PageInfo pageInfo)
        {
            PageDetails pageDetails = null;

            if (_pageCache.TryGetValue(pageInfo, out pageDetails))
            {
                // Dispose of the view and view-model

                pageDetails.ViewLifetimeContext.Dispose();

                // Remove the page from the cache

                _pageCache.Remove(pageInfo);
            }
        }

        private void DisplayNavigationEntry(PageInfo pageInfo)
        {
            if (pageInfo == null)
            {
                // If this entry is null then simply pass null to the deriving class

                DisplayPage(null);
            }
            else
            {
                // If the page is in the cache then retrieve it, otherwise create the page and view-model

                PageDetails pageDetails;

                if (!_pageCache.TryGetValue(pageInfo, out pageDetails))
                    pageDetails = CreatePage(pageInfo);

                // Navigate to the relevant page

                DisplayPage(pageDetails.ViewLifetimeContext.View);
            }
        }

        // *** Event Handlers ***

        private void navigationStack_NavigatedTo(object sender, PageNavigationEventArgs e)
        {
            if (_restoringState)
                CallNavigatedTo((PageInfo)e.Page, PageNavigationMode.Refresh);
            else
                CallNavigatedTo((PageInfo)e.Page, e.NavigationMode);
        }

        private void navigationStack_NavigatingFrom(object sender, PageNavigationEventArgs e)
        {
            CallNavigatingFrom((PageInfo)e.Page, e.NavigationMode);
        }

        private void navigationStack_PageDisposed(object sender, PageNavigationEventArgs e)
        {
            DisposePage(e.Page);
        }

        private void navigationStack_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(INavigationStack.CurrentPage):
                    DisplayNavigationEntry((PageInfo)NavigationStack.CurrentPage);
                    break;
            }
        }

        // *** Private Sub-classes ***

        private class PageDetails
        {
            // *** Constructors ***

            public PageDetails(IViewLifetimeContext viewLifetimeContext)
            {
                this.ViewLifetimeContext = viewLifetimeContext;
            }

            // *** Properties ***

            public IViewLifetimeContext ViewLifetimeContext { get; }
        }
    }
}

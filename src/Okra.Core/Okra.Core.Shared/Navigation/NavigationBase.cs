using Okra.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Okra.Navigation
{
    public abstract class NavigationBase : INavigationBase
    {
        // *** Fields ***

        private readonly IViewFactory viewFactory;
        private readonly INavigationStack navigationStack;
        private readonly NavigationContext navigationContext;

        private readonly Dictionary<PageInfo, PageDetails> pageCache = new Dictionary<PageInfo, PageDetails>();

        private bool restoringState = false;

        // *** Events ***

        [Obsolete("Use INavigationStack.PropertyChanged event for future compatibility")]
        public event EventHandler CanGoBackChanged;

        [Obsolete("Use INavigationStack.NavigatingFrom event for future compatibility")]
        public event EventHandler<PageNavigationEventArgs> NavigatingFrom;

        [Obsolete("Use INavigationStack.NavigatedTo event for future compatibility")]
        public event EventHandler<PageNavigationEventArgs> NavigatedTo;

        // *** Constructors ***

        public NavigationBase(IViewFactory viewFactory)
            : this(viewFactory, new NavigationStack())
        {
        }

        public NavigationBase(IViewFactory viewFactory, INavigationStack navigationStack)
        {
            if (viewFactory == null)
                throw new ArgumentNullException("viewFactory");

            if (navigationStack == null)
                throw new ArgumentNullException("navigationStack");

            this.viewFactory = viewFactory;
            this.navigationStack = navigationStack;

            this.navigationContext = new NavigationContext(this);

            navigationStack.NavigatedTo += navigationStack_NavigatedTo;
            navigationStack.NavigatingFrom += navigationStack_NavigatingFrom;
            navigationStack.PageDisposed += navigationStack_PageDisposed;
            navigationStack.PropertyChanged += navigationStack_PropertyChanged;
        }

        // *** Properties ***

        [Obsolete("Use INavigationStack.CanGoBack property for future compatibility")]
        public bool CanGoBack
        {
            get
            {
                return navigationStack.CanGoBack;
            }
        }

        public INavigationStack NavigationStack
        {
            get
            {
                return navigationStack;
            }
        }

        // *** Methods ***

        public bool CanNavigateTo(string pageName)
        {
            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "pageName");

            // Query the underlying view factory to see if the page exists

            return viewFactory.IsViewDefined(pageName);
        }

        public IEnumerable<object> GetPageElements(PageInfo page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            // Get the cached page if present, otherwise return an empty array

            PageDetails pageDetails;

            if (!pageCache.TryGetValue(page, out pageDetails))
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
                throw new ArgumentNullException("state");

            // Restore the navigation stack
            // NB: Flag that we are restoring state so we can pass a NavigationMode.Refresh to restored pages

            restoringState = true;

            try
            {
                NavigationStack.Push(state.NavigationStack);
            }
            finally
            {
                restoringState = false;
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

                if (pageCache.TryGetValue(pageInfo, out pageDetails))
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

        // *** Protected Event Methods ***

        protected virtual void OnNavigatedTo(PageNavigationEventArgs args)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            EventHandler<PageNavigationEventArgs> eventHandler = NavigatedTo;

            if (eventHandler != null)
                eventHandler(this, args);
        }

        protected virtual void OnNavigatingFrom(PageNavigationEventArgs args)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            EventHandler<PageNavigationEventArgs> eventHandler = NavigatingFrom;

            if (eventHandler != null)
                eventHandler(this, args);
        }

        // *** Private Methods ***

        private void CallNavigatedTo(PageInfo pageInfo, PageNavigationMode navigationMode)
        {
            // Fire the NavigatedTo event

            OnNavigatedTo(new PageNavigationEventArgs(pageInfo, navigationMode));

            // Call NavigatedTo on all page elements

            foreach (object element in GetPageElements(pageInfo))
            {
                if (element is INavigationAware)
                    ((INavigationAware)element).NavigatedTo(navigationMode);
            }
        }

        private void CallNavigatingFrom(PageInfo pageInfo, PageNavigationMode navigationMode)
        {
            // Fire the NavigatingFrom event

            OnNavigatingFrom(new PageNavigationEventArgs(pageInfo, navigationMode));

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

            IViewLifetimeContext viewLifetimeContext = viewFactory.CreateView(pageInfo.PageName, navigationContext);

            // Activate the view model if it implements IActivatable

            object viewModel = viewLifetimeContext.ViewModel;

            if (viewModel is IActivatable)
            {
                ((IActivatable)viewModel).Activate(pageInfo);
            }

            // Add the page details to the cache and return

            PageDetails pageDetails = new PageDetails(viewLifetimeContext);
            pageCache.Add(pageInfo, pageDetails);

            return pageDetails;
        }

        private void DisposePage(PageInfo pageInfo)
        {
            PageDetails pageDetails = null;

            if (pageCache.TryGetValue(pageInfo, out pageDetails))
            {
                // Dispose of the view and view-model

                pageDetails.ViewLifetimeContext.Dispose();

                // Remove the page from the cache

                pageCache.Remove(pageInfo);
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

                if (!pageCache.TryGetValue(pageInfo, out pageDetails))
                    pageDetails = CreatePage(pageInfo);

                // Navigate to the relevant page

                DisplayPage(pageDetails.ViewLifetimeContext.View);
            }
        }
        
        // *** Event Handlers ***

        private void navigationStack_NavigatedTo(object sender, PageNavigationEventArgs e)
        {
            if (restoringState)
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
                case "CanGoBack":
                    {
                        EventHandler eventHandler = CanGoBackChanged;

                        if (eventHandler != null)
                            eventHandler(this, EventArgs.Empty);
                    }
                    break;
                case "CurrentPage":
                    DisplayNavigationEntry((PageInfo)navigationStack.CurrentPage);
                    break;
            }
        }

        // *** Private Sub-classes ***

        private class PageDetails
        {
            // *** Fields ***

            private readonly IViewLifetimeContext viewLifetimeContext;

            // *** Constructors ***

            public PageDetails(IViewLifetimeContext viewLifetimeContext)
            {
                this.viewLifetimeContext = viewLifetimeContext;
            }

            // *** Properties ***

            public IViewLifetimeContext ViewLifetimeContext
            {
                get
                {
                    return viewLifetimeContext;
                }
            }
        }
    }
}

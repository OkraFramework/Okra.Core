using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Helpers;
using Okra.Navigation;
using Okra.Services;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Search;

namespace Okra.Search
{
    public class SearchManager : ISearchManager, IActivationHandler
    {
        // *** Fields ***

        private readonly INavigationManager navigationManager;

        private string searchPageName = SpecialPageNames.Search;
        private bool isActivated;

        // *** Constructors ***

        public SearchManager(INavigationManager navigationManager, IActivationManager activationManager)
        {
            if (navigationManager == null)
                throw new ArgumentNullException("navigationManager");

            if (activationManager == null)
                throw new ArgumentNullException("activationManager");

            this.navigationManager = navigationManager;

            // Register with the activation manager

            activationManager.Register(this);
            activationManager.Activated += OnActivationManagerActivated;
        }

        // *** Properties ***

        public string SearchPageName
        {
            get
            {
                return searchPageName;
            }
            set
            {
                // Validate parameters

                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "SearchPageName");

                // If we have been activated then do not allow setting of this property

                if (isActivated)
                    throw new InvalidOperationException(string.Format(ResourceHelper.GetErrorResource("Exception_InvalidOperation_PropertyCannotBeSetAfterActivation"), "SearchPageName"));

                // Set the property

                searchPageName = value;
            }
        }

        // *** Methods ***

        public Task<bool> Activate(IActivatedEventArgs activatedEventArgs)
        {
            if (activatedEventArgs == null)
                throw new ArgumentNullException("activatedEventArgs");

            return ActivateInternal(activatedEventArgs);
        }

        private async Task<bool> ActivateInternal(IActivatedEventArgs activatedEventArgs)
        {
            if (activatedEventArgs.Kind == ActivationKind.Search)
            {
                ISearchActivatedEventArgs searchEventArgs = (ISearchActivatedEventArgs)activatedEventArgs;

                // If the previous execution state was terminated then attempt to restore the navigation stack

                if (activatedEventArgs.PreviousExecutionState == ApplicationExecutionState.Terminated)
                    await navigationManager.RestoreNavigationStack();

                // Otherwise if the application is a new instance navigate to the home page

                else if (activatedEventArgs.PreviousExecutionState == ApplicationExecutionState.ClosedByUser
                      || activatedEventArgs.PreviousExecutionState == ApplicationExecutionState.NotRunning)
                    navigationManager.NavigateTo(navigationManager.HomePageName);

                // Then display the search results

                DisplaySearchResults(searchEventArgs.QueryText, searchEventArgs.Language);

                return true;
            }

            return false;
        }

        // *** Protected Methods ***

        protected void OnActivationManagerActivated(object sender, IActivatedEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            // Once the application is activated we register for the SearchPage.QuerySubmitted event
            // NB: This is a slightly more performant method of receiving search queries for running applications than via activation
            
            if (!isActivated)
            {
                RegisterQuerySubmitted();

                // Set the isActivated flag

                isActivated = true;
            }
        }

        protected void OnQuerySubmitted(SearchPane sender, SearchPaneQuerySubmittedEventArgs args)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            DisplaySearchResults(args.QueryText, args.Language);
        }

        protected virtual void RegisterQuerySubmitted()
        {
            SearchPane searchPane = SearchPane.GetForCurrentView();
            searchPane.QuerySubmitted += OnQuerySubmitted;
        }

        // *** Private Methods ***

        private void DisplaySearchResults(string queryText, string language)
        {
            // If there are no search results to display then just return

            if (queryText == "")
                return;

            // Navigate to the search page if it is not currently visible

            PageInfo currentPage = navigationManager.NavigationStack.CurrentPage;

            if (currentPage == null || currentPage.PageName != SearchPageName)
                navigationManager.NavigateTo(SearchPageName);

            // For all page elements that implement ISearchPage then execute the query

            IEnumerable<ISearchPage> searchPages = navigationManager.GetPageElements(navigationManager.NavigationStack.CurrentPage).Where(page => page is ISearchPage).Cast<ISearchPage>();

            foreach (ISearchPage searchPage in searchPages)
            {
                searchPage.PerformQuery(queryText, language);
            }
        }
    }
}

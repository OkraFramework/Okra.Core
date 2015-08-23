using $safeprojectname$.Common;
using Okra.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Composition;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// TODO: Connect the Search Results Page to your in-app search.
// The Search Results Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234240

namespace $rootnamespace$
{
    /// <summary>
    /// This view model displays search results when a global search is directed to this application.
    /// </summary>
    [ViewModelExport("$fileinputname$")]
    public class $safeitemname$ : ViewModelBase, IActivatable
    {
        private string queryText;
        private IEnumerable<Filter> filters;
        private IEnumerable<SampleDataItem> results;
        private Filter selectedFilter;
        private bool showFilters;
        private bool showResults;

        [ImportingConstructor]
        public $safeitemname$(INavigationContext navigationContext)
            : base(navigationContext)
        {
        }

        public string QueryText
        {
            get
            {
                return queryText;
            }
            protected set
            {
                SetProperty(ref queryText, value);
            }
        }

        public IEnumerable<Filter> Filters
        {
            get
            {
                return filters;
            }
            protected set
            {
                SetProperty(ref filters, value);
            }
        }

        public IEnumerable<SampleDataItem> Results
        {
            get
            {
                return results;
            }
            protected set
            {
                SetProperty(ref results, value);
            }
        }

        public Filter SelectedFilter
        {
            get
            {
                return selectedFilter;
            }
            protected set
            {
                SetProperty(ref selectedFilter, value);
                OnSelectedFilterChanged(selectedFilter);
            }
        }

        public bool ShowFilters
        {
            get
            {
                return showFilters;
            }
            protected set
            {
                SetProperty(ref showFilters, value);
            }
        }

        public bool ShowResults
        {
            get
            {
                return showResults;
            }
            protected set
            {
                SetProperty(ref showResults, value);
            }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="pageInfo">Information on the arguments and state passed to the page.</param>
        public async void Activate(PageInfo pageInfo)
        {
            string queryText = pageInfo.GetArguments<string>();

            // TODO: Application-specific searching logic.  The search process is responsible for
            //       creating a list of user-selectable result categories:
            //
            //       filterList.Add(new Filter("<filter name>", <result count>));
            //
            //       Only the first filter, typically "All", should pass true as a third argument in
            //       order to start in an active state.  Results for the active filter are provided
            //       in OnSelectedFilterChanged below.

            var filterList = new List<Filter>();
            filterList.Add(new Filter("All", 0, true));

            // Communicate results through the view model
            this.QueryText = '\u201c' + queryText + '\u201d';
            this.Filters = filterList;
            this.ShowFilters = filterList.Count > 1;

            // Attach to filter selection events
            foreach (Filter filter in filterList)
            {
                filter.PropertyChanged += (sender, e) =>
                    {
                        if (e.PropertyName == "Active" && ((Filter)sender).Active)
                            SelectedFilter = (Filter)sender;
                    };
            }

            // Set the first active filter as the selected filter
            SelectedFilter = filterList.FirstOrDefault(f => f.Active);
        }

        /// <summary>
        /// Saves any state to be recreated in a future session.
        /// </summary>
        /// <param name="pageInfo">Object to store page state.</param>
        public void SaveState(PageInfo pageInfo)
        {
        }

        private void OnSelectedFilterChanged(Filter selectedFilter)
        {
            if (selectedFilter != null)
            {
                // Mirror the results into the corresponding Filter object to allow the
                // RadioButton representation used when not snapped to reflect the change
                selectedFilter.Active = true;

                // TODO: Respond to the change in active filter by setting this.Results
                //       to a collection of items with bindable Image, Title, Subtitle, and Description properties



                // Display text if no results are found
                if (this.Results != null)
                    this.ShowResults = this.Results.Take(1).Count() > 0;
                else
                    this.ShowResults = false;
            }
            else
            {
                // Otherwise clear the results
                this.Results = null;
                this.ShowResults = false;
            }
        }

        // TODO : Replace references to placeholder classes with actual data objects

        public class SampleDataItem {}
    }
}

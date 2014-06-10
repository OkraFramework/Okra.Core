using Okra.Core;
using Okra.Navigation;
using Okra.Search;
using $safeprojectname$.Common;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Runtime.Serialization;

namespace $rootnamespace$
{
    // TODO: Add the following code to your AppBootstrapper,
    //
    //   [Import]
    //   public Okra.Search.ISearchManager SearchManager { get; set; }
    //

    /// <summary>
    /// A search contract view-model that displays search results when a global search is directed to this application.
    /// </summary>
    [ViewModelExport(SpecialPageNames.Search)]
    public class $fileinputname$ViewModel : ViewModelBase, ISearchPage, IActivatable<string, $fileinputname$ViewModel.SearchState>
    {
        private string queryText;
        private IList<Filter> filters;
        private Filter selectedFilter;
        private IList<SampleDataItem> results;
        private bool showFilters;
        private bool showResults;
        private SearchState searchState;

        protected $fileinputname$ViewModel()
        {
        }

        [ImportingConstructor]
        public $fileinputname$ViewModel(INavigationContext navigationContext)
            : base(navigationContext)
        {
        }

        public IList<Filter> Filters
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

        public IList<SampleDataItem> Results
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
            set
            {
                if (SetProperty(ref selectedFilter, value) && value != null)
                    UpdateResults(value);
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

        public void PerformQuery(string queryText, string language)
        {
            // TODO: Application-specific searching logic.  The search process is responsible for
            //       creating a list of user-selectable result categories:
            //
            //       filterList.Add(new Filter(this, "<filter name>", <result count>));

            var filterList = new List<Filter>();
            filterList.Add(new Filter(this, "All", 0));

            // Communicate results through the view model

            this.QueryText = '\u201c' + queryText + '\u201d';
            this.ShowResults = false;
            this.Filters = filterList;
            this.SelectedFilter = filterList.FirstOrDefault();
            this.ShowFilters = filterList.Count > 1;

            // Store the search state in case of suspension

            this.searchState = new SearchState(queryText, language);
        }

        private void UpdateResults(Filter selectedFilter)
        {
            // TODO: Respond to the change in active filter by setting this.Results
            //       to a collection of items with bindable Image, Title, Subtitle, and Description properties

            // Mirror any change in the ComboBox from snapped view in the full view

            selectedFilter.Active = true;

            // Display informational text when there are no search results

            this.ShowResults = Results != null && Results.Count != 0;
        }

        public void Activate(string arguments, $fileinputname$ViewModel.SearchState state)
        {
            if (state != null)
                PerformQuery(state.QueryText, state.Language);
        }

        public $fileinputname$ViewModel.SearchState SaveState()
        {
            return searchState;
        }

        /// <summary>
        /// View model describing one of the filters available for viewing search results.
        /// </summary>
        public sealed class Filter : NotifyPropertyChangedBase
        {
            $fileinputname$ViewModel parent;
            private String _name;
            private int _count;
            private bool _active;

            public Filter($fileinputname$ViewModel parent, String name, int count)
            {
                this.parent = parent;

                this.Name = name;
                this.Count = count;
            }

            public String Name
            {
                get { return _name; }
                set { if (this.SetProperty(ref _name, value)) this.OnPropertyChanged("Description"); }
            }

            public int Count
            {
                get { return _count; }
                set { if (this.SetProperty(ref _count, value)) this.OnPropertyChanged("Description"); }
            }

            public bool Active
            {
                get
                {
                    return _active;
                }
                set
                {
                    if (this.SetProperty(ref _active, value) && value == true)
                        parent.SelectedFilter = this;
                }
            }

            public String Description
            {
                get { return String.Format("{0} ({1})", _name, _count); }
            }
        }

        [DataContract]
        public class SearchState
        {
            public SearchState(string queryText, string language)
            {
                this.QueryText = queryText;
                this.Language = language;
            }

            [DataMember]
            public string QueryText { get; private set; }

            [DataMember]
            public string Language { get; private set; }
        }

        // TODO : Remove these placeholder data types and replace all references with those from your actual data model.

        public class SampleDataItem
        {
            public string UniqueId { get; private set; }

            public string Title { get; set; }
            public string Subtitle { get; set; }
            public string Content { get; set; }
        }
    }
}

using $safeprojectname$.Common;
using $safeprojectname$.Data;
using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace $safeprojectname$
{
    /// <summary>
    /// A view model for displaying details for a single item within a group.
    /// </summary>
    [ViewModelExport("ItemDetail")]
    public class ItemDetailViewModel : ViewModelBase, IActivatable
    {
        private SampleDataItem item;

        [ImportingConstructor]
        public ItemDetailViewModel(INavigationContext navigationContext)
            : base(navigationContext)
        {
        }

        public SampleDataItem Item
        {
            get
            {
                return item;
            }
            protected set
            {
                SetProperty(ref item, value);
            }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="pageInfo">Information on the arguments and state passed to the page.</param>
        public async void Activate(PageInfo pageInfo)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data

            string itemId = pageInfo.GetArguments<string>();
            var item = await SampleDataSource.GetItemAsync(itemId);
            this.Item = item;
        }

        /// <summary>
        /// Saves any state to be recreated in a future session.
        /// </summary>
        /// <param name="pageInfo">Object to store page state.</param>
        public void SaveState(PageInfo pageInfo)
        {
        }
    }
}
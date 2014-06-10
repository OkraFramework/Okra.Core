using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace Okra.DataTransfer
{
    public class ShareSourceManager : IShareSourceManager
    {
        // *** Fields ***

        private bool isRegistered = false;
        private INavigationBase navigationManager;

        // *** Constructors ***

        public ShareSourceManager(INavigationManager navigationManager)
        {
            this.navigationManager = navigationManager;
            navigationManager.NavigatedTo += NavigationManager_NavigatedTo;
        }

        // *** Properties ***

        public string DefaultFailureText
        {
            get;
            set;
        }

        // *** Methods ***

        public void ShowShareUI()
        {
            DataTransferManager.ShowShareUI();
        }

        // *** Protected Methods ***
        
        protected virtual void RegisterForSharing()
        {
            // Register with the DataTransferManager

            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }

        protected void DataRequested(IDataRequest dataRequest)
        {
            // Find the first page element that implements IShareable and forward the data request

            PageInfo currentPage = navigationManager.NavigationStack.CurrentPage;
            bool hasRequestBeenProcessed = false;

            if (currentPage != null)
            {
                foreach (object element in navigationManager.GetPageElements(currentPage))
                {
                    if (element is IShareable)
                    {
                        ((IShareable)element).ShareRequested(dataRequest);
                        hasRequestBeenProcessed = true;
                    }
                }
            }

            // If there is nothing to share and their is a default failure text specified then return this

            if (!hasRequestBeenProcessed && !string.IsNullOrEmpty(DefaultFailureText))
                dataRequest.FailWithDisplayText(DefaultFailureText);
        }

        // *** Private Methods ***

        protected void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            IDataRequest dataRequest = new DataRequestProxy(args.Request);
            DataRequested(dataRequest);
        }

        private void NavigationManager_NavigatedTo(object sender, PageNavigationEventArgs e)
        {
            // On first navigation with the navigation manager then register with the DataTransferManager
            // NB: The INavigationManager import should only be created on the main application view

            if (!isRegistered)
            {
                RegisterForSharing();
                isRegistered = true;
            }
        }

        // *** Private Sub-classes ***

        private class DataRequestProxy : IDataRequest
        {
            // *** Fields ***

            private readonly DataRequest dataRequest;

            // *** Constructors ***

            public DataRequestProxy(DataRequest dataRequest)
            {
                this.dataRequest = dataRequest;
            }

            // *** Properties ***

            public DataPackage Data
            {
                get
                {
                    return dataRequest.Data;
                }
                set
                {
                    dataRequest.Data = value;
                }
            }

            public DateTimeOffset Deadline
            {
                get
                {
                    return dataRequest.Deadline;
                }
            }

            // *** Methods ***

            public void FailWithDisplayText(string displayText)
            {
                dataRequest.FailWithDisplayText(displayText);
            }

            public DataRequestDeferral GetDeferral()
            {
                return dataRequest.GetDeferral();
            }
        }
    }
}

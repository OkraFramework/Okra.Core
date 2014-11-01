using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace Okra.Sharing
{
    public class ShareSourceManager : IShareSourceManager
    {
        // *** Fields ***

        private bool isRegistered = false;
        private INavigationBase navigationManager;

        // *** Constructors ***

        public ShareSourceManager(INavigationManager navigationManager)
        {
            if (navigationManager == null)
                throw new ArgumentNullException("navigationManager");

            this.navigationManager = navigationManager;
            navigationManager.NavigationStack.NavigatedTo += NavigationManager_NavigatedTo;
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

        protected Task ShareRequested(IShareRequest shareRequest)
        {
            if (shareRequest == null)
                throw new ArgumentNullException("shareRequest");

            return ShareRequestedInternal(shareRequest);
        }

        protected async Task ShareRequestedInternal(IShareRequest shareRequest)
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
                        await ((IShareable)element).ShareRequested(shareRequest);
                        hasRequestBeenProcessed = true;
                    }
                }
            }

            // If there is nothing to share and their is a default failure text specified then return this

            if (!hasRequestBeenProcessed && !string.IsNullOrEmpty(DefaultFailureText))
                shareRequest.FailWithDisplayText(DefaultFailureText);
        }

        // *** Private Methods ***

        private async void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequestDeferral deferral = args.Request.GetDeferral();
            IShareRequest shareRequest = new ShareRequest(args.Request);
            await ShareRequested(shareRequest);
            deferral.Complete();
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

        private class ShareRequest : IShareRequest
        {
            // *** Fields ***

            private readonly DataRequest dataRequest;
            private readonly ISharePackage sharePackage;

            // *** Constructors ***

            public ShareRequest(DataRequest dataRequest)
            {
                this.dataRequest = dataRequest;
                this.sharePackage = new SharePackage(dataRequest.Data);
            }

            // *** Properties ***

            public ISharePackage Data
            {
                get
                {
                    return sharePackage;
                }
            }

            // *** Methods ***

            public void FailWithDisplayText(string displayText)
            {
                dataRequest.FailWithDisplayText(displayText);
            }
        }
    }
}

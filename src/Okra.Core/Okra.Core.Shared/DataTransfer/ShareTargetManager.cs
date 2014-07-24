using Okra.Helpers;
using Okra.Navigation;
using Okra.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Okra.DataTransfer
{
    public class ShareTargetManager : IShareTargetManager, IActivationHandler
    {
        // *** Fields ***

        private readonly IViewFactory viewFactory;

        private string shareTargetPageName = SpecialPageNames.ShareTarget;

        // *** Constructors ***

        public ShareTargetManager(IActivationManager activationManager, IViewFactory viewFactory)
        {
            this.viewFactory = viewFactory;

            // Register with the activation manager

            activationManager.Register(this);
        }

        // *** Properties ***

        public string ShareTargetPageName
        {
            get
            {
                return shareTargetPageName;
            }
            set
            {
                // Validate parameters

                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "SearchPageName");

                // Set the property

                shareTargetPageName = value;
            }
        }

        // *** Methods ***

        public async Task<bool> Activate(IActivatedEventArgs activatedEventArgs)
        {
            if (activatedEventArgs.Kind == ActivationKind.ShareTarget)
            {
                IShareTargetActivatedEventArgs shareTargetEventArgs = (IShareTargetActivatedEventArgs)activatedEventArgs;
                IShareOperation shareOperation = WrapShareOperation(shareTargetEventArgs);

                // Create a new page to display the share target UI

                IViewLifetimeContext viewLifetimeContext = viewFactory.CreateView(ShareTargetPageName, null);

                // Call Activate(...) methods

                if (viewLifetimeContext.View is IShareTarget)
                    ((IShareTarget)viewLifetimeContext.View).Activate(shareOperation);

                if (viewLifetimeContext.ViewModel is IShareTarget)
                    ((IShareTarget)viewLifetimeContext.ViewModel).Activate(shareOperation);

                // Display the page

                DisplayPage(viewLifetimeContext);

                // Call NavigatedTo(...) methods

                if (viewLifetimeContext.View is INavigationAware)
                    ((INavigationAware)viewLifetimeContext.View).NavigatedTo(PageNavigationMode.New);

                if (viewLifetimeContext.ViewModel is INavigationAware)
                    ((INavigationAware)viewLifetimeContext.ViewModel).NavigatedTo(PageNavigationMode.New);

                return true;
            }

            return false;
        }

        // *** Protected Methods ***

        protected virtual void DisplayPage(IViewLifetimeContext viewLifetimeContext)
        {
            // Create a new content host for the page

            ContentControl contentHost = new ContentControl()
            {
                Content = viewLifetimeContext.View,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch
            };

            // Register for the window closing event to all disposal of the page and view-model

            Window.Current.Closed += (sender, e) => OnWindowClosing(viewLifetimeContext);

            // Display the page and activate the window

            Window.Current.Content = contentHost;
            Window.Current.Activate();
        }

        protected void OnWindowClosing(IViewLifetimeContext viewLifetimeContext)
        {
            // Call NavigatingFrom(...) methods

            if (viewLifetimeContext.View is INavigationAware)
                ((INavigationAware)viewLifetimeContext.View).NavigatingFrom(PageNavigationMode.Back);

            if (viewLifetimeContext.ViewModel is INavigationAware)
                ((INavigationAware)viewLifetimeContext.ViewModel).NavigatingFrom(PageNavigationMode.Back);

            // Dispose of the view lifetime context

            viewLifetimeContext.Dispose();
        }

        protected virtual IShareOperation WrapShareOperation(IShareTargetActivatedEventArgs shareTargetEventArgs)
        {
            return new ShareOperationProxy(shareTargetEventArgs.ShareOperation);
        }

        // *** Private Sub-classes ***

        private class ShareOperationProxy : IShareOperation
        {
            // *** Fields ***

            private readonly ShareOperation shareOperation;

            // *** Constructors ***

            public ShareOperationProxy(ShareOperation shareOperation)
            {
                this.shareOperation = shareOperation;
            }

            // *** Properties ***

            public DataPackageView Data
            {
                get
                {
                    return shareOperation.Data;
                }
            }

            public string QuickLinkId
            {
                get
                {
                    return shareOperation.QuickLinkId;
                }
            }

            // *** Methods ***

            public void RemoveThisQuickLink()
            {
                shareOperation.RemoveThisQuickLink();
            }

            public void ReportCompleted()
            {
                shareOperation.ReportCompleted();
            }

            public void ReportCompleted(QuickLink quicklink)
            {
                shareOperation.ReportCompleted(quicklink);
            }

            public void ReportDataRetrieved()
            {
                shareOperation.ReportDataRetrieved();
            }

            public void ReportError(string value)
            {
                shareOperation.ReportError(value);
            }

            public void ReportStarted()
            {
                shareOperation.ReportStarted();
            }

            public void ReportSubmittedBackgroundTask()
            {
                shareOperation.ReportSubmittedBackgroundTask();
            }
        }
    }
}

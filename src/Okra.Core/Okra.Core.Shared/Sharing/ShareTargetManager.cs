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

namespace Okra.Sharing
{
    public class ShareTargetManager : IShareTargetManager, IActivationHandler
    {
        // *** Fields ***

        private readonly IViewFactory _viewFactory;

        private string _shareTargetPageName = SpecialPageNames.ShareTarget;

        // *** Constructors ***

        public ShareTargetManager(IActivationManager activationManager, IViewFactory viewFactory)
        {
            if (activationManager == null)
                throw new ArgumentNullException("activationManager");

            if (viewFactory == null)
                throw new ArgumentNullException("viewFactory");

            _viewFactory = viewFactory;

            // Register with the activation manager

            activationManager.Register(this);
        }

        // *** Properties ***

        public string ShareTargetPageName
        {
            get
            {
                return _shareTargetPageName;
            }
            set
            {
                // Validate parameters

                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "SearchPageName");

                // Set the property

                _shareTargetPageName = value;
            }
        }

        // *** Methods ***

        public Task<bool> Activate(IActivatedEventArgs activatedEventArgs)
        {
            if (activatedEventArgs == null)
                throw new ArgumentNullException("activatedEventArgs");

            if (activatedEventArgs.Kind == ActivationKind.ShareTarget)
            {
                IShareTargetActivatedEventArgs shareTargetEventArgs = (IShareTargetActivatedEventArgs)activatedEventArgs;
                IShareOperation shareOperation = WrapShareOperation(shareTargetEventArgs);

                // Create a new page to display the share target UI

                IViewLifetimeContext viewLifetimeContext = _viewFactory.CreateView(ShareTargetPageName, new NullNavigationContext());

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

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        // *** Protected Methods ***

        protected virtual void DisplayPage(IViewLifetimeContext viewLifetimeContext)
        {
            if (viewLifetimeContext == null)
                throw new ArgumentNullException("viewLifetimeContext");

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
            if (viewLifetimeContext == null)
                throw new ArgumentNullException("viewLifetimeContext");

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
            if (shareTargetEventArgs == null)
                throw new ArgumentNullException("shareTargetEventArgs");

            return new ShareOperationProxy(shareTargetEventArgs.ShareOperation);
        }

        // *** Private Sub-classes ***

        private class NullNavigationContext : INavigationContext
        {
            // *** Methods ***

            public INavigationBase GetCurrent()
            {
                return null;
            }
        }

        private class ShareOperationProxy : IShareOperation
        {
            // *** Fields ***

            private readonly ShareOperation _shareOperation;
            private readonly SharePackageView _data;

            // *** Constructors ***

            public ShareOperationProxy(ShareOperation shareOperation)
            {
                _shareOperation = shareOperation;
                _data = new SharePackageView(shareOperation.Data);
            }

            // *** Properties ***

            public ISharePackageView Data
            {
                get
                {
                    return _data;
                }
            }

            // *** Methods ***

            public void ReportCompleted()
            {
                _shareOperation.ReportCompleted();
            }

            public void ReportDataRetrieved()
            {
                _shareOperation.ReportDataRetrieved();
            }

            public void ReportError(string value)
            {
                _shareOperation.ReportError(value);
            }

            public void ReportStarted()
            {
                _shareOperation.ReportStarted();
            }

            public void ReportSubmittedBackgroundTask()
            {
                _shareOperation.ReportSubmittedBackgroundTask();
            }
        }
    }
}

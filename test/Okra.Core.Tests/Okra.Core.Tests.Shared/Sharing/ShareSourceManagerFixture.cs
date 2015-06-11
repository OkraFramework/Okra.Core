using Okra.Sharing;
using Okra.Navigation;
using Okra.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Navigation;
using Xunit;

namespace Okra.Tests.Sharing
{
    public class ShareSourceManagerFixture
    {
        // *** Constructor Tests ***

        [Fact]
        public void Constructor_ThrowsException_IfNavigationManagerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ShareSourceManager(null));
        }

        // *** Property Tests ***

        [Fact]
        public void DefaultFailureText_IsNullByDefault()
        {
            IShareSourceManager sharingManager = CreateSharingManager();

            Assert.Equal(null, sharingManager.DefaultFailureText);
        }

        [Fact]
        public void DefaultFailureText_SetterSetsValue()
        {
            IShareSourceManager sharingManager = CreateSharingManager();

            sharingManager.DefaultFailureText = "Test Text";

            Assert.Equal("Test Text", sharingManager.DefaultFailureText);
        }

        // *** Method Tests ***

        [Fact]
        public void ShareRequested_ThrowsException_IfShareRequestIsNull()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();
            TestableSharingManager sharingManager = CreateSharingManager(navigationManager);

            Assert.Throws<ArgumentNullException>(() => sharingManager.ShareRequested(null));
        }

        // *** Behaviour Tests ***

        [Fact]
        public void BeforeNavigation_IsNotRegisteredWithDataTransferManager()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();
            TestableSharingManager sharingManager = CreateSharingManager(navigationManager);

            Assert.Equal(0, sharingManager.RegisterForSharingCount);
        }

        [Fact]
        public void OnFirstPageNavigation_RegistersWithDataTransferManager()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();
            TestableSharingManager sharingManager = CreateSharingManager(navigationManager);

            navigationManager.RaiseNavigatedTo(new PageNavigationEventArgs(new PageInfo("Page 1", null), PageNavigationMode.Forward));

            Assert.Equal(1, sharingManager.RegisterForSharingCount);
        }

        [Fact]
        public void OnSecondPageNavigation_OnlyRegistersWithDataTransferManagerOnce()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();
            TestableSharingManager sharingManager = CreateSharingManager(navigationManager);

            navigationManager.RaiseNavigatedTo(new PageNavigationEventArgs(new PageInfo("Page 1", null), PageNavigationMode.Forward));
            navigationManager.RaiseNavigatedTo(new PageNavigationEventArgs(new PageInfo("Page 2", null), PageNavigationMode.Forward));

            Assert.Equal(1, sharingManager.RegisterForSharingCount);
        }

        [Fact]
        public void WhenNothingToShare_DoesNotSetDisplayText_IfDefaultFailureTextIsNull()
        {
            TestableSharingManager sharingManager = CreateSharingManager();
            sharingManager.DefaultFailureText = null;

            MockShareRequest shareRequest = new MockShareRequest();
            sharingManager.ShareRequested(shareRequest);

            Assert.Equal(new string[] { }, shareRequest.FailureText);
        }

        [Fact]
        public void WhenNothingToShare_DoesNotSetDisplayText_IfDefaultFailureTextIsEmpty()
        {
            TestableSharingManager sharingManager = CreateSharingManager();
            sharingManager.DefaultFailureText = "";

            MockShareRequest shareRequest = new MockShareRequest();
            sharingManager.ShareRequested(shareRequest);

            Assert.Equal(new string[] { }, shareRequest.FailureText);
        }

        [Fact]
        public void WhenNothingToShare_ReturnsDisplayText_IfDefaultFailureTextIsSpecified()
        {
            TestableSharingManager sharingManager = CreateSharingManager();
            sharingManager.DefaultFailureText = "Test Text";

            MockShareRequest shareRequest = new MockShareRequest();
            sharingManager.ShareRequested(shareRequest);

            Assert.Equal(new string[] { "Test Text" }, shareRequest.FailureText);
        }

        [Fact]
        public void WithSharableElement_ForwardsShareRequest()
        {
            INavigationManager navigationManager = new MockNavigationManager(_ => new object[] { new MockPageElement(), new MockShareablePageElement(), new MockPageElement() });

            TestableSharingManager sharingManager = CreateSharingManager(navigationManager);

            navigationManager.NavigationStack.NavigateTo(new PageInfo("Test Page", null));

            MockShareRequest shareRequest = new MockShareRequest();
            sharingManager.ShareRequested(shareRequest);

            MockShareablePageElement sharableElement = navigationManager.GetPageElements(navigationManager.NavigationStack.CurrentPage).First(e => e is MockShareablePageElement) as MockShareablePageElement;
            Assert.Equal(new object[] { shareRequest }, sharableElement.ShareRequests);
        }

        [Fact]
        public void WithSharableElement_DoesNotSetDisplayText()
        {
            INavigationManager navigationManager = new MockNavigationManager(_ => new object[] { new MockShareablePageElement() });

            TestableSharingManager sharingManager = CreateSharingManager(navigationManager);
            sharingManager.DefaultFailureText = "Default Text";

            navigationManager.NavigationStack.NavigateTo(new PageInfo("Test Page", null));
            MockShareRequest shareRequest = new MockShareRequest();
            sharingManager.ShareRequested(shareRequest);

            Assert.Equal(new string[] { }, shareRequest.FailureText);
        }

        // *** Private Methods ***

        private TestableSharingManager CreateSharingManager(INavigationManager navigationManager = null)
        {
            if (navigationManager == null)
                navigationManager = new MockNavigationManager();

            TestableSharingManager sharingManager = new TestableSharingManager(navigationManager);

            return sharingManager;
        }

        // *** Private sub-classes ***

        private class TestableSharingManager : ShareSourceManager
        {
            // *** Fields ***

            public int RegisterForSharingCount = 0;

            // *** Constructors ***

            public TestableSharingManager(INavigationManager navigationManager)
                : base(navigationManager)
            {
            }

            // *** Methods ***

            public new void ShareRequested(IShareRequest shareRequest)
            {
                base.ShareRequested(shareRequest);
            }

            // *** Overriden base methods ***

            protected override void RegisterForSharing()
            {
                RegisterForSharingCount++;
            }
        }

        private class MockShareRequest : IShareRequest
        {
            // *** Fields ***

            public List<string> FailureText = new List<string>();

            // *** Properties ***

            public ISharePackage Data
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            // *** Methods ***

            public void FailWithDisplayText(string displayText)
            {
                FailureText.Add(displayText);
            }
        }

        private class MockPageElement
        {
        }

        private class MockShareablePageElement : IShareable
        {
            // *** Fields ***

            public readonly List<IShareRequest> ShareRequests = new List<IShareRequest>();

            // *** Methods ***

            public Task ShareRequested(IShareRequest shareRequest)
            {
                ShareRequests.Add(shareRequest);
                return Task.FromResult(true);
            }
        }
    }
}

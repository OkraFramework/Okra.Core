using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Okra.DataTransfer;
using Okra.Navigation;
using Okra.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Navigation;

namespace Okra.Tests.DataTransfer
{
    [TestClass]
    public class ShareSourceManagerFixture
    {
        // *** Property Tests ***

        [TestMethod]
        public void DefaultFailureText_IsNullByDefault()
        {
            IShareSourceManager sharingManager = CreateSharingManager();

            Assert.AreEqual(null, sharingManager.DefaultFailureText);
        }

        [TestMethod]
        public void DefaultFailureText_SetterSetsValue()
        {
            IShareSourceManager sharingManager = CreateSharingManager();

            sharingManager.DefaultFailureText = "Test Text";

            Assert.AreEqual("Test Text", sharingManager.DefaultFailureText);
        }

        // *** Behaviour Tests ***

        [TestMethod]
        public void BeforeNavigation_IsNotRegisteredWithDataTransferManager()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();
            TestableSharingManager sharingManager = CreateSharingManager(navigationManager);

            Assert.AreEqual(0, sharingManager.RegisterForSharingCount);
        }

        [TestMethod]
        public void OnFirstPageNavigation_RegistersWithDataTransferManager()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();
            TestableSharingManager sharingManager = CreateSharingManager(navigationManager);

            navigationManager.RaiseNavigatedTo(new PageNavigationEventArgs(new PageInfo("Page 1", null), PageNavigationMode.Forward));

            Assert.AreEqual(1, sharingManager.RegisterForSharingCount);
        }

        [TestMethod]
        public void OnSecondPageNavigation_OnlyRegistersWithDataTransferManagerOnce()
        {
            MockNavigationManager navigationManager = new MockNavigationManager();
            TestableSharingManager sharingManager = CreateSharingManager(navigationManager);

            navigationManager.RaiseNavigatedTo(new PageNavigationEventArgs(new PageInfo("Page 1", null), PageNavigationMode.Forward));
            navigationManager.RaiseNavigatedTo(new PageNavigationEventArgs(new PageInfo("Page 2", null), PageNavigationMode.Forward));

            Assert.AreEqual(1, sharingManager.RegisterForSharingCount);
        }

        [TestMethod]
        public void WhenNothingToShare_DoesNotSetDisplayText_IfDefaultFailureTextIsNull()
        {
            TestableSharingManager sharingManager = CreateSharingManager();
            sharingManager.DefaultFailureText = null;

            MockDataRequest dataRequest = new MockDataRequest();
            sharingManager.DataRequested(dataRequest);

            CollectionAssert.AreEqual(new string[] { }, dataRequest.FailureText);
        }

        [TestMethod]
        public void WhenNothingToShare_DoesNotSetDisplayText_IfDefaultFailureTextIsEmpty()
        {
            TestableSharingManager sharingManager = CreateSharingManager();
            sharingManager.DefaultFailureText = "";

            MockDataRequest dataRequest = new MockDataRequest();
            sharingManager.DataRequested(dataRequest);

            CollectionAssert.AreEqual(new string[] { }, dataRequest.FailureText);
        }

        [TestMethod]
        public void WhenNothingToShare_ReturnsDisplayText_IfDefaultFailureTextIsSpecified()
        {
            TestableSharingManager sharingManager = CreateSharingManager();
            sharingManager.DefaultFailureText = "Test Text";

            MockDataRequest dataRequest = new MockDataRequest();
            sharingManager.DataRequested(dataRequest);

            CollectionAssert.AreEqual(new string[] { "Test Text" }, dataRequest.FailureText);
        }

        [TestMethod]
        public void WithSharableElement_ForwardsDataRequest()
        {
            INavigationManager navigationManager = new MockNavigationManager(_ => new object[] { new MockPageElement(), new MockShareablePageElement(), new MockPageElement() });

            TestableSharingManager sharingManager = CreateSharingManager(navigationManager);

            navigationManager.NavigationStack.NavigateTo(new PageInfo("Test Page", null));
            
            MockDataRequest dataRequest = new MockDataRequest();
            sharingManager.DataRequested(dataRequest);

            MockShareablePageElement sharableElement = navigationManager.GetPageElements(navigationManager.NavigationStack.CurrentPage).First(e => e is MockShareablePageElement) as MockShareablePageElement;
            CollectionAssert.AreEqual(new object[] { dataRequest }, sharableElement.DataRequests);
        }

        [TestMethod]
        public void WithSharableElement_DoesNotSetDisplayText()
        {
            INavigationManager navigationManager = new MockNavigationManager(_=>new object[] { new MockShareablePageElement()});

            TestableSharingManager sharingManager = CreateSharingManager(navigationManager);
            sharingManager.DefaultFailureText = "Default Text";

            navigationManager.NavigationStack.NavigateTo(new PageInfo("Test Page", null));
            MockDataRequest dataRequest = new MockDataRequest();
            sharingManager.DataRequested(dataRequest);

            CollectionAssert.AreEqual(new string[] { }, dataRequest.FailureText);
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

            public new void DataRequested(IDataRequest dataRequest)
            {
                base.DataRequested(dataRequest);
            }

            // *** Overriden base methods ***

            protected override void RegisterForSharing()
            {
                RegisterForSharingCount++;
            }
        }

        private class MockDataRequest : IDataRequest
        {
            // *** Fields ***

            public List<string> FailureText = new List<string>();

            // *** Properties ***

            public DataPackage Data
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

            public DateTimeOffset Deadline
            {
                get { throw new NotImplementedException(); }
            }

            // *** Methods ***

            public void FailWithDisplayText(string displayText)
            {
                FailureText.Add(displayText);
            }

            public DataRequestDeferral GetDeferral()
            {
                throw new NotImplementedException();
            }
        }

        private class MockPageElement
        {
        }

        private class MockShareablePageElement : IShareable
        {
            // *** Fields ***

            public readonly List<IDataRequest> DataRequests = new List<IDataRequest>();

            // *** Methods ***

            public void ShareRequested(IDataRequest dataRequest)
            {
                DataRequests.Add(dataRequest);
            }
        }
    }
}

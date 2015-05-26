using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Okra.Sharing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace Okra.Tests.Sharing
{
    [TestClass]
    public class SharePackageViewExFixture
    {
        [TestMethod]
        public async Task GetApplicationLinkAsync_GetsDataFromSharePackageView()
        {
            MockSharePackageView sharePackageView = new MockSharePackageView();
            sharePackageView.Data.Add(StandardDataFormats.ApplicationLink, new Uri("http://www.example.com"));

            Uri data = await sharePackageView.GetApplicationLinkAsync();

            Assert.AreEqual(new Uri("http://www.example.com"), data);
        }

        [TestMethod]
        public void GetApplicationLinkAsync_ThrowsException_IfSharePackageViewIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SharePackageViewEx.GetApplicationLinkAsync(null));
        }

        [TestMethod]
        public async Task GetHtmlFormatAsync_GetsDataFromSharePackageView()
        {
            MockSharePackageView sharePackageView = new MockSharePackageView();
            sharePackageView.Data.Add(StandardDataFormats.Html, "Test Html");

            string data = await sharePackageView.GetHtmlFormatAsync();

            Assert.AreEqual("Test Html", data);
        }

        [TestMethod]
        public void GetHtmlFormatAsync_ThrowsException_IfSharePackageViewIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SharePackageViewEx.GetHtmlFormatAsync(null));
        }

        [TestMethod]
        public async Task GetRtfAsync_GetsDataFromSharePackageView()
        {
            MockSharePackageView sharePackageView = new MockSharePackageView();
            sharePackageView.Data.Add(StandardDataFormats.Rtf, "Test Rtf");

            string data = await sharePackageView.GetRtfAsync();

            Assert.AreEqual("Test Rtf", data);
        }

        [TestMethod]
        public void GetRtfAsync_ThrowsException_IfSharePackageViewIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SharePackageViewEx.GetRtfAsync(null));
        }

        [TestMethod]
        public async Task GetTextAsync_GetsDataFromSharePackageView()
        {
            MockSharePackageView sharePackageView = new MockSharePackageView();
            sharePackageView.Data.Add(StandardDataFormats.Text, "Test Text");

            string data = await sharePackageView.GetTextAsync();

            Assert.AreEqual("Test Text", data);
        }

        [TestMethod]
        public void GetTextAsync_ThrowsException_IfSharePackageViewIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SharePackageViewEx.GetTextAsync(null));
        }

        [TestMethod]
        public async Task GetWebLinkAsync_GetsDataFromSharePackageView()
        {
            MockSharePackageView sharePackageView = new MockSharePackageView();
            sharePackageView.Data.Add(StandardDataFormats.WebLink, new Uri("http://www.example.com"));

            Uri data = await sharePackageView.GetWebLinkAsync();

            Assert.AreEqual(new Uri("http://www.example.com"), data);
        }

        [TestMethod]
        public void GetWebLinkAsync_ThrowsException_IfSharePackageViewIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SharePackageViewEx.GetWebLinkAsync(null));
        }

        // *** Private sub-classes ***

        private class MockSharePackageView : ISharePackageView
        {
            public Dictionary<string, object> Data = new Dictionary<string, object>();

            public IReadOnlyList<string> AvailableFormats
            {
                get { throw new NotImplementedException(); }
            }

            public ISharePropertySet Properties
            {
                get { throw new NotImplementedException(); }
            }

            public bool Contains(string formatId)
            {
                throw new NotImplementedException();
            }

            public async Task<T> GetDataAsync<T>(string formatId)
            {
                await Task.Delay(100);
                return (T)Data[formatId];
            }
        }
    }
}

using Okra.Sharing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Xunit;

namespace Okra.Tests.Sharing
{
    public class SharePackageViewExFixture
    {
        [Fact]
        public async Task GetApplicationLinkAsync_GetsDataFromSharePackageView()
        {
            MockSharePackageView sharePackageView = new MockSharePackageView();
            sharePackageView.Data.Add(StandardDataFormats.ApplicationLink, new Uri("http://www.example.com"));

            Uri data = await sharePackageView.GetApplicationLinkAsync();

            Assert.Equal(new Uri("http://www.example.com"), data);
        }

        [Fact]
        public async void GetApplicationLinkAsync_ThrowsException_IfSharePackageViewIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => SharePackageViewEx.GetApplicationLinkAsync(null));
        }

        [Fact]
        public async Task GetHtmlFormatAsync_GetsDataFromSharePackageView()
        {
            MockSharePackageView sharePackageView = new MockSharePackageView();
            sharePackageView.Data.Add(StandardDataFormats.Html, "Test Html");

            string data = await sharePackageView.GetHtmlFormatAsync();

            Assert.Equal("Test Html", data);
        }

        [Fact]
        public async void GetHtmlFormatAsync_ThrowsException_IfSharePackageViewIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => SharePackageViewEx.GetHtmlFormatAsync(null));
        }

        [Fact]
        public async Task GetRtfAsync_GetsDataFromSharePackageView()
        {
            MockSharePackageView sharePackageView = new MockSharePackageView();
            sharePackageView.Data.Add(StandardDataFormats.Rtf, "Test Rtf");

            string data = await sharePackageView.GetRtfAsync();

            Assert.Equal("Test Rtf", data);
        }

        [Fact]
        public async void GetRtfAsync_ThrowsException_IfSharePackageViewIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => SharePackageViewEx.GetRtfAsync(null));
        }

        [Fact]
        public async Task GetTextAsync_GetsDataFromSharePackageView()
        {
            MockSharePackageView sharePackageView = new MockSharePackageView();
            sharePackageView.Data.Add(StandardDataFormats.Text, "Test Text");

            string data = await sharePackageView.GetTextAsync();

            Assert.Equal("Test Text", data);
        }

        [Fact]
        public async void GetTextAsync_ThrowsException_IfSharePackageViewIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => SharePackageViewEx.GetTextAsync(null));
        }

        [Fact]
        public async Task GetWebLinkAsync_GetsDataFromSharePackageView()
        {
            MockSharePackageView sharePackageView = new MockSharePackageView();
            sharePackageView.Data.Add(StandardDataFormats.WebLink, new Uri("http://www.example.com"));

            Uri data = await sharePackageView.GetWebLinkAsync();

            Assert.Equal(new Uri("http://www.example.com"), data);
        }

        [Fact]
        public async void GetWebLinkAsync_ThrowsException_IfSharePackageViewIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => SharePackageViewEx.GetWebLinkAsync(null));
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

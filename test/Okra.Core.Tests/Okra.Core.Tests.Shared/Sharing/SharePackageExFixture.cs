using Okra.Sharing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Xunit;

namespace Okra.Tests.Sharing
{
    public class SharePackageExFixture
    {
        [Fact]
        public void SetApplicationLink_SetsDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetApplicationLink(new Uri("http://www.example.com"));

            Assert.Equal(1, sharePackage.SetDataCalls.Count);
            Assert.Equal(StandardDataFormats.ApplicationLink, sharePackage.SetDataCalls[0].Item1);
            Assert.Equal(new Uri("http://www.example.com"), sharePackage.SetDataCalls[0].Item2);
        }

        [Fact]
        public void SetApplicationLink_ThrowsException_IfSharePackageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => SharePackageEx.SetApplicationLink(null, new Uri("http://www.example.com")));
        }

        [Fact]
        public void SetHtmlFormat_SetsDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetHtmlFormat("Test Html");

            Assert.Equal(1, sharePackage.SetDataCalls.Count);
            Assert.Equal(StandardDataFormats.Html, sharePackage.SetDataCalls[0].Item1);
            Assert.Equal("Test Html", sharePackage.SetDataCalls[0].Item2);
        }

        [Fact]
        public void SetHtmlFormat_ThrowsException_IfSharePackageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => SharePackageEx.SetHtmlFormat(null, "Test Html"));
        }

        [Fact]
        public void SetRtf_SetsDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetRtf("Test Rtf");

            Assert.Equal(1, sharePackage.SetDataCalls.Count);
            Assert.Equal(StandardDataFormats.Rtf, sharePackage.SetDataCalls[0].Item1);
            Assert.Equal("Test Rtf", sharePackage.SetDataCalls[0].Item2);
        }

        [Fact]
        public void SetRtf_ThrowsException_IfSharePackageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => SharePackageEx.SetRtf(null, "Test Rtf"));
        }

        [Fact]
        public void SetText_SetsDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetText("Test Text");

            Assert.Equal(1, sharePackage.SetDataCalls.Count);
            Assert.Equal(StandardDataFormats.Text, sharePackage.SetDataCalls[0].Item1);
            Assert.Equal("Test Text", sharePackage.SetDataCalls[0].Item2);
        }

        [Fact]
        public void SetText_ThrowsException_IfSharePackageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => SharePackageEx.SetText(null, "Test Text"));
        }

        [Fact]
        public void SetWebLink_SetsDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetWebLink(new Uri("http://www.example.com"));

            Assert.Equal(1, sharePackage.SetDataCalls.Count);
            Assert.Equal(StandardDataFormats.WebLink, sharePackage.SetDataCalls[0].Item1);
            Assert.Equal(new Uri("http://www.example.com"), sharePackage.SetDataCalls[0].Item2);
        }

        [Fact]
        public void SetWebLink_ThrowsException_IfSharePackageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => SharePackageEx.SetWebLink(null, new Uri("http://www.example.com")));
        }

        [Fact]
        public async Task SetAsyncApplicationLink_SetsAsyncDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetAsyncApplicationLink(async (state) =>
            {
                await Task.Delay(200);
                return new Uri("http://www.example.com");
            });

            Assert.Equal(1, sharePackage.SetAsyncDataCalls.Count);
            Assert.Equal(StandardDataFormats.ApplicationLink, sharePackage.SetAsyncDataCalls[0].Item1);
            Assert.Equal(new Uri("http://www.example.com"), await sharePackage.SetAsyncDataCalls[0].Item2(""));
        }

        [Fact]
        public void SetAsyncApplicationLink_ThrowsException_IfSharePackageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => SharePackageEx.SetAsyncApplicationLink(null, async (state) =>
            {
                await Task.Delay(200);
                return new Uri("http://www.example.com");
            }));
        }

        [Fact]
        public async Task SetAsyncHtmlFormat_SetsAsyncDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetAsyncHtmlFormat(async (state) =>
            {
                await Task.Delay(200);
                return "Test Html";
            });

            Assert.Equal(1, sharePackage.SetAsyncDataCalls.Count);
            Assert.Equal(StandardDataFormats.Html, sharePackage.SetAsyncDataCalls[0].Item1);
            Assert.Equal("Test Html", await sharePackage.SetAsyncDataCalls[0].Item2(""));
        }

        [Fact]
        public void SetAsyncHtmlFormat_ThrowsException_IfSharePackageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => SharePackageEx.SetAsyncHtmlFormat(null, async (state) =>
            {
                await Task.Delay(200);
                return "Test Html";
            }));
        }

        [Fact]
        public async Task SetAsyncRtf_SetsAsyncDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetAsyncRtf(async (state) =>
            {
                await Task.Delay(200);
                return "Test Rtf";
            });

            Assert.Equal(1, sharePackage.SetAsyncDataCalls.Count);
            Assert.Equal(StandardDataFormats.Rtf, sharePackage.SetAsyncDataCalls[0].Item1);
            Assert.Equal("Test Rtf", await sharePackage.SetAsyncDataCalls[0].Item2(""));
        }

        [Fact]
        public void SetAsyncRtf_ThrowsException_IfSharePackageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => SharePackageEx.SetAsyncRtf(null, async (state) =>
            {
                await Task.Delay(200);
                return "Test Rtf";
            }));
        }

        [Fact]
        public async Task SetAsyncText_SetsAsyncDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetAsyncText(async (state) =>
            {
                await Task.Delay(200);
                return "Test Text";
            });

            Assert.Equal(1, sharePackage.SetAsyncDataCalls.Count);
            Assert.Equal(StandardDataFormats.Text, sharePackage.SetAsyncDataCalls[0].Item1);
            Assert.Equal("Test Text", await sharePackage.SetAsyncDataCalls[0].Item2(""));
        }

        [Fact]
        public void SetAsyncText_ThrowsException_IfSharePackageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => SharePackageEx.SetAsyncText(null, async (state) =>
            {
                await Task.Delay(200);
                return "Test Text";
            }));
        }

        [Fact]
        public async Task SetAsyncWebLink_SetsAsyncDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetAsyncWebLink(async (state) =>
            {
                await Task.Delay(200);
                return new Uri("http://www.example.com");
            });

            Assert.Equal(1, sharePackage.SetAsyncDataCalls.Count);
            Assert.Equal(StandardDataFormats.WebLink, sharePackage.SetAsyncDataCalls[0].Item1);
            Assert.Equal(new Uri("http://www.example.com"), await sharePackage.SetAsyncDataCalls[0].Item2(""));
        }

        [Fact]
        public void SetAsyncWebLink_ThrowsException_IfSharePackageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => SharePackageEx.SetAsyncWebLink(null, async (state) =>
            {
                await Task.Delay(200);
                return new Uri("http://www.example.com");
            }));
        }

        // *** Private sub-classes ***

        private class MockSharePackage : ISharePackage
        {
            public List<Tuple<string, object>> SetDataCalls = new List<Tuple<string, object>>();
            public List<Tuple<string, AsyncDataProvider<object>>> SetAsyncDataCalls = new List<Tuple<string, AsyncDataProvider<object>>>();

            public ISharePropertySet Properties
            {
                get { throw new NotImplementedException(); }
            }

            public void SetData<T>(string formatId, T value)
            {
                SetDataCalls.Add(Tuple.Create<string, object>(formatId, value));
            }

            public void SetAsyncData<T>(string formatId, AsyncDataProvider<T> dataProvider)
            {
                SetAsyncDataCalls.Add(Tuple.Create<string, AsyncDataProvider<object>>(formatId, async (f) => (object)await dataProvider(f)));
            }
        }
    }
}

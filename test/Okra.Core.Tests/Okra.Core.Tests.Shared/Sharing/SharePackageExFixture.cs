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
    public class SharePackageExFixture
    {
        [TestMethod]
        public void SetApplicationLink_SetsDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetApplicationLink(new Uri("http://www.example.com"));

            Assert.AreEqual(1, sharePackage.SetDataCalls.Count);
            Assert.AreEqual(StandardDataFormats.ApplicationLink, sharePackage.SetDataCalls[0].Item1);
            Assert.AreEqual(new Uri("http://www.example.com"), sharePackage.SetDataCalls[0].Item2);
        }

        [TestMethod]
        public void SetApplicationLink_ThrowsException_IfSharePackageIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SharePackageEx.SetApplicationLink(null, new Uri("http://www.example.com")));
        }

        [TestMethod]
        public void SetHtmlFormat_SetsDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetHtmlFormat("Test Html");

            Assert.AreEqual(1, sharePackage.SetDataCalls.Count);
            Assert.AreEqual(StandardDataFormats.Html, sharePackage.SetDataCalls[0].Item1);
            Assert.AreEqual("Test Html", sharePackage.SetDataCalls[0].Item2);
        }

        [TestMethod]
        public void SetHtmlFormat_ThrowsException_IfSharePackageIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SharePackageEx.SetHtmlFormat(null, "Test Html"));
        }

        [TestMethod]
        public void SetRtf_SetsDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetRtf("Test Rtf");

            Assert.AreEqual(1, sharePackage.SetDataCalls.Count);
            Assert.AreEqual(StandardDataFormats.Rtf, sharePackage.SetDataCalls[0].Item1);
            Assert.AreEqual("Test Rtf", sharePackage.SetDataCalls[0].Item2);
        }

        [TestMethod]
        public void SetRtf_ThrowsException_IfSharePackageIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SharePackageEx.SetRtf(null, "Test Rtf"));
        }

        [TestMethod]
        public void SetText_SetsDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetText("Test Text");

            Assert.AreEqual(1, sharePackage.SetDataCalls.Count);
            Assert.AreEqual(StandardDataFormats.Text, sharePackage.SetDataCalls[0].Item1);
            Assert.AreEqual("Test Text", sharePackage.SetDataCalls[0].Item2);
        }

        [TestMethod]
        public void SetText_ThrowsException_IfSharePackageIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SharePackageEx.SetText(null, "Test Text"));
        }

        [TestMethod]
        public void SetWebLink_SetsDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetWebLink(new Uri("http://www.example.com"));

            Assert.AreEqual(1, sharePackage.SetDataCalls.Count);
            Assert.AreEqual(StandardDataFormats.WebLink, sharePackage.SetDataCalls[0].Item1);
            Assert.AreEqual(new Uri("http://www.example.com"), sharePackage.SetDataCalls[0].Item2);
        }

        [TestMethod]
        public void SetWebLink_ThrowsException_IfSharePackageIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SharePackageEx.SetWebLink(null, new Uri("http://www.example.com")));
        }

        [TestMethod]
        public async Task SetAsyncApplicationLink_SetsAsyncDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetAsyncApplicationLink(async (state) =>
            {
                await Task.Delay(200);
                return new Uri("http://www.example.com");
            });

            Assert.AreEqual(1, sharePackage.SetAsyncDataCalls.Count);
            Assert.AreEqual(StandardDataFormats.ApplicationLink, sharePackage.SetAsyncDataCalls[0].Item1);
            Assert.AreEqual(new Uri("http://www.example.com"), await sharePackage.SetAsyncDataCalls[0].Item2(""));
        }

        [TestMethod]
        public void SetAsyncApplicationLink_ThrowsException_IfSharePackageIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SharePackageEx.SetAsyncApplicationLink(null, async (state) =>
            {
                await Task.Delay(200);
                return new Uri("http://www.example.com");
            }));
        }

        [TestMethod]
        public async Task SetAsyncHtmlFormat_SetsAsyncDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetAsyncHtmlFormat(async (state) =>
            {
                await Task.Delay(200);
                return "Test Html";
            });

            Assert.AreEqual(1, sharePackage.SetAsyncDataCalls.Count);
            Assert.AreEqual(StandardDataFormats.Html, sharePackage.SetAsyncDataCalls[0].Item1);
            Assert.AreEqual("Test Html", await sharePackage.SetAsyncDataCalls[0].Item2(""));
        }

        [TestMethod]
        public void SetAsyncHtmlFormat_ThrowsException_IfSharePackageIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SharePackageEx.SetAsyncHtmlFormat(null, async (state) =>
            {
                await Task.Delay(200);
                return "Test Html";
            }));
        }

        [TestMethod]
        public async Task SetAsyncRtf_SetsAsyncDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetAsyncRtf(async (state) =>
            {
                await Task.Delay(200);
                return "Test Rtf";
            });

            Assert.AreEqual(1, sharePackage.SetAsyncDataCalls.Count);
            Assert.AreEqual(StandardDataFormats.Rtf, sharePackage.SetAsyncDataCalls[0].Item1);
            Assert.AreEqual("Test Rtf", await sharePackage.SetAsyncDataCalls[0].Item2(""));
        }

        [TestMethod]
        public void SetAsyncRtf_ThrowsException_IfSharePackageIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SharePackageEx.SetAsyncRtf(null, async (state) =>
            {
                await Task.Delay(200);
                return "Test Rtf";
            }));
        }

        [TestMethod]
        public async Task SetAsyncText_SetsAsyncDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetAsyncText(async (state) =>
            {
                await Task.Delay(200);
                return "Test Text";
            });

            Assert.AreEqual(1, sharePackage.SetAsyncDataCalls.Count);
            Assert.AreEqual(StandardDataFormats.Text, sharePackage.SetAsyncDataCalls[0].Item1);
            Assert.AreEqual("Test Text", await sharePackage.SetAsyncDataCalls[0].Item2(""));
        }

        [TestMethod]
        public void SetAsyncText_ThrowsException_IfSharePackageIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SharePackageEx.SetAsyncText(null, async (state) =>
            {
                await Task.Delay(200);
                return "Test Text";
            }));
        }

        [TestMethod]
        public async Task SetAsyncWebLink_SetsAsyncDataOnSharePackage()
        {
            MockSharePackage sharePackage = new MockSharePackage();

            sharePackage.SetAsyncWebLink(async (state) =>
            {
                await Task.Delay(200);
                return new Uri("http://www.example.com");
            });

            Assert.AreEqual(1, sharePackage.SetAsyncDataCalls.Count);
            Assert.AreEqual(StandardDataFormats.WebLink, sharePackage.SetAsyncDataCalls[0].Item1);
            Assert.AreEqual(new Uri("http://www.example.com"), await sharePackage.SetAsyncDataCalls[0].Item2(""));
        }

        [TestMethod]
        public void SetAsyncWebLink_ThrowsException_IfSharePackageIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SharePackageEx.SetAsyncWebLink(null, async (state) =>
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

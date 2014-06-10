using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Okra.DataTransfer;
using Windows.Storage.Streams;
using System.IO;
using Windows.Storage;

namespace Okra.Tests.DataTransfer
{
    [TestClass]
    public class DataPackageExFixture
    {
        // *** Static Method Tests ***

        [TestMethod]
        public async Task SetAsyncDataProvider_ReturnsValueFromAsyncTask()
        {
            DataPackage dataPackage = new DataPackage();

            dataPackage.SetAsyncDataProvider(StandardDataFormats.Text, async (formatId, deadline) =>
                {
                    await Task.Delay(200);
                    return "Test text";
                });

            DataPackageView dataPackageView = dataPackage.GetView();
            string result = await dataPackageView.GetTextAsync();

            Assert.AreEqual("Test text", result);
        }

        [TestMethod]
        public async Task SetAsyncDataProvider_ReturnsValueFromSyncTask()
        {
            DataPackage dataPackage = new DataPackage();

            dataPackage.SetAsyncDataProvider(StandardDataFormats.Text, (formatId, deadline) =>
            {
                return Task.FromResult<object>("Test text");
            });

            DataPackageView dataPackageView = dataPackage.GetView();
            string result = await dataPackageView.GetTextAsync();

            Assert.AreEqual("Test text", result);
        }

        [TestMethod]
        public async Task SetAsyncDataProvider_PassesFormatIdToMethod()
        {
            DataPackage dataPackage = new DataPackage();

            string formatIdValue = null;

            dataPackage.SetAsyncDataProvider(StandardDataFormats.Text, (formatId, deadline) =>
            {
                formatIdValue = formatId;
                return Task.FromResult<object>("Test text");
            });

            DataPackageView dataPackageView = dataPackage.GetView();
            string result = await dataPackageView.GetTextAsync();

            Assert.AreEqual(StandardDataFormats.Text, formatIdValue);
        }

        [TestMethod]
        public async Task SetAsyncDataProvider_PassesDeadlineToMethod()
        {
            DataPackage dataPackage = new DataPackage();

            DateTimeOffset deadlineValue = DateTimeOffset.MinValue;

            dataPackage.SetAsyncDataProvider(StandardDataFormats.Text, (formatId, deadline) =>
            {
                deadlineValue = deadline;
                return Task.FromResult<object>("Test text");
            });

            DataPackageView dataPackageView = dataPackage.GetView();
            string result = await dataPackageView.GetTextAsync();

            Assert.AreNotEqual(DateTimeOffset.MinValue, deadlineValue);
        }

        [TestMethod]
        public async Task SetAsyncApplicationLink_ReturnsValueFromAsyncTask()
        {
            DataPackage dataPackage = new DataPackage();

            dataPackage.SetAsyncApplicationLink(async (formatId, deadline) =>
            {
                await Task.Delay(200);
                return new Uri("http://www.example.org/");
            });

            DataPackageView dataPackageView = dataPackage.GetView();
            Uri result = await dataPackageView.GetApplicationLinkAsync();

            Assert.AreEqual("http://www.example.org/", result.AbsoluteUri);
        }

        [TestMethod]
        public async Task SetAsyncBitmap_ReturnsValueFromAsyncTask()
        {
            DataPackage dataPackage = new DataPackage();
            RandomAccessStreamReference streamRef = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Images/UnitTestLogo.png"));

            dataPackage.SetAsyncBitmap(async (formatId, deadline) =>
            {
                await Task.Delay(200);
                return streamRef;
            });

            DataPackageView dataPackageView = dataPackage.GetView();
            RandomAccessStreamReference result = await dataPackageView.GetBitmapAsync();

            Assert.AreEqual(streamRef, result);
        }

        [TestMethod]
        public async Task SetAsyncHtmlFormat_ReturnsValueFromAsyncTask()
        {
            DataPackage dataPackage = new DataPackage();

            dataPackage.SetAsyncHtmlFormat(async (formatId, deadline) =>
            {
                await Task.Delay(200);
                return "Test HTML";
            });

            DataPackageView dataPackageView = dataPackage.GetView();
            string result = await dataPackageView.GetHtmlFormatAsync();

            Assert.AreEqual("Test HTML", result);
        }

        [TestMethod]
        public async Task SetAsyncRtf_ReturnsValueFromAsyncTask()
        {
            DataPackage dataPackage = new DataPackage();

            dataPackage.SetAsyncRtf(async (formatId, deadline) =>
            {
                await Task.Delay(200);
                return "Test RTF";
            });

            DataPackageView dataPackageView = dataPackage.GetView();
            string result = await dataPackageView.GetRtfAsync();

            Assert.AreEqual("Test RTF", result);
        }

        [TestMethod]
        public async Task SetAsyncStorageItems_ReturnsValueFromAsyncTask()
        {
            DataPackage dataPackage = new DataPackage();
            StorageFile file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("Test file.xml");

            dataPackage.SetAsyncStorageItems(async (formatId, deadline) =>
            {
                await Task.Delay(200);
                return new IStorageItem[] { file };
            });

            DataPackageView dataPackageView = dataPackage.GetView();
            IReadOnlyList<IStorageItem> result = await dataPackageView.GetStorageItemsAsync();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(file.Path, result[0].Path);
        }

        [TestMethod]
        public async Task SetAsyncText_ReturnsValueFromAsyncTask()
        {
            DataPackage dataPackage = new DataPackage();

            dataPackage.SetAsyncText(async (formatId, deadline) =>
            {
                await Task.Delay(200);
                return "Test text";
            });

            DataPackageView dataPackageView = dataPackage.GetView();
            string result = await dataPackageView.GetTextAsync();

            Assert.AreEqual("Test text", result);
        }

        [TestMethod]
        public async Task SetAsyncUri_ReturnsValueFromAsyncTask()
        {
            DataPackage dataPackage = new DataPackage();

            dataPackage.SetAsyncUri(async (formatId, deadline) =>
            {
                await Task.Delay(200);
                return new Uri("http://www.example.org/");
            });

            DataPackageView dataPackageView = dataPackage.GetView();
            Uri result = await dataPackageView.GetUriAsync();

            Assert.AreEqual("http://www.example.org/", result.AbsoluteUri);
        }

        [TestMethod]
        public async Task SetAsyncWebLink_ReturnsValueFromAsyncTask()
        {
            DataPackage dataPackage = new DataPackage();

            dataPackage.SetAsyncWebLink(async (formatId, deadline) =>
            {
                await Task.Delay(200);
                return new Uri("http://www.example.org/");
            });

            DataPackageView dataPackageView = dataPackage.GetView();
            Uri result = await dataPackageView.GetWebLinkAsync();

            Assert.AreEqual("http://www.example.org/", result.AbsoluteUri);
        }
    }
}

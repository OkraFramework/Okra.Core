using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Okra.Sharing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using System.Threading.Tasks;

namespace Okra.Tests.Sharing
{
    [TestClass]
    public class SharePackageViewFixture
    {
        [TestMethod]
        public void AvailableFormats_ReturnsValuesFromDataPackageView()
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetData("Format A", "Some data");
            dataPackage.SetData("Format B", "Some data");

            SharePackageView sharePackage = new SharePackageView(dataPackage.GetView());

            Assert.AreEqual(2, sharePackage.AvailableFormats.Count);
            CollectionAssert.Contains(sharePackage.AvailableFormats.ToList(), "Format A");
            CollectionAssert.Contains(sharePackage.AvailableFormats.ToList(), "Format B");
        }

        [TestMethod]
        public void Properties_Description_GetsValueFromDataPackageView()
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.Properties.Description = "Test Value";

            SharePackageView sharePackage = new SharePackageView(dataPackage.GetView());

            Assert.AreEqual("Test Value", sharePackage.Properties.Description);
        }

        [TestMethod]
        public void Properties_Title_GetsValueFromDataPackageView()
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.Properties.Title = "Test Value";

            SharePackageView sharePackage = new SharePackageView(dataPackage.GetView());

            Assert.AreEqual("Test Value", sharePackage.Properties.Title);
        }

        [TestMethod]
        public void Properties_Description_SettingValueThrowsException()
        {
            DataPackage dataPackage = new DataPackage();
            SharePackageView sharePackage = new SharePackageView(dataPackage.GetView());

            Assert.ThrowsException<InvalidOperationException>(() => sharePackage.Properties.Description = "Test Value");
        }

        [TestMethod]
        public void Properties_Title_SettingValueThrowsException()
        {
            DataPackage dataPackage = new DataPackage();
            SharePackageView sharePackage = new SharePackageView(dataPackage.GetView());

            Assert.ThrowsException<InvalidOperationException>(() => sharePackage.Properties.Description = "Test Value");
        }

        [TestMethod]
        public void Contains_ReturnsTrueIfFormatIsAvailable()
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetData("Format A", "Some data");
            dataPackage.SetData("Format B", "Some data");

            SharePackageView sharePackage = new SharePackageView(dataPackage.GetView());

            Assert.IsTrue(sharePackage.Contains("Format A"));
        }

        [TestMethod]
        public void Contains_ReturnsFalseIfFormatIsAvailable()
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetData("Format A", "Some data");
            dataPackage.SetData("Format B", "Some data");

            SharePackageView sharePackage = new SharePackageView(dataPackage.GetView());

            Assert.IsFalse(sharePackage.Contains("Format C"));
        }

        [TestMethod]
        public async Task GetDataAsync_GetsDataFromDataPackage()
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetData("Test Format", "Test Value");

            SharePackageView sharePackageView = new SharePackageView(dataPackage.GetView());
            string data = await sharePackageView.GetDataAsync<string>("Test Format");

            Assert.AreEqual("Test Value", data);
        }
    }
}

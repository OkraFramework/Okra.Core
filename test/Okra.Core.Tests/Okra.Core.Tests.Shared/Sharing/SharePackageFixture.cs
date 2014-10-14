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
    public class SharePackageFixture
    {
        [TestMethod]
        public void Constructor_ThrowsException_IfDataPackageIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new SharePackage(null));
        }

        [TestMethod]
        public async Task SetData_SetsDataOnDataPackage()
        {
            DataPackage dataPackage = new DataPackage();
            SharePackage sharePackage = new SharePackage(dataPackage);

            sharePackage.SetData<string>("Test Format", "Test Value");

            object data = await dataPackage.GetView().GetDataAsync("Test Format");
            Assert.AreEqual("Test Value", data);
        }

        [TestMethod]
        public void SetData_ThrowsException_IfFormatIdIsNull()
        {
            DataPackage dataPackage = new DataPackage();
            SharePackage sharePackage = new SharePackage(dataPackage);

            Assert.ThrowsException<ArgumentException>(() => sharePackage.SetData<string>(null, "Test Value"));
        }

        [TestMethod]
        public void SetData_ThrowsException_IfFormatIdIsEmpty()
        {
            DataPackage dataPackage = new DataPackage();
            SharePackage sharePackage = new SharePackage(dataPackage);

            Assert.ThrowsException<ArgumentException>(() => sharePackage.SetData<string>("", "Test Value"));
        }

        [TestMethod]
        public async Task SetAsyncData_SetsDataOnDataPackage()
        {
            DataPackage dataPackage = new DataPackage();
            SharePackage sharePackage = new SharePackage(dataPackage);

            sharePackage.SetAsyncData("Test Format", async (state) =>
                {
                    await Task.Delay(200);
                    return "Test Value";
                });

            object data = await dataPackage.GetView().GetDataAsync("Test Format");
            Assert.AreEqual("Test Value", data);
        }

        [TestMethod]
        public void SetAsyncData_ThrowsException_IfFormatIdIsNull()
        {
            DataPackage dataPackage = new DataPackage();
            SharePackage sharePackage = new SharePackage(dataPackage);

            Assert.ThrowsException<ArgumentException>(() => sharePackage.SetAsyncData<string>(null, async (state) =>
            {
                await Task.Delay(200);
                return "Test Value";
            }));
        }

        [TestMethod]
        public void SetAsyncData_ThrowsException_IfFormatIdIsEmpty()
        {
            DataPackage dataPackage = new DataPackage();
            SharePackage sharePackage = new SharePackage(dataPackage);

            Assert.ThrowsException<ArgumentException>(() => sharePackage.SetAsyncData<string>("", async (state) =>
            {
                await Task.Delay(200);
                return "Test Value";
            }));
        }

        [TestMethod]
        public void SetAsyncData_ThrowsException_IfDataProviderIsNull()
        {
            DataPackage dataPackage = new DataPackage();
            SharePackage sharePackage = new SharePackage(dataPackage);

            Assert.ThrowsException<ArgumentNullException>(() => sharePackage.SetAsyncData<string>("Test Format", null));
        }

        [TestMethod]
        public void Properties_Description_SetsValueOnDataPackage()
        {
            DataPackage dataPackage = new DataPackage();
            SharePackage sharePackage = new SharePackage(dataPackage);

            sharePackage.Properties.Description = "Test Value";

            Assert.AreEqual("Test Value", dataPackage.Properties.Description);
        }

        [TestMethod]
        public void Properties_Description_GetsValueFromDataPackage()
        {
            DataPackage dataPackage = new DataPackage();
            SharePackage sharePackage = new SharePackage(dataPackage);

            dataPackage.Properties.Description = "Test Value";

            Assert.AreEqual("Test Value", sharePackage.Properties.Description);
        }

        [TestMethod]
        public void Properties_Title_SetsValueOnDataPackage()
        {
            DataPackage dataPackage = new DataPackage();
            SharePackage sharePackage = new SharePackage(dataPackage);

            sharePackage.Properties.Title = "Test Value";

            Assert.AreEqual("Test Value", dataPackage.Properties.Title);
        }

        [TestMethod]
        public void Properties_Title_GetsValueFromDataPackage()
        {
            DataPackage dataPackage = new DataPackage();
            SharePackage sharePackage = new SharePackage(dataPackage);

            dataPackage.Properties.Title = "Test Value";

            Assert.AreEqual("Test Value", sharePackage.Properties.Title);
        }
    }
}

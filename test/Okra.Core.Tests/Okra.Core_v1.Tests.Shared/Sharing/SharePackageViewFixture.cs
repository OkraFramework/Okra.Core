using Okra.Sharing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using System.Threading.Tasks;
using Xunit;

namespace Okra.Tests.Sharing
{
    public class SharePackageViewFixture
    {
        [Fact]
        public void Constructor_ThrowsException_IfDataPackageViewIsNull()
        {
            var e = Assert.Throws<ArgumentNullException>(() => new SharePackageView(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: dataPackageView", e.Message);
            Assert.Equal("dataPackageView", e.ParamName);
        }

        [Fact]
        public void AvailableFormats_ReturnsValuesFromDataPackageView()
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetData("Format A", "Some data");
            dataPackage.SetData("Format B", "Some data");

            SharePackageView sharePackage = new SharePackageView(dataPackage.GetView());

            Assert.Equal(2, sharePackage.AvailableFormats.Count);
            Assert.Contains("Format A", sharePackage.AvailableFormats.ToList());
            Assert.Contains("Format B", sharePackage.AvailableFormats.ToList());
        }

        [Fact]
        public void Properties_Description_GetsValueFromDataPackageView()
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.Properties.Description = "Test Value";

            SharePackageView sharePackage = new SharePackageView(dataPackage.GetView());

            Assert.Equal("Test Value", sharePackage.Properties.Description);
        }

        [Fact]
        public void Properties_Title_GetsValueFromDataPackageView()
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.Properties.Title = "Test Value";

            SharePackageView sharePackage = new SharePackageView(dataPackage.GetView());

            Assert.Equal("Test Value", sharePackage.Properties.Title);
        }

        [Fact]
        public void Properties_Description_SettingValueThrowsException()
        {
            DataPackage dataPackage = new DataPackage();
            SharePackageView sharePackage = new SharePackageView(dataPackage.GetView());

            var e = Assert.Throws<InvalidOperationException>(() => sharePackage.Properties.Description = "Test Value");

            Assert.Equal("Cannot modify share properties as a share target.", e.Message);
        }

        [Fact]
        public void Properties_Title_SettingValueThrowsException()
        {
            DataPackage dataPackage = new DataPackage();
            SharePackageView sharePackage = new SharePackageView(dataPackage.GetView());

            var e = Assert.Throws<InvalidOperationException>(() => sharePackage.Properties.Description = "Test Value");

            Assert.Equal("Cannot modify share properties as a share target.", e.Message);
        }

        [Fact]
        public void Contains_ReturnsTrueIfFormatIsAvailable()
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetData("Format A", "Some data");
            dataPackage.SetData("Format B", "Some data");

            SharePackageView sharePackage = new SharePackageView(dataPackage.GetView());

            Assert.True(sharePackage.Contains("Format A"));
        }

        [Fact]
        public void Contains_ReturnsFalseIfFormatIsNotAvailable()
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetData("Format A", "Some data");
            dataPackage.SetData("Format B", "Some data");

            SharePackageView sharePackage = new SharePackageView(dataPackage.GetView());

            Assert.False(sharePackage.Contains("Format C"));
        }

        [Fact]
        public void Contains_ThrowsException_IfFormatIdIsNull()
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetData("Format A", "Some data");
            dataPackage.SetData("Format B", "Some data");

            SharePackageView sharePackage = new SharePackageView(dataPackage.GetView());

            var e = Assert.Throws<ArgumentException>(() => sharePackage.Contains(null));

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: formatId", e.Message);
            Assert.Equal("formatId", e.ParamName);
        }

        [Fact]
        public void Contains_ThrowsException_IfFormatIdIsEmpty()
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetData("Format A", "Some data");
            dataPackage.SetData("Format B", "Some data");

            SharePackageView sharePackage = new SharePackageView(dataPackage.GetView());

            var e = Assert.Throws<ArgumentException>(() => sharePackage.Contains(""));

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: formatId", e.Message);
            Assert.Equal("formatId", e.ParamName);
        }

        [Fact]
        public async Task GetDataAsync_GetsDataFromDataPackage()
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetData("Test Format", "Test Value");

            SharePackageView sharePackageView = new SharePackageView(dataPackage.GetView());
            string data = await sharePackageView.GetDataAsync<string>("Test Format");

            Assert.Equal("Test Value", data);
        }

        [Fact]
        public async void GetDataAsync_ThrowsException_IfFormatIdIsNull()
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetData("Format A", "Some data");
            dataPackage.SetData("Format B", "Some data");

            SharePackageView sharePackage = new SharePackageView(dataPackage.GetView());

            var e = await Assert.ThrowsAsync<ArgumentException>(() => sharePackage.GetDataAsync<string>(null));

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: formatId", e.Message);
            Assert.Equal("formatId", e.ParamName);
        }

        [Fact]
        public async void GetDataAsync_ThrowsException_IfFormatIdIsEmpty()
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetData("Format A", "Some data");
            dataPackage.SetData("Format B", "Some data");

            SharePackageView sharePackage = new SharePackageView(dataPackage.GetView());

            var e = await Assert.ThrowsAsync<ArgumentException>(() => sharePackage.GetDataAsync<string>(""));

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: formatId", e.Message);
            Assert.Equal("formatId", e.ParamName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Okra.Services;
using Okra.Tests.Helpers;
using Windows.Storage;
using Xunit;

namespace Okra.Tests.Services
{
    public class StorageManagerFixture
    {
        // *** Method Tests ***

        [Fact]
        public async Task StoreAsync_WithStorageFile_ThrowsException_IfFileIsNull()
        {
            StorageManager storageManager = new StorageManager();

            TestData data = new TestData() { Text = "Test Text", Number = 42 };
            var e = await Assert.ThrowsAsync<ArgumentNullException>(() => storageManager.StoreAsync(null, data));

            Assert.Equal("Value cannot be null.\r\nParameter name: file", e.Message);
            Assert.Equal("file", e.ParamName);
        }

        [Fact]
        public async Task StoreAsync_WithStorageFolder_ThrowsException_IfFolderIsNull()
        {
            StorageManager storageManager = new StorageManager();

            StorageFolder folder = null;
            string name = GetTestFilename();
            TestData data = new TestData() { Text = "Test Text", Number = 42 };
            var e = await Assert.ThrowsAsync<ArgumentNullException>(() => storageManager.StoreAsync(folder, name, data));

            Assert.Equal("Value cannot be null.\r\nParameter name: folder", e.Message);
            Assert.Equal("folder", e.ParamName);
        }

        [Fact]
        public async Task StoreAsync_WithStorageFolder_ThrowsException_IfNameIsNull()
        {
            StorageManager storageManager = new StorageManager();

            StorageFolder folder = ApplicationData.Current.TemporaryFolder;
            TestData data = new TestData() { Text = "Test Text", Number = 42 };
            var e = await Assert.ThrowsAsync<ArgumentException>(() => storageManager.StoreAsync(folder, null, data));

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: name", e.Message);
            Assert.Equal("name", e.ParamName);
        }

        [Fact]
        public async Task StoreAsync_WithStorageFolder_ThrowsException_IfNameIsEmpty()
        {
            StorageManager storageManager = new StorageManager();

            StorageFolder folder = ApplicationData.Current.TemporaryFolder;
            TestData data = new TestData() { Text = "Test Text", Number = 42 };
            var e = await Assert.ThrowsAsync<ArgumentException>(() => storageManager.StoreAsync(folder, "", data));

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: name", e.Message);
            Assert.Equal("name", e.ParamName);
        }

        [Fact]
        public async Task RetrieveAsync_WithStorageFolder_ReturnsNullIfFileDoesNotExist()
        {
            StorageManager storageManager = new StorageManager();

            StorageFolder folder = ApplicationData.Current.TemporaryFolder;
            string name = GetTestFilename();

            TestData retrievedData = await storageManager.RetrieveAsync<TestData>(folder, name);

            Assert.Null(retrievedData);
        }

        [Fact]
        public async Task RetrieveAsync_WithStorageFile_ThrowsException_IfFileIsNull()
        {
            StorageManager storageManager = new StorageManager();

            var e = await Assert.ThrowsAsync<ArgumentNullException>(() => storageManager.RetrieveAsync<TestData>(null));

            Assert.Equal("Value cannot be null.\r\nParameter name: file", e.Message);
            Assert.Equal("file", e.ParamName);
        }

        [Fact]
        public async Task RetrieveAsync_WithStorageFolder_ThrowsException_IfFolderIsNull()
        {
            StorageManager storageManager = new StorageManager();

            StorageFolder folder = null;
            string name = GetTestFilename();
            var e = await Assert.ThrowsAsync<ArgumentNullException>(() => storageManager.RetrieveAsync<TestData>(folder, name));

            Assert.Equal("Value cannot be null.\r\nParameter name: folder", e.Message);
            Assert.Equal("folder", e.ParamName);
        }

        [Fact]
        public async Task RetrieveAsync_WithStorageFolder_ThrowsException_IfNameIsNull()
        {
            StorageManager storageManager = new StorageManager();

            StorageFolder folder = ApplicationData.Current.TemporaryFolder;
            var e = await Assert.ThrowsAsync<ArgumentException>(() => storageManager.RetrieveAsync<TestData>(folder, null));

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: name", e.Message);
            Assert.Equal("name", e.ParamName);
        }

        [Fact]
        public async Task RetrieveAsync_WithStorageFolder_ThrowsException_IfNameIsEmpty()
        {
            StorageManager storageManager = new StorageManager();

            StorageFolder folder = ApplicationData.Current.TemporaryFolder;
            var e = await Assert.ThrowsAsync<ArgumentException>(() => storageManager.RetrieveAsync<TestData>(folder, ""));

            Assert.Equal("The argument cannot be null or an empty string.\r\nParameter name: name", e.Message);
            Assert.Equal("name", e.ParamName);
        }

        [Fact]
        public async Task StoreAsyncRetrieveAsync_WithStorageFile_PersistsFileViaStorage()
        {
            StorageManager storageManager = new StorageManager();
            StorageFolder folder = ApplicationData.Current.TemporaryFolder;
            StorageFile file = await folder.CreateFileAsync(GetTestFilename(), CreationCollisionOption.ReplaceExisting);

            // Store some test data

            TestData data = new TestData() { Text = "Test Text", Number = 42 };
            await storageManager.StoreAsync(file, data);

            // Retrieve and validate the data from the file

            TestData retrievedData = await storageManager.RetrieveAsync<TestData>(file);

            Assert.Equal("Test Text", retrievedData.Text);
            Assert.Equal(42, retrievedData.Number);
        }

        //[TestMethod]
        //public async Task StoreAsyncRetrieveAsync_WithStorageFolder_PersistsFileViaStorage()
        //{
        //    StorageManager storageManager = new StorageManager();
        //    StorageFolder folder = ApplicationData.Current.TemporaryFolder;
        //    string name = GetTestFilename();

        //    // Store some test data

        //    TestData data = new TestData() { Text = "Test Text", Number = 42 };
        //    await storageManager.StoreAsync(folder, name, data);

        //    // Retrieve and validate the data from the file

        //    TestData retrievedData = await storageManager.RetrieveAsync<TestData>(folder, name);

        //    Assert.AreEqual("Test Text", retrievedData.Text);
        //    Assert.AreEqual(42, retrievedData.Number);
        //}

        //[TestMethod]
        //public async Task StoreAsyncRetrieveAsync_WithStorageFile_PersistsNull()
        //{
        //    StorageManager storageManager = new StorageManager();
        //    StorageFolder folder = ApplicationData.Current.TemporaryFolder;
        //    StorageFile file = await folder.CreateFileAsync(GetTestFilename(), CreationCollisionOption.ReplaceExisting);

        //    // Store some test data

        //    TestData data = null;
        //    await storageManager.StoreAsync(file, data);

        //    // Retrieve and validate the data from the file

        //    TestData retrievedData = await storageManager.RetrieveAsync<TestData>(file);

        //    Assert.IsNull(retrievedData);
        //}

        //[TestMethod]
        //public async Task StoreAsyncRetrieveAsync_WithStorageFolder_PersistsNull()
        //{
        //    StorageManager storageManager = new StorageManager();
        //    StorageFolder folder = ApplicationData.Current.TemporaryFolder;
        //    string name = GetTestFilename();

        //    // Store some test data

        //    TestData data = null;
        //    await storageManager.StoreAsync(folder, name, data);

        //    // Retrieve and validate the data from the file

        //    TestData retrievedData = await storageManager.RetrieveAsync<TestData>(folder, name);

        //    Assert.IsNull(retrievedData);
        //}

        //[TestMethod]
        //public async Task StoreAsyncRetrieveAsync_WithStorageFile_PersistsNullOverData()
        //{
        //    StorageManager storageManager = new StorageManager();
        //    StorageFolder folder = ApplicationData.Current.TemporaryFolder;
        //    StorageFile file = await folder.CreateFileAsync(GetTestFilename(), CreationCollisionOption.ReplaceExisting);

        //    // Store some test data

        //    TestData oldData = new TestData() { Text = "Test Text", Number = 42 };
        //    await storageManager.StoreAsync(file, oldData);

        //    // Overwrite test data

        //    TestData data = null;
        //    await storageManager.StoreAsync(file, data);

        //    // Retrieve and validate the data from the file

        //    TestData retrievedData = await storageManager.RetrieveAsync<TestData>(file);

        //    Assert.IsNull(retrievedData);
        //}

        //[TestMethod]
        //public async Task StoreAsyncRetrieveAsync_WithStorageFolder_PersistsNullOverData()
        //{
        //    StorageManager storageManager = new StorageManager();
        //    StorageFolder folder = ApplicationData.Current.TemporaryFolder;
        //    string name = GetTestFilename();

        //    // Store some test data

        //    TestData oldData = new TestData() { Text = "Test Text", Number = 42 };
        //    await storageManager.StoreAsync(folder, name, oldData);

        //    // Overwrite test data

        //    TestData data = null;
        //    await storageManager.StoreAsync(folder, name, data);

        //    // Retrieve and validate the data from the file

        //    TestData retrievedData = await storageManager.RetrieveAsync<TestData>(folder, name);

        //    Assert.IsNull(retrievedData);
        //}

        //[TestMethod]
        //public async Task StoreAsyncRetrieveAsync_WithStorageFile_PersistsDataOverNull()
        //{
        //    StorageManager storageManager = new StorageManager();
        //    StorageFolder folder = ApplicationData.Current.TemporaryFolder;
        //    StorageFile file = await folder.CreateFileAsync(GetTestFilename(), CreationCollisionOption.ReplaceExisting);

        //    // Store some test data

        //    TestData oldData = null;
        //    await storageManager.StoreAsync(file, oldData);

        //    // Overwrite test data

        //    TestData data = new TestData() { Text = "Test Text", Number = 42 };
        //    await storageManager.StoreAsync(file, data);

        //    // Retrieve and validate the data from the file

        //    TestData retrievedData = await storageManager.RetrieveAsync<TestData>(file);

        //    Assert.AreEqual("Test Text", retrievedData.Text);
        //    Assert.AreEqual(42, retrievedData.Number);
        //}

        //[TestMethod]
        //public async Task StoreAsyncRetrieveAsync_WithStorageFolder_PersistsDataOverNull()
        //{
        //    StorageManager storageManager = new StorageManager();
        //    StorageFolder folder = ApplicationData.Current.TemporaryFolder;
        //    string name = GetTestFilename();

        //    // Store some test data

        //    TestData oldData = null;
        //    await storageManager.StoreAsync(folder, name, oldData);

        //    // Overwrite test data

        //    TestData data = new TestData() { Text = "Test Text", Number = 42 };
        //    await storageManager.StoreAsync(folder, name, data);

        //    // Retrieve and validate the data from the file

        //    TestData retrievedData = await storageManager.RetrieveAsync<TestData>(folder, name);

        //    Assert.AreEqual("Test Text", retrievedData.Text);
        //    Assert.AreEqual(42, retrievedData.Number);
        //}

        // *** Private Methods ***

        private string GetTestFilename([CallerMemberName]string callerName = null)
        {
            return callerName;
        }

        // *** Private Sub-classes ***

        [DataContract]
        private class TestData
        {
            [DataMember]
            public string Text { get; set; }
            [DataMember]
            public int Number { get; set; }
        }
    }
}

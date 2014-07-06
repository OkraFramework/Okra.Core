using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Okra.Navigation;
using Okra.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Okra.Tests.Navigation
{
    [TestClass]
    public class PageInfoFixture
    {
        // *** Constructor Tests ***

        [TestMethod]
        public void Constructor_SetsPageName()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            Assert.AreEqual("Page Name", navigationEntry.PageName);
        }

        [TestMethod]
        public void Constructor_SetsArguments_String()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            Assert.AreEqual("Arguments", navigationEntry.GetArguments<string>());
        }

        [TestMethod]
        public void Constructor_SetsArguments_NullString()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", (string)null);

            Assert.AreEqual(null, navigationEntry.GetArguments<string>());
        }

        [TestMethod]
        public void Constructor_SetsArguments_Int()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", 42);

            Assert.AreEqual(42, navigationEntry.GetArguments<int>());
        }

        [TestMethod]
        public void Constructor_SetsArguments_Class()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", new ClassState() { Text = "Text Value", Number = 42 });

            var args = navigationEntry.GetArguments<ClassState>();
            Assert.AreEqual("Text Value", args.Text);
            Assert.AreEqual(42, args.Number);
        }

        [TestMethod]
        public void Constructor_SetsArguments_NullClass()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", (ClassState)null);

            Assert.AreEqual(null, navigationEntry.GetArguments<ClassState>());
        }

        [TestMethod]
        public void Constructor_SetsArguments_Struct()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", new StructState() { Text = "Text Value", Number = 42 });

            var args = navigationEntry.GetArguments<StructState>();
            Assert.AreEqual("Text Value", args.Text);
            Assert.AreEqual(42, args.Number);
        }

        [TestMethod]
        public void Constructor_ThrowsException_WhenPageNameIsNull()
        {
            Assert.ThrowsException<ArgumentException>(() => new PageInfo(null, "Arguments"));
        }

        [TestMethod]
        public void Constructor_ThrowsException_WhenPageNameIsEmpty()
        {
            Assert.ThrowsException<ArgumentException>(() => new PageInfo("", "Arguments"));
        }

        // *** Method Tests ***

        [TestMethod]
        public void GetSetState_String_StoresState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<string>("MyKey", "Test State");
            var result = navigationEntry.GetState<string>("MyKey");

            Assert.AreEqual("Test State", result);
        }

        [TestMethod]
        public void GetSetState_String_StoresNullState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<string>("MyKey", null);
            var result = navigationEntry.GetState<string>("MyKey");

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void GetSetState_Int_StoresState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<int>("MyKey", 42);
            var result = navigationEntry.GetState<int>("MyKey");

            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public void GetSetState_Class_StoresState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<ClassState>("MyKey", new ClassState() { Text = "Text Value", Number = 42 });
            var result = navigationEntry.GetState<ClassState>("MyKey");

            Assert.AreEqual("Text Value", result.Text);
            Assert.AreEqual(42, result.Number);
        }

        [TestMethod]
        public void GetSetState_Class_StoresNullState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<ClassState>("MyKey", null);
            var result = navigationEntry.GetState<ClassState>("MyKey");

            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void GetSetState_Struct_StoresState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<StructState>("MyKey", new StructState() { Text = "Text Value", Number = 42 });
            var result = navigationEntry.GetState<StructState>("MyKey");

            Assert.AreEqual("Text Value", result.Text);
            Assert.AreEqual(42, result.Number);
        }

        [TestMethod]
        public void TryGetSetState_String_StoresState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<string>("MyKey", "Test State");
            string result;
            bool success = navigationEntry.TryGetState<string>("MyKey", out result);

            Assert.AreEqual(true, success);
            Assert.AreEqual("Test State", result);
        }

        [TestMethod]
        public void TryGetSetState_String_StoresNullState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<string>("MyKey", null);
            string result;
            bool success = navigationEntry.TryGetState<string>("MyKey", out result);

            Assert.AreEqual(true, success);
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TryGetSetState_Int_StoresState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<int>("MyKey", 42);
            int result;
            bool success = navigationEntry.TryGetState<int>("MyKey", out result);

            Assert.AreEqual(true, success);
            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public void TryGetSetState_Class_StoresState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<ClassState>("MyKey", new ClassState() { Text = "Text Value", Number = 42 });
            ClassState result;
            bool success = navigationEntry.TryGetState<ClassState>("MyKey", out result);

            Assert.AreEqual(true, success);
            Assert.AreEqual("Text Value", result.Text);
            Assert.AreEqual(42, result.Number);
        }

        [TestMethod]
        public void TryGetSetState_Class_StoresNullState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<ClassState>("MyKey", null);
            ClassState result;
            bool success = navigationEntry.TryGetState<ClassState>("MyKey", out result);

            Assert.AreEqual(true, success);
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TryGetSetState_Struct_StoresState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<StructState>("MyKey", new StructState() { Text = "Text Value", Number = 42 });
            StructState result;
            bool success = navigationEntry.TryGetState<StructState>("MyKey", out result);

            Assert.AreEqual(true, success);
            Assert.AreEqual("Text Value", result.Text);
            Assert.AreEqual(42, result.Number);
        }

        [TestMethod]
        public void GetState_Exception_KeyIsNull()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            Assert.ThrowsException<ArgumentException>(() => navigationEntry.GetState<string>(null));
        }

        [TestMethod]
        public void GetState_Exception_KeyIsEmpty()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            Assert.ThrowsException<ArgumentException>(() => navigationEntry.GetState<string>(""));
        }

        [TestMethod]
        public void GetState_Exception_KeyIsUndefined()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            Assert.ThrowsException<KeyNotFoundException>(() => navigationEntry.GetState<string>("Undefined"));
        }

        [TestMethod]
        public void GetState_Exception_IncorrectType()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");
            navigationEntry.SetState<string>("MyKey", "Test State");

            Assert.ThrowsException<InvalidCastException>(() => navigationEntry.GetState<int>("MyKey"));
        }

        [TestMethod]
        public void TryGetState_ReturnsFalseIfKeyIsUndefined()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            string result = "Old Value";
            bool success = navigationEntry.TryGetState<string>("Undefined", out result);

            Assert.AreEqual(false, success);
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void TryGetState_Exception_KeyIsNull()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            string result;
            Assert.ThrowsException<ArgumentException>(() => navigationEntry.TryGetState<string>(null, out result));
        }

        [TestMethod]
        public void TryGetState_Exception_KeyIsEmpty()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            string result;
            Assert.ThrowsException<ArgumentException>(() => navigationEntry.TryGetState<string>("", out result));
        }

        [TestMethod]
        public void TryGetState_Exception_IncorrectType()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");
            navigationEntry.SetState<string>("MyKey", "Test State");

            int result;
            Assert.ThrowsException<InvalidCastException>(() => navigationEntry.TryGetState<int>("MyKey", out result));
        }

        [TestMethod]
        public void SetState_RaisesStateChangedEvent()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            List<string> stateChangedKeys = new List<string>();
            navigationEntry.StateChanged += (sender, e) => { stateChangedKeys.Add(e.StateKey); };

            navigationEntry.SetState<string>("MyKey", "Test State");

            CollectionAssert.AreEqual(new[] { "MyKey" }, stateChangedKeys);
        }

        [TestMethod]
        public void SetState_Exception_KeyIsNull()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            Assert.ThrowsException<ArgumentException>(() => navigationEntry.SetState<string>(null, "Test"));
        }

        [TestMethod]
        public void SetState_Exception_KeyIsEmpty()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            Assert.ThrowsException<ArgumentException>(() => navigationEntry.SetState<string>("", "Test"));
        }

        // *** Serialization Tests ***

        [TestMethod]
        public void Serialization_PersistsPageName()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", "Arguments");

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.AreEqual("Page Name", newEntry.PageName);
        }

        [TestMethod]
        public void Serialization_PersistsArguments_String()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", "Arguments");

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.AreEqual("Arguments", newEntry.GetArguments<string>());
        }

        [TestMethod]
        public void Serialization_PersistsArguments_NullString()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", (string)null);

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.AreEqual(null, newEntry.GetArguments<string>());
        }

        [TestMethod]
        public void Serialization_PersistsArguments_Int()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", 42);

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.AreEqual(42, newEntry.GetArguments<int>());
        }

        [TestMethod]
        public void Serialization_PersistsArguments_Class()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", new ClassState() { Text = "Text Value", Number = 42 });

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            var args = newEntry.GetArguments<ClassState>();
            Assert.AreEqual("Text Value", args.Text);
            Assert.AreEqual(42, args.Number);
        }

        [TestMethod]
        public void Serialization_PersistsArguments_NullClass()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", (ClassState)null);

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.AreEqual(null, newEntry.GetArguments<ClassState>());
        }

        [TestMethod]
        public void Serialization_PersistsArguments_Struct()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", new StructState() { Text = "Text Value", Number = 42 });

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            var args = newEntry.GetArguments<StructState>();
            Assert.AreEqual("Text Value", args.Text);
            Assert.AreEqual(42, args.Number);
        }

        [TestMethod]
        public void Serialization_PersistsState_String()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", "Arguments");
            sourceEntry.SetState<string>("MyKey", "Test State");

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.AreEqual("Test State", newEntry.GetState<string>("MyKey"));
        }

        [TestMethod]
        public void Serialization_PersistsState_Int()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", "Arguments");
            sourceEntry.SetState<int>("MyKey", 42);

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.AreEqual(42, newEntry.GetState<int>("MyKey"));
        }

        [TestMethod]
        public void Serialization_PersistsState_Class()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", "Arguments");
            sourceEntry.SetState<ClassState>("MyKey", new ClassState() { Text = "Text Value", Number = 42 });

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            var state = newEntry.GetState<ClassState>("MyKey");
            Assert.AreEqual("Text Value", state.Text);
            Assert.AreEqual(42, state.Number);
        }

        [TestMethod]
        public void Serialization_PersistsState_Struct()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", "Arguments");
            sourceEntry.SetState<StructState>("MyKey", new StructState() { Text = "Text Value", Number = 42 });

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            var state = newEntry.GetState<StructState>("MyKey");
            Assert.AreEqual("Text Value", state.Text);
            Assert.AreEqual(42, state.Number);
        }

        [TestMethod]
        public void Serialization_PersistsState_NullString()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", "Arguments");
            sourceEntry.SetState<string>("MyKey", null);

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.AreEqual(null, newEntry.GetState<string>("MyKey"));
        }

        [TestMethod]
        public void Serialization_PersistsState_NullClass()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", "Arguments");
            sourceEntry.SetState<ClassState>("MyKey", null);

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.AreEqual(null, newEntry.GetState<ClassState>("MyKey"));
        }

        // *** Private Classes ***

        [DataContract]
        private class ClassState
        {
            [DataMember]
            public string Text;
            [DataMember]
            public int Number;
        }

        [DataContract]
        private struct StructState
        {
            [DataMember]
            public string Text;
            [DataMember]
            public int Number;
        }
    }
}

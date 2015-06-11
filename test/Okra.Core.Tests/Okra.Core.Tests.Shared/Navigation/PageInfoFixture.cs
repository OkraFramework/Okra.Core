using Okra.Navigation;
using Okra.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Xunit;

namespace Okra.Tests.Navigation
{
    public class PageInfoFixture
    {
        // *** Constructor Tests ***

        [Fact]
        public void Constructor_SetsPageName()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            Assert.Equal("Page Name", navigationEntry.PageName);
        }

        [Fact]
        public void Constructor_SetsArguments_String()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            Assert.Equal("Arguments", navigationEntry.GetArguments<string>());
        }

        [Fact]
        public void Constructor_SetsArguments_NullString()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", (string)null);

            Assert.Equal(null, navigationEntry.GetArguments<string>());
        }

        [Fact]
        public void Constructor_SetsArguments_Int()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", 42);

            Assert.Equal(42, navigationEntry.GetArguments<int>());
        }

        [Fact]
        public void Constructor_SetsArguments_Class()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", new ClassState() { Text = "Text Value", Number = 42 });

            var args = navigationEntry.GetArguments<ClassState>();
            Assert.Equal("Text Value", args.Text);
            Assert.Equal(42, args.Number);
        }

        [Fact]
        public void Constructor_SetsArguments_NullClass()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", (ClassState)null);

            Assert.Equal(null, navigationEntry.GetArguments<ClassState>());
        }

        [Fact]
        public void Constructor_SetsArguments_Struct()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", new StructState() { Text = "Text Value", Number = 42 });

            var args = navigationEntry.GetArguments<StructState>();
            Assert.Equal("Text Value", args.Text);
            Assert.Equal(42, args.Number);
        }

        [Fact]
        public void Constructor_SetsArguments_NullReturnsDefaultInt()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", null);

            Assert.Equal(0, navigationEntry.GetArguments<int>());
        }

        [Fact]
        public void Constructor_ThrowsException_WhenPageNameIsNull()
        {
            Assert.Throws<ArgumentException>(() => new PageInfo(null, "Arguments"));
        }

        [Fact]
        public void Constructor_ThrowsException_WhenPageNameIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new PageInfo("", "Arguments"));
        }

        // *** Method Tests ***

        [Fact]
        public void GetSetState_String_StoresState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<string>("MyKey", "Test State");
            var result = navigationEntry.GetState<string>("MyKey");

            Assert.Equal("Test State", result);
        }

        [Fact]
        public void GetSetState_String_StoresNullState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<string>("MyKey", null);
            var result = navigationEntry.GetState<string>("MyKey");

            Assert.Equal(null, result);
        }

        [Fact]
        public void GetSetState_Int_StoresState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<int>("MyKey", 42);
            var result = navigationEntry.GetState<int>("MyKey");

            Assert.Equal(42, result);
        }

        [Fact]
        public void GetSetState_Class_StoresState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<ClassState>("MyKey", new ClassState() { Text = "Text Value", Number = 42 });
            var result = navigationEntry.GetState<ClassState>("MyKey");

            Assert.Equal("Text Value", result.Text);
            Assert.Equal(42, result.Number);
        }

        [Fact]
        public void GetSetState_Class_StoresNullState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<ClassState>("MyKey", null);
            var result = navigationEntry.GetState<ClassState>("MyKey");

            Assert.Equal(null, result);
        }

        [Fact]
        public void GetSetState_Struct_StoresState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<StructState>("MyKey", new StructState() { Text = "Text Value", Number = 42 });
            var result = navigationEntry.GetState<StructState>("MyKey");

            Assert.Equal("Text Value", result.Text);
            Assert.Equal(42, result.Number);
        }

        [Fact]
        public void TryGetSetState_String_StoresState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<string>("MyKey", "Test State");
            string result;
            bool success = navigationEntry.TryGetState<string>("MyKey", out result);

            Assert.Equal(true, success);
            Assert.Equal("Test State", result);
        }

        [Fact]
        public void TryGetSetState_String_StoresNullState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<string>("MyKey", null);
            string result;
            bool success = navigationEntry.TryGetState<string>("MyKey", out result);

            Assert.Equal(true, success);
            Assert.Equal(null, result);
        }

        [Fact]
        public void TryGetSetState_Int_StoresState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<int>("MyKey", 42);
            int result;
            bool success = navigationEntry.TryGetState<int>("MyKey", out result);

            Assert.Equal(true, success);
            Assert.Equal(42, result);
        }

        [Fact]
        public void TryGetSetState_Class_StoresState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<ClassState>("MyKey", new ClassState() { Text = "Text Value", Number = 42 });
            ClassState result;
            bool success = navigationEntry.TryGetState<ClassState>("MyKey", out result);

            Assert.Equal(true, success);
            Assert.Equal("Text Value", result.Text);
            Assert.Equal(42, result.Number);
        }

        [Fact]
        public void TryGetSetState_Class_StoresNullState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<ClassState>("MyKey", null);
            ClassState result;
            bool success = navigationEntry.TryGetState<ClassState>("MyKey", out result);

            Assert.Equal(true, success);
            Assert.Equal(null, result);
        }

        [Fact]
        public void TryGetSetState_Struct_StoresState()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<StructState>("MyKey", new StructState() { Text = "Text Value", Number = 42 });
            StructState result;
            bool success = navigationEntry.TryGetState<StructState>("MyKey", out result);

            Assert.Equal(true, success);
            Assert.Equal("Text Value", result.Text);
            Assert.Equal(42, result.Number);
        }

        [Fact]
        public void GetState_Exception_KeyIsNull()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            Assert.Throws<ArgumentException>(() => navigationEntry.GetState<string>(null));
        }

        [Fact]
        public void GetState_Exception_KeyIsEmpty()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            Assert.Throws<ArgumentException>(() => navigationEntry.GetState<string>(""));
        }

        [Fact]
        public void GetState_Exception_KeyIsUndefined()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            Assert.Throws<KeyNotFoundException>(() => navigationEntry.GetState<string>("Undefined"));
        }

        [Fact]
        public void GetState_Exception_IncorrectType()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");
            navigationEntry.SetState<string>("MyKey", "Test State");

            Assert.Throws<InvalidCastException>(() => navigationEntry.GetState<int>("MyKey"));
        }

        [Fact]
        public void TryGetState_ReturnsFalseIfKeyIsUndefined()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            string result = "Old Value";
            bool success = navigationEntry.TryGetState<string>("Undefined", out result);

            Assert.Equal(false, success);
            Assert.Equal(null, result);
        }

        [Fact]
        public void TryGetState_Exception_KeyIsNull()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            string result;
            Assert.Throws<ArgumentException>(() => navigationEntry.TryGetState<string>(null, out result));
        }

        [Fact]
        public void TryGetState_Exception_KeyIsEmpty()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            string result;
            Assert.Throws<ArgumentException>(() => navigationEntry.TryGetState<string>("", out result));
        }

        [Fact]
        public void TryGetState_Exception_IncorrectType()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");
            navigationEntry.SetState<string>("MyKey", "Test State");

            int result;
            Assert.Throws<InvalidCastException>(() => navigationEntry.TryGetState<int>("MyKey", out result));
        }

        [Fact]
        public void SetState_SameStateKeyOverwritesPreviousValue()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            navigationEntry.SetState<string>("MyKey", "State A");
            navigationEntry.SetState<string>("MyKey", "State B");

            string result;
            bool success = navigationEntry.TryGetState<string>("MyKey", out result);

            Assert.Equal(true, success);
            Assert.Equal("State B", result);
        }

        [Fact]
        public void SetState_RaisesStateChangedEvent()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            List<string> stateChangedKeys = new List<string>();
            navigationEntry.StateChanged += (sender, e) => { stateChangedKeys.Add(e.StateKey); };

            navigationEntry.SetState<string>("MyKey", "Test State");

            Assert.Equal(new[] { "MyKey" }, stateChangedKeys);
        }

        [Fact]
        public void SetState_Exception_KeyIsNull()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            Assert.Throws<ArgumentException>(() => navigationEntry.SetState<string>(null, "Test"));
        }

        [Fact]
        public void SetState_Exception_KeyIsEmpty()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", "Arguments");

            Assert.Throws<ArgumentException>(() => navigationEntry.SetState<string>("", "Test"));
        }

        [Fact]
        public void ToString_ReturnsReadableString_IfArgumentsAreNull()
        {
            PageInfo navigationEntry = new PageInfo("Page Name", null);

            string str = navigationEntry.ToString();

            Assert.Equal("Page Name", str);
        }

        // *** Serialization Tests ***

        [Fact]
        public void Serialization_PersistsPageName()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", "Arguments");

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.Equal("Page Name", newEntry.PageName);
        }

        [Fact]
        public void Serialization_PersistsArguments_String()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", "Arguments");

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.Equal("Arguments", newEntry.GetArguments<string>());
        }

        [Fact]
        public void Serialization_PersistsArguments_NullString()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", (string)null);

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.Equal(null, newEntry.GetArguments<string>());
        }

        [Fact]
        public void Serialization_PersistsArguments_Int()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", 42);

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.Equal(42, newEntry.GetArguments<int>());
        }

        [Fact]
        public void Serialization_PersistsArguments_Class()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", new ClassState() { Text = "Text Value", Number = 42 });

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            var args = newEntry.GetArguments<ClassState>();
            Assert.Equal("Text Value", args.Text);
            Assert.Equal(42, args.Number);
        }

        [Fact]
        public void Serialization_PersistsArguments_NullClass()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", (ClassState)null);

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.Equal(null, newEntry.GetArguments<ClassState>());
        }

        [Fact]
        public void Serialization_PersistsArguments_Struct()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", new StructState() { Text = "Text Value", Number = 42 });

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            var args = newEntry.GetArguments<StructState>();
            Assert.Equal("Text Value", args.Text);
            Assert.Equal(42, args.Number);
        }

        [Fact]
        public void Serialization_PersistsArguments_NullReturnsDefaultInt()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", null);

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.Equal(0, newEntry.GetArguments<int>());
        }

        [Fact]
        public void Serialization_PersistsState_String()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", "Arguments");
            sourceEntry.SetState<string>("MyKey", "Test State");

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.Equal("Test State", newEntry.GetState<string>("MyKey"));
        }

        [Fact]
        public void Serialization_PersistsState_Int()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", "Arguments");
            sourceEntry.SetState<int>("MyKey", 42);

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.Equal(42, newEntry.GetState<int>("MyKey"));
        }

        [Fact]
        public void Serialization_PersistsState_Class()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", "Arguments");
            sourceEntry.SetState<ClassState>("MyKey", new ClassState() { Text = "Text Value", Number = 42 });

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            var state = newEntry.GetState<ClassState>("MyKey");
            Assert.Equal("Text Value", state.Text);
            Assert.Equal(42, state.Number);
        }

        [Fact]
        public void Serialization_PersistsState_Struct()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", "Arguments");
            sourceEntry.SetState<StructState>("MyKey", new StructState() { Text = "Text Value", Number = 42 });

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            var state = newEntry.GetState<StructState>("MyKey");
            Assert.Equal("Text Value", state.Text);
            Assert.Equal(42, state.Number);
        }

        [Fact]
        public void Serialization_PersistsState_NullString()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", "Arguments");
            sourceEntry.SetState<string>("MyKey", null);

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.Equal(null, newEntry.GetState<string>("MyKey"));
        }

        [Fact]
        public void Serialization_PersistsState_NullClass()
        {
            PageInfo sourceEntry = new PageInfo("Page Name", "Arguments");
            sourceEntry.SetState<ClassState>("MyKey", null);

            byte[] data = SerializationHelper.SerializeToArray(sourceEntry);
            PageInfo newEntry = SerializationHelper.DeserializeFromArray<PageInfo>(data);

            Assert.Equal(null, newEntry.GetState<ClassState>("MyKey"));
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

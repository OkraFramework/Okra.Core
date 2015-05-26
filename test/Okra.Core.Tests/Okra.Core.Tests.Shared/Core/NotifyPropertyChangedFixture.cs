using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okra.Core;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Linq.Expressions;

namespace Okra.Tests.Core
{
    [TestClass]
    public class NotifyPropertyChangedFixture
    {
        // *** Method Tests ***

        [TestMethod]
        public void GetPropertyName_ReturnsNameOfProperty()
        {
            string propertyName = TestableNotifyPropertyChanged.GetPropertyName(o => o.MyProperty);

            Assert.AreEqual("MyProperty", propertyName);
        }

        [TestMethod]
        public void GetPropertyName_Exception_LambdaExpressionIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => TestableNotifyPropertyChanged.GetPropertyName(null));
        }

        [TestMethod]
        public void GetPropertyName_Exception_LambdaExpressionIsNotMemberAccess()
        {
            Assert.ThrowsException<ArgumentException>(() => TestableNotifyPropertyChanged.GetPropertyName(o => 42));
        }

        [TestMethod]
        public void OnPropertyChanged_FiresPropertyChangedEvent_Void()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged();

            int propertyChangedCount = 0;

            obj.PropertyChanged += (sender, e) =>
                {
                    Assert.AreEqual(obj, sender);
                    Assert.AreEqual("MyProperty", e.PropertyName);
                    propertyChangedCount++;
                };

            obj.MyProperty = 42;

            Assert.AreEqual(1, propertyChangedCount);
        }

        [TestMethod]
        public void OnPropertyChanged_FiresPropertyChangedEvent_String()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged();

            int propertyChangedCount = 0;

            obj.PropertyChanged += (sender, e) =>
            {
                Assert.AreEqual(obj, sender);
                Assert.AreEqual("MyProperty", e.PropertyName);
                propertyChangedCount++;
            };

            obj.FirePropertyChangedWithString("MyProperty");

            Assert.AreEqual(1, propertyChangedCount);
        }

        [TestMethod]
        public void OnPropertyChanged_FiresPropertyChangedEvent_PropertyChangedEventArgs()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged();

            int propertyChangedCount = 0;

            obj.PropertyChanged += (sender, e) =>
            {
                Assert.AreEqual(obj, sender);
                Assert.AreEqual("MyProperty", e.PropertyName);
                propertyChangedCount++;
            };

            obj.FirePropertyChangedWithEventArgs(new PropertyChangedEventArgs("MyProperty"));

            Assert.AreEqual(1, propertyChangedCount);
        }

        [TestMethod]
        public void OnPropertyChanged_FiresPropertyChangedEvent_LambdaExpression()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged();

            int propertyChangedCount = 0;

            obj.PropertyChanged += (sender, e) =>
            {
                Assert.AreEqual(obj, sender);
                Assert.AreEqual("MyProperty", e.PropertyName);
                propertyChangedCount++;
            };

            obj.FirePropertyChangedWithLambda(() => obj.MyProperty);

            Assert.AreEqual(1, propertyChangedCount);
        }

        [TestMethod]
        public void OnPropertyChanged_Exception_LambdaExpression_ExpressionIsNull()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged();

            Assert.ThrowsException<ArgumentNullException>(() => obj.FirePropertyChangedWithLambda<int>(null));
        }

        [TestMethod]
        public void OnPropertyChanged_Exception_LambdaExpression_IsNotMemberAccess()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged();

            Assert.ThrowsException<ArgumentException>(() => obj.FirePropertyChangedWithLambda<int>(() => 42));
        }

        [TestMethod]
        public void OnPropertyChanged_IgnoresIfNoEventHandlerAttached()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged();

            obj.FirePropertyChangedWithEventArgs(new PropertyChangedEventArgs("MyProperty"));
        }

        [TestMethod]
        public void SetProperty_FiresPropertyChangedEvent_Void_Struct()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged();

            int propertyChangedCount = 0;

            obj.PropertyChanged += (sender, e) =>
            {
                Assert.AreEqual(obj, sender);
                Assert.AreEqual("MySetProperty", e.PropertyName);
                propertyChangedCount++;
            };

            obj.MySetProperty = 42;

            Assert.AreEqual(1, propertyChangedCount);
        }

        [TestMethod]
        public void SetProperty_FiresPropertyChangedEvent_Void_Object()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged() { MyObjectProperty = new object() };

            int propertyChangedCount = 0;

            obj.PropertyChanged += (sender, e) =>
            {
                Assert.AreEqual(obj, sender);
                Assert.AreEqual("MyObjectProperty", e.PropertyName);
                propertyChangedCount++;
            };

            obj.MyObjectProperty = new object();

            Assert.AreEqual(1, propertyChangedCount);
        }

        [TestMethod]
        public void SetProperty_FiresPropertyChangedEvent_Void_Object_FromNull()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged() { MyObjectProperty = null };

            int propertyChangedCount = 0;

            obj.PropertyChanged += (sender, e) =>
            {
                Assert.AreEqual(obj, sender);
                Assert.AreEqual("MyObjectProperty", e.PropertyName);
                propertyChangedCount++;
            };

            obj.MyObjectProperty = new object();

            Assert.AreEqual(1, propertyChangedCount);
        }

        [TestMethod]
        public void SetProperty_FiresPropertyChangedEvent_Void_Object_ToNull()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged() { MyObjectProperty = new object() };

            int propertyChangedCount = 0;

            obj.PropertyChanged += (sender, e) =>
            {
                Assert.AreEqual(obj, sender);
                Assert.AreEqual("MyObjectProperty", e.PropertyName);
                propertyChangedCount++;
            };

            obj.MyObjectProperty = null;

            Assert.AreEqual(1, propertyChangedCount);
        }

        [TestMethod]
        public void SetProperty_FiresPropertyChangedEvent_String()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged();

            int propertyChangedCount = 0;

            obj.PropertyChanged += (sender, e) =>
            {
                Assert.AreEqual(obj, sender);
                Assert.AreEqual("MySetProperty", e.PropertyName);
                propertyChangedCount++;
            };

            obj.SetProperty_MySetProperty(42, "MySetProperty");

            Assert.AreEqual(1, propertyChangedCount);
        }

        [TestMethod]
        public void SetProperty_FiresPropertyChangedEvent_LambdaExpression()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged();

            int propertyChangedCount = 0;

            obj.PropertyChanged += (sender, e) =>
            {
                Assert.AreEqual(obj, sender);
                Assert.AreEqual("MySetProperty", e.PropertyName);
                propertyChangedCount++;
            };

            obj.SetProperty_MySetProperty(42, () => obj.MySetProperty);

            Assert.AreEqual(1, propertyChangedCount);
        }

        [TestMethod]
        public void SetProperty_Exception_LambdaExpression_ExpressionIsNull()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged();

            Expression<Func<int>> propertyExpression = null;
            Assert.ThrowsException<ArgumentNullException>(() => obj.SetProperty_MySetProperty(10, propertyExpression));
        }

        [TestMethod]
        public void SetProperty_Exception_LambdaExpression_IsNotMemberAccess()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged();

            Assert.ThrowsException<ArgumentException>(() => obj.SetProperty_MySetProperty(10, () => 42));
        }

        [TestMethod]
        public void SetProperty_DoesNotFireEventIfNotChanged_Struct()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged() { MySetProperty = 10 };

            int propertyChangedCount = 0;

            obj.PropertyChanged += (sender, e) =>
            {
                Assert.AreEqual(obj, sender);
                Assert.AreEqual("MySetProperty", e.PropertyName);
                propertyChangedCount++;
            };

            obj.MySetProperty = 10;

            Assert.AreEqual(0, propertyChangedCount);
        }

        [TestMethod]
        public void SetProperty_DoesNotFireEventIfNotChanged_Object()
        {
            object value = new object();
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged() { MyObjectProperty = value };

            int propertyChangedCount = 0;

            obj.PropertyChanged += (sender, e) =>
            {
                Assert.AreEqual(obj, sender);
                Assert.AreEqual("MyObjectProperty", e.PropertyName);
                propertyChangedCount++;
            };

            obj.MyObjectProperty = value;

            Assert.AreEqual(0, propertyChangedCount);
        }

        [TestMethod]
        public void SetProperty_DoesNotFireEventIfNotChanged_ObjectNull()
        {
            object value = new object();
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged() { MyObjectProperty = null };

            int propertyChangedCount = 0;

            obj.PropertyChanged += (sender, e) =>
            {
                Assert.AreEqual(obj, sender);
                Assert.AreEqual("MyObjectProperty", e.PropertyName);
                propertyChangedCount++;
            };

            obj.MyObjectProperty = null;

            Assert.AreEqual(0, propertyChangedCount);
        }

        [TestMethod]
        public void SetProperty_ReturnsTrue_IfValueHasChanged()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged() { MySetProperty = 5 };

            bool changed = obj.SetProperty_MySetProperty(10, "MySetProperty");

            Assert.AreEqual(true, changed);
        }

        [TestMethod]
        public void SetProperty_ReturnsTrue_IfValueHasNotChanged()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged() { MySetProperty = 10 };

            bool changed = obj.SetProperty_MySetProperty(10, "MySetProperty");

            Assert.AreEqual(false, changed);
        }

        [TestMethod]
        public void SetProperty_SetsProperty_Struct()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged() { MySetProperty = 5 };

            obj.SetProperty_MySetProperty(10, "MySetProperty");

            Assert.AreEqual(10, obj.MySetProperty);
        }

        [TestMethod]
        public void SetProperty_SetsProperty_Object()
        {
            TestableNotifyPropertyChanged obj = new TestableNotifyPropertyChanged();
            object value = new object();

            obj.SetProperty_MyObjectProperty(value, "MyObjectProperty");

            Assert.AreEqual(value, obj.MyObjectProperty);
        }

        // *** Private Sub-classes ***

        private class TestableNotifyPropertyChanged : NotifyPropertyChangedBase
        {
            // *** Fields ***

            private int _myProperty;
            private int _mySetProperty;
            private object _myObjectProperty;

            // *** Properties ***

            public object MyObjectProperty
            {
                get
                {
                    return _myObjectProperty;
                }
                set
                {
                    SetProperty(ref _myObjectProperty, value);
                }
            }

            public int MyProperty
            {
                get
                {
                    return _myProperty;
                }
                set
                {
                    _myProperty = value;
                    OnPropertyChanged();
                }
            }

            public int MySetProperty
            {
                get
                {
                    return _mySetProperty;
                }
                set
                {
                    SetProperty(ref _mySetProperty, value);
                }
            }

            // *** Methods ***

            public void FirePropertyChangedWithString(string propertyName)
            {
                base.OnPropertyChanged(propertyName);
            }

            public void FirePropertyChangedWithLambda<T>(Expression<Func<T>> propertyExpression)
            {
                base.OnPropertyChanged(propertyExpression);
            }

            public void FirePropertyChangedWithEventArgs(PropertyChangedEventArgs e)
            {
                base.OnPropertyChanged(e);
            }

            public bool SetProperty_MySetProperty(int value, string propertyName)
            {
                return SetProperty(ref _mySetProperty, value, propertyName);
            }

            public bool SetProperty_MySetProperty(int value, Expression<Func<int>> propertyExpression)
            {
                return SetProperty(ref _mySetProperty, value, propertyExpression);
            }

            public bool SetProperty_MyObjectProperty(object value, string propertyName)
            {
                return SetProperty(ref _myObjectProperty, value, propertyName);
            }

            // *** Static Methods ***

            public static string GetPropertyName(Expression<Func<TestableNotifyPropertyChanged, object>> propertyExpression)
            {
                return NotifyPropertyChangedBase.GetPropertyName(propertyExpression);
            }
        }
    }
}

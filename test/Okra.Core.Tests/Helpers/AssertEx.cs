using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Xunit;

namespace Okra.Tests.Helpers
{
    public static class AssertEx
    {
        public static void PropertyChangedEvents<T>(T obj, Action action) where T : INotifyPropertyChanged
        {
            TypeInfo typeInfo = typeof(T).GetTypeInfo();

            // Get current values for all properties

            var initialProperties = new Dictionary<PropertyInfo, object>();

            foreach (PropertyInfo property in typeInfo.DeclaredProperties)
            {
                if (property.GetIndexParameters().Length == 0)
                    initialProperties[property] = property.GetValue(obj);
            }

            // Perform the action, recording property changed events

            var changedPropertyNames = new List<string>();

            PropertyChangedEventHandler eventHandler = (sender, e) =>
            {
                Assert.Equal(obj, sender);
                changedPropertyNames.Add(e.PropertyName);
            };

            obj.PropertyChanged += eventHandler;
            action();
            obj.PropertyChanged -= eventHandler;

            // Check that property changed events were raised for all modified properties

            foreach (KeyValuePair<PropertyInfo, object> initialProperty in initialProperties)
            {
                if (!object.Equals(initialProperty.Key.GetValue(obj), initialProperty.Value))
                {
                    string propertyName = initialProperty.Key.Name;
                    Assert.True(changedPropertyNames.Contains(propertyName), $"Property '{propertyName}' did not raise a PropertyChanged event");
                    changedPropertyNames.Remove(propertyName);
                }
            }

            // Check that no unchanged properties raised PropertyChanged events

            if (changedPropertyNames.Count > 0)
                Assert.True(false, $"Unchanged Property '{changedPropertyNames[0]}' raised PropertyChanged event");
        }
    }
}

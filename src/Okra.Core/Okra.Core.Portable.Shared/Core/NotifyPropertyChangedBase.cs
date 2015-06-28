using Okra.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Core
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        // *** Events ***

        public event PropertyChangedEventHandler PropertyChanged;

        // *** Protected Methods ***

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            string propertyName = GetPropertyName(propertyExpression);
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler eventHandler = PropertyChanged;

            if (eventHandler != null)
                eventHandler(this, e);
        }

        protected bool SetProperty<T>(ref T storage, T value, Expression<Func<T>> propertyExpression)
        {
            string propertyName = GetPropertyName(propertyExpression);
            return SetProperty<T>(ref storage, value, propertyName);
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            // If the value hasn't changed then just return false

            if (object.Equals(storage, value))
                return false;

            // Set the property and raise the property changed notification

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        // *** Static Methods ***

        protected static string GetPropertyName<T>(Expression<Func<T, object>> propertyExpression)
        {
            // Validate arguments

            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            // Extract the unary

            UnaryExpression unaryExpression = (UnaryExpression)propertyExpression.Body;

            // Validate operand type

            if (unaryExpression.Operand.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_ShouldBeAMemberAccessLambdaExpression"), nameof(propertyExpression));

            // Extract the property name

            MemberExpression memberExpression = (MemberExpression)unaryExpression.Operand;
            return memberExpression.Member.Name;
        }

        // *** Private Methods ***

        private string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            // Validate arguments

            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            if (propertyExpression.Body.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_ShouldBeAMemberAccessLambdaExpression"), nameof(propertyExpression));

            // Extract the property name

            MemberExpression memberExpression = (MemberExpression)propertyExpression.Body;
            return memberExpression.Member.Name;
        }
    }
}

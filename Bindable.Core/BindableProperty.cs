using System;
using System.Reflection;

namespace Bindable
{
    /// <summary>
    /// Provides a bridge between an object and the property you wish to be able to bind to.
    /// </summary>
    /// <typeparam name="TOwner">The type of object you can use this property with.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    public class BindableProperty<TOwner, TProperty> : BindableProperty<TOwner>
    {
        /// <summary>
        /// Create a new bridge to a property for a bindable object.
        /// </summary>
        /// <param name="targetPropertyName">The name of the property that is going to be bound to with.</param>
        /// <param name="getter">Gets the value of the property.</param>
        /// <param name="setter">Sets the value of the property.</param>
        /// <param name="defaultValue">The default value of the property.</param>
        public BindableProperty(string targetPropertyName, Func<TOwner, TProperty> getter, Action<TOwner, TProperty> setter, TProperty defaultValue = default(TProperty))
        {
            TargetType = typeof(TProperty);
            TargetProperty = TargetType.GetTypeInfo().GetDeclaredProperty(targetPropertyName);
            Getter = getter;
            Setter = setter;
            DefaultValue = defaultValue;
        }

        internal Func<TOwner, TProperty> Getter { get; private set; }

        internal Action<TOwner, TProperty> Setter { get; private set; }

        internal TProperty DefaultValue { get; private set; }

        internal override object GetValue(TOwner item)
        {
            return Getter(item);
        }

        internal override void SetValue(TOwner item, object value)
        {
            if (!(value is TProperty)) throw new InvalidCastException($"The provided value was not of type {TargetType.FullName}.");
            Setter(item, (TProperty)value);
        }
    }

    /// <summary>
    /// Provides a bridge between an object and the property you wish to be able to bind to.
    /// </summary>
    /// <typeparam name="TOwner">The type of object you can use this property with.</typeparam>
    public abstract class BindableProperty<TOwner>
    {
        /// <summary>
        /// The type of the target property that the bridge is provided for.
        /// </summary>
        public Type TargetType { get; internal set; }

        /// <summary>
        /// The property info for the target property.
        /// </summary>
        internal PropertyInfo TargetProperty { get; set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="item">The item to get the value for.</param>
        /// <returns></returns>
        internal abstract object GetValue(TOwner item);

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="item">The item to set the value for.</param>
        /// <param name="value">The new value.</param>
        internal abstract void SetValue(TOwner item, object value);
    }
}
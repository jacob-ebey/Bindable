using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Bindable
{
    /// <summary>
    /// The manager class to help with binding to objects.
    /// </summary>
    /// <typeparam name="TOwner">The type that it manages the bindings for.</typeparam>
    public class BindingManager<TOwner> where TOwner : IBindable<TOwner>
    {
        object syncRoot = new object();

        TOwner bindableObject;

        readonly Dictionary<BindableProperty<TOwner>, BindingInfo> bindingInfoCache = new Dictionary<BindableProperty<TOwner>, BindingInfo>();
        readonly Dictionary<string, List<BindableProperty<TOwner>>> bindingPropertyCache = new Dictionary<string, List<BindableProperty<TOwner>>>();
        
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="bindableObject">The object to manage the bindings for.</param>
        public BindingManager(TOwner bindableObject)
        {
            this.bindableObject = bindableObject;

            bindableObject.DataContextChanged += DataContextChanged;
            bindableObject.DataContextChanging += DataContextChanging;
        }

        /// <summary>
        /// Sets a binding. Currently only one way bindings are supported.
        /// </summary>
        /// <param name="bindableProperty">The property to bind to.</param>
        /// <param name="sourcePropertyName">the name of the data context property to get the value from.</param>
        /// <param name="converter">Converter to convert between object and the type the BindableProperty expects.</param>
        public void SetBinding(BindableProperty<TOwner> bindableProperty, string sourcePropertyName, IBindingConverter converter = null)
        {
            lock (syncRoot)
            {
                if (bindingInfoCache.ContainsKey(bindableProperty))
                    throw new Exception("Can not bind to the same property more than once.");

                if (bindingPropertyCache.ContainsKey(sourcePropertyName))
                    bindingPropertyCache[sourcePropertyName].Add(bindableProperty);
                else
                    bindingPropertyCache.Add(sourcePropertyName, new List<BindableProperty<TOwner>>(new[] { bindableProperty }));

                UpdateBinding(bindableProperty, sourcePropertyName, converter);
            }
        }

        private void UpdateBinding(BindableProperty<TOwner> bindableProperty, string sourcePropertyName, IBindingConverter converter)
        {
            PropertyInfo propertyInfo = null;
            if (bindableObject.DataContext != null)
                propertyInfo = bindableObject.DataContext.GetType().GetRuntimeProperty(sourcePropertyName);

            BindingInfo info = new BindingInfo(sourcePropertyName, propertyInfo, converter);

            lock (syncRoot)
            {
                if (bindingInfoCache.ContainsKey(bindableProperty))
                    bindingInfoCache[bindableProperty] = info;
                else
                    bindingInfoCache.Add(bindableProperty, info);
            }
        }

        private void UpdateBindings()
        {
            lock (syncRoot)
            {
                foreach (var key in bindingInfoCache.Keys)
                {
                    var bindingInfo = bindingInfoCache[key];
                    UpdateBinding(key, bindingInfo.SourcePropertyName, bindingInfo.Converter);
                    DataContextPropertyChanged(bindableObject.DataContext, new PropertyChangedEventArgs(bindingInfo.SourcePropertyName));
                }
            }
        }

        private void DataContextChanged(object sender, EventArgs e)
        {
            var notifyPropertyChanged = bindableObject as INotifyPropertyChanged;
            if (notifyPropertyChanged != null)
                notifyPropertyChanged.PropertyChanged += DataContextPropertyChanged;

            UpdateBindings();
        }

        private void DataContextChanging(object sender, EventArgs e)
        {
            var notifyPropertyChanged = bindableObject as INotifyPropertyChanged;
            if (notifyPropertyChanged != null)
                notifyPropertyChanged.PropertyChanged -= DataContextPropertyChanged;
        }

        private void DataContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!bindingPropertyCache.ContainsKey(e.PropertyName)) return;

            IEnumerable<BindableProperty<TOwner>> bindableProperties;

            lock (syncRoot)
                bindableProperties = bindingPropertyCache[e.PropertyName];

            foreach (var bindableProperty in bindableProperties)
            {
                BindingInfo bindingInfo = bindingInfoCache[bindableProperty];

                if (bindingInfo.SourceProperty == null) return;

                object value = bindingInfo.SourceProperty.GetValue(bindableObject.DataContext);

                if (!bindableProperty.TargetType.GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo()))
                {
                    if (bindingInfo.Converter != null)
                    {
                        value = bindingInfo.Converter.Convert(value, bindableProperty.TargetType);
                        if (!bindableProperty.TargetType.GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo()))
                            return;
                    }
                    else
                        return;
                }

                bindableProperty.SetValue(bindableObject, value);
            }
        }
    }
}

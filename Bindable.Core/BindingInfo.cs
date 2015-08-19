using System.Reflection;

namespace Bindable
{
    class BindingInfo
    {
        public BindingInfo(string sourcePropertyName, PropertyInfo sourceProperty, IBindingConverter converter)
        {
            SourcePropertyName = sourcePropertyName;
            SourceProperty = sourceProperty;
            Converter = converter;
        }

        public string SourcePropertyName { get; private set; }

        public PropertyInfo SourceProperty { get; private set; }

        public IBindingConverter Converter { get; private set; }
    }
}

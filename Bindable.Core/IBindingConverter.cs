using System;

namespace Bindable
{
    /// <summary>
    /// Responsible for converting between types for bindings.
    /// </summary>
    public interface IBindingConverter
    {
        /// <summary>
        /// Convert from an object to the provided type.
        /// </summary>
        /// <param name="valueToConvert">The value to convert.</param>
        /// <param name="typeToConvertTo">The type to convert to.</param>
        /// <returns>The converted type.</returns>
        object Convert(object valueToConvert, Type typeToConvertTo);
    }
}
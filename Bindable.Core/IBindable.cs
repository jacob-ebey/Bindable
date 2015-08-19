using System;

namespace Bindable
{
    /// <summary>
    /// A bindable object.
    /// </summary>
    /// <typeparam name="TOwner"></typeparam>
    public interface IBindable<TOwner>
    {
        /// <summary>
        /// Set a binding on the object.
        /// </summary>
        /// <param name="property">The property to bind to.</param>
        /// <param name="sourcePropertyName">The name of the data context property to bind to.</param>
        /// <param name="converter">The converter.</param>
        void SetBinding(BindableProperty<TOwner> property, string sourcePropertyName, IBindingConverter converter = null);

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        object DataContext { get; set; }

        /// <summary>
        /// Occurs when the data context is about to change.
        /// </summary>
        event EventHandler DataContextChanging;

        /// <summary>
        /// Occurs when the data context has changed.
        /// </summary>
        event EventHandler DataContextChanged;
    }
}

using System;
using Android.App;
using Android.Runtime;

namespace Bindable.Droid
{
    /// <summary>
    /// A bindable Activity.
    /// </summary>
    public class BindableActivity : Activity, IBindable<BindableActivity>
    {
        BindingManager<BindableActivity> manager;

        /// <summary>
        /// Create as new instance.
        /// </summary>
        public BindableActivity() : base()
        {
            manager = new BindingManager<BindableActivity>(this);
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="javaReference">The java reference.</param>
        /// <param name="transfer">The ownership transfer.</param>
        protected BindableActivity(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        object dataContext;

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        public object DataContext 
        {
            get { return dataContext; }
            set 
            {
                if (dataContext == value)
                    return;
                OnDataContextChanging(new EventArgs());
                dataContext = value;
                OnDataContextChanged(new EventArgs());
            }
        }

        private void OnDataContextChanging(EventArgs e) => DataContextChanging?.Invoke(this, e);

        private void OnDataContextChanged(EventArgs e) => DataContextChanged?.Invoke(this, e);

        /// <summary>
        /// Set a binding on the object.
        /// </summary>
        /// <param name="property">The property to bind to.</param>
        /// <param name="sourcePropertyName">The name of the data context property to bind to.</param>
        /// <param name="converter">The converter.</param>
        public void SetBinding (BindableProperty<BindableActivity> property, string sourcePropertyName, IBindingConverter converter = null)
        {
            manager.SetBinding(property, sourcePropertyName, converter);
        }

        /// <summary>
        /// Occurs when the data context is about to change.
        /// </summary>
        public event EventHandler DataContextChanging;

        /// <summary>
        /// Occurs when the data context has changed.
        /// </summary>
        public event EventHandler DataContextChanged;

        /// <summary>
        /// The bindable property for Activity.Title.
        /// </summary>
        public static BindableProperty<BindableActivity> TitleProperty { get; } = new BindableProperty<BindableActivity, string>(
            "Title",
            i => i.Title,
            (i, v) => i.Title = v);
    }
}


using System;
using Android.Widget;
using Android.Content;
using Android.Util;
using Android.Runtime;
using System.ComponentModel;
using Android.Views;

namespace Bindable.Droid
{
    /// <summary>
    /// A bindable EditText.
    /// </summary>
    public class BindableEditText : EditText, IBindable<BindableEditText>
    {
        BindingManager<BindableEditText> manager;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="context"></param>
        public BindableEditText(Context context) : base(context)
        {
            manager = new BindingManager<BindableEditText>(this);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        public BindableEditText(Context context, IAttributeSet attrs) : base(context, attrs) { }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        /// <param name="defStyleAttr"></param>
        public BindableEditText(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        { }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        /// <param name="defStyleAttr"></param>
        /// <param name="defStyleRes"></param>
        public BindableEditText(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        { }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="javaReference"></param>
        /// <param name="transfer"></param>
        protected BindableEditText(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        private object dataContext;

        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        public object DataContext
        {
            get { return dataContext; }
            set
            {
                if (dataContext == value) return;
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
        public void SetBinding(BindableProperty<BindableEditText> property, string sourcePropertyName, IBindingConverter converter = null)
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
        /// The bindable property for EditText.Text.
        /// </summary>
        public static BindableProperty<BindableEditText> TextProperty { get; } = new BindableProperty<BindableEditText, string>(
            "Text",
            i => i.Text,
            (i, v) => i.Text = v);

        /// <summary>
        /// The bindable property for EditText.TextSize.
        /// </summary>
        public static BindableProperty<BindableEditText> TextSizeProperty { get; } = new BindableProperty<BindableEditText, float>(
            "TextSize",
            i => i.TextSize,
            (i, v) => i.TextSize = v,
            1.0f);

        /// <summary>
        /// The bindable property for EditText.Visibility that relates a true value to ViewStates.Visible and a false value to ViewStates.Invisible.
        /// </summary>
		public static BindableProperty<BindableEditText> IsVisibleProperty { get; } = new BindableProperty<BindableEditText, bool>(
			"IsVisible",
			i => i.Visibility == ViewStates.Visible,
			(i, v) => 
			{
				if (v) i.Visibility = ViewStates.Visible;
				else i.Visibility = ViewStates.Invisible;
			},
			true);
    }
}
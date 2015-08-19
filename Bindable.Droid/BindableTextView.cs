using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Bindable.Droid
{
    /// <summary>
    /// A bindable TextView.
    /// </summary>
    public class BindableTextView : TextView, IBindable<BindableTextView>
    {
        BindingManager<BindableTextView> manager;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="context"></param>
        public BindableTextView(Context context) : base(context)
        {
            manager = new BindingManager<BindableTextView>(this);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        public BindableTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            manager = new BindingManager<BindableTextView>(this);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        /// <param name="defStyleAttr"></param>
        public BindableTextView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            manager = new BindingManager<BindableTextView>(this);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        /// <param name="defStyleAttr"></param>
        /// <param name="defStyleRes"></param>
        public BindableTextView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            manager = new BindingManager<BindableTextView>(this);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="javaReference"></param>
        /// <param name="transfer"></param>
        protected BindableTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            manager = new BindingManager<BindableTextView>(this);
        }

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
        public void SetBinding(BindableProperty<BindableTextView> property, string sourcePropertyName, IBindingConverter converter = null)
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
        /// The bindable property for TextView.Text.
        /// </summary>
        public static BindableProperty<BindableTextView> TextProperty { get; } = new BindableProperty<BindableTextView, string>(
            "Text",
            i => i.Text,
            (i, v) => i.Text = v);

        /// <summary>
        /// The bindable property for TextView.TextSize.
        /// </summary>
		public static BindableProperty<BindableTextView> TextSizeProperty { get; } = new BindableProperty<BindableTextView, float>(
            "TextSize",
            i => i.TextSize,
            (i, v) => i.TextSize = v,
            1.0f);

        /// <summary>
        /// The bindable property for TextView.Visibility that relates a true value to ViewStates.Visible and a false value to ViewStates.Invisible.
        /// </summary>
        public static BindableProperty<BindableTextView> IsVisibleProperty { get; } = new BindableProperty<BindableTextView, bool>(
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
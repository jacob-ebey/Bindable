using System;

using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

using Bindable;

namespace Bindable.Droid
{
    /// <summary>
    /// A bindable ListView.
    /// </summary>
	public class BindableListView : ListView, IBindable<BindableListView>
	{
		BindingManager<BindableListView> manager;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="context"></param>
        public BindableListView(Context context) : base(context)
        { 
            manager = new BindingManager<BindableListView>(this);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        public BindableListView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        { 
            manager = new BindingManager<BindableListView>(this);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        /// <param name="defStypeAttr"></param>
        public BindableListView(Context context, IAttributeSet attrs, int defStypeAttr)
            : base(context, attrs, defStypeAttr) 
        { 
            manager = new BindingManager<BindableListView>(this);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        /// <param name="defStypeAttr"></param>
        /// <param name="defStypeRes"></param>
        public BindableListView(Context context, IAttributeSet attrs, int defStypeAttr, int defStypeRes)
            : base(context, attrs, defStypeAttr, defStypeRes)
        { 
            manager = new BindingManager<BindableListView>(this);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="javaReference"></param>
        /// <param name="transfer"></param>
        protected BindableListView(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer) 
        { 
            manager = new BindingManager<BindableListView>(this);
        }

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
        public void SetBinding(BindableProperty<BindableListView> property, string sourcePropertyName, IBindingConverter converter = null)
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
        /// The bindable property for ListView.Clickable.
        /// </summary>
        public static BindableProperty<BindableListView> ClickableProperty { get; } = new BindableProperty<BindableListView, bool>(
			"Clickable",
			i => i.Clickable,
			(i, v) => i.Clickable = v,
			false);
	}
}


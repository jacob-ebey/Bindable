using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using System.Threading;

namespace Bindable.Core.Tests
{
    [TestClass]
    public class BindingTest
    {
        [TestMethod]
        public void TestBindings()
        {
            const string testData = "TestData";

            TestDataContext dataContext = new TestDataContext();
            TestBindableObject bindableObject = new TestBindableObject();
            bindableObject.DataContext = dataContext;
            bindableObject.SetBinding(TestBindableObject.LabelProperty, "Data");
            
            dataContext.Data = testData;
            Assert.AreEqual(testData, bindableObject.Label);
        }

        [TestMethod]
        public void TestDataContextChanged()
        {
            const string testData = "TestData";

            TestDataContext dataContext = new TestDataContext();
            TestBindableObject bindableObject = new TestBindableObject();
            bindableObject.SetBinding(TestBindableObject.LabelProperty, "Data");

            dataContext.Data = testData;
            bindableObject.DataContext = dataContext;

            Assert.AreEqual(testData, bindableObject.Label);
        }
    }

    public class TestDataContext : INotifyPropertyChanged
    {
        string data;
        public string Data
        {
            get { return data; }
            set
            {
                if (data == value) return;
                data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class TestBindableObject : IBindable<TestBindableObject>
    {
        BindingManager<TestBindableObject> manager;
        object dataContext;

        public TestBindableObject()
        {
            manager = new BindingManager<TestBindableObject>(this);
        }

        public string Label { get; set; }

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
        public void SetBinding(BindableProperty<TestBindableObject> property, string sourcePropertyName, IBindingConverter converter = null)
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

        public static BindableProperty<TestBindableObject> LabelProperty = new BindableProperty<TestBindableObject, string>(
            "Label",
            i => i.Label,
            (i, v) => i.Label = v);
    }
}

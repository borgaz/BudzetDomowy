using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Analisys
{
    class TestChart
    {

        public ObservableCollection<TestClass> Errors { get; private set; }

        public TestChart()
        {
            Errors = new ObservableCollection<TestClass>();
            Errors.Add(new TestClass() {Category = "Globalization", Number = 75});
            Errors.Add(new TestClass() {Category = "Features", Number = 2});
            Errors.Add(new TestClass() {Category = "ContentTypes", Number = 12});
            Errors.Add(new TestClass() {Category = "Correctness", Number = 83});
            Errors.Add(new TestClass() {Category = "Best Practices", Number = 29});
        }

        private object selectedItem = null;

        public object SelectedItem
        {
            get { return selectedItem; }
            set
            {
                // selected item has changed
                selectedItem = value;
            }
        }

        public class TestClass
        {
            public string Category { get; set; }

            public int Number { get; set; }
        }
    }
}

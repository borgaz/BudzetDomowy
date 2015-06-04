using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.Analisys
{
    class CategoryChart
    {

        public ObservableCollection<TestClass> Salary { get; private set; }
        public ObservableCollection<TestClass> Payments { get; private set; }

        public CategoryChart(DateTime start, DateTime end)
        {
            Payments = new ObservableCollection<TestClass>();
            Salary = new ObservableCollection<TestClass>();

            foreach (KeyValuePair<int, Main_Classes.Category> c in Main_Classes.Budget.Instance.Categories)
            {
                if (c.Value.Name != "-") //wyeliminowanie salda początkowego
                {
                    double sum = 0;
                    double sum2 = 0;
                    foreach (KeyValuePair<int, Main_Classes.Payment> s in Main_Classes.Budget.Instance.Payments)
                    {
                        if (!c.Value.Type) 
                            if (s.Value.GetType() == typeof(Main_Classes.SinglePayment))
                                if ((s.Value.CompareDateTo(start) >= 0) && (s.Value.CompareDateTo(end) <= 0))
                                    if (c.Key == s.Value.CategoryID)
                                        sum += s.Value.Amount;
                        if (c.Value.Type)
                            if (s.Value.GetType() == typeof(Main_Classes.SinglePayment))
                                if ((s.Value.CompareDateTo(start) >= 0) && (s.Value.CompareDateTo(end) <= 0))
                                    if (c.Key == s.Value.CategoryID)
                                        sum2 += s.Value.Amount;
                    }
                    if (sum != 0)
                        Payments.Add(new TestClass() { Category = c.Value.Name, Number = sum });
                    if (sum2 != 0)
                        Salary.Add(new TestClass() { Category = c.Value.Name, Number = sum2 });
                }
            }
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

            public double Number { get; set; }
        }
    }
}

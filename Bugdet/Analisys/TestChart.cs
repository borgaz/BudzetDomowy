﻿using System;
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
            foreach (KeyValuePair<int, Main_Classes.Category> c in Main_Classes.Budget.Instance.Categories)
            {
                double sum = 0;
                foreach (KeyValuePair<int, Main_Classes.Payment> s in Main_Classes.Budget.Instance.Payments)
                {
                    if (!c.Value.Type) 
                        if (s.Value.GetType() == typeof(Main_Classes.SinglePayment))
                            if (c.Key == s.Value.CategoryID)
                                sum += s.Value.Amount;

                }
                if (sum != 0)
                    Errors.Add(new TestClass() { Category = c.Value.Name, Number = sum });
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

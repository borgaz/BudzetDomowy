using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget.Main_Classes;

namespace Budget.Analisys
{
    class BalanceChart
    {

      //  public ObservableCollection<Now> Salary { get; private set; }
        public ObservableCollection<Now> Payments { get; private set; }
        public ObservableCollection<Now> Paymentsa { get; private set; }
        public ObservableCollection<Now> Paymentsb { get; private set; }

        public BalanceChart()
        {
            Payments = new ObservableCollection<Now>();
            Paymentsa = new ObservableCollection<Now>();
            Paymentsb = new ObservableCollection<Now>();
          //  Salary = new ObservableCollection<Now>();
            double suma = 0;
            double sum2a = 0;

            double sumb = 0;
            double sum2b = 0;

            double sumc = 0;
            double sum2c = 0;
                foreach (KeyValuePair<int, Main_Classes.Payment> s in Main_Classes.Budget.Instance.Payments)
                {
                    if (!s.Value.Type)
                        if (s.Value.GetType() == typeof (Main_Classes.SinglePayment))
                        {
                            var temp = (SinglePayment) s.Value;
                            if(temp.Date.Month == DateTime.Now.Month)
                                suma += s.Value.Amount;
                            if(temp.Date.Month == DateTime.Now.AddMonths(-1).Month)
                                sumb += s.Value.Amount;
                            if (temp.Date.Month == DateTime.Now.AddMonths(-2).Month)
                                sumc += s.Value.Amount;

                        }
                    if (s.Value.Type)
                        if (s.Value.GetType() == typeof (Main_Classes.SinglePayment))
                        {
                            var temp = (SinglePayment)s.Value;
                            if (temp.Date.Month == DateTime.Now.Month)
                                sum2a += s.Value.Amount;
                            if (temp.Date.Month == DateTime.Now.AddMonths(-1).Month)
                                sum2b += s.Value.Amount;
                            if (temp.Date.Month == DateTime.Now.AddMonths(-2).Month)
                                sum2c += s.Value.Amount;
                        }

                }
            if (suma != 0)
                Payments.Add(new Now() { Payment = "Wydatki", Number = suma });
            if (sum2a != 0)
                Payments.Add(new Now() { Payment = "Przychody", Number = sum2a });

            if (sumb != 0)
                Paymentsa.Add(new Now() { Payment = "Wydatki", Number = sumb });
            if (sum2b != 0)
                Paymentsa.Add(new Now() { Payment = "Przychody", Number = sum2b });

            if (sumc != 0)
                Paymentsb.Add(new Now() { Payment = "Wydatki", Number = sumc });
            if (sum2c != 0)
                Paymentsb.Add(new Now() { Payment = "Przychody", Number = sum2c });
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

        public class Now
        {
            public string Payment { get; set; }

            public double Number { get; set; }
        }
    }
}

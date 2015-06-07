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

        public DateTime DateOne { get; set; }
        public DateTime DateTwo { get; set; }
        public DateTime DateThree { get; set; }


        public BalanceChart(DateTime one,DateTime two,DateTime three)
        {
            Payments = new ObservableCollection<Now>();
            Paymentsa = new ObservableCollection<Now>();
            Paymentsb = new ObservableCollection<Now>();
            DateOne = one;
            DateTwo = two;
            DateThree = three;
          //  Salary = new ObservableCollection<Now>();
            double suma = 0;
            double sum2a = 0;

            double sumb = 0;
            double sum2b = 0;

            double sumc = 0;
            double sum2c = 0;
                foreach (KeyValuePair<int, Payment> s in Main_Classes.Budget.Instance.Payments)
                {
                    if (s.Value.GetType() != typeof (SinglePayment)) continue;
                    
                    if (s.Value.Type)
                    {
                        var temp = (SinglePayment) s.Value;
                        if (temp.Date.Month == DateOne.Month && temp.Date.Year == DateOne.Year)
                            suma += s.Value.Amount;
                        if (temp.Date.Month == DateTwo.Month && temp.Date.Year == DateTwo.Year)
                            sumb += s.Value.Amount;
                        if (temp.Date.Month == DateThree.Month && temp.Date.Year == DateThree.Year)
                            sumc += s.Value.Amount;
                    }
                    else
                    {
                        var temp = (SinglePayment) s.Value;
                        if (temp.Date.Month == DateOne.Month && temp.Date.Year == DateOne.Year)
                            sum2a += s.Value.Amount;
                        if (temp.Date.Month == DateTwo.Month && temp.Date.Year == DateTwo.Year)
                            sum2b += s.Value.Amount;
                        if (temp.Date.Month == DateThree.Month && temp.Date.Year == DateThree.Year)
                            sum2c += s.Value.Amount;
                    }

                }
            if (DateOne.Month > DateTime.Now.Month)
            {
                Payments.Add(new Now() { Payment = "Wydatki", Number = CountFutureMonth(DateOne,true) });
                Payments.Add(new Now() { Payment = "Przychody", Number = CountFutureMonth(DateOne, false) });
            }
            else
            {
                if (suma != 0)
                    Payments.Add(new Now() { Payment = "Wydatki", Number = suma });
                if (sum2a != 0)
                    Payments.Add(new Now() { Payment = "Przychody", Number = sum2a });
            }

            if (DateTwo.Month > DateTime.Now.Month)
            {
                Paymentsa.Add(new Now() { Payment = "Wydatki", Number = CountFutureMonth(DateTwo, true) });
                Paymentsa.Add(new Now() { Payment = "Przychody", Number = CountFutureMonth(DateTwo, false) });
            }
            else
            {
                if (sumb != 0)
                    Paymentsa.Add(new Now() { Payment = "Wydatki", Number = sumb });
                if (sum2b != 0)
                    Paymentsa.Add(new Now() { Payment = "Przychody", Number = sum2b });    
            }

            if (DateThree.Month > DateTime.Now.Month)
            {
                Paymentsb.Add(new Now() { Payment = "Wydatki", Number = CountFutureMonth(DateThree, true) });
                Paymentsb.Add(new Now() { Payment = "Przychody", Number = CountFutureMonth(DateThree, false) });
            }
            else
            {
                if (sumc != 0)
                    Paymentsb.Add(new Now() { Payment = "Wydatki", Number = sumc });
                if (sum2c != 0)
                    Paymentsb.Add(new Now() { Payment = "Przychody", Number = sum2c });
            }

        }

        private DateTime AddDateFrequency(DateTime date, string freq,int times)
        {
            if (freq.Equals("DZIEŃ"))
                return date.AddDays(times);
            if (freq.Equals("TYDZIEŃ"))
                return date.AddDays(times * 7);
            if (freq.Equals("MIESIĄC"))
                return date.AddMonths(times);
            if (freq.Equals("ROK")) 
                return date.AddYears(times);
            else
            {
                return DateTime.Now;
            }
        }
        /// <summary>
        /// return salary prediction on month from parameter
        /// </summary>
        /// <param name="fUtUrE">DateTime with month you want to predict</param>
        /// <param name="payment">true for payment, false for salaries</param>
        /// <returns>prediction value</returns>
        public Int32 CountFutureMonth(DateTime fUtUrE,bool payment)
        {
            var result = 0.0; // ile bedzie periodow w przyszlym miesiacu
            var singleResult = 0.0; // suma realnych singlePaymentsow z 3 miesiecy wstecz
            var months = 0; // ilosc miesiacy w ktorych byly dane
            var tempDate = new DateTime(); // zmienna daty pomocnicza
            tempDate = DateTime.Now.AddMonths(-1);

            foreach (var p in Main_Classes.Budget.Instance.Payments)
            {
                if (p.Value.GetType() == typeof (SinglePayment) || p.Value.Type == !payment) continue;
                var pp = (PeriodPayment) p.Value;
                var pDate = DateTime.Now.AddMonths(1);
                var pLast = pp.LastUpdate;
                while (pLast.Month < DateTime.Now.AddMonths(2).Month)
                {
                    if (pLast.Month == pDate.Month && pLast.Year == pDate.Year) // gdy akurat mamy ten miesiac
                        result += pp.Amount;
                    if (pp.EndDate > AddDateFrequency(pLast, pp.Period, pp.Frequency)) // czy koniec periodu nie wykracza poza kolejna "rate"
                        pLast = AddDateFrequency(pLast, pp.Period, pp.Frequency); // dodanie odstepu do kolejnej raty
                    else break;
                }
            }
            
            while (tempDate > DateTime.Now.AddMonths(-4))
            {
                var avg = 0.0; // srednia arytmetyczna
                var actualAvg = 0.0; // srednia po odjeciu wartosci wykraczajacych poza odchylenie standartowe
                var anyPayment = 0; // zmienna pomocnicza
                var variationList = new List<double>(); // lista wartosci do liczenia odchylenia standartowego
                foreach (var p in Main_Classes.Budget.Instance.Payments)
                {
                    if (p.Value.GetType() == typeof(PeriodPayment) || p.Value.Type == !payment) continue;
                    var pp = (SinglePayment) p.Value;
                    if (pp.Date.Month != tempDate.Month || pp.Date.Year != tempDate.Year)
                        continue;
                    avg += pp.Amount;
                    variationList.Add(pp.Amount);
                    anyPayment++;
                }
                avg /= anyPayment; // liczenie sredniej arytmetycznej
                var varSum = variationList.Sum(v => Math.Exp(v - avg)); // liczenie sumy roznic kwadratow wartosci i sredniej
                varSum = Math.Sqrt(varSum/(anyPayment - 1)); // pierwiastek z sumy/n-1
                foreach (var p in Main_Classes.Budget.Instance.Payments)
                {
                    if (p.Value.GetType() == typeof(PeriodPayment) || p.Value.Type == !payment) continue;
                    var pp = (SinglePayment)p.Value;
                    if (pp.Date.Month != tempDate.Month || pp.Date.Year != tempDate.Year)
                        continue;
                    if (!IsInRange(pp.Amount, avg, varSum)) continue; // jesli wartosc wykracza poza odchylenie standartowe
                    // if (pp.Amount / avg > 0.4 && pp.Amount / avg != 1.0) continue;
                    actualAvg += pp.Amount; // dodanie do rzeczywistej sredniej
                }
                tempDate = tempDate.AddMonths(-1);
                singleResult += actualAvg; // dodanie do koncowego wyniku z miesiaca
                if (anyPayment != 0) // jesli nie bylo zadnych wydatkow/przychodow
                    months++;
            }
            if (months == 0)
                return 0;
            singleResult /= months; // wyliczenie sredniej arytmetycznej z miesiecy z ktorych mamy dane
            result += singleResult;
            return Convert.ToInt32(result);  
        }
        /// <summary>
        /// Returns if 'a' is in range of [b - c ; b + c] 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool IsInRange(double a, double b, double c)
        {
            return a >= b - c && a <= b + c;
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

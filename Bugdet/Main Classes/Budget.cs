using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Budget.Utility_Classes;
using ComboBoxItem = Budget.Utility_Classes.ComboBoxItem;

// First part of Budget class - constructor, properties, minor methods
namespace Budget.Main_Classes
{
    public sealed partial class Budget
    {
        private static Budget instance = null;

        private String name; // nazwa budzetu
        private String note; // notatka
        private String password; // haslo do budzetu
        private Dictionary<int, Payment> payments; // slownik platnosci // minus dla period, plus dla single
        private Dictionary<int, Category> categories; // slownik kategorii
        private Dictionary<int, SavingsTarget> savingsTargets; //slownik celow, na ktore oszczedzamy
        private Dictionary<int, BalanceLog> balanceLogs; // słownik logow
        private double balance; // saldo
        private DateTime creationDate; // data stworzenia budzetu
        private int numberOfPeople; // ilosc osob, dla ktorych prowadzony jest budzet domowy
        private List<Changes> listOfAdds; //lista dodanych rekordów
        private List<Changes> listOfDels; //lista usuniętych rekordów
        private List<Changes> listOfEdts; //lista zedytowanych rekordów
        private DateTime minDate; // najwcześniejsza data płatności/wydatku w bazie
        private DateTime maxDate; // najpóźniejsza data płatności/wydatku w bazie
        private double maxAmount; // maksymalna kwota płatności/wydatku w bazie

        public enum CategoryTypeEnum
        {
          PAYMENT,SALARY,ANY }
        public override string ToString()
        {
            return "NAME: " + name + ", NOTE: " + note + ", PASSWORD: " + password + ", NUMBER_OF_PEOPLE: " + numberOfPeople
                + ", CREATION_DATE: " + creationDate + ", BALANCE: " + balance + "\n";
        }

        private Budget(String note, String name, String password, Dictionary<int, Payment> payments,
            Dictionary<int, Category> categories, Dictionary<int, SavingsTarget> savingsTargets,
            Dictionary<int, BalanceLog> balanceLogs, double balance, int numberOfPeople, DateTime creationDate,
            DateTime minDate, DateTime maxDate, double maxAmount)
        {
            this.note = note;
            this.name = name;
            this.password = password;
            this.payments = payments;
            this.categories = categories;
            this.savingsTargets = savingsTargets;
            this.balanceLogs = balanceLogs;
            this.numberOfPeople = numberOfPeople;
            this.balance = balance;
            this.creationDate = creationDate;
            listOfAdds = new List<Changes>();
            listOfDels = new List<Changes>();
            listOfEdts = new List<Changes>();
            this.minDate = minDate;
            this.maxDate = maxDate;
            this.maxAmount = maxAmount;
        }

        public static Budget Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = FetchAll();
                }
                return instance;
            }
        }

        public static void ResetInstance()
        {
            instance = FetchAll();
        }
        public double MaxAmount
        {
            get
            {
                return maxAmount;
            }
        }

        public DateTime MaxDate
        {
            get
            {
                return maxDate;
            }
        }

        public DateTime MinDate
        {
            get
            {
                return minDate;
            }
        }

        public List<Changes> ListOfAdds
        {
            get
            {
                return this.listOfAdds;
            }
        }

        public List<Changes> ListOfDels
        {
            get
            {
                return this.listOfDels;
            }
        }

        public List<Changes> ListOfEdts
        {
            get
            {
                return this.listOfEdts;
            }
        }

        public DateTime CreationDate
        {
            get
            {
                return this.creationDate;
            }
        }

        public String Note
        {
            set
            {
                note = value;
            }
            get
            {
                return this.note;
            }
        }

        public String Name
        {
            get
            {
                return this.name;
            }
        }

        public String Password
        {
            get
            {
                return this.password;
            }
        }

        public int NumberOfPeople
        {
            get
            {
                return this.numberOfPeople;
            }
            set
            {
                this.numberOfPeople = value;
            }
        }

        public double Balance
        {
            get
            {
                return balance;
            }
        }
        public Dictionary<int, BalanceLog> BalanceLog
        {
            get
            {
                return this.balanceLogs;
            }
        }

        public Dictionary<int, Payment> Payments
        {
            get
            {
                return this.payments;
            }
        }

        public Dictionary<int, Category> Categories
        {
            get
            {
                return this.categories;
            }
        }

        public Dictionary<int, SavingsTarget> SavingsTargets
        {
            get
            {
                return this.savingsTargets;
            }
        }

        public void AddSinglePayment(int index, SinglePayment payment)
        {
            // prace nad dodawaniem singlepays z inną datą

            //int key = 1;
            //foreach (KeyValuePair<int, Payment>s in payments.Reverse() )
            //{
            //    if (s.Value.GetType() == typeof(SinglePayment))
            //    {
            //        SinglePayment sp = (SinglePayment)s.Value;
            //        if (DateTime.Compare(sp.Date, payment.Date) <= 0)
            //        {
            //            key = s.Key + 1;
            //            break;
            //        }
            //        else
            //    }
            //}
            //Console.WriteLine(key);
            //payments.Add(key, payment);
            //ListOfAdds.Add(new Changes(typeof(SinglePayment), key));


            payments.Add(index, payment);
            ListOfAdds.Add(new Changes(typeof(SinglePayment), index));
        }

        public void DeleteSinglePayment(int indexSinglePayment, int indexBalanceLog)
        {
            payments.Remove(indexSinglePayment);
            balanceLogs.Remove(indexBalanceLog);
            ListOfDels.Add(new Changes(typeof(SinglePayment), indexSinglePayment));
        }

        public void AddSavingsTarget(int index, SavingsTarget target)
        {
            savingsTargets.Add(index, target);
            ListOfAdds.Add(new Changes(typeof(SavingsTarget), index));

        }

        public void DeleteSavingsTarget(int index)
        {
            savingsTargets.Remove(index);
            ListOfDels.Add(new Changes(typeof(SavingsTarget), index));
        }

        public void AddPeriodPayment(int index, PeriodPayment payment)
        {
            payments.Add(index, payment);
            ListOfAdds.Add(new Changes(typeof(PeriodPayment), index));
        }

        public void DeletePeriodPayment(int index)
        {
            payments.Remove(index);
            ListOfDels.Add(new Changes(typeof(PeriodPayment), index));
        }

        public void AddBalanceLog(int index, BalanceLog log)
        {
            balanceLogs.Add(index, log);
            ListOfAdds.Add(new Changes(typeof(BalanceLog), index));
        }

        // BalanceLog usuwa sie automatycznie po usunieciu skojarzonego singlepaymentid

        //public void DeleteBalanceLog(int index)
        //{
        //    balanceLogs.Remove(index);
        //    ListOfDels.Add(new Changes(typeof(BalanceLog), index));
        //}

        /// <summary>
        /// Inserts Categories to ComboBox
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="type"></param>
        public void InsertCategories(ComboBox comboBox, CategoryTypeEnum type)
        {
            comboBox.Items.Clear();
            try
            {
                bool catType = false || type == CategoryTypeEnum.SALARY;
                foreach (KeyValuePair<int,Category> c in categories)
                {
                    if (c.Value.Type == catType || type == CategoryTypeEnum.ANY)
                    {
                        comboBox.Items.Add(new ComboBoxItem(c.Key, c.Value.Name));
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(categories.Count + "");
                Console.WriteLine(categories.ToArray());
            }
        }

    }
}
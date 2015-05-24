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
        private List<Changes> listOfAdds; //lista dodanych rekordów
        private List<Changes> listOfDels; //lista usuniętych rekordów
        private List<Changes> listOfEdits; //lista zedytowanych rekordów
        private DateTime minDate; // najwcześniejsza data płatności/wydatku w bazie
        private DateTime maxDate; // najpóźniejsza data płatności/wydatku w bazie
        private double maxAmount; // maksymalna kwota płatności/wydatku w bazie

        public enum CategoryTypeEnum
        {
            PAYMENT,SALARY,ANY // co to?
        }

        public override string ToString()
        {
            return "NAME: " + name + ", NOTE: " + note + ", PASSWORD: " + password +
                ", CREATION_DATE: " + creationDate + ", BALANCE: " + balance + "\n";
        }

        private Budget(String note, String name, String password, Dictionary<int, Payment> payments,
            Dictionary<int, Category> categories, Dictionary<int, SavingsTarget> savingsTargets,
            Dictionary<int, BalanceLog> balanceLogs, double balance, DateTime creationDate,
            DateTime minDate, DateTime maxDate, double maxAmount)
        {
            this.note = note;
            this.name = name;
            this.password = password;
            this.payments = payments;
            this.categories = categories;
            this.savingsTargets = savingsTargets;
            this.balanceLogs = balanceLogs;
            this.balance = balance;
            this.creationDate = creationDate;
            listOfAdds = new List<Changes>();
            listOfDels = new List<Changes>();
            listOfEdits = new List<Changes>();
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

        public List<Changes> ListOfEdits
        {
            get
            {
                return this.listOfEdits;
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

        public double Balance
        {
            get
            {
                this.balance = this.balanceLogs[this.balanceLogs.Keys.Max()].Balance;
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

        private void CheckPayment(SinglePayment payment,int delete)
        {
            if (!payment.Date.Month.Equals(DateTime.Now.Month)) return;
            if (payment.Type)
                SqlConnect.Instance.monthlyPayments = (delete == 1 ? SqlConnect.Instance.monthlyPayments - payment.Amount : SqlConnect.Instance.monthlyPayments + payment.Amount);
            else
                SqlConnect.Instance.monthlySalaries = (delete == 1 ? SqlConnect.Instance.monthlySalaries - payment.Amount : SqlConnect.Instance.monthlySalaries + payment.Amount);
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
            CheckPayment(payment,0);

            payments.Add(index, payment);
            ListOfAdds.Add(new Changes(typeof(SinglePayment), index));

            if (DateTime.Compare(payment.Date, DateTime.Now) <= 0 )
            {
                if (payment.Type == false)
                {
                    int balanceMaxKey = BalanceLog.Keys.Max();
                    int tempIdBalance = balanceMaxKey + 1;
                    double currentBalance = BalanceLog[balanceMaxKey].Balance + payment.Amount;
                    AddBalanceLog(tempIdBalance, new BalanceLog(currentBalance, DateTime.Today, index, 0));
                }
                else
                {
                    int temp_id_balance = BalanceLog.Keys.Max() + 1;
                    double currentBalance = BalanceLog.Last().Value.Balance - payment.Amount;
                    AddBalanceLog(temp_id_balance, new BalanceLog(currentBalance, DateTime.Today, index, 0));
                }
            }
        }

        public void EditSinglePayment(int index, SinglePayment payment, double amountBeforeChange)
        {
            double amountToRefactor;
            if (this.payments[index].Type == false)
                amountToRefactor = this.payments[index].Amount - amountBeforeChange;
            else
                amountToRefactor = amountBeforeChange - this.payments[index].Amount;

            foreach (KeyValuePair<int, BalanceLog> b in this.balanceLogs)
            {
                if (b.Value.SinglePaymentID >= index)
                    b.Value.Balance += amountToRefactor;
            }
            ListOfEdits.Add(new Changes(typeof(SinglePayment), index));
        }

        public void DeleteSinglePayment(int indexSinglePayment, int indexBalanceLog)
        {
            double amountToRefactor = this.payments[indexSinglePayment].Amount;
            if (this.payments[indexSinglePayment].Type == false)
                amountToRefactor = (-1) * amountToRefactor;
            foreach (KeyValuePair<int, BalanceLog> b in this.balanceLogs)
            {
                if (b.Value.SinglePaymentID > indexSinglePayment)
                    b.Value.Balance += amountToRefactor;
            }
            CheckPayment((SinglePayment)Payments[indexSinglePayment], 1);

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

        public void EditPeriodPayment(int index, PeriodPayment payment)
        {
            ListOfEdits.Add(new Changes(typeof(PeriodPayment), index));
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
            }
        }

        /// <summary>
        /// Checks which period payments should be transformed into a single payments.
        /// </summary>
        /// <returns>
        /// List of transformed payemnts.
        /// </returns>
        public List<SinglePayment> CheckPeriodPayments()
        {
            List<SinglePayment> editList = new List<SinglePayment>();
            int periodCount = 0;
            foreach (KeyValuePair<int, Payment> p in payments)
            {
                if (p.Value.GetType() == typeof(PeriodPayment))
                {
                    try
                    {
                        periodCount = p.Value.CountPeriods();
                        int tempCount = periodCount;
                        //if (periodCount > 0)
                        //{
                            while (periodCount > 0)
                            {
                                editList.Add(p.Value.CreateSingleFromPeriod(periodCount));
                                periodCount--;
                            }
                        //}
                        if (tempCount > 0)
                        {
                            p.Value.changeUpdateDate(tempCount);
                            Instance.EditPeriodPayment(p.Key, (PeriodPayment)p.Value);
                        }
                    }
                    catch (NotImplementedException ex) { }
                }
            }
            return editList;
        }

        /// <summary>
        /// Checks which single payments from the future should be added to balance logs.
        /// </summary>
        public void FutureSinglePaymentsCheck()
        {
            foreach (KeyValuePair<int, Payment> entry in Payments)
            {
                if (entry.Value.GetType() == typeof(SinglePayment))
                {
                    if (entry.Value.CompareDate() <= 0)
                    {
                        bool found = false; 

                        foreach (BalanceLog balance in balanceLogs.Values)
                        {
                            if (balance.SinglePaymentID == entry.Key)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (found == false)
                        {
                            if (entry.Value.Type == false)
                            {
                                int balanceMaxKey = BalanceLog.Keys.Max();
                                int tempIdBalance = balanceMaxKey + 1;
                                double currentBalance = BalanceLog[balanceMaxKey].Balance + entry.Value.Amount;
                                AddBalanceLog(tempIdBalance, new BalanceLog(currentBalance, DateTime.Today, entry.Key, 0));
                            }
                            else
                            {
                                int temp_id_balance = BalanceLog.Keys.Max() + 1;
                                double currentBalance = BalanceLog.Last().Value.Balance - entry.Value.Amount;
                                AddBalanceLog(temp_id_balance, new BalanceLog(currentBalance, DateTime.Today, entry.Key, 0));
                            }
                        }
                    }
                }
            }
        }

        public Boolean CheckCategory(string name)
        {
            var contains = false;
            foreach (Category c in categories.Values)
            {
                if (c.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                    contains = true;
            }
            return contains;
        }
    }
}
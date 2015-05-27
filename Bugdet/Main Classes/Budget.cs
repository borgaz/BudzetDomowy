using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

// First part of Budget class - constructor, properties, minor methods
namespace Budget.Main_Classes
{
    public sealed partial class Budget : ViewModelBase
    {
        private static Budget instance = null;

        private String name; // nazwa budzetu
        private String note; // notatka
        private String password; // haslo do budzetu
        private double maxAmount; // maksymalna kwota płatności/wydatku w bazie
        private double balance; // saldo
        private Dictionary<int, Payment> payments; // slownik platnosci // indeksowanie: (-INF, -1> period, <1, INF) single
        private Dictionary<int, Category> categories; // slownik kategorii
        private Dictionary<int, SavingsTarget> savingsTargets; //slownik celow, na ktore oszczedzamy
        private Dictionary<int, BalanceLog> balanceLogs; // słownik logow
        private List<Utility_Classes.Changes> listOfAdds; //lista dodanych rekordów
        private List<Utility_Classes.Changes> listOfDels; //lista usuniętych rekordów
        private List<Utility_Classes.Changes> listOfEdits; //lista zedytowanych rekordów
        private DateTime creationDate; // data stworzenia budzetu 
        private DateTime minDate; // najwcześniejsza data płatności/wydatku w bazie
        private DateTime maxDate; // najpóźniejsza data płatności/wydatku w bazie
        public enum CategoryTypeEnum
        {
            PAYMENT,
            SALARY,
            ANY
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
            listOfAdds = new List<Utility_Classes.Changes>();
            listOfDels = new List<Utility_Classes.Changes>();
            listOfEdits = new List<Utility_Classes.Changes>();
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

        private void CheckPayment(SinglePayment payment, int delete)
        {
            if (payment.Date.Month.Equals(DateTime.Now.Month))
            {
                if (payment.Type)
                {
                    SqlConnect.Instance.monthlyPayments = (delete == 1 ? SqlConnect.Instance.monthlyPayments - payment.Amount : SqlConnect.Instance.monthlyPayments + payment.Amount);
                }   
                else
                {
                    SqlConnect.Instance.monthlySalaries = (delete == 1 ? SqlConnect.Instance.monthlySalaries - payment.Amount : SqlConnect.Instance.monthlySalaries + payment.Amount);
                }  
            }
        }

        public void AddSinglePayment(int index, SinglePayment payment)
        {
            int balanceMaxKey;
            double currentBalance;

            CheckPayment(payment, 0);
            payments.Add(index, payment);
            ListOfAdds.Add(new Utility_Classes.Changes(typeof(SinglePayment), index));
            if (payment.Amount > maxAmount)
            {
                maxAmount = payment.Amount;
                if (SettingsPage.Settings.Instance.Serializable == false)
                {
                    SettingsPage.Settings.Instance.SH_AmountTo = maxAmount;
                    SettingsPage.Settings.Instance.PP_AmountTo = maxAmount;
                }
            }

            if (DateTime.Compare(payment.Date, DateTime.Now) <= 0 )
            {
                balanceMaxKey = BalanceLog.Keys.Max() + 1;
                if (payment.Type == false)
                {
                    currentBalance = BalanceLog.Last().Value.Balance + payment.Amount;
                }
                else
                {
                    currentBalance = BalanceLog.Last().Value.Balance - payment.Amount;
                }
                AddBalanceLog(balanceMaxKey, new BalanceLog(currentBalance, DateTime.Today, index, 0));
            }
        }

        public void EditSinglePayment(int index, SinglePayment payment, double amountBeforeChange)
        {
            double amountToRefactor;
            if (this.payments[index].Type == false)
            {
                amountToRefactor = this.payments[index].Amount - amountBeforeChange;
            }
            else
            {
                amountToRefactor = amountBeforeChange - this.payments[index].Amount;
            }
            foreach (KeyValuePair<int, BalanceLog> b in this.balanceLogs)
            {
                if (b.Value.SinglePaymentID >= index)
                {
                    b.Value.Balance += amountToRefactor;
                }    
            }
            ListOfEdits.Add(new Utility_Classes.Changes(typeof(SinglePayment), index));
        }

        public void DeleteSinglePayment(int indexSinglePayment, int indexBalanceLog)
        {
            double amountToRefactor = this.payments[indexSinglePayment].Amount;
            if (this.payments[indexSinglePayment].Type == false)
            {
                amountToRefactor = (-1) * amountToRefactor;
            }  
            foreach (KeyValuePair<int, BalanceLog> b in this.balanceLogs)
            {
                if (b.Value.SinglePaymentID > indexSinglePayment)
                {
                    b.Value.Balance += amountToRefactor;
                }           
            }
            CheckPayment((SinglePayment)Payments[indexSinglePayment], 1);
            payments.Remove(indexSinglePayment);
            balanceLogs.Remove(indexBalanceLog);
            ListOfDels.Add(new Utility_Classes.Changes(typeof(SinglePayment), indexSinglePayment));
        }

        public void AddSavingsTarget(int index, SavingsTarget target)
        {
            savingsTargets.Add(index, target);
            ListOfAdds.Add(new Utility_Classes.Changes(typeof(SavingsTarget), index));
        }

        public void DeleteSavingsTarget(int index)
        {
            savingsTargets.Remove(index);
            ListOfDels.Add(new Utility_Classes.Changes(typeof(SavingsTarget), index));
        }

        public void AddPeriodPayment(int index, PeriodPayment payment)
        {
            if (payment.Amount > maxAmount)
            {
                maxAmount = payment.Amount;
                if (SettingsPage.Settings.Instance.Serializable == false)
                {
                    SettingsPage.Settings.Instance.SH_AmountTo = maxAmount;
                    SettingsPage.Settings.Instance.PP_AmountTo = maxAmount;
                }
            }
            payments.Add(index, payment);
            ListOfAdds.Add(new Utility_Classes.Changes(typeof(PeriodPayment), index));
        }

        public void EditPeriodPayment(int index, PeriodPayment payment)
        {
            ListOfEdits.Add(new Utility_Classes.Changes(typeof(PeriodPayment), index));
        }

        public void DeletePeriodPayment(int index)
        {
            payments.Remove(index);
            ListOfDels.Add(new Utility_Classes.Changes(typeof(PeriodPayment), index));
        }

        public void AddBalanceLog(int index, BalanceLog log)
        {
            balanceLogs.Add(index, log);
            ListOfAdds.Add(new Utility_Classes.Changes(typeof(BalanceLog), index));
            OnPropertyChanged("BalanceLog");
        }

        /// <summary>
        /// Inserts Categories to ComboBox
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="type"></param>
        public void InsertCategories(ComboBox comboBox, CategoryTypeEnum type)
        {
            bool catType = (false || type == CategoryTypeEnum.SALARY);
            
            comboBox.Items.Clear();
            foreach (KeyValuePair<int,Category> c in categories)
            {
                if (c.Value.Type == catType || type == CategoryTypeEnum.ANY)
                {
                    comboBox.Items.Add(new Utility_Classes.ComboBoxItem(c.Key, c.Value.Name));
                }
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
            int periodCount, tempCount;
            List<SinglePayment> editList = new List<SinglePayment>();
           
            foreach (KeyValuePair<int, Payment> p in payments)
            {
                if (p.Value.GetType() == typeof(PeriodPayment))
                {
                    PeriodPayment pP = (PeriodPayment) p.Value;
                    tempCount = periodCount = pP.CountPeriods();  
                    while (periodCount > 0)
                    {
                        editList.Add(pP.CreateSingleFromPeriod(periodCount));
                        periodCount--;
                    } 
                    if (tempCount > 0)
                    {
                        pP.changeUpdateDate(tempCount);
                        Instance.EditPeriodPayment(p.Key, pP);
                    }
                }
            }
            return editList;
        }

        /// <summary>
        /// Checks which single payments from the future should be added to balance logs.
        /// </summary>
        public void FutureSinglePaymentsCheck()
        {
            bool found;
            double currentBalance;

            foreach (KeyValuePair<int, Payment> entry in Payments)
            {
                if (entry.Value.GetType() == typeof(SinglePayment))
                {
                    if (entry.Value.CompareDate() <= 0)
                    {
                        found = false;
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
                            int balanceMaxKey = BalanceLog.Keys.Max() + 1;
                            if (entry.Value.Type == false)
                            {
                                currentBalance = BalanceLog.Last().Value.Balance + entry.Value.Amount;
                            }
                            else
                            {
                                currentBalance = BalanceLog.Last().Value.Balance - entry.Value.Amount;
                            }
                            AddBalanceLog(balanceMaxKey, new BalanceLog(currentBalance, DateTime.Today, entry.Key, 0));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sprawdza, czy kategoria, ktora chcemy dodac, jest juz w slowniku
        /// </summary>
        public Boolean CheckCategory(string name)
        {
            foreach (Category c in categories.Values)
            {
                if (c.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    return true;
                }   
            }
            return false;
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

        public List<Utility_Classes.Changes> ListOfAdds
        {
            get
            {
                return this.listOfAdds;
            }
        }

        public List<Utility_Classes.Changes> ListOfDels
        {
            get
            {
                return this.listOfDels;
            }
        }

        public List<Utility_Classes.Changes> ListOfEdits
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
    }
}
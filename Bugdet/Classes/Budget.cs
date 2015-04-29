using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Budget.Classes
{
    public sealed class Budget
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
            payments.Add(index, payment);
            ListOfAdds.Add(new Changes(typeof(SinglePayment), index));
        }

        public void DeleteSinglePayment(int index)
        {
            payments.Remove(index);
            ListOfDels.Add(new Changes(typeof(SinglePayment),index));
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

        public void DeleteBalanceLog(int index)
        {
            balanceLogs.Remove(index);
            ListOfDels.Add(new Changes(typeof(BalanceLog), index));
        }
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

        private static Budget FetchAll()
        {
            try
            {
                /////////////////////////////////////////////////////////////////////////////////////////////
                // Budzetu
                /////////////////////////////////////////////////////////////////////////////////////////////

                String name = String.Empty;
                String note = String.Empty;
                String password = String.Empty;
                double maximumAmount = 0;
                DateTime creationDate = DateTime.Now;
                int numberOfPeople = 0;
                double balance = 0;
                DateTime maximumDate = new DateTime(1900, 01, 01);
                DateTime minimumDate = new DateTime(2100, 12, 31);

                System.Data.DataSet nameFromSelect = SqlConnect.Instance.SelectQuery("SELECT * FROM Budget");
                if (nameFromSelect.Tables[0].Rows.Count > 0)
                {
                    name = Convert.ToString(nameFromSelect.Tables[0].Rows[0]["name"]);
                    note = Convert.ToString(nameFromSelect.Tables[0].Rows[0]["note"]);
                    balance = Convert.ToDouble(nameFromSelect.Tables[0].Rows[0]["balance"]);
                    password = Convert.ToString(nameFromSelect.Tables[0].Rows[0]["password"]);
                    creationDate = Convert.ToDateTime(nameFromSelect.Tables[0].Rows[0]["creation"]);
                    numberOfPeople = Convert.ToInt32(nameFromSelect.Tables[0].Rows[0]["numberOfPeople"]);
                }

                /////////////////////////////////////////////////////////////////////////////////////////////
                // Lista kategorii
                /////////////////////////////////////////////////////////////////////////////////////////////

                Dictionary<int, Category> categories = new Dictionary<int, Category>();
                System.Data.DataSet categoriesFromSelect = SqlConnect.Instance.SelectQuery("SELECT * FROM Categories");
                for (int i = 0; i < categoriesFromSelect.Tables[0].Rows.Count; i++)
                {
                    categories.Add(Convert.ToInt32(categoriesFromSelect.Tables[0].Rows[i]["id"]),
                    new Category(Convert.ToString(categoriesFromSelect.Tables[0].Rows[i]["name"]),
                        Convert.ToString(categoriesFromSelect.Tables[0].Rows[i]["note"]),
                        Convert.ToBoolean(categoriesFromSelect.Tables[0].Rows[i]["type"])
                        ));
                }

                /////////////////////////////////////////////////////////////////////////////////////////////
                // Aktualny stan konta
                /////////////////////////////////////////////////////////////////////////////////////////////

                Dictionary<int, BalanceLog> balanceLogs = new Dictionary<int, BalanceLog>();
                System.Data.DataSet balanceLogsFromSelect = SqlConnect.Instance.SelectQuery("SELECT * FROM BalanceLogs");
                for (int i = 0; i < balanceLogsFromSelect.Tables[0].Rows.Count; i++)
                {
                    balanceLogs.Add(Convert.ToInt32(balanceLogsFromSelect.Tables[0].Rows[i]["id"]),
                        new BalanceLog(Convert.ToDouble(balanceLogsFromSelect.Tables[0].Rows[i]["balance"]),
                        Convert.ToDateTime(balanceLogsFromSelect.Tables[0].Rows[i]["date"]),
                        Convert.ToInt32(balanceLogsFromSelect.Tables[0].Rows[i]["singlePaymentId"]),
                        Convert.ToInt32(balanceLogsFromSelect.Tables[0].Rows[i]["periodPaymentId"])
                        ));
                }

                /////////////////////////////////////////////////////////////////////////////////////////////
                // Lista płatności
                /////////////////////////////////////////////////////////////////////////////////////////////

                Dictionary<int, Payment> payments = new Dictionary<int, Payment>();
                System.Data.DataSet periodPayFromSelect = SqlConnect.Instance.SelectQuery("SELECT * FROM PeriodPayments");
                for (int i = 0; i < periodPayFromSelect.Tables[0].Rows.Count; i++)
                {
                    payments.Add(Convert.ToInt32(periodPayFromSelect.Tables[0].Rows[i]["id"]),
                        new PeriodPayment(Convert.ToInt32(periodPayFromSelect.Tables[0].Rows[i]["categoryId"]),
                        Convert.ToDouble(periodPayFromSelect.Tables[0].Rows[i]["amount"]),
                        Convert.ToString((periodPayFromSelect.Tables[0].Rows[i]["note"])),
                        Convert.ToBoolean(periodPayFromSelect.Tables[0].Rows[i]["type"]),
                        Convert.ToString(periodPayFromSelect.Tables[0].Rows[i]["name"]),
                        Convert.ToInt32(periodPayFromSelect.Tables[0].Rows[i]["frequency"]),
                        Convert.ToString(periodPayFromSelect.Tables[0].Rows[i]["period"]),
                        Convert.ToDateTime(periodPayFromSelect.Tables[0].Rows[i]["lastUpdate"]),
                        Convert.ToDateTime(periodPayFromSelect.Tables[0].Rows[i]["startDate"]),
                        Convert.ToDateTime(periodPayFromSelect.Tables[0].Rows[i]["endDate"])
                        ));
                }

             //   int nOPP = periodPayFromSelect.Tables[0].Rows.Count; // numberOfPeriodPayments - potrzebny, zeby nie pokrywaly sie ID pejmentsow

                System.Data.DataSet singlePayFromSelect = SqlConnect.Instance.SelectQuery("SELECT * FROM SinglePayments");
                for (int i = 0; i < singlePayFromSelect.Tables[0].Rows.Count; i++)
                {
                    double amount = Convert.ToDouble(singlePayFromSelect.Tables[0].Rows[i]["amount"]);
                    if (amount > maximumAmount)
                        maximumAmount = amount;

                    DateTime date = Convert.ToDateTime(singlePayFromSelect.Tables[0].Rows[i]["date"]);
                    if (DateTime.Compare(date,minimumDate) < 0)
                        minimumDate = date;

                    if (DateTime.Compare(date, maximumDate) > 0)
                        maximumDate = date;

                    payments.Add(Convert.ToInt32(singlePayFromSelect.Tables[0].Rows[i]["id"]),
                        new SinglePayment(Convert.ToString(singlePayFromSelect.Tables[0].Rows[i]["note"]),
                        amount,
                        Convert.ToInt32(singlePayFromSelect.Tables[0].Rows[i]["categoryId"]),
                        Convert.ToBoolean(singlePayFromSelect.Tables[0].Rows[i]["type"]),
                        Convert.ToString(singlePayFromSelect.Tables[0].Rows[i]["name"]),
                        date
                        ));
                }

                /////////////////////////////////////////////////////////////////////////////////////////////
                // Cele oszczędzania
                /////////////////////////////////////////////////////////////////////////////////////////////

                Dictionary<int, SavingsTarget> savingsTargets = new Dictionary<int, SavingsTarget>();
                System.Data.DataSet savTargetsFromSelect = SqlConnect.Instance.SelectQuery("SELECT * FROM SavingsTargets");
                for (int i = 0; i < savTargetsFromSelect.Tables[0].Rows.Count; i++)
                {
                    savingsTargets.Add(Convert.ToInt32(savTargetsFromSelect.Tables[0].Rows[i]["id"]),
                        new SavingsTarget(Convert.ToString(savTargetsFromSelect.Tables[0].Rows[i]["target"]),
                        Convert.ToString(savTargetsFromSelect.Tables[0].Rows[i]["note"]),
                        Convert.ToDateTime(savTargetsFromSelect.Tables[0].Rows[i]["deadLine"]),
                        Convert.ToInt32(savTargetsFromSelect.Tables[0].Rows[i]["priority"]),
                        Convert.ToDouble(savTargetsFromSelect.Tables[0].Rows[i]["moneyHoldings"]),
                        Convert.ToDateTime(savTargetsFromSelect.Tables[0].Rows[i]["addedDate"]),
                        Convert.ToDouble(savTargetsFromSelect.Tables[0].Rows[i]["neededAmount"])
                        ));
                }

                if (DateTime.Compare(maximumDate, new DateTime(1900, 01, 01)) == 0)
                    maximumDate = DateTime.Today; // gdy brak SinglePayments w bazie max i minDate = Today
                if (DateTime.Compare(minimumDate, new DateTime(2100, 12, 31)) == 0)
                    minimumDate = DateTime.Today;

                return new Budget(note, name, password, payments, categories, savingsTargets, balanceLogs, balance, 
                    numberOfPeople, creationDate,minimumDate,maximumDate,maximumAmount);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Wystąpił błąd w FetchAll");
                SqlConnect.Instance.ErrLog(ex);
                return null;
            }
        }

        private Boolean AddToDB(List<Changes> listOfAdds)
        {
            try
            {
                foreach (Changes C in listOfAdds)
                {
                    if (C.Type == typeof(BalanceLog))
                    {
                        BalanceLog b = Budget.Instance.BalanceLog[C.ID];
                        SqlConnect.Instance.ExecuteSqlNonQuery("INSERT INTO BalanceLogs(id, balance, date, singlePaymentId, periodPaymentId) values('" +
                                       C.ID + "','" + b.Balance + "','" + b.Date.ToShortDateString() + "','" + b.SinglePaymentID +
                                       "','" + b.PeriodPaymentID + "')");
                    }
                    if (C.Type == typeof(PeriodPayment))
                    {
                        //((p.Amount > 0) ? false : true)
                        PeriodPayment p = (PeriodPayment)Budget.Instance.Payments[C.ID];
                        SqlConnect.Instance.ExecuteSqlNonQuery("INSERT INTO PeriodPayments(id, categoryId, amount, note, type, name, frequency, period, lastUpdate, startDate, endDate) values('" +
                            C.ID + "','" + p.CategoryID + "','" + p.Amount + "','" + p.Note + "','" + Convert.ToInt32(p.Type) + "','" + p.Name + "','" + p.Frequency + "','" + p.Period + "','" + p.LastUpdate + "','" +
                            p.StartDate.ToShortDateString() + "','" + p.EndDate.ToShortDateString() + "')");
                    }
                    if (C.Type == typeof(SinglePayment))
                    {
                        // ((p.Amount > 0) ? false : true) 
                        SinglePayment p = (SinglePayment)Budget.Instance.Payments[C.ID];
                        SqlConnect.Instance.ExecuteSqlNonQuery(
                            "INSERT INTO SinglePayments(id, categoryId, amount, note, type, name, date) values('" +
                            C.ID + "','" + p.CategoryID + "','" + p.Amount + "','" + p.Note + "','" + Convert.ToInt32(p.Type) + "','" +
                            p.Name + "','" + p.Date.ToShortDateString() + "')");
                    }
                    if (C.Type == typeof(Category))
                    {
                        Category c = Budget.Instance.Categories[C.ID];
                        SqlConnect.Instance.ExecuteSqlNonQuery("INSERT INTO Categories(id, name, note, type) values('" +
                                        C.ID + "','" + c.Name + "','" + c.Note + "','" + Convert.ToInt32(c.Type) + "')");
                    }
                    if (C.Type == typeof(SavingsTarget))
                    {
                        SavingsTarget s = Budget.Instance.SavingsTargets[C.ID];
                        SqlConnect.Instance.ExecuteSqlNonQuery(
                            "INSERT INTO SavingsTargets(id, target, note, deadLine, priority, moneyHoldings, addedDate, neededAmount) values('" +
                            C.ID + "','" + s.Target + "','" + s.Note + "','" + s.Deadline.ToShortDateString() + "','" +
                            s.Priority + "','" + s.MoneyHoldings + "','" + s.AddedDate.ToShortDateString() + "','" +
                            s.NeededAmount + "')");
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            //listOfAdds.Clear();
            return true;
        }

        private Boolean DeleteFromDB(List<Changes> listOfDels)
        {
            try
            {
                foreach (Changes C in listOfDels)
                {
                    if (C.Type == typeof(Category))
                    {
                        SqlConnect.Instance.ExecuteSqlNonQuery("DELETE FROM CATEGORIES WHERE id = " + C.ID);
                    }
                    if (C.Type == typeof(PeriodPayment))
                    {
                        SqlConnect.Instance.ExecuteSqlNonQuery("DELETE FROM PERIODPAYMENTS WHERE id = " + C.ID);
                    }
                    if (C.Type == typeof(SinglePayment))
                    {
                        SqlConnect.Instance.ExecuteSqlNonQuery("DELETE FROM SINGLEPAYMENTS WHERE id = " + C.ID);
                    }
                    if (C.Type == typeof(SavingsTarget))
                    {
                        SqlConnect.Instance.ExecuteSqlNonQuery("DELETE FROM SAVINGSTARGETS WHERE id = " + C.ID);
                    }

                }
            }
            catch (Exception ex)
            {
                SqlConnect.Instance.ErrLog(ex);
                return false;
            }
            //listOfDels.Clear();
            return true;
        }

        private Boolean EditDB(List<Changes> listOfEdts)
        {
            if (DeleteFromDB(listOfEdts) && AddToDB(listOfEdts))
            {
                //listOfEdts.Clear();
                return true;
            }
            return false;
        }

        public void Dump()
        {
            if (Budget.Instance.AddToDB(Budget.Instance.ListOfAdds)
                && Budget.Instance.EditDB(Budget.Instance.ListOfEdts)
                && Budget.Instance.DeleteFromDB(Budget.Instance.ListOfDels))
            {
                MessageBox.Show("Poprawnie zapisana baza danych!");
                listOfEdts.Clear();
                listOfDels.Clear();
                listOfAdds.Clear();
                instance = Budget.FetchAll();
            }
            else
                MessageBox.Show("Błąd w zapisie bazy danych!");
            
        }
    }
}
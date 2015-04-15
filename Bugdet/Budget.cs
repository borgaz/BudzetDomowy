using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Bugdet
{
    public sealed class Budget
    {
        private static Budget instance = null;

        private String name; // nazwa budzetu
        private String note; // notatka
        private String password; // haslo do budzetu
        private Dictionary<int, Payment> payments; // slownik platnosci
        private Dictionary<int, Category> categories; // slownik platnosci
        private Dictionary<int, SavingsTarget> savingsTargets; //slownik celow, na ktore oszczedzamy
        private BalanceLog balance; // aktualnie najnowsze saldo, w przyszłosci przerobimy na słownik
        private int numberOfPeople; // ilosc osob, dla ktorych prowadzony jest budzet domowy
        private DateTime creationDate; // data stworzenia budzetu

        public override string ToString()
        {
            return "NAME: " + name + " NOTE: " + note + " PASSWORD: " + password + " NUMBER_OF_PEOPLE: " + numberOfPeople
                + " CREATION_DATE: " + creationDate + " BALANCE: \n" + balance;
        }

        private Budget(String note, String name, String password, Dictionary<int, Payment> payments, Dictionary<int, Category> categories,
            Dictionary<int, SavingsTarget> savingsTargets, BalanceLog balance, int numberOfPeople, DateTime creationDate)
        {
            this.note = note;
            this.name = name;
            this.password = password;
            this.payments = payments;
            this.categories = categories;
            this.savingsTargets = savingsTargets;
            this.balance = balance;
            this.numberOfPeople = numberOfPeople;
            this.creationDate = creationDate;
        }

        public static Budget Instance
        {
            get {
                    if (instance == null)
                    {
                        instance = FetchAll();
                        instance.SetDefaultCategories();
                    }
                return instance; 
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

        public int NumberOfPeople
        {
            get
            {
                return this.numberOfPeople;
            }
        }

        public BalanceLog Balance
        {
            get
            {
                return this.balance;
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

        public void SetNumberOfPeople(int number)
        {
            this.numberOfPeople = number;
        }

        public void AddNote(String note)
        {
            this.note = note; 
        }

        public void DeleteNumberOfPeople ()
        {
            this.numberOfPeople = 0;
        }

        public void AddSinglePayment(int index, SinglePayment payment)
        {
            payments.Add(index, payment);
        }

        public void DeleteSinglePayment(int index)
        {
            payments.Remove(index);
        }

        public void AddSavingsTarget(int index, SavingsTarget target)
        {
            savingsTargets.Add(index, target);
        }

        public void DeleteSavingsTarget(int index)
        {
            savingsTargets.Remove(index);
        }

        public void AddPeriodPayment(int index, PeriodPayment payment)
        {
            payments.Add(index, payment);
        }

        public void DeletePeriodPayment(int index)
        {
            payments.Remove(index);
        }

        public void SetDefaultCategories()
        {
            var defaultCategories = new Dictionary<int, Category>();
            try
            {
                this.categories.Add(1, new Category("Paliwo", "Benzyna do samochodu"));
                this.categories.Add(2, new Category("Jedzenie", "Zakupy okresowe"));
                this.categories.Add(3, new Category("Prąd", "Rachunki za energie"));
                this.categories.Add(4, new Category("Woda", "Rachunki za wodę"));
                this.categories.Add(5, new Category("Gaz", "Rachunki za gaz"));
                this.categories.Add(6, new Category("Internet", "Rachunki za internet"));
                this.categories.Add(7, new Category("Praca", "wypłata"));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Wystąpił błąd");
                Console.WriteLine(ex.GetBaseException() + "\n addDefaultCategories()");
            }
        }

        /// <summary>
        /// Pobiera wszystkie dane z bazy do obiektu
        /// </summary>
        /// <returns> Zwraca obiekt z wszystkimi danymi z bazy </returns>
        private static Budget FetchAll()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////
            // Nazwa budzetu
            /////////////////////////////////////////////////////////////////////////////////////////////
            String name = "";
            System.Data.DataSet nameFromSelect = SqlConnect.Instance.SelectQuery("SELECT name FROM Budget");

            if (nameFromSelect.Tables[0].Rows.Count == 0)
                throw new System.Data.Entity.Core.ObjectNotFoundException("Empty datebase");
            else
            {
                name = (string)nameFromSelect.Tables[0].Rows[0]["name"];
            }

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Opis budzetu
            /////////////////////////////////////////////////////////////////////////////////////////////
            String note = "";
            note = (string)SqlConnect.Instance.SelectQuery("SELECT note FROM Budget").Tables[0].Rows[0]["note"];

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Hasło budżetu
            /////////////////////////////////////////////////////////////////////////////////////////////
            String password = "";
            password = (string)SqlConnect.Instance.SelectQuery("SELECT password FROM Budget").Tables[0].Rows[0]["password"];

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Data powstania budżetu
            /////////////////////////////////////////////////////////////////////////////////////////////
            DateTime creationDate;
            creationDate = (DateTime)SqlConnect.Instance.SelectQuery("SELECT creation FROM Budget").Tables[0].Rows[0]["creation"];

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Liczba osób w budżecie
            /////////////////////////////////////////////////////////////////////////////////////////////
            int numberOfPeople;
            numberOfPeople = Convert.ToInt32(SqlConnect.Instance.SelectQuery("SELECT numberOfPeople FROM Budget").Tables[0].Rows[0]["numberOfPeople"]);

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Lista kategorii
            /////////////////////////////////////////////////////////////////////////////////////////////
            Dictionary<int, Category> categories = new Dictionary<int, Category>();
            System.Data.DataSet categoriesFromSelect = SqlConnect.Instance.SelectQuery("SELECT * FROM Categories");
            for (int i = 0; i < categoriesFromSelect.Tables[0].Rows.Count; i++)
            {
                categories.Add(Convert.ToInt32(categoriesFromSelect.Tables[0].Rows[i]["id"]),
                new Category(
                    (string)categoriesFromSelect.Tables[0].Rows[i]["name"],
                    (string)categoriesFromSelect.Tables[0].Rows[i]["note"]));
            }

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Aktualny stan konta
            /////////////////////////////////////////////////////////////////////////////////////////////
            System.Data.DataSet balanceFromSelect = SqlConnect.Instance.SelectQuery("SELECT * FROM BalanceLogs");
            BalanceLog balance;
            if (balanceFromSelect.Tables[0].Rows.Count - 1 >= 0)
            {
                balance = new BalanceLog(
                    //Convert.ToInt32(balanceFromSelect.Tables[0].Rows[balanceFromSelect.Tables[0].Rows.Count - 1]["id"]),
                    Convert.ToDouble(balanceFromSelect.Tables[0].Rows[balanceFromSelect.Tables[0].Rows.Count - 1]["balance"]),
                    (DateTime)(balanceFromSelect.Tables[0].Rows[balanceFromSelect.Tables[0].Rows.Count - 1]["date"]),
                    Convert.ToInt32(balanceFromSelect.Tables[0].Rows[balanceFromSelect.Tables[0].Rows.Count - 1]["singlePaymentId"]),
                    Convert.ToInt32(balanceFromSelect.Tables[0].Rows[balanceFromSelect.Tables[0].Rows.Count - 1]["periodPaymentId"])
                    );
            }
            else
                throw new System.Data.Entity.Core.ObjectNotFoundException("No balance in datebase");

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Lista płatności
            /////////////////////////////////////////////////////////////////////////////////////////////
            Dictionary<int, Payment> payments = new Dictionary<int, Payment>();

            System.Data.DataSet periodPayFromSelect = SqlConnect.Instance.SelectQuery("SELECT * FROM PeriodPayments");
            for (int i = 0; i < periodPayFromSelect.Tables[0].Rows.Count; i++)
            {
                payments.Add(Convert.ToInt32(periodPayFromSelect.Tables[0].Rows[i]["id"]),
                    new PeriodPayment(
                    Convert.ToInt32(periodPayFromSelect.Tables[0].Rows[i]["categoryId"]),
                    Convert.ToDouble(periodPayFromSelect.Tables[0].Rows[i]["amount"]),
                    (string)(periodPayFromSelect.Tables[0].Rows[i]["note"]),
                    (bool)(periodPayFromSelect.Tables[0].Rows[i]["type"]),
                    (string)(periodPayFromSelect.Tables[0].Rows[i]["name"]),
                    Convert.ToInt32(periodPayFromSelect.Tables[0].Rows[i]["frequency"]),
                    (string)(periodPayFromSelect.Tables[0].Rows[i]["period"]),
                    (DateTime)(periodPayFromSelect.Tables[0].Rows[i]["lastUpdate"]),
                    (DateTime)(periodPayFromSelect.Tables[0].Rows[i]["startDate"]),
                    (DateTime)(periodPayFromSelect.Tables[0].Rows[i]["endDate"])
                    ));
            }

            System.Data.DataSet singlePayFromSelect = SqlConnect.Instance.SelectQuery("SELECT * FROM SinglePayments");
            for (int i = 0; i < singlePayFromSelect.Tables[0].Rows.Count; i++)
            {
                payments.Add(Convert.ToInt32(singlePayFromSelect.Tables[0].Rows[i]["id"]),
                    new SinglePayment(
                    (string)(singlePayFromSelect.Tables[0].Rows[i]["note"]),
                    Convert.ToDouble(singlePayFromSelect.Tables[0].Rows[i]["amount"]),
                    Convert.ToInt32(singlePayFromSelect.Tables[0].Rows[i]["categoryId"]),
                    (bool)(singlePayFromSelect.Tables[0].Rows[i]["type"]),
                    (string)(singlePayFromSelect.Tables[0].Rows[i]["name"]),
                    (DateTime)(singlePayFromSelect.Tables[0].Rows[i]["date"])
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
                    new SavingsTarget(
                    (string)(savTargetsFromSelect.Tables[0].Rows[i]["target"]),
                    (string)(savTargetsFromSelect.Tables[0].Rows[i]["note"]),
                    (DateTime)(savTargetsFromSelect.Tables[0].Rows[i]["deadLine"]),
                    (string)(savTargetsFromSelect.Tables[0].Rows[i]["priority"]),
                    Convert.ToDouble(savTargetsFromSelect.Tables[0].Rows[i]["moneyHoldings"]),
                    (DateTime)(savTargetsFromSelect.Tables[0].Rows[i]["addedDate"]),
                    Convert.ToDouble(savTargetsFromSelect.Tables[0].Rows[i]["neededAmount"])
                    ));
            }
            return new Budget(note, name, password, payments, categories,
                                    savingsTargets, balance, numberOfPeople, creationDate);
        }

        public Boolean DumpAll()
        {
            try
            {
                /////////////////////////////////////////////////////////////////////////////////////////////
                // budzet
                /////////////////////////////////////////////////////////////////////////////////////////////
                SqlConnect.Instance.ExecuteSqlNonQuery("INSERT INTO( name,note,creationdate,numberofpeople,password) values('" +
                                   this.name + "'," + this.note + this.creationDate +
                                   "'," + this.numberOfPeople + ",'" + this.password + "')");


                /////////////////////////////////////////////////////////////////////////////////////////////
                // Lista kategorii
                /////////////////////////////////////////////////////////////////////////////////////////////

                for (int i = 0; i < this.categories.Count; i++)
                {
                    SqlConnect.Instance.ExecuteSqlNonQuery("INSERT INTO Categories(name,note) values('" + 
                                        this.categories[i + 1].Name +
                                       "','" + this.categories[i + 1].Note + "')");
                }


                /////////////////////////////////////////////////////////////////////////////////////////////
                // Aktualny stan konta
                /////////////////////////////////////////////////////////////////////////////////////////////
                SqlConnect.Instance.ExecuteSqlNonQuery("INSERT INTO BalanceLogs(balance,date,singlePaymentId,periodPaymentId) values(" +
                                   this.balance.Balance + ",' " + this.balance.Date + "'," +
                                   this.balance.SinglePaymentID + "," + this.balance.PeriodPaymentID + ")");

                /////////////////////////////////////////////////////////////////////////////////////////////
                // Lista płatności
                /////////////////////////////////////////////////////////////////////////////////////////////

                for (int i = 0; i < this.payments.Count; i++)
                {
                    PeriodPayment p = (PeriodPayment)(this.payments[i + 1]);
                    SqlConnect.Instance.ExecuteSqlNonQuery(
                        "INSERT INTO PeriodPayments(categoryId,amount,note,type,name,frequency,period,lastUpdate,startDate,endDate) values(" +
                        p.CategoryID + ", " + p.Amount + ",'" + p.Note + "',1,'" + p.LastUpdate + "','" + p.StartDate +
                        "','" + p.EndDate + "')");
                }

                /////////////////////////////////////////////////////////////////////////////////////////////
                // Cele oszczędzania
                /////////////////////////////////////////////////////////////////////////////////////////////

                for (int i = 0; i < this.savingsTargets.Count; i++)
                {
                    SqlConnect.Instance.ExecuteSqlNonQuery(
                        "INSERT INTO(target,note,deadline,priority,moneyHoldings,addedDate,neededAmount) values('" +
                        this.savingsTargets[i + 1].Target + "','" + this.savingsTargets[i + 1].Note + "','" +
                        this.savingsTargets[i + 1].Deadline + "','" + this.savingsTargets[i + 1].Priority + "'," +
                        this.savingsTargets[i + 1].MoneyHoldings + ",'" + this.savingsTargets[i + 1].AddedDate +
                        "'," + this.savingsTargets[i + 1].NeededAmount + ")");
                }

                return true;
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                System.Windows.MessageBox.Show("Błąd");
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

    }
}
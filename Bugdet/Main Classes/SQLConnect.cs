using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace Budget.Main_Classes
{
    public sealed class SqlConnect
    {
        private static SqlConnect _instance = null;

        private SQLiteConnection _mydb;
        private SQLiteCommand _command;
        public double monthlySalaries;
        public double monthlyPayments;
        public int _savingMinutes;

        public static SqlConnect Instance
        {
            get
            { 
                return _instance ?? (_instance = new SqlConnect()); 
            }
        }

        public void Disconnect ()
        {
            _mydb.Close();
            GC.Collect();
        }

        private Boolean Connect(String budget, String password)
        {
            try
            {
                string connectionString = "";
                connectionString += "Data Source=" + budget + ".sqlite;Version=3";
                connectionString += ";Password=" + password + ";";
                _mydb = new SQLiteConnection(connectionString);
                _mydb.Open();
                using (SQLiteCommand command = new SQLiteCommand("PRAGMA schema_version;", _mydb))
                {
                    var ret = command.ExecuteScalar(); // sprawdzanie czy wyskoczy wyjatek, jak tak to zapewne haslo jest nieprawidlowe
                }
                return true;
            }
            catch(SQLiteException)
            {
                return false;
            }
        }

        public Boolean CheckBaseName(String name)
        {
            if (!File.Exists("./" + name + ".sqlite")) // sprawdzanie czy juz jest baza o takiej nazwie
            {
                return true;
            }
                MessageBox.Show("Istnieje już taki budzet.");
                return false;

        }
        public Boolean MakeBudget(String name, String password)
        {
            try
            {
                if (CheckBaseName(name))
                {
                    SQLiteConnection.CreateFile(name + ".sqlite");
                    monthlySalaries = 0;
                    monthlyPayments = 0;
                    _mydb = new SQLiteConnection("Data Source=" + name + ".sqlite;Version=3");
                    _mydb.SetPassword(password);
                    _mydb.Open();
                    MakeDb();
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException() + "");
                return false;
            }
        }

        public Boolean CheckPassword(String budget, String password)
        {
            if (!Connect(budget, password))
                return false;

            DataSet result = SelectQuery("Select count(*) as count from Budget where name = '" + budget + "' AND password = '" + password + "'");
            if (Convert.ToInt32(result.Tables[0].Rows[0]["count"].ToString()) < 1)
            {
                return false;
            }   
            else
            {
                return true;
            }     
        }

        public void ErrLog(Exception ex)
        {
            MessageBox.Show("Wystąpił błąd skontaktuj się z Developerami!");
            File.AppendAllText("./logs", "\n--------------------------\n" + DateTime.Now.ToString() + "\n--------------------------\n" + ex.ToString());
        } // TODO: Przerobic wszystkie exception, zeby wchodzily do tej metody


        public Boolean MakeDb()
        {
            try
            {
                ExecuteSqlNonQuery("CREATE TABLE Budget (name varchar(50)," + 
                                                        "note varchar (200)," +
                                                        "password varchar (20)," +
                                                        "balance double," +
                                                        "creation date," +
                                                        "numberOfPeople integer)");

                ExecuteSqlNonQuery("CREATE TABLE PeriodPayments (id INTEGER PRIMARY KEY," +
                                                                      "categoryId integer," +
                                                                      "name varchar(50)," +
                                                                      "amount double," +
                                                                      "frequency integer," +
                                                                      "period varchar(20)," +
                                                                      "startDate date," +
                                                                      "endDate date," +
                                                                      "lastUpdate date," +
                                                                      "type boolean," +
                                                                      "note varchar(200))");

                ExecuteSqlNonQuery("CREATE TABLE SinglePayments (id INTEGER PRIMARY KEY," +
                                                                      "categoryId integer," +
                                                                      "name varchar(50)," +
                                                                      "amount double," +
                                                                      "date date," +
                                                                      "type boolean," +
                                                                      "note varchar(200))");

                ExecuteSqlNonQuery("CREATE TABLE BalanceLogs (id INTEGER PRIMARY KEY," +
                                                                      "periodPaymentId integer," + 
                                                                      "singlePaymentId integer," +
                                                                      "balance double," + 
                                                                      "date date)");

                ExecuteSqlNonQuery("CREATE TABLE Categories (id INTEGER PRIMARY KEY," +
                                                            "name varchar(50) not null," +
                                                            "note varchar(200)," +
                                                            "type boolean)");

                ExecuteSqlNonQuery("CREATE TABLE SavingsTargets (id INTEGER PRIMARY KEY," +
                                                                      "target varchar(50)," +
                                                                      "note varchar(200)," +
                                                                      "deadLine date," +
                                                                      "type boolean," +
                                                                      "moneyHoldings double," +
                                                                      "neededAmount double," +
                                                                      "priority integer," +
                                                                      "addedDate date)");
                return true;
            }
            catch(SQLiteException ex)
            {
                MessageBox.Show(ex.GetBaseException() + "\n" + "SQLConnect.MakeDB()");
                return false;
            }
        }
        public Boolean DumpCreator(Dictionary<int, Category> _categories, Dictionary<int, PeriodPayment> _payments, 
            String _name, String _password, BalanceLog _balance, SinglePayment _firstPayment)
        {
            try
            {
                MakeBudget(_name, _password);
                /////////////////////////////////////////////////////////////////////////////////////////////
                // budzet
                /////////////////////////////////////////////////////////////////////////////////////////////
                string balanceWithDot = _balance.Balance.ToString().Replace(",", ".");
                Instance.ExecuteSqlNonQuery("INSERT INTO Budget(name,note,creation,numberofpeople,password,balance) values('" +
                                   _name + "'," + "'note','" + DateTime.Now.ToShortDateString() +
                                   "'," + 0 + ",'" + _password + "','" + balanceWithDot + "')");

                /////////////////////////////////////////////////////////////////////////////////////////////
                // Lista kategorii
                /////////////////////////////////////////////////////////////////////////////////////////////

                for (int i = 0; i < _categories.Count; i++)
                {
                    Instance.ExecuteSqlNonQuery("INSERT INTO Categories(name,note,type) values('" +
                                        _categories[i].Name +
                                       "','" + _categories[i].Note + "','" + Convert.ToInt32(_categories[i].Type) + "')");
                }

                /////////////////////////////////////////////////////////////////////////////////////////////
                // Aktualny stan konta
                /////////////////////////////////////////////////////////////////////////////////////////////

                Instance.ExecuteSqlNonQuery("INSERT INTO BalanceLogs(balance,date,singlePaymentId,periodPaymentId) values('" +
                                   balanceWithDot + "','" + _balance.Date.ToShortDateString() + "','" +
                                   _balance.SinglePaymentID + "','" + _balance.PeriodPaymentID + "')");

                /////////////////////////////////////////////////////////////////////////////////////////////
                // Zalożenie budżetu jako pierwszy pojedyńczy przychód
                /////////////////////////////////////////////////////////////////////////////////////////////

                string amountWithDot = _firstPayment.Amount.ToString().Replace(",", ".");
                Instance.ExecuteSqlNonQuery(
                            "INSERT INTO SinglePayments(id, categoryId, amount, note, type, name, date) values('" +
                            1 + "','" + _firstPayment.CategoryID + "','" + amountWithDot + "','" + _firstPayment.Note + "','" + Convert.ToInt32(_firstPayment.Type) + "','" +
                            _firstPayment.Name + "','" + _firstPayment.Date.ToShortDateString() + "')");

                /////////////////////////////////////////////////////////////////////////////////////////////
                // Lista płatności
                /////////////////////////////////////////////////////////////////////////////////////////////

                foreach (KeyValuePair<int, PeriodPayment> pay in _payments)
                {
                    PeriodPayment p = pay.Value;
                    amountWithDot = p.Amount.ToString().Replace(",", ".");
                    Instance.ExecuteSqlNonQuery(
                        "INSERT INTO PeriodPayments(id, categoryId,amount,note,type,name,frequency,period,lastUpdate,startDate,endDate) values(" +
                        pay.Key*(-1) + "," + p.CategoryID + ",'" + amountWithDot + "','" + p.Note + "'," + Convert.ToInt32(p.Type) + ",'" + p.Name + "'," + p.Frequency + ",'" + p.Period + "','" + p.LastUpdate.ToShortDateString() + "','" +
                        p.StartDate.ToShortDateString() +
                        "','" + p.EndDate.ToShortDateString() + "')");
                }
                return true;
            }
            catch (SQLiteException)
            {
                MessageBox.Show("Błąd w SQL");
                return false;
            }
        }

        /// <summary>
        /// - wykonuje zapytanie, zwraca true jak sie uda
        /// </summary>
        /// <param name="query">zapytanie (insert, create itp )</param>
        public Boolean ExecuteSqlNonQuery(String query)
        {
            try
            {
                _command = new SQLiteCommand(query, _mydb);
                _command.ExecuteNonQuery();
                _command.Dispose();
                return true;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show( ex.GetBaseException() + "\n SQLConnect.ExecuteSQLNoNQuery");
                return false;
            }
        }

        /// <summary>
        /// - Zwraca DataSet z zadanego Selecta
        /// </summary>
        /// <param name="query">zapytanie ( select )</param>
        /// <returns></returns>
        public DataSet SelectQuery(String query)
        {
            try
            {
                DataSet dataSet = new DataSet();
                SQLiteDataAdapter result = new SQLiteDataAdapter {SelectCommand = new SQLiteCommand(query, _mydb)};
                result.Fill(dataSet);
                return dataSet;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.GetBaseException().Message, "Infomation", MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }
        }

        /// <summary>
        /// Hashuje string za pomocą MD5
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public String HashPasswordMd5(String password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(Encoding.ASCII.GetBytes(password));
            byte[] result = md5.Hash;
            StringBuilder hashedPassword = new StringBuilder();
            foreach (byte resultChunk in result)
            {
                hashedPassword.Append(resultChunk.ToString("x2"));
            }
            return hashedPassword.ToString(); 
        }

        public Dictionary<int, Category> AddDefaultCategories()
        {
            var defaultCategories = new Dictionary<int, Category>();
            try
            {
                defaultCategories.Add(0, new Category("Oszczędzanie", "Odłożenie pieniędzy na jakis cel", false));
                defaultCategories.Add(1, new Category("-", "Saldo początkowe", true));
                defaultCategories.Add(2, new Category("Paliwo", "Benzyna do samochodu", false));
                defaultCategories.Add(3, new Category("Jedzenie", "Zakupy okresowe", false));
                defaultCategories.Add(4, new Category("Woda", "Rachunki za wodę", false));
                defaultCategories.Add(5, new Category("Gaz", "Rachunki za gaz", false));
                defaultCategories.Add(6, new Category("Internet", "Rachunki za internet", false));
                defaultCategories.Add(7, new Category("Prąd", "Rachunki za energie", false));
                defaultCategories.Add(8, new Category("Praca", "Wypłata", true));
                defaultCategories.Add(9, new Category("Emerytura", "Emerytura", true));
                defaultCategories.Add(10, new Category("Stypednium", "Stypendium, np. socjalne lub naukowe", true));
                defaultCategories.Add(11, new Category("Renta", "Renta", true));
                return defaultCategories;
            }
            catch(Exception)
            {
                MessageBox.Show("Błąd");
                return null;
            }
        }

        public static String RemoveUnnecessarySymbols(String str)
        {
            string replacedString = Regex.Replace(str, @"[?<>!;:*+-_&@#%^()='/]", "");
            return replacedString;
        }
    }
}
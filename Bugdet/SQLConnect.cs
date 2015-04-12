using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Bugdet
{
    public sealed class SqlConnect
    {
        private static SqlConnect _instance = null;
        private SQLiteConnection _mydb;
        private SQLiteCommand _command;

        private SqlConnect()
        {
            Connect();
        }

        public static SqlConnect Instance
        {
            get { return _instance ?? (_instance = new SqlConnect()); }
        }

        public void Connect()
        {
            try
            {
                if (!File.Exists("./budzet.sqlite")) // sprawdzanie czy juz jest baza -  w pozniejszym projekcie sie inaczej zrobi
                {
                    SQLiteConnection.CreateFile("budzet.sqlite");
                    _mydb = new SQLiteConnection("Data Source=budzet.sqlite;Version=3");
                    _mydb.Open();
                    MakeDb();
                }
                else
                {
                    _mydb = new SQLiteConnection("Data Source=budzet.sqlite;Version=3");
                    _mydb.Open();
                }
            }
            catch(SQLiteException)
            {       
            }
        }

        public Boolean MakeDb()
        {
            try
            {
                ExecuteSqlNonQuery("CREATE TABLE Budget (name varchar(50)," + 
                                                        "balance double," +
                                                        "note varchar (200)," +
                                                        "password varchar (20)," +
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
                                                                      "type integer," +
                                                                      "note varchar(200))");

                ExecuteSqlNonQuery("CREATE TABLE SinglePayments (id INTEGER PRIMARY KEY," +
                                                                      "categoryId integer," +
                                                                      "name varchar(50)," +
                                                                      "amount double," +
                                                                      "date date," +
                                                                      "type integer," +
                                                                      "note varchar(200))");

                ExecuteSqlNonQuery("CREATE TABLE BalanceLogs (id INTEGER PRIMARY KEY," +
                                                                      "periodPaymentId integer," + 
                                                                      "singlePaymentId integer," +
                                                                      "balance double," + 
                                                                      "date date)");

                ExecuteSqlNonQuery("CREATE TABLE Categories (id INTEGER PRIMARY KEY," +
                                                            "name varchar(50) not null," +
                                                            "note varchar(200))");

                ExecuteSqlNonQuery("CREATE TABLE SavingsTargets (id INTEGER PRIMARY KEY," +
                                                                      "target varchar(50)," +
                                                                      "note varchar(200)," +
                                                                      "deadLine date," +
                                                                      "moneyHoldings double," +
                                                                      "neededAmount double," +
                                                                      "priority varchar(50)," +
                                                                      "addedDate date)");

                AddDefaultCategories();
                return true;

            }
            catch(SQLiteException ex)
            {
                MessageBox.Show(ex.GetBaseException() + "\n" + "SQLConnect.MakeDB()");
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

        public Boolean CheckCategory(String category,String note)
        {
            int result = (int)SelectQuery("Select count(id) as count from Categories where name='"+ category +"'").Tables[0].Rows[0]["count"];
            if (result == 0)
            {
                ExecuteSqlNonQuery("INSERT into Categories(name,note) values('" + category + "','" + note + "'"); 
                return true;
            }
            else
                return false;
        }

        public Boolean AddSinglePayment(String name,double value,int category,String note)
        {
            try
            {
                _command = new SQLiteCommand
                {
                    CommandText =
                        "INSERT INTO BalanceLogs(periodPaymentId,categoryId,income,date,note) values(null,@category,@income,date('now'),@note)"
                };
                _command.Parameters.AddWithValue("@category", ++category);
                _command.Parameters.AddWithValue("@income", value * (-1));
                _command.Parameters.AddWithValue("@note", name + "|" + note);
                _command.Connection = _mydb;
                _command.ExecuteNonQuery();
                _command.Dispose();
                return true;
            }
            catch(SQLiteException ex)
            {
                MessageBox.Show("Błąd");
                Console.WriteLine(ex.GetBaseException() + @"AddSinglePayment()");
                return false;
            }
        }

        public Boolean AddSingleSalary(String name,double value,int category,String note)
        {
            try
            {
                _command = new SQLiteCommand
                {
                    CommandText =
                        "INSERT INTO BalanceLogs(periodPaymentId,categoryId,income,date,note) values(null,@category,@income,date('now'),@note)"
                };
                _command.Parameters.AddWithValue("@category", ++category);
                _command.Parameters.AddWithValue("@income", value);
                _command.Parameters.AddWithValue("@note", name + "|" + note);
                _command.Connection = _mydb;
                _command.ExecuteNonQuery();
                _command.Dispose();
                return true;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Błąd");
                Console.WriteLine(ex.GetBaseException() + "\n AddSinglePayment()");
                return false;
            }
        }

        /// <summary>
        /// Pobiera wszystkie dane z bazy do obiektu
        /// </summary>
        /// <returns> Zwraca obiekt z wszystkimi danymi z bazy </returns>
        public Budget FetchAll()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////
            // Nazwa budzetu
            /////////////////////////////////////////////////////////////////////////////////////////////
            String name = "";
            DataSet nameFromSelect = SelectQuery("SELECT name FROM Budget");

            if (nameFromSelect.Tables[0].Rows.Count == 0)
                throw new ObjectNotFoundException("Empty datebase");
            else
            {
                name = (string)nameFromSelect.Tables[0].Rows[0]["name"];
            }

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Opis budzetu
            /////////////////////////////////////////////////////////////////////////////////////////////
            String note = "";
            note = (string)SelectQuery("SELECT note FROM Budget").Tables[0].Rows[0]["note"];

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Hasło budżetu
            /////////////////////////////////////////////////////////////////////////////////////////////
            String password = "";
            password = (string)SelectQuery("SELECT password FROM Budget").Tables[0].Rows[0]["password"];

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Data powstania budżetu
            /////////////////////////////////////////////////////////////////////////////////////////////
            DateTime creationDate;
            creationDate = (DateTime)SelectQuery("SELECT creation FROM Budget").Tables[0].Rows[0]["creation"];

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Liczba osób w budżecie
            /////////////////////////////////////////////////////////////////////////////////////////////
            int numberOfPeople;
            numberOfPeople = Convert.ToInt32(SelectQuery("SELECT numberOfPeople FROM Budget").Tables[0].Rows[0]["numberOfPeople"]);

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Lista kategorii
            /////////////////////////////////////////////////////////////////////////////////////////////
            Dictionary<int, Category> categories = new Dictionary<int, Category>();
            DataSet categoriesFromSelect = SelectQuery("SELECT * FROM Categories");
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
            DataSet balanceFromSelect = SelectQuery("SELECT * FROM BalanceLogs");
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
                throw new ObjectNotFoundException("No balance in datebase");

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Lista płatności
            /////////////////////////////////////////////////////////////////////////////////////////////
            Dictionary<int, Payment> payments = new Dictionary<int, Payment>();

            DataSet periodPayFromSelect = SelectQuery("SELECT * FROM PeriodPayments");
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

            DataSet singlePayFromSelect = SelectQuery("SELECT * FROM SinglePayments");
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
            DataSet savTargetsFromSelect = SelectQuery("SELECT * FROM SavingsTargets");
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

        private Boolean AddDefaultCategories()
        {
            try
            {
                ExecuteSqlNonQuery("INSERT into Categories(name,note) values('Paliwo','Benzyna do samochodu')");
                ExecuteSqlNonQuery("INSERT into Categories(name,note) values('Jedzenie','Zakupy okresowe')");
                ExecuteSqlNonQuery("INSERT into Categories(name,note) values('Prąd','Rachunki za prąd')");
                ExecuteSqlNonQuery("INSERT into Categories(name,note) values('Woda','Rachunki za wodę')");
                ExecuteSqlNonQuery("INSERT into Categories(name,note) values('Gaz','Rachunki za gaz')");
                return true;
            }
            catch(SQLiteException ex)
            {
                MessageBox.Show("Błąd");
                Console.WriteLine(ex.GetBaseException() + "\n addDefaultCategories()");
                return false;
            }
        }
    }
}
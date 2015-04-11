using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Data.SQLite;
using System.IO;
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
                ExecuteSqlNonQuery("CREATE TABLE Budget (name varchar(50), balance integer)");
                ExecuteSqlNonQuery("CREATE TABLE PeriodPayments (id INTEGER PRIMARY KEY," +
                                                                      "categoryId integer," +
                                                                      "name varchar(50)," +
                                                                      "amount double," +
                                                                      "frequency integer," +
                                                                      "period varchar(20)," +
                                                                      "startDate date," +
                                                                      "lastUpdate date," +
                                                                      "type integer," +
                                                                      "note varchar(100))");
                ExecuteSqlNonQuery("CREATE TABLE BalanceLogs (id INTEGER PRIMARY KEY," +
                                                                      "periodPaymentId integer," + 
                                                                      "categoryId integer not null," +
                                                                      "income double," + 
                                                                      "date date," +
                                                                      "note varchar(100))");
                ExecuteSqlNonQuery("CREATE TABLE Categories (id INTEGER PRIMARY KEY, name varchar(50) not null,note varchar(100))");
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
            String note = ""; //chwilowo brak w bazie
            String password = ""; //chwilowo brak w bazie
            Dictionary<int, SavingsTarget> savingsTargets = null; //chwilowo brak w bazie
            int numberOfPeople = 0; // chwilowo brak w bazie
            DateTime creationDate = DateTime.Today; //chwilowo brak w bazie

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Nazwa bazy
            /////////////////////////////////////////////////////////////////////////////////////////////
            String name = "";
            DataSet nameFromSelect = SelectQuery("SELECT name FROM Budget");

            if (nameFromSelect.Tables[0].Rows.Count == 0)
                throw new ObjectNotFoundException("Empty datebase");
            else
            {
                name = (string)SelectQuery("SELECT name FROM Budget").Tables[0].Rows[0]["name"];
            }

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
            BalanceLog balance = new BalanceLog(
                Convert.ToInt32(balanceFromSelect.Tables[0].Rows[balanceFromSelect.Tables[0].Rows.Count - 1]["id"]),
                Convert.ToInt32(balanceFromSelect.Tables[0].Rows[balanceFromSelect.Tables[0].Rows.Count - 1]["income"]),
                Convert.ToInt32(balanceFromSelect.Tables[0].Rows[balanceFromSelect.Tables[0].Rows.Count - 1]["categoryId"]),
                Convert.ToInt32(balanceFromSelect.Tables[0].Rows[balanceFromSelect.Tables[0].Rows.Count - 1]["periodPaymentId"]),
                (DateTime)(balanceFromSelect.Tables[0].Rows[balanceFromSelect.Tables[0].Rows.Count - 1]["date"]),
                (string)(balanceFromSelect.Tables[0].Rows[balanceFromSelect.Tables[0].Rows.Count - 1]["note"])
                );

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
                    Convert.ToDouble(periodPayFromSelect.Tables[0].Rows[i]["income"]),
                    (string)(periodPayFromSelect.Tables[0].Rows[i]["note"]),
                    (int)(periodPayFromSelect.Tables[0].Rows[i]["type"]),
                    (string)(periodPayFromSelect.Tables[0].Rows[i]["name"]),
                    Convert.ToInt32(periodPayFromSelect.Tables[0].Rows[i]["repeat"]),
                    "", // no period in datebase
                    (DateTime)(periodPayFromSelect.Tables[0].Rows[i]["lastUpdate"]),
                    (DateTime)(periodPayFromSelect.Tables[0].Rows[i]["startDate"]),
                    (DateTime)(periodPayFromSelect.Tables[0].Rows[i]["endDate"])
                    ));
                
            }

            Budget temporary = new Budget (note, name, password, payments, categories,
                                    savingsTargets, balance, numberOfPeople, creationDate);
            
            return temporary;

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
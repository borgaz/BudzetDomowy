using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace Bugdet
{
    public sealed class SQLConnect
    {
        private static SQLConnect _instance = null;
        private SQLiteConnection _MYDB;
        private SQLiteCommand command;

        private SQLConnect()
        {
            Connect();
        }

        public static SQLConnect Instance
        {
            get { return _instance ?? (_instance = new SQLConnect()); }
        }
        public void Connect()
        {
            try
            {
                if (!File.Exists("./budzet.sqlite")) // sprawdzanie czy juz jest baza -  w pozniejszym projekcie sie inaczej zrobi
                {
                    SQLiteConnection.CreateFile("budzet.sqlite");
                    _MYDB = new SQLiteConnection("Data Source=budzet.sqlite;Version=3");
                    _MYDB.Open();
                    MakeDb();
                }
                else
                {
                    _MYDB = new SQLiteConnection("Data Source=budzet.sqlite;Version=3");
                    _MYDB.Open();
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
                this.ExecuteSQLNoNQuery("CREATE TABLE Budget (name varchar(50), balance integer)");
                this.ExecuteSQLNoNQuery("CREATE TABLE PeriodPayments (id INTEGER PRIMARY KEY," +
                                                                      "categoryId integer," +
                                                                      "name varchar(50)," +
                                                                      "amount double," +
                                                                      "frequency integer," +
                                                                      "period varchar(20)," +
                                                                      "startDate date," +
                                                                      "lastUpdate date," +
                                                                      "type integer," +
                                                                      "note varchar(100))");
                this.ExecuteSQLNoNQuery("CREATE TABLE BalanceLogs (id INTEGER PRIMARY KEY," +
                                                                      "periodPaymentId integer," + // period.... literowka :)
                                                                      "categoryId integer not null," +
                                                                      "income double," + // czemu nie double (?) w sumie nie wiem czemu, poprawione :)
                                                                      "date date," +
                                                                      "note varchar(100))");
                this.ExecuteSQLNoNQuery("CREATE TABLE Categories (id INTEGER PRIMARY KEY, name varchar(50) not null,note varchar(100))");
                this.addDefaultCategories();
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
        public Boolean ExecuteSQLNoNQuery(String query)
        {
            try
            {
                command = new SQLiteCommand(query, _MYDB);
                command.ExecuteNonQuery();
                command.Dispose();
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
                SQLiteDataAdapter result = new SQLiteDataAdapter {SelectCommand = new SQLiteCommand(query, _MYDB)};
                result.Fill(dataSet);
                return dataSet;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.GetBaseException().Message, "Infomation", MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }
        }
        public Boolean CheckCategory(String category,String note)
        {
            int result = (int)SelectQuery("Select count(id) as count from Categories where name='"+ category +"'").Tables[0].Rows[0]["count"];
            if (result == 0)
            {
                this.ExecuteSQLNoNQuery("INSERT into Categories(name,note) values('" + category + "','" + note + "'"); 
                return true;
            }
            else
                return false;
        }
        public Boolean AddSinglePayment(String name,double value,int category,String note)
        {
            try
            {
                command = new SQLiteCommand
                {
                    CommandText =
                        "INSERT INTO BalanceLogs(periodPaymentId,categoryId,income,date,note) values(null,@category,@income,date('now'),@note)"
                };
                command.Parameters.AddWithValue("@category", ++category);
                command.Parameters.AddWithValue("@income", value * (-1));
                command.Parameters.AddWithValue("@note", name + "|" + note);
                command.Connection = _MYDB;
                command.ExecuteNonQuery();
                command.Dispose();
                return true;
            }
            catch(SQLiteException ex)
            {
                MessageBox.Show("Błąd");
                Console.WriteLine(ex.GetBaseException() + "\n AddSinglePayment()");
                return false;
            }
        }
        public Boolean AddSingleSalary(String name,double value,int category,String note)
        {
            try
            {
                command = new SQLiteCommand
                {
                    CommandText =
                        "INSERT INTO BalanceLogs(periodPaymentId,categoryId,income,date,note) values(null,@category,@income,date('now'),@note)"
                };
                command.Parameters.AddWithValue("@category", ++category);
                command.Parameters.AddWithValue("@income", value);
                command.Parameters.AddWithValue("@note", name + "|" + note);
                command.Connection = _MYDB;
                command.ExecuteNonQuery();
                command.Dispose();
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
            List<SavingsTarget> savingsTargets = null; //chwilowo brak w bazie
            int numberOfPeople = 0; // chwilowo brak w bazie
            DateTime creationDate = DateTime.Today; //chwilowo brak w bazie

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Nazwa bazy
            /////////////////////////////////////////////////////////////////////////////////////////////
            String name = "";
            DataSet nameFromSelect = this.SelectQuery("SELECT name FROM Budget");

            if (nameFromSelect.Tables[0].Rows.Count == 0)
                throw new ObjectNotFoundException("Empty datebase");
            else
            {
                name = (string)this.SelectQuery("SELECT name FROM Budget").Tables[0].Rows[0]["name"];
            }

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Lista kategorii
            /////////////////////////////////////////////////////////////////////////////////////////////
            List<Category> categories = new List <Category>();
            DataSet categoriesFromSelect = this.SelectQuery("SELECT * FROM Categories");
            for (int i = 0; i < categoriesFromSelect.Tables[0].Rows.Count; i++)
            {
                categories.Add(new Category(
                    Convert.ToInt32(categoriesFromSelect.Tables[0].Rows[i]["id"]), 
                    (string)categoriesFromSelect.Tables[0].Rows[i]["name"], 
                    (string)categoriesFromSelect.Tables[0].Rows[i]["note"]));
            }

            /////////////////////////////////////////////////////////////////////////////////////////////
            // Aktualny stan konta
            /////////////////////////////////////////////////////////////////////////////////////////////
            DataSet balanceFromSelect = this.SelectQuery("SELECT * FROM BalanceLogs");
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
            List<Payment> payments = new List<Payment>();

            DataSet periodPayFromSelect = this.SelectQuery("SELECT * FROM PeriodPayments");
            for (int i = 0; i < periodPayFromSelect.Tables[0].Rows.Count; i++)
            {
                payments.Add(new PeriodPayment(
                    Convert.ToInt32(periodPayFromSelect.Tables[0].Rows[i]["id"]),
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
        private Boolean addDefaultCategories()
        {
            try
            {
                this.ExecuteSQLNoNQuery("INSERT into Categories(name,note) values('Paliwo','Benzyna do samochodu')");
                this.ExecuteSQLNoNQuery("INSERT into Categories(name,note) values('Jedzenie','Zakupy okresowe')");
                this.ExecuteSQLNoNQuery("INSERT into Categories(name,note) values('Prąd','Rachunki za prąd')");
                this.ExecuteSQLNoNQuery("INSERT into Categories(name,note) values('Woda','Rachunki za wodę')");
                this.ExecuteSQLNoNQuery("INSERT into Categories(name,note) values('Gaz','Rachunki za gaz')");
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
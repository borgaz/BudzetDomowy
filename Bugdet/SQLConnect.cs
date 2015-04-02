using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows;
using System.IO;

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
            get
            {
                if (_instance == null)
                    _instance = new SQLConnect();
                return _instance;
            }
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
                    MakeDB();
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
        public Boolean MakeDB()
        {
            try
            {
                this.ExecuteSQLNoNQuery("CREATE TABLE Budget (name varchar(50), balance integer)");
                this.ExecuteSQLNoNQuery("CREATE TABLE PeriodPayments (id INTEGER PRIMARY KEY," +
                                                                      "categoryId integer," +
                                                                      "name varchar(50)," +
                                                                      "income integer," +
                                                                      "repeat integer," +
                                                                      "startDate date," +
                                                                      "lastUpdate date," +
                                                                      "type integer," +
                                                                      "note varchar(100))");
                this.ExecuteSQLNoNQuery("CREATE TABLE BalanceLogs (id INTEGER PRIMARY KEY," +
                                                                      "pariodPaymentId integer not null," +
                                                                      "categoryId integer not null," +
                                                                      "income integer," +
                                                                      "date date," +
                                                                      "note varchar(100))");
                this.ExecuteSQLNoNQuery("CREATE TABLE Categories (id INTEGER PRIMARY KEY, name varchar(50) not null,note varchar(100))");
                return true;

            }
            catch(SQLiteException ex)
            {
                MessageBox.Show(ex.GetBaseException() + "\n" + "SQLConnect.MakeDB()");
                return false;
                // gunwo
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
                SQLiteDataAdapter result = new SQLiteDataAdapter();
                result.SelectCommand = new SQLiteCommand(query, _MYDB);
                result.Fill(dataSet);
                return dataSet;
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.GetBaseException().Message, "Infomation", MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }
        }
        public int CheckCategory(String category) // do dokonczenia
        {
            int id = 0;
            DataSet result;
            result = SelectQuery("Select count(id) from Categories where name='"+ category +"'");

            return 0;
        }
        /// <summary>
        /// Pobiera wszystkie dane z bazy do obiektu
        /// </summary>
        /// <returns> Zwraca obiekt z wszystkimi danymi z bazy </returns>
        public Budget FetchAll()
        {
            String note = ""; //chwilowo brak w bazie
            String password = ""; //chwilowo brak w bazie
            String name = (string)this.SelectQuery("SELECT name FROM Budget").Tables[0].Rows[0]["name"];

            List<Payment> payments = null;

            List<Category> categories = null;

            List<SavingsTarget> savingsTargets = null;

            BalanceLog balance = null;

            int numberOfPeople = 0; // liczba budzetów
            DateTime creationDate = DateTime.Today; //chwilowo brak w bazie



            Budget temporary = new Budget (note, name, password, payments, categories,
                                    savingsTargets, balance, numberOfPeople, creationDate);
            
            return temporary;

        }
    }
}
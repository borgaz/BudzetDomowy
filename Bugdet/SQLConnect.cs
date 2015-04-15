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
                                                                      "moneyHoldings double," +
                                                                      "neededAmount double," +
                                                                      "priority integer," +
                                                                      "addedDate date)");

                //SetDefaultCategories();
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


        public Dictionary<int, Category> AddDefaultCategories()
        {
            var defaultCategories = new Dictionary<int, Category>();
            try
            {
                defaultCategories.Add(1, new Category("Paliwo","Benzyna do samochodu", false));
                defaultCategories.Add(2, new Category("Jedzenie", "Zakupy okresowe", false));
                defaultCategories.Add(3, new Category("Prąd", "Rachunki za energie", false));
                defaultCategories.Add(4, new Category("Woda", "Rachunki za wodę", false));
                defaultCategories.Add(5, new Category("Gaz", "Rachunki za gaz", false));
                defaultCategories.Add(6, new Category("Internet", "Rachunki za internet", false));
                defaultCategories.Add(7, new Category("Praca", "Wypłata", true));
                defaultCategories.Add(8, new Category("Emerytura", "Emerytura", true));
                defaultCategories.Add(9, new Category("Renta", "Renta", true));
                defaultCategories.Add(10, new Category("Stypednium", "Stypendium, np. socjalne lub naukowe", true));
                return defaultCategories;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Błąd");
                Console.WriteLine(ex.GetBaseException() + "\n addDefaultCategories()");
                return null;
            }
        }
        
    }
}
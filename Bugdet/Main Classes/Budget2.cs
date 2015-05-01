﻿using Budget.Utility_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

// Second part of Budget class - FetchAll() and Dump()
namespace Budget.Main_Classes
{
    public sealed partial class Budget
    {
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
                System.Data.DataSet singlePayFromSelect = SqlConnect.Instance.SelectQuery("SELECT * FROM SinglePayments");
                for (int i = 0; i < singlePayFromSelect.Tables[0].Rows.Count; i++)
                {
                    double amount = Convert.ToDouble(singlePayFromSelect.Tables[0].Rows[i]["amount"]);
                    if (amount > maximumAmount)
                        maximumAmount = amount;

                    DateTime date = Convert.ToDateTime(singlePayFromSelect.Tables[0].Rows[i]["date"]);
                    if (DateTime.Compare(date, minimumDate) < 0)
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
                    numberOfPeople, creationDate, minimumDate, maximumDate, maximumAmount);
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
                        PeriodPayment p = (PeriodPayment)Budget.Instance.Payments[C.ID];
                        SqlConnect.Instance.ExecuteSqlNonQuery("INSERT INTO PeriodPayments(id, categoryId, amount, note, type, name, frequency, period, lastUpdate, startDate, endDate) values('" +
                            C.ID + "','" + p.CategoryID + "','" + p.Amount + "','" + p.Note + "','" + Convert.ToInt32(p.Type) + "','" + p.Name + "','" + p.Frequency + "','" + p.Period + "','" + p.LastUpdate + "','" +
                            p.StartDate.ToShortDateString() + "','" + p.EndDate.ToShortDateString() + "')");
                    }
                    if (C.Type == typeof(SinglePayment))
                    {
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
            return true;
        }

        private Boolean DeleteFromDB(List<Changes> listOfDels, bool isPartOfEdts) //troche to dziwne rozwiazanie, 
            //ale funkcja troche inaczej zachowuje sie gdy usuwanie jest czescia edytowania - wtedy nie usuwamy balanceloga 
            // tylko go uaktualniamy
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
                        if (!isPartOfEdts)
                        {
                            System.Data.DataSet tempSelect = SqlConnect.Instance.SelectQuery("SELECT amount,type FROM SinglePayments WHERE id = " + C.ID);
                            double amountToRefactor = (double)tempSelect.Tables[0].Rows[0]["amount"];
                            if ((bool)tempSelect.Tables[0].Rows[0]["type"] == false)
                                amountToRefactor = (-1) * amountToRefactor;

                            SqlConnect.Instance.ExecuteSqlNonQuery("UPDATE BALANCELOGS SET BALANCE = (BALANCE + '" + amountToRefactor + "') WHERE singlePaymentid > " + C.ID);
                            SqlConnect.Instance.ExecuteSqlNonQuery("DELETE FROM BALANCELOGS WHERE singlePaymentId = " + C.ID);
                        }
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
            return true;
        }

        private Boolean EditDB(List<Changes> listOfEdts)
        {
            foreach (Changes C in listOfEdts)
            {
                if (C.Type == typeof(SinglePayment))
                {
                    System.Data.DataSet tempSelect = SqlConnect.Instance.SelectQuery("SELECT amount,type FROM SinglePayments WHERE id = " + C.ID);
                    if (payments[C.ID].Amount != (double)tempSelect.Tables[0].Rows[0]["amount"])
                    {
                        double amountToRefactor;
                        if ((bool)tempSelect.Tables[0].Rows[0]["type"] == false)
                            amountToRefactor = payments[C.ID].Amount - (double)tempSelect.Tables[0].Rows[0]["amount"];
                        else
                            amountToRefactor = (double)tempSelect.Tables[0].Rows[0]["amount"] - payments[C.ID].Amount;

                       SqlConnect.Instance.ExecuteSqlNonQuery("UPDATE BALANCELOGS SET BALANCE = (BALANCE + '" + amountToRefactor + "') WHERE singlePaymentid >= " + C.ID);
                    }
                }
            }

            if (DeleteFromDB(listOfEdts,true) && AddToDB(listOfEdts))
            {
                return true;
            }
            return false;
        }

        public void Dump()
        {
            if (Budget.Instance.AddToDB(Budget.Instance.ListOfAdds)
                && Budget.Instance.EditDB(Budget.Instance.ListOfEdts)
                && Budget.Instance.DeleteFromDB(Budget.Instance.ListOfDels,false))
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
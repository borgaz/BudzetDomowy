using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using Budget.Utility_Classes;

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
                double balance = 0;
                DateTime maximumDate = new DateTime(1900, 01, 01);
                DateTime minimumDate = new DateTime(2100, 12, 31);

                DataSet nameFromSelect = SqlConnect.Instance.SelectQuery("SELECT * FROM Budget");
                if (nameFromSelect.Tables[0].Rows.Count > 0)
                {
                    name = Convert.ToString(nameFromSelect.Tables[0].Rows[0]["name"]);
                    note = Convert.ToString(nameFromSelect.Tables[0].Rows[0]["note"]);
                    balance = Convert.ToDouble(nameFromSelect.Tables[0].Rows[0]["balance"]);
                    password = Convert.ToString(nameFromSelect.Tables[0].Rows[0]["password"]);
                    creationDate = Convert.ToDateTime(nameFromSelect.Tables[0].Rows[0]["creation"]);
                }

                /////////////////////////////////////////////////////////////////////////////////////////////
                // Lista kategorii
                /////////////////////////////////////////////////////////////////////////////////////////////

                Dictionary<int, Category> categories = new Dictionary<int, Category>();
                DataSet categoriesFromSelect = SqlConnect.Instance.SelectQuery("SELECT * FROM Categories");
                for (int i = 0; i < categoriesFromSelect.Tables[0].Rows.Count; i++)
                {
                    categories.Add(Convert.ToInt32(categoriesFromSelect.Tables[0].Rows[i]["id"]),
                    new Category(Convert.ToString(categoriesFromSelect.Tables[0].Rows[i]["name"]),
                        Convert.ToString(categoriesFromSelect.Tables[0].Rows[i]["note"]),
                        Convert.ToBoolean(categoriesFromSelect.Tables[0].Rows[i]["type"])));
                }

                /////////////////////////////////////////////////////////////////////////////////////////////
                // Aktualny stan konta
                /////////////////////////////////////////////////////////////////////////////////////////////

                Dictionary<int, BalanceLog> balanceLogs = new Dictionary<int, BalanceLog>();
                DataSet balanceLogsFromSelect = SqlConnect.Instance.SelectQuery("SELECT * FROM BalanceLogs");
                for (int i = 0; i < balanceLogsFromSelect.Tables[0].Rows.Count; i++)
                {
                    balanceLogs.Add(Convert.ToInt32(balanceLogsFromSelect.Tables[0].Rows[i]["id"]),
                        new BalanceLog(Convert.ToDouble(balanceLogsFromSelect.Tables[0].Rows[i]["balance"]),
                        Convert.ToDateTime(balanceLogsFromSelect.Tables[0].Rows[i]["date"]),
                        Convert.ToInt32(balanceLogsFromSelect.Tables[0].Rows[i]["singlePaymentId"]),
                        Convert.ToInt32(balanceLogsFromSelect.Tables[0].Rows[i]["periodPaymentId"])));
                }

                /////////////////////////////////////////////////////////////////////////////////////////////
                // Lista płatności
                /////////////////////////////////////////////////////////////////////////////////////////////

                Dictionary<int, Payment> payments = new Dictionary<int, Payment>();
                DataSet periodPayFromSelect = SqlConnect.Instance.SelectQuery("SELECT * FROM PeriodPayments");
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
                        Convert.ToDateTime(periodPayFromSelect.Tables[0].Rows[i]["endDate"])));
                }
                DataSet singlePayFromSelect = SqlConnect.Instance.SelectQuery("SELECT * FROM SinglePayments");
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
                            date));
                    //do miesiecznej sumy
                    if (!Convert.ToDateTime(singlePayFromSelect.Tables[0].Rows[i]["date"]).Month.Equals(DateTime.Now.Month))
                        continue;

                    if (Convert.ToInt32(singlePayFromSelect.Tables[0].Rows[i]["type"]) == 1)
                        SqlConnect.Instance.monthlyPayments +=
                            Convert.ToDouble(singlePayFromSelect.Tables[0].Rows[i]["amount"]);
                    else
                    {
                        SqlConnect.Instance.monthlySalaries +=
                             Convert.ToDouble(singlePayFromSelect.Tables[0].Rows[i]["amount"]);
                    }
                }

                /////////////////////////////////////////////////////////////////////////////////////////////
                // Cele oszczędzania
                /////////////////////////////////////////////////////////////////////////////////////////////

                Dictionary<int, SavingsTarget> savingsTargets = new Dictionary<int, SavingsTarget>();
                DataSet savTargetsFromSelect = SqlConnect.Instance.SelectQuery("SELECT * FROM SavingsTargets");
                for (int i = 0; i < savTargetsFromSelect.Tables[0].Rows.Count; i++)
                {
                    savingsTargets.Add(Convert.ToInt32(savTargetsFromSelect.Tables[0].Rows[i]["id"]),
                        new SavingsTarget(Convert.ToString(savTargetsFromSelect.Tables[0].Rows[i]["target"]),
                        Convert.ToString(savTargetsFromSelect.Tables[0].Rows[i]["note"]),
                        Convert.ToDateTime(savTargetsFromSelect.Tables[0].Rows[i]["deadLine"]),
                        Convert.ToInt32(savTargetsFromSelect.Tables[0].Rows[i]["priority"]),
                        Convert.ToDouble(savTargetsFromSelect.Tables[0].Rows[i]["moneyHoldings"]),
                        Convert.ToDateTime(savTargetsFromSelect.Tables[0].Rows[i]["addedDate"]),
                        Convert.ToDouble(savTargetsFromSelect.Tables[0].Rows[i]["neededAmount"])));
                }

                if (DateTime.Compare(maximumDate, new DateTime(1900, 01, 01)) == 0)
                    maximumDate = DateTime.Today; // gdy brak SinglePayments w bazie max i minDate = Today
                if (DateTime.Compare(minimumDate, new DateTime(2100, 12, 31)) == 0)
                    minimumDate = DateTime.Today;

                return new Budget(note, name, password, payments, categories, savingsTargets, balanceLogs, balance,
                    creationDate, minimumDate, maximumDate, maximumAmount);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd w FetchAll");
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
                        BalanceLog b = Instance.BalanceLog[C.ID];
                        string balanceWithDot = b.Balance.ToString().Replace(",", ".");
                        SqlConnect.Instance.ExecuteSqlNonQuery("INSERT INTO BalanceLogs(id, balance, date, singlePaymentId, periodPaymentId) values('" +
                                       C.ID + "','" + balanceWithDot + "','" + b.Date.ToShortDateString() + "','" + b.SinglePaymentID +
                                       "','" + b.PeriodPaymentID + "')");
                    }
                    if (C.Type == typeof(PeriodPayment))
                    {
                        PeriodPayment p = (PeriodPayment)Instance.Payments[C.ID];
                        string amountWithDot = p.Amount.ToString().Replace(",", ".");
                        SqlConnect.Instance.ExecuteSqlNonQuery("INSERT INTO PeriodPayments(id, categoryId, amount, note, type, name, frequency, period, lastUpdate, startDate, endDate) values('" +
                            C.ID + "','" + p.CategoryID + "','" + amountWithDot + "','" + p.Note + "','" + Convert.ToInt32(p.Type) + "','" + p.Name + "','" + p.Frequency + "','" + p.Period + "','" + p.LastUpdate.ToShortDateString() + "','" +
                            p.StartDate.ToShortDateString() + "','" + p.EndDate.ToShortDateString() + "')");
                    }
                    if (C.Type == typeof(SinglePayment))
                    {
                        SinglePayment p = (SinglePayment)Instance.Payments[C.ID];
                        string amountWithDot = p.Amount.ToString().Replace(",", ".");
                        SqlConnect.Instance.ExecuteSqlNonQuery(
                            "INSERT INTO SinglePayments(id, categoryId, amount, note, type, name, date) values('" +
                            C.ID + "','" + p.CategoryID + "','" + amountWithDot + "','" + p.Note + "','" + Convert.ToInt32(p.Type) + "','" +
                            p.Name + "','" + p.Date.ToShortDateString() + "')");
                    }
                    if (C.Type == typeof(Category))
                    {
                        Category c = Instance.Categories[C.ID];
                        SqlConnect.Instance.ExecuteSqlNonQuery("INSERT INTO Categories(id, name, note, type) values('" +
                                        C.ID + "','" + c.Name + "','" + c.Note + "','" + Convert.ToInt32(c.Type) + "')");
                    }
                    if (C.Type == typeof(SavingsTarget))
                    {
                        SavingsTarget s = Instance.SavingsTargets[C.ID];
                        string moneyHoldingsWithDot = s.MoneyHoldings.ToString().Replace(",", ".");
                        string neededAmountWithDot = s.NeededAmount.ToString().Replace(",", ".");
                        SqlConnect.Instance.ExecuteSqlNonQuery(
                            "INSERT INTO SavingsTargets(id, target, note, deadLine, priority, moneyHoldings, addedDate, neededAmount) values('" +
                            C.ID + "','" + s.Target + "','" + s.Note + "','" + s.Deadline.ToShortDateString() + "','" +
                            s.Priority + "','" + moneyHoldingsWithDot + "','" + s.AddedDate.ToShortDateString() + "','" +
                            neededAmountWithDot + "')");
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
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
                        DataSet tempSelect = SqlConnect.Instance.SelectQuery("SELECT amount,type FROM SinglePayments WHERE id = " + C.ID);
                        double amountToRefactor = (double)tempSelect.Tables[0].Rows[0]["amount"];
                        if ((bool)tempSelect.Tables[0].Rows[0]["type"] == false)
                            amountToRefactor = (-1) * amountToRefactor;
                        SqlConnect.Instance.ExecuteSqlNonQuery("UPDATE BALANCELOGS SET BALANCE = (BALANCE + '" + amountToRefactor + "') WHERE singlePaymentid > " + C.ID);
                        SqlConnect.Instance.ExecuteSqlNonQuery("DELETE FROM BALANCELOGS WHERE singlePaymentId = " + C.ID);

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
            try
            {
                foreach (Changes C in listOfEdts)
                {
                    if (C.Type == typeof(SinglePayment))
                    {
                        DataSet tempSelect = SqlConnect.Instance.SelectQuery("SELECT amount,type FROM SinglePayments WHERE id = " + C.ID);
                        if (payments[C.ID].Amount != (double)tempSelect.Tables[0].Rows[0]["amount"])
                        {
                            double amountToRefactor;
                            if ((bool)tempSelect.Tables[0].Rows[0]["type"] == false)
                                amountToRefactor = payments[C.ID].Amount - (double)tempSelect.Tables[0].Rows[0]["amount"];
                            else
                                amountToRefactor = (double)tempSelect.Tables[0].Rows[0]["amount"] - payments[C.ID].Amount;
                            string amountToRefactorWithDot = amountToRefactor.ToString().Replace(",", ".");
                            SqlConnect.Instance.ExecuteSqlNonQuery("UPDATE BALANCELOGS SET BALANCE = (BALANCE + '" + amountToRefactorWithDot + "') WHERE singlePaymentid >= " + C.ID);
                        }
                        SinglePayment p = (SinglePayment)Instance.Payments[C.ID];
                        string amountWithDot = p.Amount.ToString().Replace(",", ".");
                        SqlConnect.Instance.ExecuteSqlNonQuery("UPDATE SINGLEPAYMENTS SET CATEGORYID = '" + p.CategoryID +
                            "', NAME ='" + p.Name +
                            "', AMOUNT ='" + amountWithDot +
                            "', DATE ='" + p.Date.ToShortDateString() +
                            "', TYPE = '" + Convert.ToInt32(p.Type) +
                            "', NOTE = '" + p.Note +
                            "'WHERE id = " + C.ID);

                    }

                    if (C.Type == typeof(Category))
                    {
                        SqlConnect.Instance.ExecuteSqlNonQuery("UPDATE CATEGORIES SET NAME = '" + categories[C.ID].Name +
                            "', NOTE ='" + categories[C.ID].Note +
                            "', TYPE = '" + Convert.ToInt32(categories[C.ID].Type) +
                            "' WHERE id = " + C.ID);
                    }
                    if (C.Type == typeof(PeriodPayment))
                    {
                        PeriodPayment p = (PeriodPayment)Instance.Payments[C.ID];
                        string amountWithDot = p.Amount.ToString().Replace(",", ".");
                        SqlConnect.Instance.ExecuteSqlNonQuery("UPDATE PERIODPAYMENTS SET CATEGORYID = '" + p.CategoryID +
                            "', NAME ='" + p.Name +
                            "', AMOUNT ='" + amountWithDot +
                            "', FREQUENCY ='" + p.Frequency +
                            "', PERIOD ='" + p.Period +
                            "', STARTDATE ='" + p.StartDate.ToShortDateString() +
                            "', ENDDATE ='" + p.EndDate.ToShortDateString() +
                            "', LASTUPDATE ='" + p.LastUpdate.ToShortDateString() +
                            "', TYPE = '" + Convert.ToInt32(p.Type) +
                            "', NOTE = '" + p.Note +
                            "'WHERE id = " + C.ID);
                    }
                    if (C.Type == typeof(SavingsTarget))
                    {
                        SavingsTarget s = Instance.SavingsTargets[C.ID];
                        string moneyHoldingsWithDot = s.MoneyHoldings.ToString().Replace(",", ".");
                        string neededAmountWithDot = s.NeededAmount.ToString().Replace(",", ".");
                        SqlConnect.Instance.ExecuteSqlNonQuery("UPDATE SAVINGSTARGETS SET TARGET = '" + s.Target +
                            "', NOTE = '" + s.Note +
                            "', DEADLINE ='" + s.Deadline.ToShortDateString() +
                            "', MONEYHOLDINGS='" + moneyHoldingsWithDot +
                            "', NEEDEDAMOUNT='" + neededAmountWithDot +
                            "', PRIORITY = '" + s.Priority +
                            "', ADDEDDATE = '" + s.AddedDate.ToShortDateString() +
                            "' WHERE id = " + C.ID);
                    }
                }
            }
            catch (Exception ex)
            {
                SqlConnect.Instance.ErrLog(ex);
                return false;
            }
            //if (DeleteFromDB(listOfEdts,true) && AddToDB(listOfEdts))
            //{
            //    return true;
            //}
            return true;
        }

        public void Dump()
        {
            if (Instance.AddToDB(Instance.ListOfAdds)
                && Instance.EditDB(Instance.ListOfEdits)
                && Instance.DeleteFromDB(Instance.ListOfDels))
            {
                MessageBox.Show("Zapisano");
                listOfEdits.Clear();
                listOfDels.Clear();
                listOfAdds.Clear();
                SqlConnect.Instance.ExecuteSqlNonQuery("UPDATE BUDGET SET BALANCE = '" + balance.ToString().Replace(",", ".") +
                    "'");
                instance = FetchAll();
            }
            else
                MessageBox.Show("Błąd w zapisie bazy danych!");
        }
    }
}
﻿using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Data;
using System.Windows.Media;
using Budget.Main_Classes;

namespace Budget.WelcomePage
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {

        public WelcomePage()
        {
            InitializeComponent();
            InsertBars();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            providedPaymentsDataGrid.ItemsSource = CreateDataForPaymentForDataGrid();
            shortHistoryDataGrid.ItemsSource = CreataDataForShortHistoryDataGrid();
            //int balanceMaxKey = Main_Classes.Budget.Instance.Balance;
            Balance.Text = Convert.ToString(Main_Classes.Budget.Instance.Balance);
            //Balance.Text = Convert.ToString(Main_Classes.Budget.Instance.BalanceLog[Main_Classes.Budget.Instance.BalanceLog.Count].Balance);
        }

        private List<PaymentForDataGrid> CreateDataForPaymentForDataGrid()
        {
            DateTime lastDate;
            List<PaymentForDataGrid> providedPayments = new List<PaymentForDataGrid>();
            foreach(Main_Classes.Payment payment in Main_Classes.Budget.Instance.Payments.Values)
            {
                if (payment.GetType() == typeof(Main_Classes.PeriodPayment))
                {
                    var pP = (Main_Classes.PeriodPayment) payment;
                    if (pP.Amount < SettingsPage.Settings.Instance.PP_AmountTo && pP.Amount > SettingsPage.Settings.Instance.PP_AmountOf)
                    {
                        lastDate = SettingsPage.Settings.Instance.PP_LastDateToShow() <= pP.EndDate ? SettingsPage.Settings.Instance.PP_LastDateToShow() : pP.EndDate;
                        providedPayments.AddRange(Main_Classes.PeriodPayment.CreateListOfSelectedPeriodPayments(pP, lastDate));
                    }
                }
                else
                {
                    var sP = (Main_Classes.SinglePayment) payment;
                    if (sP.CompareDate() >= 0 && sP.Amount < SettingsPage.Settings.Instance.PP_AmountTo && sP.Amount > SettingsPage.Settings.Instance.PP_AmountOf && sP.Date < SettingsPage.Settings.Instance.PP_LastDateToShow())
                    {
                        providedPayments.Add(new PaymentForDataGrid(sP.Name, sP.Amount, "Pojedynczy", sP.Date, sP.Type, sP.CategoryID));
                    }    
                }
            }

            providedPayments.Sort();
            if (providedPayments.Count > SettingsPage.Settings.Instance.PP_NumberOfRow)
            {
                providedPayments.RemoveRange(SettingsPage.Settings.Instance.PP_NumberOfRow, providedPayments.Count - SettingsPage.Settings.Instance.PP_NumberOfRow);
            }
            return providedPayments;
        }

        private List<PaymentForDataGrid> CreataDataForShortHistoryDataGrid()
        {
            List<PaymentForDataGrid> shortHistory = new List<PaymentForDataGrid>();

            foreach(Main_Classes.BalanceLog balanceLog in Main_Classes.Budget.Instance.BalanceLog.Values)
            {
                if(balanceLog.SinglePaymentID != 0 && balanceLog.PeriodPaymentID == 0)
                {
                    var sP = (Main_Classes.SinglePayment)Main_Classes.Budget.Instance.Payments[balanceLog.SinglePaymentID];
                    if (sP.CompareDate() <= 0 && sP.Date >= SettingsPage.Settings.Instance.SH_LastDateToShow() && sP.Amount < SettingsPage.Settings.Instance.SH_AmountTo && sP.Amount > SettingsPage.Settings.Instance.SH_AmountOf)
                    {
                        shortHistory.Add(new PaymentForDataGrid(sP.Name, sP.Amount, "Pojedynczy", sP.Date, sP.Type, sP.CategoryID));
                    }
                }
                else if(balanceLog.SinglePaymentID == 0 && balanceLog.PeriodPaymentID != 0)
                {
                    var pP = (Main_Classes.PeriodPayment)Main_Classes.Budget.Instance.Payments[balanceLog.PeriodPaymentID];

                    if (balanceLog.Date >= SettingsPage.Settings.Instance.SH_LastDateToShow() && pP.Amount < SettingsPage.Settings.Instance.SH_AmountTo && pP.Amount > SettingsPage.Settings.Instance.SH_AmountOf)
                    {
                        shortHistory.Add(new PaymentForDataGrid(pP.Name, pP.Amount, "Okresowy", balanceLog.Date, pP.Type, pP.CategoryID));
                    }
                }
            }

            shortHistory.Sort((a, b) => -1 * a.CompareTo(b));
            if (shortHistory.Count > SettingsPage.Settings.Instance.SH_NumberOfRow)
            {
                shortHistory.RemoveRange(SettingsPage.Settings.Instance.SH_NumberOfRow, shortHistory.Count - SettingsPage.Settings.Instance.SH_NumberOfRow);
            }
            return shortHistory;
        }

        private void InsertBars()
        {
            PaymentsBar.Maximum = SalariesBar.Maximum = SqlConnect.Instance.monthlySalaries + SqlConnect.Instance.monthlyPayments;
            SalariesBar.Value = SqlConnect.Instance.monthlySalaries;
            PaymentsBar.Value = SqlConnect.Instance.monthlyPayments;
            SalariesBar.ToolTip = SalariesBar.Value;
            PaymentsBar.ToolTip = PaymentsBar.Value;
        }
        private void ProvidedPaymentsDataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
        }
    }
}
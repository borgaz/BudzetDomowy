using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

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

            Balance.Text = Convert.ToString(Main_Classes.Budget.Instance.Balance);
            if (Main_Classes.Budget.Instance.Balance > 0)
            {
                Balance.Foreground = Brushes.Green;
            }
            if (Main_Classes.Budget.Instance.Balance <= 0)
            {
                Balance.Foreground = Brushes.Red;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            providedPaymentsDataGrid.ItemsSource = CreateDataForProvidedPaymentDataGrid();
            shortHistoryDataGrid.ItemsSource = CreataDataForShortHistoryDataGrid();
            savingsTargetsDataGrid.ItemsSource = CreataDataForSavingsTargetsDataGrid();

            Main_Classes.Budget.Instance.PropertyChanged += (s, propertyChangedEventArgs) =>
            {
                if (propertyChangedEventArgs.PropertyName.Equals("BalanceLog"))
                {
                    Balance.Text = Convert.ToString(Main_Classes.Budget.Instance.Balance);
                    if (Main_Classes.Budget.Instance.Balance > 0)
                    {
                        Balance.Foreground = Brushes.Green;
                    }
                    if (Main_Classes.Budget.Instance.Balance <= 0)
                    {
                        Balance.Foreground = Brushes.Red;
                    }
                }
            };
        }

        private List<Main_Classes.SavingsTarget> CreataDataForSavingsTargetsDataGrid()
        {
            List<Main_Classes.SavingsTarget> savingsTargets = new List<Main_Classes.SavingsTarget>();
            foreach(Main_Classes.SavingsTarget sT in Main_Classes.Budget.Instance.SavingsTargets.Values)
            {
                savingsTargets.Add(sT);
            }
            savingsTargets.Sort((a, b) => -1 * a.CompareTo(b));
            return savingsTargets;
        }

        private List<PaymentForDataGrid> CreateDataForProvidedPaymentDataGrid()
        {
            DateTime lastDate;
            List<PaymentForDataGrid> providedPayments = new List<PaymentForDataGrid>();
            foreach (Main_Classes.Payment payment in Main_Classes.Budget.Instance.Payments.Values)
            {
                if (payment.GetType() == typeof(Main_Classes.PeriodPayment))
                {
                    var pP = (Main_Classes.PeriodPayment)payment;
                    if (pP.Amount <= SettingsPage.Settings.Instance.PP_AmountTo && pP.Amount >= SettingsPage.Settings.Instance.PP_AmountOf)
                    {
                        lastDate = SettingsPage.Settings.Instance.PP_LastDateToShow() <= pP.EndDate ? SettingsPage.Settings.Instance.PP_LastDateToShow() : pP.EndDate;
                        if (pP.CountNextDate() <= lastDate)
                        {
                            providedPayments.AddRange(Main_Classes.PeriodPayment.CreateListOfSelectedPeriodPayments(pP, lastDate));
                        }
                    }
                }
                else
                {
                    var sP = (Main_Classes.SinglePayment)payment;
                    if (sP.CompareDate() >= 0 && sP.Amount <= SettingsPage.Settings.Instance.PP_AmountTo && sP.Amount >= SettingsPage.Settings.Instance.PP_AmountOf && sP.Date <= SettingsPage.Settings.Instance.PP_LastDateToShow())
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

            foreach (Main_Classes.BalanceLog balanceLog in Main_Classes.Budget.Instance.BalanceLog.Values)
            {
                if(balanceLog.SinglePaymentID != 0 && balanceLog.PeriodPaymentID == 0)
                {
                    var sP = (Main_Classes.SinglePayment)Main_Classes.Budget.Instance.Payments[balanceLog.SinglePaymentID];
                    if (sP.CompareDate() <= 0 && sP.Date >= SettingsPage.Settings.Instance.SH_LastDateToShow() && sP.Amount <= SettingsPage.Settings.Instance.SH_AmountTo && sP.Amount >= SettingsPage.Settings.Instance.SH_AmountOf)
                    {
                        if (sP.Name.StartsWith("[Okresowy]"))
                        {
                            String TempName = sP.Name.Substring(10);
                            shortHistory.Add(new PaymentForDataGrid(TempName, sP.Amount, "Okresowy", sP.Date, sP.Type, sP.CategoryID));
                        }
                        else 
                        {
                            shortHistory.Add(new PaymentForDataGrid(sP.Name, sP.Amount, "Pojedynczy", sP.Date, sP.Type, sP.CategoryID));
                        }
                    }
                }
                else if(balanceLog.SinglePaymentID == 0 && balanceLog.PeriodPaymentID != 0)
                {
                    var pP = (Main_Classes.PeriodPayment)Main_Classes.Budget.Instance.Payments[balanceLog.PeriodPaymentID];
                    if (balanceLog.Date >= SettingsPage.Settings.Instance.SH_LastDateToShow() && pP.Amount <= SettingsPage.Settings.Instance.SH_AmountTo && pP.Amount >= SettingsPage.Settings.Instance.SH_AmountOf)
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
            PaymentsBar.Maximum = SalariesBar.Maximum = Main_Classes.SqlConnect.Instance.monthlySalaries + Main_Classes.SqlConnect.Instance.monthlyPayments;
            SalariesBar.Value = Main_Classes.SqlConnect.Instance.monthlySalaries;
            PaymentsBar.Value = Main_Classes.SqlConnect.Instance.monthlyPayments;
            SalariesBar.ToolTip = SalariesBar.Value + " zł";
            PaymentsBar.ToolTip = PaymentsBar.Value + " zł";
        }

        private void Grid_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            InsertBars();
        }

        private void ProvidedPaymentsDataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            colorDataGridRow(e);
        }

        private void shortHistoryDataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            colorDataGridRow(e);
        }

        private void savingsTargetsDataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            colorDataGridRow(e);
        }

        private void colorDataGridRow (DataGridRowEventArgs e)
        {
            var p = e.Row.Item;
            if (p.GetType() == typeof(PaymentForDataGrid))
            {
                PaymentForDataGrid pFDG = (PaymentForDataGrid) p;
                if (pFDG.Sign == true)
                {
                    e.Row.Background = new SolidColorBrush(Colors.Tomato);
                }
                else
                {
                    e.Row.Background = new SolidColorBrush(Colors.SpringGreen);
                }    
            }
            else if (p.GetType() == typeof(Main_Classes.SavingsTarget))
            {
                Main_Classes.SavingsTarget sT = (Main_Classes.SavingsTarget)p;
                if (sT.CountMoneyLeft() == 0)
                {
                    e.Row.Background = new SolidColorBrush(Colors.SpringGreen);
                }
            }     
        }

        private void NewTargetButton_Click(object sender, RoutedEventArgs e)
        {
            SavingsTargetsWindow.Instance.PropertyChanged += (s, propertyChangedEventArgs) =>
            {
                if (propertyChangedEventArgs.PropertyName.Equals("Update_sTDG"))
                    savingsTargetsDataGrid.ItemsSource = CreataDataForSavingsTargetsDataGrid();
                if (propertyChangedEventArgs.PropertyName.Equals("Update_sHDG"))
                    shortHistoryDataGrid.ItemsSource = CreataDataForShortHistoryDataGrid();
                if (propertyChangedEventArgs.PropertyName.Equals("Update_pPDG"))
                    providedPaymentsDataGrid.ItemsSource = CreateDataForProvidedPaymentDataGrid();
            };
            SavingsTargetsWindow.Instance.ShowDialog();
        }

        private void AddAmountToTargetButton_Click(object sender, RoutedEventArgs e)
        { 
            AddAmountToSavingsTargetWindow.Instance.PropertyChanged += (s, propertyChangedEventArgs) =>
            {
                if (propertyChangedEventArgs.PropertyName.Equals("Update_sHDG"))
                    shortHistoryDataGrid.ItemsSource = CreataDataForShortHistoryDataGrid();
                if (propertyChangedEventArgs.PropertyName.Equals("Update_sTDG"))
                    savingsTargetsDataGrid.ItemsSource = CreataDataForSavingsTargetsDataGrid();
                if (propertyChangedEventArgs.PropertyName.Equals("Refresh_sTDG"))
                    savingsTargetsDataGrid.Items.Refresh();
            };
            AddAmountToSavingsTargetWindow.Instance.ShowDialog();
        }
    }
}
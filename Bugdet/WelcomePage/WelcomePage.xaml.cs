using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Budget.Main_Classes;

namespace Budget.WelcomePage
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        private const int INF = 9999999;
        public DateTime startDate, endDate;

        public WelcomePage()
        {
            InitializeComponent();
            InsertBars();
            SetColours();
            PrevMonth();
        }

        private void SetColours()
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
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            providedPaymentsDataGrid.ItemsSource = CreateDataForProvidedPaymentDataGrid("grid");
            shortHistoryDataGrid.ItemsSource = CreataDataForShortHistoryDataGrid("grid", DateTime.Today, DateTime.Today);
            savingsTargetsDataGrid.ItemsSource = CreataDataForSavingsTargetsDataGrid();

            Main_Classes.Budget.Instance.PropertyChanged += (s, propertyChangedEventArgs) =>
            {
                if (propertyChangedEventArgs.PropertyName.Equals("BalanceLog"))
                {
                    SetColours();
                }
            };
        }

        private void PrevMonth()
        {
            var res = 0.0;
            var date = DateTime.Now.AddMonths(-1);
            var firstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            foreach (var p in Main_Classes.Budget.Instance.Payments)
            {
                if (p.Value.GetType() == typeof(PeriodPayment))
                    continue;
                var pp = (SinglePayment) p.Value;
                if (pp.Date <= date || pp.Date >= firstDay) continue;
                res = Main_Classes.Budget.Instance.BalanceLog[p.Key].Balance;
                date = pp.Date;
            }
            if (res == 0.00)
            {
                PrevMonthLabel.Visibility = Visibility.Hidden;
                PrevMonthTextBox.Visibility = Visibility.Hidden;
            }
            else
            {
                PrevMonthTextBox.Foreground = res < 0 ? Brushes.Red : Brushes.Green;
                PrevMonthTextBox.Text = res.ToString();
            }
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

        private List<PaymentForDataGrid> CreateDataForProvidedPaymentDataGrid(String destination)
        {
            Double amountOf = 0, amountTo = 0;
            int numberOfRow = 0;
            DateTime lastDate;
            if (destination.Equals("grid"))
            {
                amountOf = SettingsPage.Settings.Instance.PP_AmountOf;
                amountTo = SettingsPage.Settings.Instance.PP_AmountTo;
                numberOfRow = SettingsPage.Settings.Instance.PP_NumberOfRow;
                lastDate = SettingsPage.Settings.Instance.PP_LastDateToShow();
            }
            else if(destination.Equals("pdf"))
            {
                amountOf = 0;
                amountTo = INF;
                numberOfRow = INF;
                lastDate = DateTime.MinValue; 
            }
            else
            {
                lastDate = DateTime.Today;
            }

            List<PaymentForDataGrid> providedPayments = new List<PaymentForDataGrid>();
            foreach (Main_Classes.Payment payment in Main_Classes.Budget.Instance.Payments.Values)
            {
                if (payment.GetType() == typeof(Main_Classes.PeriodPayment))
                {
                    var pP = (Main_Classes.PeriodPayment)payment;
                    if (pP.Amount <= amountTo && pP.Amount >= amountOf)
                    {
                        if (!lastDate.Equals(DateTime.MinValue))
                        {
                            lastDate = SettingsPage.Settings.Instance.PP_LastDateToShow() <= pP.EndDate ? SettingsPage.Settings.Instance.PP_LastDateToShow() : pP.EndDate;
                        }
                        if (pP.CountNextDate() <= lastDate)
                        {
                            providedPayments.AddRange(Main_Classes.PeriodPayment.CreateListOfSelectedPeriodPayments(pP, lastDate));
                        }
                    }
                }
                else
                {
                    var sP = (Main_Classes.SinglePayment)payment;
                    if (sP.CompareDate() > 0 && sP.Amount <= amountTo && sP.Amount >= amountOf && sP.Date <= lastDate)
                    {
                        providedPayments.Add(new PaymentForDataGrid(sP.Name, sP.Amount, "Pojedynczy", sP.Date, sP.Type, sP.CategoryID, 0));
                    }    
                }
            }

            providedPayments.Sort();
            if (providedPayments.Count > numberOfRow)
            {
                providedPayments.RemoveRange(numberOfRow, providedPayments.Count - numberOfRow);
            }
            return providedPayments;
        }

        private List<PaymentForDataGrid> CreataDataForShortHistoryDataGrid(String destination, DateTime firstDate, DateTime lastDate)
        {
            Double amountOf = 0, amountTo = 0;
            int numberOfRow = 0;
            List<PaymentForDataGrid> shortHistory = new List<PaymentForDataGrid>();

            if (destination.Equals("grid"))
            {
                amountOf = SettingsPage.Settings.Instance.PP_AmountOf;
                amountTo = SettingsPage.Settings.Instance.PP_AmountTo;
                numberOfRow = SettingsPage.Settings.Instance.PP_NumberOfRow;
                firstDate = SettingsPage.Settings.Instance.SH_LastDateToShow();
                lastDate = DateTime.Today;
            }
            else if (destination.Equals("pdf"))
            {
                amountOf = 0;
                amountTo = INF;
                numberOfRow = INF;
                if (firstDate.Equals(DateTime.Today))
                {
                    firstDate = DateTime.MinValue;
                }
            }
            foreach (Main_Classes.BalanceLog balanceLog in Main_Classes.Budget.Instance.BalanceLog.Values)
            {
                if ((balanceLog.SinglePaymentID != 0 && balanceLog.PeriodPaymentID == 0) || (balanceLog.SinglePaymentID == 0 && balanceLog.PeriodPaymentID == 0))
                {
                    var sP = (Main_Classes.SinglePayment)Main_Classes.Budget.Instance.Payments[balanceLog.SinglePaymentID];
                    if (sP.Date <= lastDate && sP.Date >= firstDate && sP.Amount <= amountTo && sP.Amount >= amountOf)
                    {
                        if (sP.Name.StartsWith("Okresowy:"))
                        {
                            String TempName = sP.Name.Substring(9);
                            shortHistory.Add(new PaymentForDataGrid(TempName, sP.Amount, "Okresowy", sP.Date, sP.Type, sP.CategoryID, balanceLog.Balance));
                        }
                        else 
                        {
                            shortHistory.Add(new PaymentForDataGrid(sP.Name, sP.Amount, "Pojedynczy", sP.Date, sP.Type, sP.CategoryID, balanceLog.Balance));
                        }
                    }
                }
                else if(balanceLog.SinglePaymentID == 0 && balanceLog.PeriodPaymentID != 0)
                {
                    var pP = (Main_Classes.PeriodPayment)Main_Classes.Budget.Instance.Payments[balanceLog.PeriodPaymentID];
                    if (balanceLog.Date >= firstDate && pP.Amount <= amountTo && pP.Amount >= amountOf)
                    {
                        shortHistory.Add(new PaymentForDataGrid(pP.Name, pP.Amount, "Okresowy", balanceLog.Date, pP.Type, pP.CategoryID, balanceLog.Balance));
                    }
                }
            }
            if (destination == "grid")
            {
                shortHistory.Sort((a, b) => -1 * a.CompareTo(b));
            }
            if (shortHistory.Count > numberOfRow)
            {
                shortHistory.RemoveRange(numberOfRow, shortHistory.Count - numberOfRow);
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
                    shortHistoryDataGrid.ItemsSource = CreataDataForShortHistoryDataGrid("grid", DateTime.Today, DateTime.Today);
                if (propertyChangedEventArgs.PropertyName.Equals("Update_pPDG"))
                    providedPaymentsDataGrid.ItemsSource = CreateDataForProvidedPaymentDataGrid("grid");
            };
            SavingsTargetsWindow.Instance.ShowDialog();
        }

        private void AddAmountToTargetButton_Click(object sender, RoutedEventArgs e)
        { 
            AddAmountToSavingsTargetWindow.Instance.PropertyChanged += (s, propertyChangedEventArgs) =>
            {
                if (propertyChangedEventArgs.PropertyName.Equals("Update_sHDG"))
                    shortHistoryDataGrid.ItemsSource = CreataDataForShortHistoryDataGrid("grid", DateTime.Today, DateTime.Today);
                if (propertyChangedEventArgs.PropertyName.Equals("Update_sTDG"))
                    savingsTargetsDataGrid.ItemsSource = CreataDataForSavingsTargetsDataGrid();
                if (propertyChangedEventArgs.PropertyName.Equals("Refresh_sTDG"))
                    savingsTargetsDataGrid.Items.Refresh();
            };
            AddAmountToSavingsTargetWindow.Instance.ShowDialog();
        }

        private void PDFButton_Click(object sender, RoutedEventArgs e)
        {
            if (AdvancePDFCheckBox.IsChecked == true)
            {
                PDFOptionsWindow.Instance.ShowDialog();
                startDate = PDFOptionsWindow.Instance.StartDate;
                endDate = PDFOptionsWindow.Instance.EndDate;
                if (PDFOptionsWindow.Instance.Generate == true)
                {
                    GeneratePDF();
                }
                PDFOptionsWindow.Instance = null;
                AdvancePDFCheckBox.IsChecked = false;
            }
            else
            {
                startDate = DateTime.Today;
                endDate = DateTime.Today;
                GeneratePDF();
            }   
        }

        private void GeneratePDF()
        {
            try
            {
                ReportViewer report = new ReportViewer();
                report.LocalReport.ReportPath = @".\..\..\WelcomePage\Report.rdlc"; 
                report.LocalReport.DataSources.Add(new ReportDataSource()
                {
                    Name = "DataSet",
                    Value = CreataDataForShortHistoryDataGrid("pdf", startDate, endDate)
                });
                ReportParameter startDateParameter = new ReportParameter("StartDate", startDate.ToShortDateString());
                ReportParameter endDateParameter = new ReportParameter("EndDate", endDate.ToShortDateString());
                ReportParameter nameParameter = new ReportParameter("Name", Main_Classes.Budget.Instance.Name);
                report.LocalReport.SetParameters(new ReportParameter[] { startDateParameter, endDateParameter, nameParameter });
                byte[] bytes = report.LocalReport.Render("PDF");
                var dialog = new FolderBrowserDialog();
                DialogResult result = dialog.ShowDialog();
                if (result.ToString() == "OK")
                {
                    System.IO.FileStream newFile = new System.IO.FileStream(
                    dialog.SelectedPath + "\\" + Main_Classes.Budget.Instance.Name + ".pdf", System.IO.FileMode.Create);
                    newFile.Write(bytes, 0, bytes.Length);
                    newFile.Flush();
                    newFile.Close();
                    System.Windows.MessageBox.Show("Raport PDF wygenerowany poprawnie");
                }               
            }
           catch
           {
 
           }
        }
    }
}
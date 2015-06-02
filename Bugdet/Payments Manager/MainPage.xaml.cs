using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Budget.Main_Classes;

namespace Budget.Payments_Manager
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private AddPaymentPage _singlePaymentPage;
        private AddSalaryPage _singleSalaryPage;
        private int _lastSingleId = Main_Classes.Budget.Instance.Payments.Count > 0 ? Main_Classes.Budget.Instance.Payments.Keys.Max() : 0;
        private int _lastPeriodId = Main_Classes.Budget.Instance.Payments.Count > 0 ? Main_Classes.Budget.Instance.Payments.Keys.Min() : 0;

        public MainPage()
        {
            _singlePaymentPage = new AddPaymentPage(this);
            _singleSalaryPage = new AddSalaryPage(this);
            InitializeComponent();   
        }

        private void addPaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            BtnsContentFrame.Content = _singlePaymentPage;
            _singlePaymentPage.SinglePaymentRadio.IsChecked = true;
            Main_Classes.Budget.Instance.InsertCategories(_singlePaymentPage.CategoryBox,Main_Classes.Budget.CategoryTypeEnum.PAYMENT);
        }

        private void addSalaryBtn_Click(object sender, RoutedEventArgs e)
        {
            BtnsContentFrame.Content = _singleSalaryPage;
            _singleSalaryPage.SinglePaymentRadio.IsChecked = true;
            Main_Classes.Budget.Instance.InsertCategories(_singleSalaryPage.CategoryBox, Main_Classes.Budget.CategoryTypeEnum.SALARY);
        }

        public void LoadLastAddedDataGrid()
        {
            var added = new DataTable();
            added.Columns.Add("Id", typeof(int));
            added.Columns.Add("Type", typeof(int));
            added.Columns.Add("Nazwa", typeof(string));
            added.Columns.Add("Kwota", typeof(int));
            added.Columns.Add("Kategoria", typeof(string));
            foreach (var ch in Main_Classes.Budget.Instance.Payments)
            {
                if (ch.Value.GetType() == typeof (SinglePayment))
                {
                    if (ch.Key <= _lastSingleId)
                        continue;
                    var p = (SinglePayment) Main_Classes.Budget.Instance.Payments[ch.Key];
                    added.Rows.Add(ch.Key, p.Type, p.Name, p.Amount, Main_Classes.Budget.Instance.Categories[p.CategoryID].Name);
                }
                if (ch.Value.GetType() == typeof (PeriodPayment))
                {
                    if (ch.Key >= _lastPeriodId)
                        continue;
                    var p = (PeriodPayment) Main_Classes.Budget.Instance.Payments[ch.Key];
                    added.Rows.Add(ch.Key, p.Type, p.Name, p.Amount, Main_Classes.Budget.Instance.Categories[p.CategoryID].Name);
                }
            }
            LastAddedDataGrid.ItemsSource = added.DefaultView;
            if (LastAddedDataGrid.Columns.Count > 0)
            {
                LastAddedDataGrid.Columns[0].Visibility = Visibility.Hidden;
                LastAddedDataGrid.Columns[1].Visibility = Visibility.Hidden;
            }
        }

        public void LoadAllPeriodDataGrid()
        {
            var history = new DataTable();
            history.Columns.Add("Id", typeof (int));
            history.Columns.Add("Type", typeof(int));
            history.Columns.Add("Nazwa", typeof(string));
            history.Columns.Add("Kwota", typeof(int));
            history.Columns.Add("Kategoria", typeof(string));
            foreach(KeyValuePair<int,Payment> p in Main_Classes.Budget.Instance.Payments)
            {
                if (p.Value.GetType() != typeof (PeriodPayment))
                {
                    continue;
                }
                var pp = (PeriodPayment)p.Value;
                history.Rows.Add(p.Key,pp.Type,pp.Name, pp.Amount, Main_Classes.Budget.Instance.Categories[pp.CategoryID].Name);
            }
            PeriodPaymentsTable.ItemsSource = history.DefaultView;
            if (PeriodPaymentsTable.Columns.Count > 0)
            {
                PeriodPaymentsTable.Columns[0].Visibility = Visibility.Hidden;
                PeriodPaymentsTable.Columns[1].Visibility = Visibility.Hidden;
            }
        }

        private void AllPeriodDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAllPeriodDataGrid();
            if (PeriodPaymentsTable.Columns.Count > 0)
            {
                PeriodPaymentsTable.Columns[0].Visibility = Visibility.Hidden;
                PeriodPaymentsTable.Columns[1].Visibility = Visibility.Hidden;
            }
        }

        private void LastAddedDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            LoadLastAddedDataGrid();
            if (LastAddedDataGrid.Columns.Count > 0)
            {
                LastAddedDataGrid.Columns[0].Visibility = Visibility.Hidden;
                LastAddedDataGrid.Columns[1].Visibility = Visibility.Hidden;
            } 
        }

        private void LastAddedDataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dataRow = e.Row.DataContext as DataRowView;
            if (dataRow != null)
            {

                try
                {
                    e.Row.Background = (int)dataRow.Row.ItemArray[1] == 1 ? Brushes.Tomato : Brushes.SpringGreen;
                }
                catch (InvalidCastException)
                { }
            }
        }

        private void AllPeriodDataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dataRow = e.Row.DataContext as DataRowView;
            if (dataRow != null)
            {
                try
                {
                    e.Row.Background = (int)dataRow.Row.ItemArray[1] == 1 ? Brushes.Tomato : Brushes.SpringGreen;
                }
                catch (InvalidCastException)
                { }
            }
        }
    }
}
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
        private AddPaymentPage _singlePaymentPage = new AddPaymentPage();
        private AddSalaryPage _singleSalaryPage = new AddSalaryPage();
        private int _lastSingleId = Main_Classes.Budget.Instance.Payments.Keys.Max();
        private int _lastPeriodId = Main_Classes.Budget.Instance.Payments.Keys.Min();

        public MainPage()
        {
            InitializeComponent();
            Console.WriteLine(_lastPeriodId);
            Console.WriteLine(_lastSingleId);
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

        private void LoadAddedGrid()
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
                    Console.WriteLine(ch.Key);
                    if (ch.Key >= _lastPeriodId)
                        continue;
                    var p = (PeriodPayment) Main_Classes.Budget.Instance.Payments[ch.Key];
                    added.Rows.Add(ch.Key, p.Type, p.Name, p.Amount, Main_Classes.Budget.Instance.Categories[p.CategoryID].Name);
                }
            }
            LastAddedDataGrid.ItemsSource = added.DefaultView;
        }

        private void LoadHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshTable();
            LoadAddedGrid();
            if (LastAddedDataGrid.Columns.Count < 1)
            {
                return;
            } 
            LastAddedDataGrid.Columns[0].Visibility = Visibility.Hidden;
            LastAddedDataGrid.Columns[1].Visibility = Visibility.Hidden;
        }

        private void RefreshTable()
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
            PeriodPaymentsTable.Columns[0].Visibility = Visibility.Hidden;
            PeriodPaymentsTable.Columns[1].Visibility = Visibility.Hidden;
        }

        private void DumpAllButton_Click(object sender, RoutedEventArgs e)
        {
            Main_Classes.Budget.Instance.Dump();
        }

        private void dataGridView_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshTable();
        }

        private void DataGridView_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dataRow = e.Row.DataContext as DataRowView;
            if (dataRow == null)
            {
                return;
            }       
            try
            {
                e.Row.Background = (int)dataRow.Row.ItemArray[1] == 1 ? Brushes.Tomato : Brushes.SpringGreen;
            }
            catch (InvalidCastException)
            { }
        }

        private void UIElement_OnFocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            LoadAddedGrid();
            if (LastAddedDataGrid.Columns.Count < 1)
            {
                return;
            } 
            LastAddedDataGrid.Columns[0].Visibility = Visibility.Hidden;
            LastAddedDataGrid.Columns[1].Visibility = Visibility.Hidden;
        }

        private void LastAddedDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAddedGrid();
            if (LastAddedDataGrid.Columns.Count < 1)
            {
                return;
            }
            LastAddedDataGrid.Columns[0].Visibility = Visibility.Hidden;
            LastAddedDataGrid.Columns[1].Visibility = Visibility.Hidden;
        }

        private void LastAddedDataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            var dataRow = e.Row.DataContext as DataRowView;
            if (dataRow == null)
            {
                return;
            }
            try
            {
                e.Row.Background = (int)dataRow.Row.ItemArray[1] == 1 ? Brushes.Tomato : Brushes.SeaGreen;
            }
            catch (InvalidCastException)
            { }
        }
    }
}
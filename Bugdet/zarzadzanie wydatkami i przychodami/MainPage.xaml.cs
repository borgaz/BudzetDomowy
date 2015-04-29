using System;
using System.Windows;
using System.Collections.Generic;
using System.Data;
using System.Windows.Controls;
using Budget.Nowy_budzet;
using System.Windows.Media;
using Budget.Main_Classes;

namespace Budget.zarzadzanie_wydatkami_i_przychodami
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private AddPaymentPage _singlePaymentPage = new AddPaymentPage();
        private AddSalaryPage _singleSalaryPage = new AddSalaryPage();
        public MainPage()
        {
            InitializeComponent();
        }

        private void addPaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            BtnsContentFrame.Content = _singlePaymentPage;
        }

        private void addSalaryBtn_Click(object sender, RoutedEventArgs e)
        {
            BtnsContentFrame.Content = _singleSalaryPage;
        }

        private void NewBudgetButton_Click(object sender, RoutedEventArgs e)
        {
            new MakeBudgetWindow(1).ShowDialog();
        }

        private void LoadHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshTable();
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
                    continue;
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
                return;
            try
            {
                e.Row.Background = (int)dataRow.Row.ItemArray[1] == 1 ? Brushes.Red : Brushes.ForestGreen;
            }
            catch (InvalidCastException)
            {
                
            }
        }
    }
}
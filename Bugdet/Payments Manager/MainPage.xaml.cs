using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Budget.Main_Classes;
using Budget.New_Budget;
using Budget.Utility_Classes;
using Budget = Budget.Main_Classes.Budget;


namespace Budget.Payments_Manager
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
            var history = new DataTable();
            history.Columns.Add("Id", typeof(int));
            history.Columns.Add("Type", typeof(int));
            history.Columns.Add("Nazwa", typeof(string));
            history.Columns.Add("Kwota", typeof(int));
            history.Columns.Add("Kategoria", typeof(string));
            foreach (var ch in Main_Classes.Budget.Instance.ListOfAdds)
            {
                if (ch.Type == typeof (SinglePayment))
                {
                    var p = (SinglePayment) Main_Classes.Budget.Instance.Payments[ch.ID];
                    history.Rows.Add(ch.ID, p.Type, p.Name, p.Amount,
                        Main_Classes.Budget.Instance.Categories[p.CategoryID].Name);
                }
                if (ch.Type == typeof (PeriodPayment))
                {
                    var p = (PeriodPayment) Main_Classes.Budget.Instance.Payments[ch.ID];
                    history.Rows.Add(ch.ID, p.Type, p.Name, p.Amount,
                        Main_Classes.Budget.Instance.Categories[p.CategoryID].Name);
                }
            }
        }

        private void LoadHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshTable();
            //Main_Classes.Budget.Instance.Payments[2].Amount = 12.22;
            //Main_Classes.Budget.Instance.ListOfEdts.Add(new Changes(typeof(SinglePayment), 2));
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

        private void LastAddedDataGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadAddedGrid();
        }

        private void UIElement_OnFocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            LoadAddedGrid();
        }
    }
}
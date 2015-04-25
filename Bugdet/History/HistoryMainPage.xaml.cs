using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Budget.History
{
    /// <summary>
    /// Interaction logic for HistoryMainPage.xaml
    /// </summary>
    public partial class HistoryMainPage : Page
    {

        public HistoryMainPage()
        {
            InitializeComponent();
            RefreshTable();
        }

        private void RefreshTableButton_Click(object sender, RoutedEventArgs e)
        {
           RefreshTable();
        }

        private void RefreshTable()
        {
            DataTable history = new DataTable();
            history.Columns.Add("Type", typeof(bool));
            history.Columns.Add("Id", typeof(int));
            history.Columns.Add("Nazwa", typeof(string));
            history.Columns.Add("Wynagrodzenie", typeof(int));
            history.Columns.Add("Kategoria", typeof(string));
            history.Columns.Add("Powtarzalność", typeof(string));
            history.Columns.Add("Od Kiedy", typeof(string));
            history.Columns.Add("Do Kiedy", typeof(string));
            history.Columns.Add("Saldo", typeof(double));
            foreach (KeyValuePair<int, Payment> p in Budget.Instance.Payments)
            {
                if (p.Value.Type && p.Value.GetType() == typeof(PeriodPayment) && PeriodPaymentCheckBox.IsChecked == true)
                {
                    PeriodPayment pp = (PeriodPayment)p.Value;
                    history.Rows.Add(pp.Type, p.Key, "[OKRESOWE]" + pp.Name, pp.Amount, Budget.Instance.Categories[pp.CategoryID].Name,
                        "Co " + pp.Frequency + " " + pp.Period, pp.StartDate,
                        (pp.EndDate.Equals(DateTime.MaxValue) ? "Nie zdefiniowano" : pp.EndDate.ToString()));
                }
                if (p.Value.Type && p.Value.GetType() == typeof(SinglePayment) && SinglePaymentCheckBox.IsChecked == true)
                {
                    SinglePayment pp = (SinglePayment)p.Value;
                    history.Rows.Add(pp.Type, p.Key, pp.Name, pp.Amount, Budget.Instance.Categories[pp.CategoryID].Name,
                        "Brak", pp.Date,
                        "Brak");
                }
                if (!p.Value.Type && p.Value.GetType() == typeof(PeriodPayment) && PeriodSalaryCheckBox.IsChecked == true)
                {
                    PeriodPayment pp = (PeriodPayment)p.Value;
                    history.Rows.Add(pp.Type, p.Key, "[OKRESOWE]" + pp.Name, pp.Amount, Budget.Instance.Categories[pp.CategoryID].Name,
                        "Co " + pp.Frequency + " " + pp.Period, pp.StartDate,
                        (pp.EndDate.Equals(DateTime.MaxValue) ? "Nie zdefiniowano" : pp.EndDate.ToString()));
                }
                if (!p.Value.Type && p.Value.GetType() == typeof(SinglePayment) && SingleSalaryCheckBox.IsChecked == true)
                {
                    SinglePayment pp = (SinglePayment)p.Value;
                    history.Rows.Add(pp.Type, p.Key, pp.Name, pp.Amount, Budget.Instance.Categories[pp.CategoryID].Name,
                        "Brak", pp.Date,
                        "Brak");
                }
            }
            HistoryDataGrid.ItemsSource = history.DefaultView;

            new Thread(HideThread).Start();
        }

        private void HideThread()
        {
            while (HistoryDataGrid.Columns.Count == 0)
            { }
            Dispatcher.Invoke(HideColumns);
        }
        private void HideColumns()
        {
            
            HistoryDataGrid.Columns[0].Visibility = Visibility.Hidden;
            HistoryDataGrid.Columns[1].Visibility = Visibility.Hidden;
        }
        private void HistoryDataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataRowView dataRow = e.Row.DataContext as DataRowView;
            if (dataRow == null)
                return;
            if ((bool)dataRow.Row.ItemArray[0])
            {
                e.Row.Background = Brushes.Firebrick;
            }
            else
            {
                e.Row.Background = Brushes.ForestGreen;
            }
        }

        private void DeleteItem_OnClick(object sender, RoutedEventArgs e)
        {
            DataRowView dataRow;
            try
            {
                dataRow = (DataRowView)HistoryDataGrid.SelectedItem;
                if (dataRow.Row.ItemArray[2].ToString().Contains("[OKRESOWE]"))
                {
                    Budget.Instance.DeletePeriodPayment(Convert.ToInt32(dataRow.Row.ItemArray[1]));
                }
                else
                {
                    Budget.Instance.DeleteSinglePayment(Convert.ToInt32(dataRow.Row.ItemArray[1]));
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Zaznacz poprawny rekord!");
            }
            RefreshTable();
        }

        private void UpdateItem_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PeriodPaymentCheckBox_OnClick(object sender, RoutedEventArgs e)
        {
            RefreshTable();
        }
    }
}

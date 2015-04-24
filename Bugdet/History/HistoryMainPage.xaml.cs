using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
        }

        private void RefreshTableButton_Click(object sender, RoutedEventArgs e)
        {
            DataTable history = new DataTable();
           // history.Columns.Add("id", typeof(int));
            history.Columns.Add("Type", typeof(bool));
            history.Columns.Add("Nazwa", typeof(string));
            history.Columns.Add("Wynagrodzenie", typeof(int));
            history.Columns.Add("Kategoria", typeof(string));
            history.Columns.Add("Powtarzalność", typeof(string));
            history.Columns.Add("Od Kiedy", typeof(string));
            history.Columns.Add("Do Kiedy", typeof(string));
            history.Columns.Add("Saldo", typeof (double));
            foreach(Payment p in Budget.Instance.Payments.Values)
            {
                if (p.Type && p.GetType() == typeof(PeriodPayment) && PeriodPaymentCheckBox.IsChecked == true)
                {
                    PeriodPayment pp = (PeriodPayment) p;
                    history.Rows.Add(pp.Type,"[OKRESOWE]" + pp.Name, pp.Amount, Budget.Instance.Categories[pp.CategoryID].Name,
                        "Co " + pp.Frequency + " " + pp.Period, pp.StartDate,
                        (pp.EndDate.Equals(DateTime.MaxValue) ? "Nie zdefiniowano" : pp.EndDate.ToString()));
                }
                if (p.Type && p.GetType() == typeof (SinglePayment) && SinglePaymentCheckBox.IsChecked == true)
                {
                    SinglePayment pp = (SinglePayment)p;
                    history.Rows.Add(pp.Type,pp.Name, pp.Amount, Budget.Instance.Categories[pp.CategoryID].Name,
                        "Brak", pp.Date,
                        "Brak");
                }
                if (!p.Type && p.GetType() == typeof(PeriodPayment) && PeriodSalaryCheckBox.IsChecked == true)
                {
                    PeriodPayment pp = (PeriodPayment)p;
                    history.Rows.Add(pp.Type,"[OKRESOWE]" + pp.Name, pp.Amount, Budget.Instance.Categories[pp.CategoryID].Name,
                        "Co " + pp.Frequency + " " + pp.Period, pp.StartDate,
                        (pp.EndDate.Equals(DateTime.MaxValue) ? "Nie zdefiniowano" : pp.EndDate.ToString()));
                }
                if (!p.Type && p.GetType() == typeof(SinglePayment) && SingleSalaryCheckBox.IsChecked == true)
                {
                    SinglePayment pp = (SinglePayment)p;
                    history.Rows.Add(pp.Type,pp.Name, pp.Amount, Budget.Instance.Categories[pp.CategoryID].Name,
                        "Brak", pp.Date,
                        "Brak");
                }
            }
            HistoryDataGrid.ItemsSource = history.DefaultView;

            HistoryDataGrid.Columns[0].Visibility = Visibility.Hidden;
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
    }
}

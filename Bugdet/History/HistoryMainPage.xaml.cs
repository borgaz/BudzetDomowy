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
using Budget.Classes;

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
            Classes.Budget.Instance.InsertCategories(CategoryComboBox,Classes.Budget.CategoryTypeEnum.ANY);
        }
        private void RefreshTable()
        {
            DataTable history = new DataTable();
            history.Columns.Add("Type", typeof(bool));
            history.Columns.Add("Id", typeof(int));
            history.Columns.Add("Nazwa", typeof(string));
            history.Columns.Add("Kategoria", typeof(string));
            history.Columns.Add("Data", typeof(string));
            history.Columns.Add("Kwota", typeof(double));
            foreach (KeyValuePair<int, BalanceLog> p in Classes.Budget.Instance.BalanceLog)
            {
                if (!((p.Value.Date >= (StartDateCheckBox.IsChecked == true ? StartDatePicker.SelectedDate : DateTime.MinValue)) &&
                      (p.Value.Date <= (EndDateCheckBox.IsChecked == true ? EndDatePicker.SelectedDate : DateTime.MaxValue))))
                    continue;
                if (p.Value.PeriodPaymentID == 0)
                {
                    if (p.Value.SinglePaymentID == 0)
                        continue;
                    SinglePayment pp = (SinglePayment)Classes.Budget.Instance.Payments[p.Value.SinglePaymentID];
                    if (pp.Type && SinglePaymentCheckBox.IsChecked == true)
                    {
                        history.Rows.Add(pp.Type, p.Key, pp.Name, Classes.Budget.Instance.Categories[pp.CategoryID].Name,
                            p.Value.Date.ToShortDateString(), pp.Amount);
                    }
                    if (!pp.Type && SingleSalaryCheckBox.IsChecked == true)
                    {
                        history.Rows.Add(pp.Type, p.Key, pp.Name, Classes.Budget.Instance.Categories[pp.CategoryID].Name,
                            p.Value.Date.ToShortDateString(), pp.Amount);
                    }
                }
                else
                {
                    PeriodPayment pp = (PeriodPayment)Classes.Budget.Instance.Payments[p.Value.PeriodPaymentID];
                    if (pp.Type && PeriodPaymentCheckBox.IsChecked == true)
                    {
                        history.Rows.Add(pp.Type, p.Key, pp.Name, Classes.Budget.Instance.Categories[pp.CategoryID].Name,
                            p.Value.Date.ToShortDateString(), pp.Amount);
                    }
                    if (!pp.Type && PeriodSalaryCheckBox.IsChecked == true)
                    {
                        history.Rows.Add(pp.Type, p.Key, pp.Name, Classes.Budget.Instance.Categories[pp.CategoryID].Name,
                            p.Value.Date.ToShortDateString(), pp.Amount);
                    }
                }
            }
            HistoryDataGrid.ItemsSource = history.DefaultView;

            HistoryDataGrid.Columns[0].Visibility = Visibility.Hidden;
            HistoryDataGrid.Columns[1].Visibility = Visibility.Hidden;
        }
        private void HistoryDataGrid_OnLoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataRowView dataRow = e.Row.DataContext as DataRowView;
            if (dataRow == null)
                return;
            try
            {
                if ((bool)dataRow.Row.ItemArray[0])
                {
                    e.Row.Background = Brushes.Red;
                }
                else
                {
                    e.Row.Background = Brushes.ForestGreen;
                }
            }
            catch (InvalidCastException)
            {
                MessageBox.Show("Zaznacz poprawny rekord");
                throw;
            }

        }

        private void DeleteItem_OnClick(object sender, RoutedEventArgs e)
        {
            DataRowView dataRow;
            try
            {
                dataRow = (DataRowView)HistoryDataGrid.SelectedItem;
                if ((int)dataRow.Row.ItemArray[1] < 0)
                {
                    Classes.Budget.Instance.DeletePeriodPayment(Convert.ToInt32(dataRow.Row.ItemArray[1]));
                }
                else
                {
                    Classes.Budget.Instance.DeleteSinglePayment(Convert.ToInt32(dataRow.Row.ItemArray[1]));
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

        private void InfoItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView dataRow = (DataRowView)HistoryDataGrid.SelectedItem;
                if ((int)dataRow.Row.ItemArray[1] < 0) // Dla Period -- wydaja sie ze to samo,ale to po prostu taka furtka jakby w przyszlosci sie wyswietlalo rozne wiadomosci
                {
                    MessageBox.Show("Opis: " +
                                    Classes.Budget.Instance.Payments[
                                        Classes.Budget.Instance.BalanceLog[(int) dataRow.Row.ItemArray[1]].PeriodPaymentID].Note +
                                    "\n Saldo w trakcie tej transakcji: " +
                                    Classes.Budget.Instance.BalanceLog[(int) dataRow.Row.ItemArray[1]].Balance +
                                    "\nTyp Płatności: OKRESOWA");
                }
                else // dla single
                {
                    MessageBox.Show("Opis: " +
                                    Classes.Budget.Instance.Payments[
                                        Classes.Budget.Instance.BalanceLog[(int) dataRow.Row.ItemArray[1]].SinglePaymentID].Note +
                                    "\n Saldo w trakcie tej transakcji: " +
                                    Classes.Budget.Instance.BalanceLog[(int) dataRow.Row.ItemArray[1]].Balance +
                                    "\nTyp Płatności: POJEDYNCZA");
                        // wiem ze ladny lancuszek
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("zaznacz poprawnie rekord!");
            }

        }

        private void StartDateCheckBox_OnClick(object sender, RoutedEventArgs e)
        {
            StartDatePicker.IsEnabled = StartDateCheckBox.IsChecked == true;
            RefreshTable();
        }

        private void EndDateCheckBox_OnClick(object sender, RoutedEventArgs e)
        {
            EndDatePicker.IsEnabled = EndDateCheckBox.IsChecked == true;
            RefreshTable();
        }

        private void CategoryCheckBox_OnClick(object sender, RoutedEventArgs e)
        {
            CategoryComboBox.IsEnabled = CategoryCheckBox.IsChecked == true;
            RefreshTable();
        }

        private void HistoryDataGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            RefreshTable();
        }
    }
}

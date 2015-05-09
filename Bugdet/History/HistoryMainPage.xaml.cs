using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Budget.Main_Classes;
using ComboBoxItem = Budget.Utility_Classes.ComboBoxItem;

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
            Main_Classes.Budget.Instance.InsertCategories(CategoryComboBox,Main_Classes.Budget.CategoryTypeEnum.ANY);
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
            foreach (KeyValuePair<int, BalanceLog> p in Main_Classes.Budget.Instance.BalanceLog)
            {
                if (!((p.Value.Date >= (StartDateCheckBox.IsChecked == true ? StartDatePicker.SelectedDate : DateTime.MinValue)) &&
                      (p.Value.Date <= (EndDateCheckBox.IsChecked == true ? EndDatePicker.SelectedDate : DateTime.MaxValue))))
                    continue;
                if (p.Value.PeriodPaymentID == 0)
                {
                    // oba id sa 0 to nie bierzemy pod uwage - zepsute rekordy przy dodawaniu w kreatorze
                    if (p.Value.SinglePaymentID == 0)
                        continue;
                    var pp = (SinglePayment)Main_Classes.Budget.Instance.Payments[p.Value.SinglePaymentID];
                    // jesli wartosc nie zawiera sie w sliderze to nie bierzemy pod uwage
                    if (
                        !((pp.Amount < AmountSlider.Value && LowerRadio.IsChecked == true) ||
                          (pp.Amount > AmountSlider.Value && GreaterRadio.IsChecked == true)))
                        continue;
                    // jesli kategoria jest ustawiona i nie jest taka sama to nie bierzemy pod uwage.
                    if (CategoryCheckBox.IsChecked == true && CategoryComboBox.SelectedIndex != -1)
                    {
                        var categoryItem = (ComboBoxItem)CategoryComboBox.SelectedValue;
                        if (categoryItem.Id != pp.CategoryID)
                            continue;
                    }

                    if (pp.Type && SinglePaymentCheckBox.IsChecked == true)
                    {
                        history.Rows.Add(pp.Type, p.Key, pp.Name, Main_Classes.Budget.Instance.Categories[pp.CategoryID].Name,
                            p.Value.Date.ToShortDateString(), pp.Amount);
                    }
                    if (!pp.Type && SingleSalaryCheckBox.IsChecked == true)
                    {
                        history.Rows.Add(pp.Type, p.Key, pp.Name, Main_Classes.Budget.Instance.Categories[pp.CategoryID].Name,
                            p.Value.Date.ToShortDateString(), pp.Amount);
                    }
                }
                else
                {
                    var pp = (PeriodPayment)Main_Classes.Budget.Instance.Payments[p.Value.PeriodPaymentID];
                    if (
                        !((pp.Amount < AmountSlider.Value && LowerRadio.IsChecked == true) ||
                          (pp.Amount > AmountSlider.Value && GreaterRadio.IsChecked == true)))
                        continue;
                    if (CategoryCheckBox.IsChecked == true &&
                        !Main_Classes.Budget.Instance.Categories[pp.CategoryID].Name.Equals(
                            CategoryComboBox.SelectedValue.ToString()))
                        continue;
                    if (pp.Type && PeriodPaymentCheckBox.IsChecked == true)
                    {
                        history.Rows.Add(pp.Type, p.Key, pp.Name, Main_Classes.Budget.Instance.Categories[pp.CategoryID].Name,
                            p.Value.Date.ToShortDateString(), pp.Amount);
                    }
                    if (!pp.Type && PeriodSalaryCheckBox.IsChecked == true)
                    {
                        history.Rows.Add(pp.Type, p.Key, pp.Name, Main_Classes.Budget.Instance.Categories[pp.CategoryID].Name,
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
            var dataRow = e.Row.DataContext as DataRowView;
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
                    Main_Classes.Budget.Instance.DeletePeriodPayment(
                        Main_Classes.Budget.Instance.BalanceLog[(int)dataRow.Row.ItemArray[1]].PeriodPaymentID
                        );
                }
                else
                {
                    Main_Classes.Budget.Instance.DeleteSinglePayment(
                        Main_Classes.Budget.Instance.BalanceLog[(int)dataRow.Row.ItemArray[1]].SinglePaymentID, (int)dataRow.Row.ItemArray[1]);
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
                var dataRow = (DataRowView)HistoryDataGrid.SelectedItem;
                if ((int)dataRow.Row.ItemArray[1] < 0) // Dla Period -- wydaja sie ze to samo,ale to po prostu taka furtka jakby w przyszlosci sie wyswietlalo rozne wiadomosci
                {
                    MessageBox.Show("Opis: " +
                                    Main_Classes.Budget.Instance.Payments[
                                        Main_Classes.Budget.Instance.BalanceLog[(int) dataRow.Row.ItemArray[1]].PeriodPaymentID].Note +
                                    "\nSaldo po operacji: " +
                                    Main_Classes.Budget.Instance.BalanceLog[(int) dataRow.Row.ItemArray[1]].Balance +
                                    "\nTyp Płatności: OKRESOWA");
                }
                else // dla single
                {
                    Console.WriteLine((int)dataRow.Row.ItemArray[1]);
                    MessageBox.Show("Opis: " +
                                    Main_Classes.Budget.Instance.Payments[
                                        Main_Classes.Budget.Instance.BalanceLog[(int) dataRow.Row.ItemArray[1]].SinglePaymentID].Note +
                                    "\nSaldo po operacji: " +
                                    Main_Classes.Budget.Instance.BalanceLog[(int) dataRow.Row.ItemArray[1]].Balance +
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

        private void AmountSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SliderValueTextBox.Text = Convert.ToInt32(AmountSlider.Value).ToString();
            RefreshTable();
        }

        private void SliderValueTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
           if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private void SliderValueTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!SliderValueTextBox.IsFocused)
                return;
            if (SliderValueTextBox.Text == "")
                AmountSlider.Value = 0;
            AmountSlider.Value = Convert.ToInt32(SliderValueTextBox.Text);
        }

        private void AmountSlider_Loaded(object sender, RoutedEventArgs e)
        {
            AmountSlider.Value = 0;
            AmountSlider.Maximum = Main_Classes.Budget.Instance.MaxAmount;
        }
    }
}

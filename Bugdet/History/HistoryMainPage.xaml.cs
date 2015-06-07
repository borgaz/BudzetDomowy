using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Budget.Main_Classes;
using System.Linq;
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
           // Main_Classes.Budget.Instance.InsertCategories(CategoryComboBox, Main_Classes.Budget.CategoryTypeEnum.ANY);
            CheckBoxForCategories();
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
            history.Columns.Add("Saldo", typeof(double));

            foreach (KeyValuePair<int, BalanceLog> p in Main_Classes.Budget.Instance.BalanceLog.Reverse())
            {
                if (!((p.Value.Date >= (StartDateCheckBox.IsChecked == true ? StartDatePicker.SelectedDate : DateTime.MinValue)) &&
                      (p.Value.Date <= (EndDateCheckBox.IsChecked == true ? EndDatePicker.SelectedDate : DateTime.MaxValue))))
                    continue;
                if (p.Value.PeriodPaymentID != 0) continue;
                // oba id sa 0 to nie bierzemy pod uwage - zepsute rekordy przy dodawaniu w kreatorze
                if (p.Value.SinglePaymentID == 0)
                    continue;
                var pp = (SinglePayment)Main_Classes.Budget.Instance.Payments[p.Value.SinglePaymentID];
                // jesli wartosc nie zawiera sie w sliderze to nie bierzemy pod uwage
                if (pp.Amount > LowerAmountSlider.Value || pp.Amount < HigherAmountSlider.Value)
                    continue;
                // jesli kategoria jest odhaczona to nie bierzemy pod uwage.
                if (CategoryCheckBox.IsChecked == true)
                {
                    var add = false;
                    foreach (var cat in CategoryComboBox.Items.Cast<CheckBox>().Where(cat => Convert.ToInt32(cat.Name.Substring(1)) == pp.CategoryID && cat.IsChecked == true)) {add = true; }
                    if (!add) continue;
                }

                if (pp.Type && SinglePaymentCheckBox.IsChecked == true)
                {
                    var fd = Main_Classes.Budget.Instance.Categories[pp.CategoryID].Name;
                    history.Rows.Add(pp.Type, p.Key, pp.Name, Main_Classes.Budget.Instance.Categories[pp.CategoryID].Name,
                        pp.Date.ToShortDateString(), pp.Amount, p.Value.Balance);
                }
                if (!pp.Type && SingleSalaryCheckBox.IsChecked == true)
                {
                    history.Rows.Add(pp.Type, p.Key, pp.Name, Main_Classes.Budget.Instance.Categories[pp.CategoryID].Name,
                        pp.Date.ToShortDateString(), pp.Amount, p.Value.Balance);
                }
            }

            HistoryDataGrid.ItemsSource = history.DefaultView;

            HistoryDataGrid.Columns[0].Visibility = Visibility.Hidden;
            HistoryDataGrid.Columns[1].Visibility = Visibility.Hidden;
            HistoryDataGrid.Columns[5].DisplayIndex = 1;

            // -- to powinno dzialac wg MSDN ;(
            //var a = new DataTemplate();
            //a.VisualTree = new FrameworkElementFactory(typeof(TextBlock));
            //a.VisualTree.SetValue(TextBlock.FontSizeProperty,12);
            //HistoryDataGrid.Columns[5].HeaderTemplate = a;


            HistoryDataGrid.Columns[2].DisplayIndex = 2;
            HistoryDataGrid.Columns[2].Width = 170;
            HistoryDataGrid.Columns[4].DisplayIndex = 3;
            HistoryDataGrid.Columns[3].DisplayIndex = 4;
            HistoryDataGrid.Columns[6].DisplayIndex = 5;
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
                    e.Row.Background = Brushes.Tomato;
                }
                else
                {
                    //e.Row.Background = Brushes.ForestGreen;
                    e.Row.Background = Brushes.SpringGreen;
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

            DataRowView dataRow;
            try
            {
                dataRow = (DataRowView)HistoryDataGrid.SelectedItem;
                var balanceId = Main_Classes.Budget.Instance.BalanceLog[(int)dataRow.Row.ItemArray[1]];
                new EditRecordWindow(balanceId.SinglePaymentID).ShowDialog();
            }
            catch (Exception ex)
            {
                SqlConnect.Instance.ErrLog(ex);
            }

            RefreshTable();
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
                                        Main_Classes.Budget.Instance.BalanceLog[(int)dataRow.Row.ItemArray[1]].PeriodPaymentID].Note +
                                    "\nSaldo po operacji: " +
                                    Main_Classes.Budget.Instance.BalanceLog[(int)dataRow.Row.ItemArray[1]].Balance +
                                    "\nTyp Płatności: OKRESOWA");
                }
                else // dla single
                {
                    MessageBox.Show("Opis: " +
                                    Main_Classes.Budget.Instance.Payments[
                                        Main_Classes.Budget.Instance.BalanceLog[(int)dataRow.Row.ItemArray[1]].SinglePaymentID].Note +
                                    "\nSaldo po operacji: " +
                                    Main_Classes.Budget.Instance.BalanceLog[(int)dataRow.Row.ItemArray[1]].Balance +
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
            HigherSliderValueTextBox.Text = Convert.ToInt32(HigherAmountSlider.Value).ToString();
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
            if (!HigherSliderValueTextBox.IsFocused)
                return;
            if (HigherSliderValueTextBox.Text == "")
                HigherAmountSlider.Value = 0;
            HigherAmountSlider.Value = Convert.ToInt32(HigherSliderValueTextBox.Text);
        }

        private void CheckBoxForCategories()
        {
            foreach (var cat in Main_Classes.Budget.Instance.Categories)
            {
                var c = new CheckBox {Content = cat.Value.Name, Name = "a" + cat.Key, IsChecked = true};
                CategoryComboBox.Items.Add(c);
            }
        }

        private void AmountSlider_Loaded(object sender, RoutedEventArgs e)
        {
            HigherAmountSlider.Value = 0;
            HigherAmountSlider.Maximum = Main_Classes.Budget.Instance.MaxAmount;
        }

        private void CategoryComboBox_OnDropDownClosed(object sender, EventArgs e)
        {
            RefreshTable();
        }

        private void LowerAmountSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LowerSliderValueTextBox.Text = Convert.ToInt32(LowerAmountSlider.Value).ToString();
            RefreshTable();
        }

        private void LowerAmountSlider_OnLoaded(object sender, RoutedEventArgs e)
        {
            LowerAmountSlider.Maximum = Main_Classes.Budget.Instance.MaxAmount;
            LowerAmountSlider.Value = LowerAmountSlider.Maximum;
        }

        private void LowerSliderValueTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private void LowerSliderValueTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!LowerSliderValueTextBox.IsFocused)
                return;
            if (LowerSliderValueTextBox.Text == "")
                LowerAmountSlider.Value = 0;
            LowerAmountSlider.Value = Convert.ToInt32(LowerSliderValueTextBox.Text);
        }
    }
}

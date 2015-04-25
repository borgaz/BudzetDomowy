using Budget.Nowy_budzet;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Budget.zarzadzanie_wydatkami_i_przychodami
{
    /// <summary>
    /// Interaction logic for AddSalaryPage.xaml
    /// </summary>
    public partial class AddSalaryPage : Page
    {
        public AddSalaryPage()
        {
            InitializeComponent();
            Budget.Instance.InsertCategories(CategoryBox, true);
            InsertDateTypes(TypeOfDayComboBox);
        }
        private void addSalaryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentName.Text != "" && PaymentValue.Text != "" && CategoryBox.SelectedIndex != -1)
            {
                ComboBoxItem categoryItem = (ComboBoxItem)CategoryBox.SelectedValue;
                if (PeriodCheckBox.IsChecked == true)
                {
                    int temp_id = -1;
                    try
                    {
                        temp_id = Budget.Instance.Payments.First().Key - 1;
                    }
                    catch (Exception ex)
                    { } //gdy brak elementów w tablicy temp_id = 1
                   // Budget.Instance.ListOfAdds.Add(new Changes(typeof(PeriodPayment), temp_id));
                    Budget.Instance.AddPeriodPayment(temp_id,
                        new PeriodPayment(categoryItem.Id,
                            Convert.ToDouble(PaymentValue.Text),
                            Note.Text,
                            false,
                            PaymentName.Text,
                            Convert.ToInt32(NumberOfTexBox.Text),
                            TypeOfDayComboBox.Text,
                            Convert.ToDateTime(StartDatePicker.Text),
                            Convert.ToDateTime(StartDatePicker.Text),
                            (EndDatePicker.IsEnabled == true ? Convert.ToDateTime(EndDatePicker.Text) : DateTime.MaxValue)));
                }
                else
                {
                    int temp_id = 1;
                    try
                    {
                        temp_id = Budget.Instance.Payments.Last().Key + 1;
                    }
                    catch (Exception ex)
                    { } //gdy brak elementów w tablicy temp_id = 1
                    int temp_id_balance = Budget.Instance.BalanceLog.Last().Key + 1;

                    //Budget.Instance.ListOfAdds.Add(new Changes(typeof(SinglePayment), temp_id));
                    Budget.Instance.AddSinglePayment(temp_id,
                        new SinglePayment(Note.Text, Convert.ToDouble(PaymentValue.Text), categoryItem.Id, false,
                            PaymentName.Text, DateTime.Now));
                    // uwaga tutaj dodajemy
                    double currentBalance = Budget.Instance.BalanceLog.Last().Value.Balance + Convert.ToDouble(PaymentValue.Text);
                    Budget.Instance.AddBalanceLog(temp_id_balance,
                        new BalanceLog(currentBalance, DateTime.Today, temp_id, 0));
                   // Budget.Instance.ListOfAdds.Add(new Changes(typeof(BalanceLog), temp_id_balance));

                }

                InfoBox.Text = "Dodano!";
                InfoBox.Foreground = Brushes.Green;
                PaymentName.Text = "";
                PaymentValue.Text = "";
                CategoryBox.SelectedIndex = -1;
                NumberOfTexBox.Text = "";
                TypeOfDayComboBox.SelectedIndex = -1;
                PeriodCheckBox.IsChecked = false;
                StartDatePicker.Text = "";
                EndDateEnableCheckBox.IsChecked = false;
                EndDatePicker.Text = "";
            }
            else
            {
                InfoBox.Text = "Uzupełnij wymagane pola.";
                InfoBox.Foreground = Brushes.Red;
            }
        }

        public static void InsertDateTypes(ComboBox cBox)
        {
            cBox.Items.Add("DZIEŃ");
            cBox.Items.Add("TYDZIEŃ");
            cBox.Items.Add("MIESIĄC");
            cBox.Items.Add("ROK");
        }

        private void periodCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PeriodInfoGrid.IsEnabled = true;
            if (EndDateEnableCheckBox.IsChecked == false)
                EndDatePicker.IsEnabled = false;
        }

        private void PeriodCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            PeriodInfoGrid.IsEnabled = false;
        }

        private void EndDateEnableCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            EndDatePicker.IsEnabled = true;
        }

        private void EndDateEnableCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            EndDatePicker.IsEnabled = false;
        }

        private void AddCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddCategoryWindow().ShowDialog();
            Budget.Instance.InsertCategories(CategoryBox, true);
        }
    }
}
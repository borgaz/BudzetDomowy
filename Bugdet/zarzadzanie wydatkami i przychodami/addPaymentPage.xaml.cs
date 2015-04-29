using Budget.Nowy_budzet;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Budget.Main_Classes;
using ComboBoxItem = Budget.Utility_Classes.ComboBoxItem;

namespace Budget.zarzadzanie_wydatkami_i_przychodami
{
    /// <summary>
    /// Interaction logic for addPaymentPage.xaml
    /// </summary>
    public partial class AddPaymentPage : Page
    {
        public AddPaymentPage()
        {
            InitializeComponent();
            AddSalaryPage.InsertDateTypes(TypeOfDayComboBox);
            Main_Classes.Budget.Instance.InsertCategories(CategoryBox, Main_Classes.Budget.CategoryTypeEnum.PAYMENT);
            StartDatePicker.Text = DateTime.Now.Date.ToString();
            //System.Console.WriteLine("asd: " + StartDatePicker.Text + " .");
        }

        private void addPaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentName.Text != "" && PaymentValue.Text != "" && CategoryBox.SelectedIndex != -1)
            {
                ComboBoxItem categoryItem = (ComboBoxItem)CategoryBox.SelectedValue;
                
                if (PeriodCheckBox.IsChecked == true)
                {
                    int temp_id = -1;
                    try
                    { 
                        temp_id = Main_Classes.Budget.Instance.Payments.First().Key - 1; 
                    }
                    catch (Exception ex)
                    { } //gdy brak elementów w tablicy temp_id = 1
                  //  Budget.Instance.ListOfAdds.Add(new Changes(typeof(PeriodPayment), temp_id));
                    Main_Classes.Budget.Instance.AddPeriodPayment(temp_id,
                        new PeriodPayment(categoryItem.Id, 
                            Convert.ToDouble(PaymentValue.Text), 
                            Note.Text, 
                            true,
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
                        temp_id = Main_Classes.Budget.Instance.Payments.Last().Key + 1;
                        Console.WriteLine(temp_id);
                        if (temp_id == 0) // w bazie chcemy singlePays indeksowane od 1
                            temp_id = 1;
                    }
                    catch (Exception ex)
                    { } //gdy brak elementów w tablicy temp_id = 1
                    //Budget.Instance.ListOfAdds.Add(new Changes(typeof(SinglePayment), temp_id));
                    Main_Classes.Budget.Instance.AddSinglePayment(temp_id,
                        new SinglePayment(Note.Text, Convert.ToDouble(PaymentValue.Text), categoryItem.Id, true,
                            PaymentName.Text, DateTime.Now));

                    int temp_id_balance = Main_Classes.Budget.Instance.BalanceLog.Last().Key + 1;
                    // uwaga tutaj odejmujemy
                    double currentBalance = Main_Classes.Budget.Instance.BalanceLog.Last().Value.Balance - Convert.ToDouble(PaymentValue.Text);
                    Main_Classes.Budget.Instance.AddBalanceLog(temp_id_balance,
                        new BalanceLog(currentBalance, DateTime.Today, temp_id, 0));
                    //Budget.Instance.ListOfAdds.Add(new Changes(typeof(BalanceLog), temp_id_balance));

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
        private void periodCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PeriodInfoGrid.IsEnabled = true;
            if (EndDateEnableCheckBox.IsChecked == false)
            {
                EndDatePicker.IsEnabled = false;
            }    
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
            Main_Classes.Budget.Instance.InsertCategories(CategoryBox, Main_Classes.Budget.CategoryTypeEnum.PAYMENT);
        }

        private void PaymentName_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (PaymentName.Text.Length == 50)
            {
                PaymentName.Text = PaymentName.Text.Substring(0, 50);
                e.Handled = true;
                PaymentName.ToolTip = "Nazwa nie może byc dłuższa niż 50 znaków.";
            }
            else
            {
                PaymentName.ToolTip = null;
            }
        }

        private void PaymentValue_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && !char.IsPunctuation(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
                PaymentValue.ToolTip = "Podaj kwotę liczbą.";
            }
            else
            {
                PaymentValue.ToolTip = null;
            }
        }
    }
}
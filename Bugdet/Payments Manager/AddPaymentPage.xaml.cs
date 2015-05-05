using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Budget.Main_Classes;
using Budget.Utility_Classes;
using ComboBoxItem = Budget.Utility_Classes.ComboBoxItem;

namespace Budget.Payments_Manager
{
    /// <summary>
    /// Interaction logic for addPaymentPage.xaml
    /// </summary>
    public partial class AddPaymentPage : Page
    {
        private PeriodDateGrid _periodDateGrid = new PeriodDateGrid(Main_Classes.Budget.CategoryTypeEnum.PAYMENT);
        private SingleDateGrid _singleDateGrid = new SingleDateGrid(Main_Classes.Budget.CategoryTypeEnum.PAYMENT);
        public AddPaymentPage()
        {
            InitializeComponent();
            AddSalaryPage.InsertDateTypes(_periodDateGrid.TypeOfDayComboBox);
            Main_Classes.Budget.Instance.InsertCategories(CategoryBox, Main_Classes.Budget.CategoryTypeEnum.PAYMENT);
            _periodDateGrid.StartDatePicker.Text = DateTime.Now.Date.ToString();
            _singleDateGrid.SingleDatePicker.Text = DateTime.Now.Date.ToString();
          //  DateTypeFrame.Content = _singleDateGrid;
            //System.Console.WriteLine("asd: " + StartDatePicker.Text + " .");
        }

        private void addPaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentName.Text != "" && PaymentValue.Text != "" && CategoryBox.SelectedIndex != -1)
            {
                var categoryItem = (ComboBoxItem)CategoryBox.SelectedValue;
                
                if (PeriodPaymentRadio.IsChecked == true)
                {
                    var temp_id = -1;
                    try
                    { 
                        temp_id = Main_Classes.Budget.Instance.Payments.First().Key - 1; 
                    }
                    catch (Exception)
                    { } //gdy brak elementów w tablicy temp_id = 1
                  //  Budget.Instance.ListOfAdds.Add(new Changes(typeof(PeriodPayment), temp_id));
                    Main_Classes.Budget.Instance.AddPeriodPayment(temp_id,
                        new PeriodPayment(categoryItem.Id, 
                            Convert.ToDouble(PaymentValue.Text.Replace(".",",")), 
                            Note.Text, 
                            true,
                            PaymentName.Text, 
                            _periodDateGrid.NumberOf, 
                            _periodDateGrid.TypeOfDay,
                            _periodDateGrid.StartDate,
                            _periodDateGrid.StartDate,
                            (_periodDateGrid.EndDateChecker == true ? Convert.ToDateTime(_periodDateGrid.EndDate) : DateTime.MaxValue)));
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
                    catch (Exception)
                    { } //gdy brak elementów w tablicy temp_id = 1
                    //Budget.Instance.ListOfAdds.Add(new Changes(typeof(SinglePayment), temp_id));
                    Main_Classes.Budget.Instance.AddSinglePayment(temp_id,
                        new SinglePayment(Note.Text, Convert.ToDouble(PaymentValue.Text.Replace(".",",")), categoryItem.Id, true,
                            PaymentName.Text, _singleDateGrid.SelectedDate));

                    int temp_id_balance = Main_Classes.Budget.Instance.BalanceLog.Last().Key + 1;
                    // uwaga tutaj odejmujemy
                    double currentBalance = Main_Classes.Budget.Instance.BalanceLog.Last().Value.Balance - Convert.ToDouble(PaymentValue.Text.Replace(".",","));
                    Main_Classes.Budget.Instance.AddBalanceLog(temp_id_balance,
                        new BalanceLog(currentBalance, DateTime.Today, temp_id, 0));
                    //Budget.Instance.ListOfAdds.Add(new Changes(typeof(BalanceLog), temp_id_balance));

                }

                InfoBox.Text = "Dodano!";
                InfoBox.Foreground = Brushes.Green;
                PaymentName.Text = "";
                PaymentValue.Text = "";
                CategoryBox.SelectedIndex = -1;
                _periodDateGrid.ClearComponents();
                Note.Text = "";
            }
            else
            {
                InfoBox.Text = "Uzupełnij wymagane pola.";
                InfoBox.Foreground = Brushes.Red;
            }
        }
        private void AddCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddCategoryWindow(CategoryBox,Main_Classes.Budget.CategoryTypeEnum.PAYMENT).ShowDialog();
         //   Main_Classes.Budget.Instance.InsertCategories(CategoryBox, Main_Classes.Budget.CategoryTypeEnum.PAYMENT);
        }

        private void PaymentName_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void PaymentValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void SinglePaymentRadio_OnChecked(object sender, RoutedEventArgs e)
        {
            if (DateTypeFrame == null)
                return;
            DateTypeFrame.Content = _singleDateGrid;
        }

        private void PeriodPaymentRadio_OnChecked(object sender, RoutedEventArgs e)
        {
            if (DateTypeFrame == null)
                return;
            DateTypeFrame.Content = _periodDateGrid;
        }

        private void PaymentName_OnGotFocus(object sender, RoutedEventArgs e)
        {
            InfoBox.Text = "";  
        }
    }
}
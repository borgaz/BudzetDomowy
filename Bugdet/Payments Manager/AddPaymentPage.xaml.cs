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
            Main_Classes.Budget.Instance.InsertCategories(CategoryBox, Main_Classes.Budget.CategoryTypeEnum.PAYMENT);
            AddSalaryPage.InsertDateTypes(_periodDateGrid.TypeOfDayComboBox);
            
            _periodDateGrid.StartDatePicker.Text = DateTime.Now.Date.ToString();
            _periodDateGrid.EndDatePicker.Text = DateTime.Now.Date.ToString();
            _singleDateGrid.SingleDatePicker.Text = DateTime.Now.Date.ToString();
        }

        private void addPaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentName.Text != "" && PaymentValue.Text != "" && CategoryBox.SelectedIndex != -1)
            {
                var categoryItem = (ComboBoxItem)CategoryBox.SelectedValue;
                if (PeriodPaymentRadio.IsChecked == true)
                {
                    int temp_id = -1;
                    try
                    {
                        temp_id = Main_Classes.Budget.Instance.Payments.Keys.Min() - 1;
                        if (temp_id == 0) // w bazie chcemy periodPays indeksowane od -1
                            temp_id = -1;
                    }
                    catch (Exception)
                    { }

                    Main_Classes.Budget.Instance.AddPeriodPayment(temp_id,
                        new PeriodPayment(categoryItem.Id, 
                            Convert.ToDouble(PaymentValue.Text.Replace(".", ",")), 
                            Note.Text, 
                            true,
                            PaymentName.Text, 
                            _periodDateGrid.NumberOf, 
                            _periodDateGrid.TypeOfDay,
                            _periodDateGrid.StartDate,
                            _periodDateGrid.StartDate,
                            _periodDateGrid.EndDate));
                    _periodDateGrid.ClearComponents();
                }
                else
                {
                    int temp_id = 1;
                    try
                    {
                        temp_id = Main_Classes.Budget.Instance.Payments.Keys.Max() + 1;
                        if (temp_id == 0)
                        {
                            temp_id = 1;
                        }  
                    }
                    catch (Exception) { }

                    Main_Classes.Budget.Instance.AddSinglePayment(temp_id,
                        new SinglePayment(Note.Text, 
                            Convert.ToDouble(PaymentValue.Text.Replace(".",",")), 
                            categoryItem.Id, 
                            true, 
                            PaymentName.Text, 
                            _singleDateGrid.SelectedDate));
                    _singleDateGrid.SingleDatePicker.Text = "";
                }

                InfoBox.Text = "Dodano!";
                InfoBox.Foreground = Brushes.Green;
                PaymentName.Text = "";
                PaymentValue.Text = "";
                CategoryBox.SelectedIndex = -1;
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
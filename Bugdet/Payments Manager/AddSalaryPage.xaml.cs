using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Budget.Main_Classes;
using Budget.New_Budget;
using Budget.Utility_Classes;
using ComboBoxItem = Budget.Utility_Classes.ComboBoxItem;

namespace Budget.Payments_Manager
{
    /// <summary>
    /// Interaction logic for AddSalaryPage.xaml
    /// </summary>
    public partial class AddSalaryPage : Page
    {
        private PeriodDateGrid _periodDateGrid = new PeriodDateGrid(Main_Classes.Budget.CategoryTypeEnum.SALARY);
        private SingleDateGrid _singleDateGrid = new SingleDateGrid(Main_Classes.Budget.CategoryTypeEnum.SALARY);
        public AddSalaryPage()
        {
            InitializeComponent();
            Main_Classes.Budget.Instance.InsertCategories(CategoryBox, Main_Classes.Budget.CategoryTypeEnum.SALARY);
            InsertDateTypes(_periodDateGrid.TypeOfDayComboBox);

            //_periodDateGrid.StartDatePicker.Text = DateTime.Now.Date.ToString();
            //_periodDateGrid.EndDatePicker.Text = DateTime.MaxValue.ToString();
            //_singleDateGrid.SingleDatePicker.Text = DateTime.Now.Date.ToString();
        }

        private void addPaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SalaryName.Text != "" && SalaryValue.Text != "" && CategoryBox.SelectedIndex != -1)
            {
                var categoryItem = (ComboBoxItem)CategoryBox.SelectedValue;
                if (PeriodPaymentRadio.IsChecked == true)
                {
                    var temp_id = -1;
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
                            Convert.ToDouble(SalaryValue.Text.Replace(".", ",")),
                            Note.Text,
                            false,
                            SalaryName.Text,
                            _periodDateGrid.NumberOf,
                            _periodDateGrid.TypeOfDay,
                            _periodDateGrid.StartDate,
                            _periodDateGrid.StartDate,
                            _periodDateGrid.EndDate));
                    _periodDateGrid.ClearComponents();
                }
                else
                {
                    var temp_id = 1;
                    try
                    {
                        temp_id = Main_Classes.Budget.Instance.Payments.Keys.Max() + 1;
                        if (temp_id == 0) // w bazie chcemy singlePays indeksowane od 1
                            temp_id = 1;

                    }
                    catch (Exception)
                    { } //gdy brak elementów w tablicy temp_id = 1
                    

                    //Budget.Instance.ListOfAdds.Add(new Changes(typeof(SinglePayment), temp_id));
                    Main_Classes.Budget.Instance.AddSinglePayment(temp_id,
                        new SinglePayment(Note.Text, Convert.ToDouble(SalaryValue.Text.Replace(".", ",")), categoryItem.Id, false, SalaryName.Text, _singleDateGrid.SelectedDate));
                    // uwaga tutaj dodajemy
                    if (_singleDateGrid.SelectedDate <= DateTime.Now)
                    {
                        int balanceMaxKey = Main_Classes.Budget.Instance.BalanceLog.Keys.Max();
                        int tempIdBalance = balanceMaxKey + 1;
                        double currentBalance = Main_Classes.Budget.Instance.BalanceLog[balanceMaxKey].Balance + Convert.ToDouble(SalaryValue.Text.Replace(".", ","));
                        Main_Classes.Budget.Instance.AddBalanceLog(tempIdBalance, new BalanceLog(currentBalance, DateTime.Today, temp_id, 0));
                        // Budget.Instance.ListOfAdds.Add(new Changes(typeof(BalanceLog), temp_id_balance));
                    }
                    _singleDateGrid.SingleDatePicker.Text = "";
                }

                InfoBox.Text = "Dodano!";
                InfoBox.Foreground = Brushes.Green;
                SalaryName.Text = "";
                SalaryValue.Text = "";
                CategoryBox.SelectedIndex = -1;
                Note.Text = "";
               
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
        private void AddCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddCategoryWindow(CategoryBox,Main_Classes.Budget.CategoryTypeEnum.SALARY).ShowDialog();
         //   Main_Classes.Budget.Instance.InsertCategories(CategoryBox, Main_Classes.Budget.CategoryTypeEnum.SALARY);
        }

        private void SalaryName_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (SalaryName.Text.Length == 50)
            {
                SalaryName.Text = SalaryName.Text.Substring(0, 50);
                e.Handled = true;
                SalaryName.ToolTip = "Nazwa nie może byc dłuższa niż 50 znaków.";
            }
            else
            {
                SalaryName.ToolTip = null;
            }
        }

        private void SalaryValue_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && !char.IsPunctuation(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
                SalaryValue.ToolTip = "Podaj kwotę liczbą.";
            }
            else
            {
                SalaryValue.ToolTip = null;
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

        private void SalaryName_OnGotFocus(object sender, RoutedEventArgs e)
        {
            InfoBox.Text = ""; 
        }
       
    }
}
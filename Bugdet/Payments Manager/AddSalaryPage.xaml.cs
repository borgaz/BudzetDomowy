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
    /// Interaction logic for AddSalaryPage.xaml
    /// </summary>
    public partial class AddSalaryPage : Page
    {
        private PeriodDateGrid _periodDateGrid = new PeriodDateGrid(Main_Classes.Budget.CategoryTypeEnum.SALARY);
        private SingleDateGrid _singleDateGrid = new SingleDateGrid(Main_Classes.Budget.CategoryTypeEnum.SALARY);
        private MainPage mP;

        public AddSalaryPage(MainPage mP)
        {
            InitializeComponent();
            this.mP = mP;
            Main_Classes.Budget.Instance.InsertCategories(CategoryBox, Main_Classes.Budget.CategoryTypeEnum.SALARY);
            InsertDateTypes(_periodDateGrid.TypeOfDayComboBox);
            _periodDateGrid.StartDatePicker.Text = DateTime.Now.Date.ToString();
            _periodDateGrid.EndDatePicker.Text = DateTime.Now.Date.ToString();
            _singleDateGrid.SingleDatePicker.Text = DateTime.Now.Date.ToString();
        }

        private void addPaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!Utility_Classes.UtilityFunctions.IsValueCorrect(SalaryValue.Text))
            {
                InfoLabel.Content = "Nie Dodano";
                InfoLabel.Foreground = Brushes.Red;// ="#FF000000";
                return;
            }

            if (SalaryName.Text != "" && SalaryValue.Text != "" && CategoryBox.SelectedIndex != -1)
            {
                var categoryItem = (ComboBoxItem)CategoryBox.SelectedValue;
                if (PeriodPaymentRadio.IsChecked == true)
                {
                    int temp_id = -1;
                    try
                    {
                        temp_id = Main_Classes.Budget.Instance.Payments.Keys.Min() - 1;
                        if (temp_id == 0)
                        {
                            temp_id = -1;
                        }     
                    }
                    catch (Exception) { }

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
                            Convert.ToDouble(SalaryValue.Text.Replace(".", ",")), 
                            categoryItem.Id, 
                            false, 
                            SalaryName.Text, 
                            _singleDateGrid.SelectedDate));
                    _singleDateGrid.SingleDatePicker.Text = "";
                }

                InfoLabel.Content = "Dodano!";
                InfoLabel.Foreground = Brushes.Green;
                InfoLabel.FontSize = 16;
                SalaryName.Text = "";
                SalaryValue.Text = "";
                CategoryBox.SelectedIndex = -1;
                Note.Text = ""; 
            }
            else
            {
                InfoLabel.FontSize = 14;
                InfoLabel.Content = "Uzupełnij wymagane pola.";
                InfoLabel.Foreground = Brushes.Red;
            }
            mP.LoadLastAddedDataGrid();
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
        }

        private void SalaryName_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void SalaryValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
            InfoLabel.Content = ""; 
        }   
    }
}
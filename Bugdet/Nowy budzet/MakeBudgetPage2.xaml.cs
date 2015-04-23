using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Budget.Nowy_budzet
{
    /// <summary>
    /// Interaction logic for MakeBudgetPage2.xaml
    /// </summary>
    public partial class MakeBudgetPage2 : Page
    {
        private Dictionary<int, PeriodPayment> _periodPayments;
        private Dictionary<int, Category> _categories;

        public MakeBudgetPage2(Dictionary<int, PeriodPayment> payments, Dictionary<int, Category> categories)
        {
            InitializeComponent();
            _periodPayments = payments;
            _categories = categories;
            InsertDateTypes();
            InsertCategories();
        }

        private void InsertDateTypes()
        {
            DateTypeBox.Items.Add("DZIEŃ");
            DateTypeBox.Items.Add("TYDZIEŃ");
            DateTypeBox.Items.Add("MIESIĄC");
            DateTypeBox.Items.Add("ROK");
        }

        public Boolean CheckInfo()
        {
            try
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void addSalaryBtn_Click(object sender, RoutedEventArgs e)
        {
            if(SalaryName.Text != "" && SalaryValue.Text != "" && NumberOfTextBox.Text != "" && DateTypeBox.SelectedIndex != -1 && CategoryComboBox.SelectedIndex != -1)
            {
                int temp = 1;
                try
                { 
                    temp = _periodPayments.Last().Key + 1; 
                }
                catch { }
                _periodPayments.Add(temp, new PeriodPayment(CategoryComboBox.SelectedIndex + 1,
                    Convert.ToDouble(SalaryValue.Text),
                    NoteTextBox.Text,
                    false,
                    SalaryName.Text,
                    Convert.ToInt32(NumberOfTextBox.Text),
                    DateTypeBox.SelectedValue.ToString(),
                    Convert.ToDateTime(StartDatePicker.Text),
                    Convert.ToDateTime(StartDatePicker.Text),
                    (EndDatePicker.IsEnabled == true ? Convert.ToDateTime(EndDatePicker.Text) : DateTime.MaxValue)));
                InfoLbl.Content = "Dodano";
                InfoLbl.Foreground = Brushes.Green;// ="#00FF0000";
                SalaryName.Text = "";
                SalaryValue.Text = "";
                NumberOfTextBox.Text = "";
            }
            else
            {
                MessageBox.Show("Uzupełnij wszystkie pola!");
                InfoLbl.Content = "Nie Dodano";
                InfoLbl.Foreground = Brushes.Red;// ="#FF000000";
            }
        }

        private void addedSalariesBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddedSalariesWindow(1, _periodPayments, _categories).ShowDialog();
        }

        /// <summary>
        /// Inserts categories in CategoryComboBox
        /// </summary>
        public void InsertCategories()
        {
            CategoryComboBox.Items.Clear();
            try
            {
                for (int i = 0; i < _categories.Count; i++)
                {
                    if (_categories[i + 1].Type == true)
                    {
                        CategoryComboBox.Items.Add(_categories[i + 1].Name);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(_categories.Count + " ");
                Console.WriteLine(_categories.ToArray());
            }
        }

        public Boolean BackToThisPage() // do usuniecia po refaktoryzacji
        {
            return true;
        }

        private void AddCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddCategoryWindow(_categories).ShowDialog();
            InsertCategories();
        }

        private void EndDateCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            EndDatePicker.IsEnabled = true;
        }

        private void EndDateCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            EndDatePicker.IsEnabled = false;
        }

        private void SalaryName_OnGotFocus(object sender, RoutedEventArgs e)
        {
            InfoLbl.Content = "";
        }

        private void SalaryValue_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
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
    }
}

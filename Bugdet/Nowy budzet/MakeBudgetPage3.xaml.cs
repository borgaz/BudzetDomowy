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
    /// Interaction logic for MakeBudgetPage3.xaml
    /// </summary>
    public partial class MakeBudgetPage3 : Page
    {
        private Dictionary<int, PeriodPayment> _periodPayments;
        private Dictionary<int, Category> _categories;
      //  private int _salaries;
        public MakeBudgetPage3(Dictionary<int,PeriodPayment> d,Dictionary<int,Category> c)
        {
            InitializeComponent();
            _periodPayments = d;
            _categories = c;
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
        private void addSalaryBtn_Click(object sender, RoutedEventArgs e)
        {
            if(SalaryName.Text != "" && SalaryValue.Text != "" && NumberOfTextBox.Text != "" && DateTypeBox.SelectedIndex != -1 && CategoryComboBox.SelectedIndex != -1 )
            {
                int temp = 1;
                try {temp = _periodPayments.Last().Key + 1;} catch { }
                _periodPayments.Add(temp,new PeriodPayment(CategoryComboBox.SelectedIndex + 1,
                    Double.Parse(SalaryValue.Text),
                    NoteTextBox.Text,
                    true,SalaryName.Text,
                    int.Parse(NumberOfTextBox.Text),
                    DateTypeBox.SelectedValue.ToString(),
                    StartDatePicker.DisplayDate,StartDatePicker.DisplayDate,
                    (EndDateCheckBox.IsEnabled == true ? EndDatePicker.DisplayDate : StartDatePicker.DisplayDate)));
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
            new AddedSalariesWindow(2,_periodPayments,_categories).ShowDialog();
        }

        public void InsertCategories()
        {
            CategoryComboBox.Items.Clear();
            try
            {
                for (int i = 0; i < _categories.Count; i++)
                {
                    if (_categories[i + 1].Type == false)
                        CategoryComboBox.Items.Add(_categories[i + 1].Name);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(_categories.Count + "");
                Console.WriteLine(_categories.ToArray());
            }
        }

        public Boolean CheckInfo()
        {
            return true;
        }
        public Boolean BackToThisPage()
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Budget.Main_Classes;
using Budget.Utility_Classes;

namespace Budget.New_Budget
{
    /// <summary>
    /// Interaction logic for MakeBudgetPage2.xaml
    /// </summary>
    public partial class MakeBudgetPage2 : Page
    {
        private Dictionary<int, PeriodPayment> _periodPayments;
        private Dictionary<int, Category> _categories;
        List<int> originalCategoryID;

        public MakeBudgetPage2(Dictionary<int, PeriodPayment> payments, Dictionary<int, Category> categories)
        {
            InitializeComponent();
            _periodPayments = payments;
            _categories = categories;
            MakeBudgetWindow.InsertDateTypes(DateTypeBox);
            originalCategoryID = MakeBudgetWindow.InsertCategories(CategoryComboBox, _categories, true);
            StartDatePicker.Text = DateTime.Now.Date.ToString();
        }

        private void addSalaryBtn_Click(object sender, RoutedEventArgs e)
        {
            if(SalaryName.Text != "" && SalaryValue.Text != "" && NumberOfTextBox.Text != "" && DateTypeBox.SelectedIndex != -1 && CategoryComboBox.SelectedIndex != -1)
            {
                int temp = 1;
                try
                { 
                    temp = _periodPayments.Keys.Max() + 1; 
                }
                catch { }
                _periodPayments.Add(temp, new PeriodPayment(originalCategoryID[CategoryComboBox.SelectedIndex],
                    Convert.ToDouble(SalaryValue.Text.Replace(".", ",")),
                    NoteTextBox.Text,
                    false,
                    SalaryName.Text,
                    Convert.ToInt32(NumberOfTextBox.Text),
                    DateTypeBox.SelectedValue.ToString(),
                    Convert.ToDateTime(StartDatePicker.Text),
                    Convert.ToDateTime(StartDatePicker.Text),
                    (EndDatePicker.IsEnabled ? Convert.ToDateTime(EndDatePicker.Text) : DateTime.MaxValue)));
                InfoLbl.Content = "Dodano";
                InfoLbl.Foreground = Brushes.Green;// ="#00FF0000";
                SalaryName.Text = "";
                SalaryValue.Text = "";
                NoteTextBox.Text = "";
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

        private void AddCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddCategoryWindow(_categories,CategoryComboBox,true,originalCategoryID).ShowDialog();
            originalCategoryID = MakeBudgetWindow.InsertCategories(CategoryComboBox, _categories, true);
            
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

    }
}

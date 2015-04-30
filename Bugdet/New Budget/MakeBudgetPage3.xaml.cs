using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Budget.Main_Classes;

namespace Budget.New_Budget
{
    /// <summary>
    /// Interaction logic for MakeBudgetPage3.xaml
    /// </summary>
    public partial class MakeBudgetPage3 : Page
    {
        private Dictionary<int, PeriodPayment> _periodPayments;
        private Dictionary<int, Category> _categories;
        List<int> originalCategoryID;
      //  private int _salaries;
        public MakeBudgetPage3(Dictionary<int,PeriodPayment> d,Dictionary<int,Category> c)
        {
            InitializeComponent();
            _periodPayments = d;
            _categories = c;
            MakeBudgetWindow.InsertDateTypes(DateTypeBox);
            originalCategoryID = MakeBudgetWindow.InsertCategories(CategoryComboBox, _categories, false);
            StartDatePicker.Text = DateTime.Now.Date.ToString();
        }

        private void addPaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentName.Text != "" && PaymentValue.Text != "" && NumberOfTextBox.Text != "" && DateTypeBox.SelectedIndex != -1 && CategoryComboBox.SelectedIndex != -1)
            {
                int temp = 1;
                try
                {
                    temp = _periodPayments.Last().Key + 1;
                }
                catch { }
                //System.Console.WriteLine(StartDatePicker.Text);
                //System.Console.WriteLine(StartDatePicker.DisplayDate);
                _periodPayments.Add(temp, new PeriodPayment(originalCategoryID[CategoryComboBox.SelectedIndex],
                    Convert.ToDouble(PaymentValue.Text),
                    NoteTextBox.Text,
                    true,
                    PaymentName.Text,
                    Convert.ToInt32(NumberOfTextBox.Text),
                    DateTypeBox.SelectedValue.ToString(),
                    Convert.ToDateTime(StartDatePicker.Text),
                    Convert.ToDateTime(StartDatePicker.Text),
                    (EndDatePicker.IsEnabled == true ? Convert.ToDateTime(EndDatePicker.Text) : DateTime.MaxValue)));
                InfoLbl.Content = "Dodano";
                InfoLbl.Foreground = Brushes.Green;// ="#00FF0000";
                PaymentName.Text = "";
                PaymentValue.Text = "";
                NumberOfTextBox.Text = "";
            }
            else
            {
                MessageBox.Show("Uzupełnij wszystkie pola!");
                InfoLbl.Content = "Nie Dodano";
                InfoLbl.Foreground = Brushes.Red;// ="#FF000000";
            }
        }

        public Boolean BackToThisPage()
        {
            return true;
        }

        private void AddCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddCategoryWindow(_categories).ShowDialog();
            originalCategoryID = MakeBudgetWindow.InsertCategories(CategoryComboBox, _categories, false);
        }

        private void EndDateCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            EndDatePicker.IsEnabled = true;
        }

        private void EndDateCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            EndDatePicker.IsEnabled = false;
        }

        private void PaymentName_OnGotFocus(object sender, RoutedEventArgs e)
        {
            InfoLbl.Content = "";
        }

        private void PaymentValue_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void addedPaymentsBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddedSalariesWindow(2, _periodPayments, _categories).ShowDialog();
        }

        
    }
}

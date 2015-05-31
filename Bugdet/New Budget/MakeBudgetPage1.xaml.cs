using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Budget.Main_Classes;

namespace Budget.New_Budget
{
    /// <summary>
    /// Interaction logic for MakeBudzetPage1.xaml
    /// </summary>
    public partial class MakeBudgetPage1 : Page
    {
        private SalaryInfo salaryInfo;
        private LinearGradientBrush red = new LinearGradientBrush();
        private LinearGradientBrush white = new LinearGradientBrush();
        private LinearGradientBrush green = new LinearGradientBrush();

        public MakeBudgetPage1(SalaryInfo s)
        {
            InitializeComponent();
            InsertColors();
            salaryInfo = s;
            passTextBox.Foreground = red;

        }

        private void InsertColors()
        {
            red.GradientStops.Insert(0, new GradientStop(Color.FromArgb(255, 236, 23, 23), 0));
            red.GradientStops.Insert(1, new GradientStop(Color.FromArgb(255, 255, 255, 255), 1));
            red.StartPoint = new Point(0.5,0);
            red.EndPoint = new Point(0.5, 1);
            white.GradientStops.Insert(0, new GradientStop(Color.FromArgb(255, 255, 255, 255), 0));
            green.GradientStops.Insert(0, new GradientStop(Color.FromArgb(255, 6, 119, 58), 0));
            green.GradientStops.Insert(1, new GradientStop(Color.FromArgb(255, 255, 255, 255), 1));
            green.StartPoint = new Point(0.5, 0);
            green.EndPoint = new Point(0.5, 1);
        }

        private void CheckWindow() // Proba kolorowania pustych okienek 
        {
            try
            {
                foreach (TextBox box in Page1Grid.Children)
                {
                    if (box.Text == "")
                    {
                        box.Background = Brushes.Red;
                    }
                    else
                    {
                        box.Background = white;
                    }   
                }
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// - sprawdza czy wypelnione pola oraz wrzuca na stos informacje
        /// </summary>
        public Boolean CheckInfo()
        {
            if (budgetBalance.Text != "" && budgetNameText.Text != "" && (passTextBox.Password.Equals(passRepeatTextBox.Password)) && passTextBox.Password != "")
            {
                if (!SqlConnect.Instance.CheckBaseName(budgetNameText.Text))
                    return false;
                salaryInfo.Name = budgetNameText.Text;
                salaryInfo.Amount = double.Parse(budgetBalance.Text.Replace(".", ","));
                salaryInfo.Password = SqlConnect.Instance.HashPasswordMd5((passTextBox.Password));
                return true;
            }
            else
            {
                MessageBox.Show("Uzupełnij puste pola.");
                return false;
            }
        }

        private void BudgetNameText_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            budgetNameText.Background = budgetNameText.Text == "" ? red : green;
        }

        private void BudgetBalance_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            budgetBalance.Background = budgetBalance.Text == "" ? red : green;
        }

        private void PassRepeatTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (passTextBox.Password == "")
                return;
            passRepeatTextBox.Background = passTextBox.Password.Equals(passRepeatTextBox.Password) ? green : red;
        }

        private void NumberOfPeopleTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && !char.IsPunctuation(e.Text, e.Text.Length - 1) && char.IsPunctuation(e.Text, e.Text.Length - 2))
            {
                e.Handled = true;
                budgetBalance.ToolTip = "Wpisz kwotę liczbą!";
            }
            else
            {
                budgetBalance.ToolTip = null;
            }
        }

        private void BudgetNameText_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
                budgetNameText.ToolTip = "Nazwa moze sie skladac jedynie z liter i liczb";
            }
            else
            {
                budgetNameText.ToolTip = null;
            }
        }
    }
}
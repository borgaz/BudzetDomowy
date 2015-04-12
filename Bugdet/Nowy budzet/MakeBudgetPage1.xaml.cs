using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Bugdet.Nowy_budzet
{
    /// <summary>
    /// Interaction logic for MakeBudzetPage1.xaml
    /// </summary>
    public partial class MakeBudgetPage1 : Page
    {
        public MakeBudgetPage1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// - sprawdza czy wypelnione pola oraz wrzuca na stos informacje
        /// </summary>
        public Boolean CheckInfo()
        {
            if(budgetBalance.Text != "" && budgetNameText.Text != "" && (passTextBox.Password.Equals(passRepeatTextBox.Password)) && passTextBox.Password != "")
            {
                MakeBudgetWindow.Budgetstack.Push(budgetNameText.Text);
                MakeBudgetWindow.Budgetstack.Push(budgetBalance.Text);
                MakeBudgetWindow.Budgetstack.Push(SqlConnect.Instance.HashPasswordMd5(passTextBox.Password));
                return true;
            }
            else
            {
                MessageBox.Show("uzupelnij wszystkie pola!");
                return false;
            }
        }
        public Boolean BackToThisPage()
        {
            try
            {
                MakeBudgetWindow.Budgetstack.Pop();
                MakeBudgetWindow.Budgetstack.Pop();
                MakeBudgetWindow.Budgetstack.Pop();
                return true;
            }
            catch(InsufficientExecutionStackException)
            {
                return false;
            }
        }

        private void BudgetNameText_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if(budgetNameText.Text == "")
            {
                NameBoxGradientBrush.GradientStops.Clear();
                NameBoxGradientBrush.GradientStops.Insert(0, new GradientStop(Color.FromArgb(255, 236, 23, 23), 0));
                NameBoxGradientBrush.GradientStops.Insert(1, new GradientStop(Color.FromArgb(255, 255, 255, 255), 1));
            }
            else
            {
                NameBoxGradientBrush.GradientStops.Clear();
                NameBoxGradientBrush.GradientStops.Insert(0, new GradientStop(Color.FromArgb(255, 6, 119, 58), 0));
                NameBoxGradientBrush.GradientStops.Insert(1, new GradientStop(Color.FromArgb(255, 255, 255, 255), 1));                
            }
        }

        private void BudgetBalance_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (budgetBalance.Text == "")
            {
                BalanceBoxGradientBrush.GradientStops.Clear();
                BalanceBoxGradientBrush.GradientStops.Insert(0, new GradientStop(Color.FromArgb(255, 236, 23, 23), 0));
                BalanceBoxGradientBrush.GradientStops.Insert(1, new GradientStop(Color.FromArgb(255, 255, 255, 255), 1));
            }
            else
            {
                BalanceBoxGradientBrush.GradientStops.Clear();
                BalanceBoxGradientBrush.GradientStops.Insert(0, new GradientStop(Color.FromArgb(255, 6, 119, 58), 0));
                BalanceBoxGradientBrush.GradientStops.Insert(1, new GradientStop(Color.FromArgb(255, 255, 255, 255), 1));
            }
        }

        private void PassRepeatTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (passTextBox.Password.Equals(passRepeatTextBox.Password))
            {
                PassRepeatGradientBrush.GradientStops.Clear();
                PassRepeatGradientBrush.GradientStops.Insert(0, new GradientStop(Color.FromArgb(255, 6, 119, 58), 0));
                PassRepeatGradientBrush.GradientStops.Insert(1, new GradientStop(Color.FromArgb(255, 255, 255, 255), 1));
            }
            else
            {
                PassRepeatGradientBrush.GradientStops.Clear();
                PassRepeatGradientBrush.GradientStops.Insert(0, new GradientStop(Color.FromArgb(255, 236, 23, 23), 0));
                PassRepeatGradientBrush.GradientStops.Insert(1, new GradientStop(Color.FromArgb(255, 255, 255, 255), 1)); 
            }
        }
    }
}

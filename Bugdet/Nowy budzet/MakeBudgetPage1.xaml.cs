using System;
using System.Windows;
using System.Windows.Controls;

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
            if(budgetBalance.Text != "" && budgetNameText.Text != "")
            {
                MakeBudgetWindow._budgetstack.Push(budgetNameText.Text);
                MakeBudgetWindow._budgetstack.Push(budgetBalance.Text);
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
                MakeBudgetWindow._budgetstack.Pop();
                MakeBudgetWindow._budgetstack.Pop();
                return true;
            }
            catch(InsufficientExecutionStackException)
            {
                return false;
            }
        }
    }
}

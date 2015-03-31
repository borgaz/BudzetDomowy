using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bugdet
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

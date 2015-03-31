using Bugdet.Nowy_budzet;
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
    /// Interaction logic for MakeBudgetPage2.xaml
    /// </summary>
    public partial class MakeBudgetPage2 : Page
    {
        public static List<Salaryinfo> budgetList = new List<Salaryinfo>();
        private int salaries;
        public MakeBudgetPage2()
        {
            InitializeComponent();
        }

        private void type1Radio_Checked(object sender, RoutedEventArgs e)
        {
            if(descLbl != null)
                descLbl.Content = "Co ile dni jest wypłata";
        }

        private void type2Radio_Checked(object sender, RoutedEventArgs e)
        {
            if (descLbl != null)
                descLbl.Content = "W jaki dzień miesiąca jest wypłata";
        }

        private void addSalaryBtn_Click(object sender, RoutedEventArgs e)
        {
            if(salaryName.Text != "" && salaryValue.Text != "" && dateText.Text != "")
            {
                budgetList.Add(new Salaryinfo(salaryName.Text,
                                              int.Parse(salaryValue.Text),
                                              (type1Radio.IsChecked == true ? 1 : 2),
                                              int.Parse(dateText.Text)));
                salaries++;
                MessageBox.Show("Dodano");
                salaryName.Text = "";
                salaryValue.Text = "";
                dateText.Text = "";
            }
            else
            {
                MessageBox.Show("Uzupełnij wszystkie pola!");
            }
        }

        private void addedSalariesBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddedSalariesWindow(1).ShowDialog();
        }

        public Boolean CheckInfo()
        {
            try
            {
                for (int i = 0; i < salaries; i++)
                {
                    MakeBudgetWindow._budgetstack.Push(budgetList.ElementAt(i).Name); // String
                    MakeBudgetWindow._budgetstack.Push(budgetList.ElementAt(i).Value); // String
                    MakeBudgetWindow._budgetstack.Push(budgetList.ElementAt(i).Repeat); // String
                    MakeBudgetWindow._budgetstack.Push(budgetList.ElementAt(i).Type); // int
                }
                if (salaries != 0)
                    MakeBudgetWindow._budgetstack.Push(-1); // identyfikator periodSalary
                MakeBudgetWindow._budgetstack.Push(salaries);
                return true;
            }
            catch(InsufficientExecutionStackException)
            {
                return false;
            }
        }
        public Boolean BackToThisPage()
        {
            try
            {
                int _salaries = (int)MakeBudgetWindow._budgetstack.Pop(); // ile pakietow
                if (_salaries != 0)
                    MakeBudgetWindow._budgetstack.Pop(); // identyfikator periodsalary
                for (int i = 0; i < _salaries; i++)
                {
                    MakeBudgetWindow._budgetstack.Pop();
                    MakeBudgetWindow._budgetstack.Pop();
                    MakeBudgetWindow._budgetstack.Pop();
                    MakeBudgetWindow._budgetstack.Pop();
                }
                return true;
            }
            catch(InsufficientExecutionStackException)
            {
                return false;
            }
        }
    }
}

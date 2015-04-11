using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Bugdet.Nowy_budzet
{
    /// <summary>
    /// Interaction logic for MakeBudgetPage2.xaml
    /// </summary>
    public partial class MakeBudgetPage2 : Page
    {
        public static List<SalaryInfo> BudgetList = new List<SalaryInfo>();
        private int _salaries;
        public MakeBudgetPage2()
        {
            InitializeComponent();
        }

        private void type1Radio_Checked(object sender, RoutedEventArgs e)
        {
            if(DescLbl != null)
                DescLbl.Content = "Co ile dni jest wypłata";
        }

        private void type2Radio_Checked(object sender, RoutedEventArgs e)
        {
            if (DescLbl != null)
                DescLbl.Content = "W jaki dzień miesiąca jest wypłata";
        }

        private void addSalaryBtn_Click(object sender, RoutedEventArgs e)
        {
            if(SalaryName.Text != "" && SalaryValue.Text != "" && DateText.Text != "")
            {
                BudgetList.Add(new SalaryInfo(SalaryName.Text,
                                              int.Parse(SalaryValue.Text),
                                              (Type1Radio.IsChecked == true ? 1 : 2),
                                              int.Parse(DateText.Text)));
                _salaries++;
                InfoLbl.Content = "Dodano";
                InfoLbl.Foreground = Brushes.Green;// ="#00FF0000";
                SalaryName.Text = "";
                SalaryValue.Text = "";
                DateText.Text = "";
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
            new AddedSalariesWindow(1).ShowDialog();
        }

        public Boolean CheckInfo()
        {
            try
            {
                for (int i = 0; i < _salaries; i++)
                {
                    MakeBudgetWindow.Budgetstack.Push(BudgetList.ElementAt(i).Name); // String
                    MakeBudgetWindow.Budgetstack.Push(BudgetList.ElementAt(i).Value); // String
                    MakeBudgetWindow.Budgetstack.Push(BudgetList.ElementAt(i).Repeat); // String
                    MakeBudgetWindow.Budgetstack.Push(BudgetList.ElementAt(i).Type); // int
                }
                if (_salaries != 0)
                    MakeBudgetWindow.Budgetstack.Push(-1); // identyfikator periodSalary
                MakeBudgetWindow.Budgetstack.Push(_salaries);
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
                int _salaries = (int)MakeBudgetWindow.Budgetstack.Pop(); // ile pakietow
                if (_salaries != 0)
                    MakeBudgetWindow.Budgetstack.Pop(); // identyfikator periodsalary
                for (int i = 0; i < _salaries; i++)
                {
                    MakeBudgetWindow.Budgetstack.Pop();
                    MakeBudgetWindow.Budgetstack.Pop();
                    MakeBudgetWindow.Budgetstack.Pop();
                    MakeBudgetWindow.Budgetstack.Pop();
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

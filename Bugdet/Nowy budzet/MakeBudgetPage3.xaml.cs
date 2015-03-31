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
    /// Interaction logic for MakeBudgetPage3.xaml
    /// </summary>
    public partial class MakeBudgetPage3 : Page
    {
        public static List<Salaryinfo> paymentList = new List<Salaryinfo>();
        private int salaries;
        public MakeBudgetPage3()
        {
            InitializeComponent();
        }

        private void type1Radio_Checked(object sender, RoutedEventArgs e)
        {
            if(descLbl != null)
                descLbl.Content = "Co ile dni jest opłata";
        }

        private void type2Radio_Checked(object sender, RoutedEventArgs e)
        {
            if (descLbl != null)
                descLbl.Content = "W jaki dzień miesiąca jest opłacane";
        }

        private void addSalaryBtn_Click(object sender, RoutedEventArgs e)
        {
            if(salaryName.Text != "" && salaryValue.Text != "" && dateText.Text != "")
            {
                paymentList.Add(new Salaryinfo(salaryName.Text,
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
            new AddedSalariesWindow(2).ShowDialog();
        }
        public Boolean CheckInfo()
        {
            try
            {
                for (int i = 0; i < salaries; i++)
                {
                    MakeBudgetWindow._budgetstack.Push(paymentList.ElementAt(i).Name); // String
                    MakeBudgetWindow._budgetstack.Push(paymentList.ElementAt(i).Value); // String
                    MakeBudgetWindow._budgetstack.Push(paymentList.ElementAt(i).Repeat); // String
                    MakeBudgetWindow._budgetstack.Push(paymentList.ElementAt(i).Type); // int
                }
                if (salaries != 0)
                    MakeBudgetWindow._budgetstack.Push(-2); // identyfikator ze sa jakies periodPayments
                MakeBudgetWindow._budgetstack.Push(salaries); // ilosc "pakietow"
                return true;
            }
            catch (InsufficientExecutionStackException)
            {
                return false;
            }
        }
        public Boolean BackToThisPage()
        {
            try
            {
                int _salaries = (int)MakeBudgetWindow._budgetstack.Pop(); // ile "pakietow" danych bylo na stosie
                if(_salaries != 0)
                    MakeBudgetWindow._budgetstack.Pop(); // identyfikator periodPayments
                for (int i = 0; i < _salaries; i++) // zbieranie ze stosu wrzuconych informacji
                {
                    MakeBudgetWindow._budgetstack.Pop();
                    MakeBudgetWindow._budgetstack.Pop();
                    MakeBudgetWindow._budgetstack.Pop();
                    MakeBudgetWindow._budgetstack.Pop(); 
                }
                return true;
            }
            catch (InsufficientExecutionStackException)
            {
                return false;
            }
        }
    }
}

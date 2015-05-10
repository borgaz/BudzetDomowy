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
using System.Windows.Shapes;
using Budget.Main_Classes;
using Budget.Utility_Classes;
using Budget.New_Budget;

namespace Budget.Payments_Manager
{
    /// <summary>
    /// Interaction logic for CheckingPeriodPayments.xaml
    /// </summary>
    public partial class CheckingPeriodPayments : Window
    {
        int listIndex = 0;
        List<SinglePayment> singlePaymentsList = new List<SinglePayment>();

        public CheckingPeriodPayments(List<SinglePayment> _singlePaymentsList)
        {
            InitializeComponent();
            singlePaymentsList = _singlePaymentsList;
            listIndex = _singlePaymentsList.Count-1;
            PaymentText.Text = singlePaymentsList[listIndex].ToString();
        }

        private void PostponeButton_Click(object sender, RoutedEventArgs e)
        {
            Main_Classes.Budget.Instance.AddSinglePayment(Main_Classes.Budget.Instance.Payments.Keys.Max() + 1, 
                                                            singlePaymentsList[listIndex]);
            if (listIndex >= 0)
            {
                this.Close();
            }
            else
            {
                listIndex--;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (listIndex >= 0)
            {
                this.Close();
            }
            else
            {
                listIndex--;
            }
        }
    }
}

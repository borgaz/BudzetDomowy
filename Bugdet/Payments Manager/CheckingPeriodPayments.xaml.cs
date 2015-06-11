using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Budget.Main_Classes;

namespace Budget.Payments_Manager
{
    public partial class CheckingPeriodPayments : Window
    {
        int listIndex = 0;
        List<SinglePayment> singlePaymentsList = new List<SinglePayment>();

        public CheckingPeriodPayments(List<SinglePayment> _singlePaymentsList)
        {
            InitializeComponent();
            singlePaymentsList = _singlePaymentsList;
            listIndex = _singlePaymentsList.Count - 1;

            String tempName = singlePaymentsList[listIndex].Name;
            if (singlePaymentsList[listIndex].Name.StartsWith("Okresowy:"))
            {
                tempName = singlePaymentsList[listIndex].Name.Substring(10);
            }

            PaymentName.Content = tempName; 
            DateText.Content = singlePaymentsList[listIndex].Date.ToShortDateString();
            Amount.Text = singlePaymentsList[listIndex].Amount.ToString();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int indexOfNewSingle = Main_Classes.Budget.Instance.Payments.Count > 0 && Main_Classes.Budget.Instance.Payments.Keys.Max() > 0 ? Main_Classes.Budget.Instance.Payments.Keys.Max() + 1 : 1;
            singlePaymentsList[listIndex].Amount = Convert.ToDouble(Amount.Text);
            Main_Classes.Budget.Instance.AddSinglePayment(indexOfNewSingle, singlePaymentsList[listIndex]);
            if (listIndex > 0)
            {
                listIndex--;
                PaymentName.Content = singlePaymentsList[listIndex].Name;
                DateText.Content = singlePaymentsList[listIndex].Date.ToShortDateString();
                Amount.Text = singlePaymentsList[listIndex].Amount.ToString();
            }
            else
            {
                this.Close();
            }      
        }

        private void PreviewTextInput_Amount(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && !char.IsPunctuation(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
                Amount.ToolTip = "Podaj kwotę liczbą.";
            }
            else
            {
                Amount.ToolTip = null;
            }
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

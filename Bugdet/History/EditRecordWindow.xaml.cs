using System;
using System.Windows;
using System.Windows.Forms;
using Budget.Main_Classes;
using MessageBox = System.Windows.MessageBox;

namespace Budget.History
{
    /// <summary>
    /// Interaction logic for EditRecordWindow.xaml
    /// </summary>
    public partial class EditRecordWindow : Window
    {
        private int _paymentId;
        public EditRecordWindow()
        {
            InitializeComponent();
        }

        public EditRecordWindow(int paymentId)
        {
            InitializeComponent();
            _paymentId = paymentId;
            InsertInformation(paymentId);
        }

        private void InsertInformation(int id)
        {
            var payment = Main_Classes.Budget.Instance.Payments[id];
            NameTextBox.Text = payment.Name;
            AmountTextBox.Text = payment.Amount.ToString();
            DescTextBox.Text = payment.Note;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NameTextBox.Text != "" && AmountTextBox.Text != "")
                {
                    Main_Classes.Budget.Instance.Payments[_paymentId].Name = NameTextBox.Text;
                    Main_Classes.Budget.Instance.Payments[_paymentId].Note = DescTextBox.Text;
                    Main_Classes.Budget.Instance.Payments[_paymentId].Amount = Convert.ToDouble(AmountTextBox.Text);
                    Main_Classes.Budget.Instance.EditSinglePayment(_paymentId, (SinglePayment)Main_Classes.Budget.Instance.Payments[_paymentId]);
                    MessageBox.Show("Zapisano zmiany!");
                    Close();
                }
                else
                {
                    MessageBox.Show("Pola nie mogą zostać puste");
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Wpisz poprawnie dane");
                
            }

        }

    }
}

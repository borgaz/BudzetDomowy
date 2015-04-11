using System.Data;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Bugdet.Nowy_budzet
{
    /// <summary>
    /// Interaction logic for AddedPaymentPage.xaml
    /// </summary>
    public partial class AddedPaymentPage : Page
    {
        public AddedPaymentPage()
        {
            InitializeComponent();
            MessageBox.Show(MakeBudgetPage3.PaymentList.Count + "");
            Refresh();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if(PaymentTable.SelectedItem != null)
            {
                DataRowView dataRow = (DataRowView)PaymentTable.SelectedItem;
                MakeBudgetPage3.PaymentList.RemoveAt((int)dataRow.Row.ItemArray[3]);
                PaymentTable.Items.Remove(dataRow);
            }
        }
        private void Refresh()
        {

            DataTable payment = new DataTable();
            payment.Columns.Add("id", typeof(int));
            payment.Columns.Add("Nazwa", typeof(string));
            payment.Columns.Add("Wynagrodzenie", typeof(int));
            payment.Columns.Add("Powtarzalność", typeof(string));
            for (int i = 0; i < MakeBudgetPage3.PaymentList.Count; i++)
            {
                SalaryInfo item = (SalaryInfo)MakeBudgetPage3.PaymentList.ToArray().GetValue(i);

                payment.Rows.Add(i, item.Name, item.Value, (item.Type == 1 ? "co " + item.Repeat + " dni" : "w każdy " + item.Repeat + " dzień miesiąca"));
            }
            PaymentTable.ItemsSource = payment.DefaultView;
          //  salaryTable.Columns[0].Visibility = Visibility.Hidden;
            new Thread(new ThreadStart(HideColumns)).Start();
        }
        private void HideColumns()
        {
            while (PaymentTable.Columns.Count == 0)
            { }
            this.Dispatcher.Invoke(HideColums2);
        }
        private void HideColums2()
        {
            PaymentTable.Columns[0].Visibility = Visibility.Hidden;

        }
    }
}

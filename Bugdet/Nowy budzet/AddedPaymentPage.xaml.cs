using Bugdet.Nowy_budzet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for AddedPaymentPage.xaml
    /// </summary>
    public partial class AddedPaymentPage : Page
    {
        public AddedPaymentPage()
        {
            InitializeComponent();
            MessageBox.Show(MakeBudgetPage3.paymentList.Count + "");
            Refresh();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if(paymentTable.SelectedItem != null)
            {
                DataRowView dataRow = (DataRowView)paymentTable.SelectedItem;
                MakeBudgetPage3.paymentList.RemoveAt((int)dataRow.Row.ItemArray[3]);
                paymentTable.Items.Remove(dataRow);
            }
        }
        private void Refresh()
        {

            DataTable payment = new DataTable();
            payment.Columns.Add("id", typeof(int));
            payment.Columns.Add("Nazwa", typeof(string));
            payment.Columns.Add("Wynagrodzenie", typeof(int));
            payment.Columns.Add("Powtarzalność", typeof(string));
            for (int i = 0; i < MakeBudgetPage3.paymentList.Count; i++)
            {
                Salaryinfo item = (Salaryinfo)MakeBudgetPage3.paymentList.ToArray().GetValue(i);

                payment.Rows.Add(i, item.Name, item.Value, (item.Type == 1 ? "co " + item.Repeat + " dni" : "w każdy " + item.Repeat + " dzień miesiąca"));
            }
            paymentTable.ItemsSource = payment.DefaultView;
          //  salaryTable.Columns[0].Visibility = Visibility.Hidden;
            new Thread(new ThreadStart(HideColumns)).Start();
        }
        private void HideColumns()
        {
            while (paymentTable.Columns.Count == 0)
            { }
            this.Dispatcher.Invoke(HideColums2);
        }
        private void HideColums2()
        {
            paymentTable.Columns[0].Visibility = Visibility.Hidden;

        }
    }
}

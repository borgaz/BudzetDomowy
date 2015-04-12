using System;
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
        //    MessageBox.Show(MakeBudgetPage3.PaymentList.Count + "");
            Refresh();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PaymentTable.SelectedItem == null) return;
                DataRowView dataRow = (DataRowView)PaymentTable.SelectedItem;
                MakeBudgetPage2._periodPayments.Remove((int)dataRow.Row.ItemArray[0]);
                Refresh();
            }
            catch (Exception ex)
            {
                // niech se klika
            }
        }
        private void Refresh()
        {
            DataTable salary = new DataTable();
            salary.Columns.Add("id", typeof(int));
            salary.Columns.Add("Nazwa", typeof(string));
            salary.Columns.Add("Zapłata", typeof(int));
            salary.Columns.Add("Kategoria", typeof(string));
            salary.Columns.Add("Powtarzalność", typeof(string));
            salary.Columns.Add("Od Kiedy", typeof(string));
            salary.Columns.Add("Do Kiedy", typeof(string));
            for (int i = 0; i < MakeBudgetPage2._periodPayments.Count; i++)
            {
                PeriodPayment p = MakeBudgetPage2._periodPayments[i + 1];
                salary.Rows.Add(i + 1, p.Name, p.Amount, MakeBudgetWindow._categories[p.CategoryID].Name, "Co " + p.Frequency + " " + p.Period, p.StartDate.ToString(), p.EndDate.ToString());
            }
            PaymentTable.ItemsSource = salary.DefaultView;
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

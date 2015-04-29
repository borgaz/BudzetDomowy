using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Budget.Classes;

namespace Budget.Nowy_budzet
{
    /// <summary>
    /// Interaction logic for AddedPaymentPage.xaml
    /// </summary>
    public partial class AddedPaymentPage : Page
    {
        private Dictionary<int, PeriodPayment> _payments;
        private Dictionary<int, Category> _categories;
        public AddedPaymentPage(Dictionary<int,PeriodPayment> d,Dictionary<int,Category> c)
        {
            InitializeComponent();
            _payments = d;
            _categories = c;
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (PaymentTable.SelectedItem == null) return;
                DataRowView dataRow = (DataRowView)PaymentTable.SelectedItem;
                _payments.Remove((int)dataRow.Row.ItemArray[0]);
                Refresh(_payments);
            }
            catch (Exception ex)
            {
                // niech se klika
            }
        }
        private void Refresh(Dictionary<int,PeriodPayment> d)
        {
            DataTable salary = new DataTable();
            salary.Columns.Add("id", typeof(int));
            salary.Columns.Add("Nazwa", typeof(string));
            salary.Columns.Add("Zapłata", typeof(int));
            salary.Columns.Add("Kategoria", typeof(string));
            salary.Columns.Add("Powtarzalność", typeof(string));
            salary.Columns.Add("Od Kiedy", typeof(string));
            salary.Columns.Add("Do Kiedy", typeof(string));
            for (int i = 0; i < d.Count; i++)
            {
                PeriodPayment p = d[i + 1];
                salary.Rows.Add(i + 1, p.Name, p.Amount, _categories[p.CategoryID].Name, "Co " + p.Frequency + " " + p.Period, p.StartDate.ToShortDateString(), (p.EndDate.Equals(DateTime.MaxValue) ? "Nie zdefiniowano" : p.EndDate.ToString()));
            }
            PaymentTable.ItemsSource = salary.DefaultView;
            PaymentTable.Columns[0].Visibility = Visibility.Hidden;
        }

        private void PaymentTable_OnLoaded(object sender, RoutedEventArgs e)
        {
            Refresh(_payments);
        }
    }
}

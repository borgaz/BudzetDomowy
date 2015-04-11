using System;
using System.Data;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Bugdet.Nowy_budzet
{
    /// <summary>
    /// Interaction logic for addedSalariesPage.xaml
    /// </summary>
    public partial class AddedSalariesPage : Page
    {
        public AddedSalariesPage()
        {
            InitializeComponent();
            MessageBox.Show(MakeBudgetPage2.BudgetList.Count + "");
            Refresh();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SalaryTable.SelectedItem == null) return;
                DataRowView dataRow = (DataRowView)SalaryTable.SelectedItem;
                MakeBudgetPage2.BudgetList.RemoveAt((int)dataRow.Row.ItemArray[0]);
                Refresh();
            }
            catch(Exception ex)
            {
                // niech se klika
            }
        }
        private void Refresh()
        {

            DataTable salary = new DataTable();
            salary.Columns.Add("id", typeof(int));
            salary.Columns.Add("Nazwa", typeof(string));
            salary.Columns.Add("Wynagrodzenie", typeof(int));
            salary.Columns.Add("Powtarzalność", typeof(string));
            for(int i = 0; i < MakeBudgetPage2.BudgetList.Count;i++)
            {
                SalaryInfo item = (SalaryInfo)MakeBudgetPage2.BudgetList.ToArray().GetValue(i);

                salary.Rows.Add(i,item.Name, item.Value, (item.Type == 1 ? "co " + item.Repeat + " dni" : "w każdy " + item.Repeat + " dzień miesiąca"));
            }
            SalaryTable.ItemsSource = salary.DefaultView;
          //  salaryTable.Columns[0].Visibility = Visibility.Hidden;
            new Thread(new ThreadStart(HideColumns)).Start();
        }
        private void HideColumns()
        {
            while (SalaryTable.Columns.Count == 0)
            { }
            this.Dispatcher.Invoke(HideColums2);
        }
        private void HideColums2()
        {
            SalaryTable.Columns[0].Visibility = Visibility.Hidden;

        }

        private void salaryTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}

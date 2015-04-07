﻿using Bugdet.Nowy_budzet;
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
    /// Interaction logic for addedSalariesPage.xaml
    /// </summary>
    public partial class AddedSalariesPage : Page
    {
        public AddedSalariesPage()
        {
            InitializeComponent();
            MessageBox.Show(MakeBudgetPage2.budgetList.Count + "");
            Refresh();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (salaryTable.SelectedItem != null)
                {
                    DataRowView dataRow = (DataRowView)salaryTable.SelectedItem;
                    MakeBudgetPage2.budgetList.RemoveAt((int)dataRow.Row.ItemArray[0]);
                    Refresh();
                }
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
            for(int i = 0; i < MakeBudgetPage2.budgetList.Count;i++)
            {
                SalaryInfo item = (SalaryInfo)MakeBudgetPage2.budgetList.ToArray().GetValue(i);

                salary.Rows.Add(i,item.Name, item.Value, (item.Type == 1 ? "co " + item.Repeat + " dni" : "w każdy " + item.Repeat + " dzień miesiąca"));
            }
            salaryTable.ItemsSource = salary.DefaultView;
          //  salaryTable.Columns[0].Visibility = Visibility.Hidden;
            new Thread(new ThreadStart(HideColumns)).Start();
        }
        private void HideColumns()
        {
            while (salaryTable.Columns.Count == 0)
            { }
            this.Dispatcher.Invoke(HideColums2);
        }
        private void HideColums2()
        {
            salaryTable.Columns[0].Visibility = Visibility.Hidden;

        }

        private void salaryTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}

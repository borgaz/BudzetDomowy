﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Budget.Main_Classes;

namespace Budget.New_Budget
{
    /// <summary>
    /// Interaction logic for addedSalariesPage.xaml
    /// </summary>
    public partial class AddedSalariesPage : Page
    {
        private Dictionary<int, PeriodPayment> _salaries;
        private Dictionary<int, Category> _categories;
        public AddedSalariesPage(Dictionary<int,PeriodPayment> d,Dictionary<int,Category> c)
        {
            InitializeComponent();
            _salaries = d;
            _categories = c;
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SalaryTable.SelectedItem == null) return;
                DataRowView dataRow = (DataRowView)SalaryTable.SelectedItem;
                _salaries.Remove((int)dataRow.Row.ItemArray[0]);
                Refresh(_salaries);
            }
            catch(Exception)
            {
                // Nic sie nie dzieje
                // niech se klika
            }
        }
        private void Refresh(Dictionary<int,PeriodPayment> d)
        {
            DataTable salary = new DataTable();
            salary.Columns.Add("id", typeof(int));
            salary.Columns.Add("Nazwa", typeof(string));
            salary.Columns.Add("Wynagrodzenie", typeof(int));
            salary.Columns.Add("Kategoria", typeof (string));
            salary.Columns.Add("Powtarzalność", typeof(string));
            salary.Columns.Add("Od Kiedy", typeof (string));
            salary.Columns.Add("Do Kiedy", typeof (string));
            for(int i = 0; i < d.Count;i++)
            {
                PeriodPayment p = d[i + 1];
                salary.Rows.Add(i + 1, p.Name, p.Amount, _categories[p.CategoryID].Name, "Co " + p.Frequency + " " + p.Period, p.StartDate.ToShortDateString(),(p.EndDate.Equals(DateTime.MaxValue) ? "Nie zdefiniowano" : p.EndDate.ToString()));
            }
            SalaryTable.ItemsSource = salary.DefaultView;
            SalaryTable.Columns[0].Visibility = Visibility.Hidden;
        }

        private void SalaryTable_OnLoaded(object sender, RoutedEventArgs e)
        {
            Refresh(_salaries);
        }
    }
}

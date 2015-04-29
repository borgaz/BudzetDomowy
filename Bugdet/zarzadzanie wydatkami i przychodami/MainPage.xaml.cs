using System;
using System.Windows;
using System.Collections.Generic;
using System.Data;
using System.Windows.Controls;
using Budget.Classes;
using Budget.Nowy_budzet;

namespace Budget.zarzadzanie_wydatkami_i_przychodami
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private AddPaymentPage _singlePaymentPage = new AddPaymentPage();
        private AddSalaryPage _singleSalaryPage = new AddSalaryPage();
        public MainPage()
        {
            InitializeComponent();
        }

        private void addPaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            BtnsContentFrame.Content = _singlePaymentPage;
        }

        private void addSalaryBtn_Click(object sender, RoutedEventArgs e)
        {
            BtnsContentFrame.Content = _singleSalaryPage;
        }

        private void NewBudgetButton_Click(object sender, RoutedEventArgs e)
        {
            new MakeBudgetWindow(1).ShowDialog();
        }

        private void LoadHistoryButton_Click(object sender, RoutedEventArgs e)
        {
          //  dataGridView.Items.Clear();
            RefreshTable();

            //dataGridView.ItemsSource = Budget.Instance.Payments.Values;
        }

        private void RefreshTable()
        {
            DataTable history = new DataTable();
            history.Columns.Add("Nazwa", typeof(string));
            history.Columns.Add("Kwota", typeof(int));
            history.Columns.Add("Kategoria", typeof(string));
            foreach(Payment p in Classes.Budget.Instance.Payments.Values)
            {
               // Payment p = Budget.Instance.Payments[i];
                history.Rows.Add(p.Name, p.Amount, Classes.Budget.Instance.Categories[p.CategoryID].Name);
            }
            dataGridView.ItemsSource = history.DefaultView; 
        }
        private void DumpAllButton_Click(object sender, RoutedEventArgs e)
        {
            //SqlConnect.Instance.CleanDatabase();
            //Budget.Instance.DumpAll();
            Classes.Budget.Instance.Dump();
        }

        private void dataGridView_Loaded(object sender, RoutedEventArgs e)
        {
          //  dataGridView.Items.Clear();
            RefreshTable();

           // dataGridView.ItemsSource = Budget.Instance.Payments.Values;
        }
    }
}
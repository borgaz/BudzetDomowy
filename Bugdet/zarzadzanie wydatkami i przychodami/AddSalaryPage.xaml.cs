﻿using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Bugdet.zarzadzanie_wydatkami_i_przychodami
{
    /// <summary>
    /// Interaction logic for AddSalaryPage.xaml
    /// </summary>
    public partial class AddSalaryPage : Page
    {
        public AddSalaryPage()
        {
            InitializeComponent();
        }
        private void addSalaryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (paymentName.Text != "" && paymentValue.Text != "" && categoryBox.SelectedIndex != -1)
            {
                if (SQLConnect.Instance.AddSingleSalary(paymentName.Text, double.Parse(paymentValue.Text), categoryBox.SelectedIndex, note.Text))
                {
                    infoBox.Text = "Dodano";
                    infoBox.Foreground = Brushes.Green;
                    paymentName.Text = "";
                    paymentValue.Text = "";
                    categoryBox.SelectedIndex = -1;
                }
                else
                {
                    infoBox.Text = "Wystąpił Błąd";
                    infoBox.Foreground = Brushes.Red;
                }
            }
            else
            {
                infoBox.Text = "Uzupełnij Wszystko";
                infoBox.Foreground = Brushes.Red;
            }
        }
        private void InsertCategories()
        {
            DataSet result = SQLConnect.Instance.SelectQuery("Select id,name from Categories order by id asc");
            foreach (DataTable table in result.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    categoryBox.Items.Add(row["name"]);
                }
            }
        }
    }
}

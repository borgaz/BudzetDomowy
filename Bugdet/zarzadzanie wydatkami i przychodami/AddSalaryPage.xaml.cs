using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Budget.zarzadzanie_wydatkami_i_przychodami
{
    /// <summary>
    /// Interaction logic for AddSalaryPage.xaml
    /// </summary>
    public partial class AddSalaryPage : Page
    {
        public AddSalaryPage()
        {
            InitializeComponent();
            //InsertCategories();
            Budget.Instance.InsertCategories(CategoryBox, true);
            InsertDateTypes(TypeOfDayComboBox);
        }
        private void addSalaryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentName.Text != "" && PaymentValue.Text != "" && CategoryBox.SelectedIndex != -1)
            {
                ComboBoxItem categoryItem = (ComboBoxItem)CategoryBox.SelectedValue;
                if (PeriodCheckBox.IsChecked == true)
                {

                    Budget.Instance.AddPeriodPayment(Budget.Instance.Payments.Last().Key+1,
                        new PeriodPayment(categoryItem.Id, Convert.ToDouble(PaymentValue.Text), Note.Text, false,
                            PaymentName.Text, Convert.ToInt32(NumberOfTexBox.Text), TypeOfDayComboBox.Text,
                            StartDatePicker.DisplayDate, StartDatePicker.DisplayDate,
                            (EndDateEnableCheckBox.IsChecked == true
                                ? EndDatePicker.DisplayDate
                                : StartDatePicker.DisplayDate.AddYears(10).Date)));
                }
                else
                {
                    Budget.Instance.AddSinglePayment(Budget.Instance.Payments.Last().Key+1,
                        new SinglePayment(Note.Text, Convert.ToDouble(PaymentValue.Text), categoryItem.Id, false,
                            PaymentName.Text, DateTime.Now));

                }
                InfoBox.Text = "Dodano";
                InfoBox.Foreground = Brushes.Green;
                PaymentName.Text = "";
                PaymentValue.Text = "";
                CategoryBox.SelectedIndex = -1;
                TypeOfDayComboBox.SelectedIndex = -1;
                NumberOfTexBox.Text = "";
            }
            else
            {
                InfoBox.Text = "Uzupełnij Wszystko";
                InfoBox.Foreground = Brushes.Red;
            }
        }

        public static void InsertDateTypes(ComboBox cBox)
        {
            cBox.Items.Add("DZIEŃ");
            cBox.Items.Add("TYDZIEŃ");
            cBox.Items.Add("MIESIĄC");
            cBox.Items.Add("ROK");
        }
        private void InsertCategories()
        {
            DataSet result = SqlConnect.Instance.SelectQuery("Select id,name from Categories order by id asc");
            foreach (DataTable table in result.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    CategoryBox.Items.Add(row["name"]);
                }
            }
        }

        private void periodCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PeriodInfoGrid.IsEnabled = true;
            if (EndDateEnableCheckBox.IsChecked == false)
                EndDatePicker.IsEnabled = false;
        }

        private void PeriodCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            PeriodInfoGrid.IsEnabled = false;
        }

        private void EndDateEnableCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            EndDatePicker.IsEnabled = true;
        }

        private void EndDateEnableCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            EndDatePicker.IsEnabled = false;
        }
    }
}
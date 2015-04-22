using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Budget.zarzadzanie_wydatkami_i_przychodami
{
    /// <summary>
    /// Interaction logic for addPaymentPage.xaml
    /// </summary>
    public partial class AddPaymentPage : Page
    {
        public AddPaymentPage()
        {
            InitializeComponent();
            AddSalaryPage.InsertDateTypes(TypeOfDayComboBox);
            Budget.Instance.InsertCategories(CategoryBox,false);
        }

        private void addPaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentName.Text != "" && PaymentValue.Text != "" && CategoryBox.SelectedIndex != -1)
            {
                ComboBoxItem categoryItem = (ComboBoxItem)CategoryBox.SelectedValue;
                int temp_id = 1;
                if (PeriodCheckBox.IsChecked == true)
                {
                    try
                    { temp_id = Budget.Instance.Payments.Last().Key+1; }
                    catch (Exception ex)
                    { } //gdy brak elementów w tablicy temp_id = 1
                    Budget.Instance.ListOfAdds.Add(new Changes(typeof(PeriodPayment), temp_id));
                    Budget.Instance.AddPeriodPayment(temp_id,
                        new PeriodPayment(categoryItem.Id, Convert.ToDouble(PaymentValue.Text), Note.Text, true,
                            PaymentName.Text, Convert.ToInt32(NumberOfTexBox.Text), TypeOfDayComboBox.Text,
                            StartDatePicker.DisplayDate, StartDatePicker.DisplayDate,
                            (EndDateEnableCheckBox.IsChecked == true
                                ? EndDatePicker.DisplayDate
                                : StartDatePicker.DisplayDate.AddYears(10).Date)));
                }
                else
                {
                    try
                    { temp_id = Budget.Instance.Payments.Last().Key + 1; }
                    catch (Exception ex)
                    { } //gdy brak elementów w tablicy temp_id = 1
                    Budget.Instance.ListOfAdds.Add(new Changes(typeof(SinglePayment), temp_id));
                    Budget.Instance.AddSinglePayment(temp_id,
                        new SinglePayment(Note.Text, Convert.ToDouble(PaymentValue.Text), categoryItem.Id, true,
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
        private void InsertCategories()
        {


            //DataSet result = SqlConnect.Instance.SelectQuery("Select id, name, type FROM Categories order by id asc");
            //foreach (DataTable table in result.Tables)
            //{
            //    foreach (DataRow row in table.Rows)
            //    {
            //        CategoryBox.Items.Add(row["name"]);
            //    }
            //}
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
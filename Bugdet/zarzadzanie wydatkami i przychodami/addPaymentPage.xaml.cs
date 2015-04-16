using System.Data;
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
            InsertCategories();
        }

        private void addPaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentName.Text != "" && PaymentValue.Text != "" && CategoryBox.SelectedIndex != -1)
            {
                if (SqlConnect.Instance.AddSinglePayment(PaymentName.Text, double.Parse(PaymentValue.Text), CategoryBox.SelectedIndex, Note.Text))
                {
                    InfoBox.Text = "Dodano";
                    InfoBox.Foreground = Brushes.Green;
                    PaymentName.Text = "";
                    PaymentValue.Text = "";
                    CategoryBox.SelectedIndex = -1;
                }
                else
                {
                    InfoBox.Text = "Wystąpił Błąd";
                    InfoBox.Foreground = Brushes.Red;
                }
            }
            else
            {
                InfoBox.Text = "Uzupełnij Wszystko";
                InfoBox.Foreground = Brushes.Red;
            }
        }
        private void InsertCategories()
        {
            DataSet result = SqlConnect.Instance.SelectQuery("Select id, name, type FROM Categories order by id asc");
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
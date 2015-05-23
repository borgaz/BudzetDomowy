using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Budget.WelcomePage
{
    /// <summary>
    /// Interaction logic for SavingsTargetsPage.xaml
    /// </summary>
    public partial class SavingsTargetsWindow : Window
    {
        public static SavingsTargetsWindow _instance = null;

        private SavingsTargetsWindow()
        {
            InitializeComponent();
            AutoCheckBox.IsChecked = false;
            AmountTextBox.IsEnabled = false;
            FrequencyTextBox.IsEnabled = false;
            PeriodComboBox.IsEnabled = false;
            Label1.IsEnabled = false;
            Label2.IsEnabled = false;
        }

        public static SavingsTargetsWindow Instance
        {
            get
            {
                return _instance ?? (_instance = new SavingsTargetsWindow());
            }
            set 
            { 
                _instance = value; 
            }
        }

        private void AutoCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            AmountTextBox.IsEnabled = true;
            FrequencyTextBox.IsEnabled = true;
            PeriodComboBox.IsEnabled = true;
            Label1.IsEnabled = true;
            Label2.IsEnabled = true;
            DatePicker.IsEnabled = false;
        }

        private void AutoCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            AutoCheckBox.IsChecked = false;
            AmountTextBox.IsEnabled = false;
            FrequencyTextBox.IsEnabled = false;
            PeriodComboBox.IsEnabled = false;
            Label1.IsEnabled = false;
            Label2.IsEnabled = false;
            DatePicker.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int indexOfSavingsTarget = Main_Classes.Budget.Instance.SavingsTargets.Keys.Max();
            double lastPostponedAmount = 0;
            Main_Classes.SavingsTarget sT = null;
            Main_Classes.PeriodPayment pP;
            Main_Classes.SinglePayment sP;
            if (!NameTextBox.Text.Equals(String.Empty) && !AmountTextBox.Text.Equals(String.Empty))
            {
                if (AutoCheckBox.IsChecked == false)
                {
                    sT = new Main_Classes.SavingsTarget(NameTextBox.Text, NoteTextBox.Text, DatePicker.SelectedDate.Value,
                                                           PriorityComboBox.SelectedIndex, 0, DateTime.Today, Convert.ToDouble(AmountTextBox.Text));
                }
                else if (AutoCheckBox.IsChecked == true)
                {
                    sT = new Main_Classes.SavingsTarget(NameTextBox.Text, NoteTextBox.Text, countDeadline(out lastPostponedAmount),
                                                            PriorityComboBox.SelectedIndex, 0, DateTime.Today, Convert.ToDouble(AmountTextBox.Text));
                    int indexOfPeriodPayment = Main_Classes.Budget.Instance.Payments.Keys.Min() - 1;
                    pP = new Main_Classes.PeriodPayment(0, Convert.ToDouble(AmountTextBox), NoteTextBox.Text, false,
                                                            NameTextBox.Text, Convert.ToInt32(FrequencyTextBox.Text), Convert.ToString(PeriodComboBox.SelectedIndex), DateTime.Today, DateTime.Today, countDeadline(out lastPostponedAmount));
                    int indexOfSinglePayment = Main_Classes.Budget.Instance.Payments.Keys.Max() + 1;
                    sP = new Main_Classes.SinglePayment(NoteTextBox.Text, lastPostponedAmount, 0, false, NameTextBox.Text, DateTime.Today);
                    Main_Classes.Budget.Instance.AddSinglePayment(indexOfSinglePayment, sP);
                    Main_Classes.Budget.Instance.AddPeriodPayment(indexOfPeriodPayment, pP);
                }
                Main_Classes.Budget.Instance.AddSavingsTarget(indexOfSavingsTarget, sT);
            }
            else
            {
                MessageBox.Show("Uzupełnij wszystkie pola!");
            }
        }

        private DateTime countDeadline(out double lastPostponedAmount)
        {
            double neededAmount = Convert.ToDouble(AmountTextBox.Text), postponedAmount = Convert.ToDouble(Amount2TextBox.Text);
            int frequency = Convert.ToInt32(FrequencyTextBox.Text);
            string period = Convert.ToString(PeriodComboBox.SelectedIndex);
            DateTime deadLine = DateTime.Today;
            while(neededAmount > postponedAmount)
            {
                if (period == "MIESIĄC")
                    deadLine.AddMonths(frequency);
                else if (period == "DZIEŃ")
                    deadLine.AddDays(frequency);
                else if (period == "TYDZIEŃ")
                    deadLine.AddDays(7 * frequency);
                else if (period == "ROK")
                    deadLine.AddYears(frequency);
                neededAmount -= postponedAmount;
            }
            lastPostponedAmount = neededAmount;
            return deadLine;
        }
    }
}
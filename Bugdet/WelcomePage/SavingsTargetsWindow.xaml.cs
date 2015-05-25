using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class SavingsTargetsWindow :  INotifyPropertyChanged
    {
        private static SavingsTargetsWindow _instance = null;

        private SavingsTargetsWindow()
        {
            InitializeComponent();

            DisabledElements();
            InsertPriorityTypes();
            New_Budget.MakeBudgetWindow.InsertDateTypes(PeriodComboBox);
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

        protected override void OnClosed(EventArgs e)
        {
            _instance = null;
        }

        private void InsertPriorityTypes()
        {
            PriorityComboBox.Items.Add(Main_Classes.SavingsTarget.Priorities.BardzoWysoki);
            PriorityComboBox.Items.Add(Main_Classes.SavingsTarget.Priorities.Wysoki);
            PriorityComboBox.Items.Add(Main_Classes.SavingsTarget.Priorities.Normalny);
            PriorityComboBox.Items.Add(Main_Classes.SavingsTarget.Priorities.Niski);
            PriorityComboBox.Items.Add(Main_Classes.SavingsTarget.Priorities.BardzoNiski);
        }

        private void AutoCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Amount2TextBox.IsEnabled = true;
            FrequencyTextBox.IsEnabled = true;
            PeriodComboBox.IsEnabled = true;
            Label1.IsEnabled = true;
            Label2.IsEnabled = true;
            EndDatePicker.IsEnabled = false;
            StartDatePicker.IsEnabled = true;
            CheckButton.IsEnabled = true;
        }

        private void AutoCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DisabledElements();
        }

        private void DisabledElements()
        {
            CheckButton.IsEnabled = false;
            AutoCheckBox.IsChecked = false;
            Amount2TextBox.IsEnabled = false;
            FrequencyTextBox.IsEnabled = false;
            PeriodComboBox.IsEnabled = false;
            Label1.IsEnabled = false;
            Label2.IsEnabled = false;
            EndDatePicker.IsEnabled = true;
            StartDatePicker.IsEnabled = false;
            CheckButton.IsEnabled = false;

            EndDatePicker.SelectedDate = DateTime.Now;
            StartDatePicker.SelectedDate = DateTime.Now;
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime deadLine, deadLine2;
            double temp;
            if (!Amount2TextBox.Text.Equals(String.Empty) && !FrequencyTextBox.Text.Equals(String.Empty) && !AmountTextBox.Text.Equals(String.Empty) && PeriodComboBox.SelectedIndex != -1)
            {
                deadLine = CountDeadline(out temp, out deadLine2);
                if (deadLine2.Equals(DateTime.Today))
                {
                    EndDatePicker.Text = Convert.ToString(deadLine);
                }
                else
                {
                    EndDatePicker.Text = Convert.ToString(deadLine2);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String text = String.Empty;
            int indexOfSavingsTarget = Main_Classes.Budget.Instance.SavingsTargets.Count > 0 ? Main_Classes.Budget.Instance.SavingsTargets.Keys.Max() + 1: 0;
            double lastPostponedAmount = 0;
            DateTime lastPay;
            Main_Classes.SavingsTarget sT = null;
            Main_Classes.PeriodPayment pP;
            Main_Classes.SinglePayment sP;
            if (!NameTextBox.Text.Equals(String.Empty) && !AmountTextBox.Text.Equals(String.Empty) && PriorityComboBox.SelectedIndex != -1)
            {
                if (AutoCheckBox.IsChecked == false)
                {
                    sT = new Main_Classes.SavingsTarget(NameTextBox.Text, NoteTextBox.Text, EndDatePicker.SelectedDate.Value,
                                                           Convert.ToInt32(PriorityComboBox.SelectedItem), 0, DateTime.Now, Convert.ToDouble(AmountTextBox.Text), false);
                    Main_Classes.Budget.Instance.AddSavingsTarget(indexOfSavingsTarget, sT);
                    OnPropertyChanged("Update_sTDG");
                    this.Close();
                }
                else if (AutoCheckBox.IsChecked == true)
                {
                    if (!Amount2TextBox.Text.Equals(String.Empty) && !FrequencyTextBox.Text.Equals(String.Empty) && PeriodComboBox.SelectedIndex != -1)
                    {
                        sT = new Main_Classes.SavingsTarget(NameTextBox.Text, NoteTextBox.Text, CountDeadline(out lastPostponedAmount, out lastPay),
                                                            Convert.ToInt32(PriorityComboBox.SelectedItem), 0, DateTime.Now, Convert.ToDouble(AmountTextBox.Text), true);
                        int indexOfPeriodPayment = Main_Classes.Budget.Instance.Payments.Count > 0 ? Main_Classes.Budget.Instance.Payments.Keys.Min() - 1 : -1;
                        pP = new Main_Classes.PeriodPayment(0, Convert.ToDouble(Amount2TextBox.Text), NoteTextBox.Text, true,
                                                                "Oszczędzanie: " + NameTextBox.Text, Convert.ToInt32(FrequencyTextBox.Text), Convert.ToString(PeriodComboBox.SelectedItem), 
                                                                StartDatePicker.SelectedDate.Value, StartDatePicker.SelectedDate.Value, CountDeadline(out lastPostponedAmount, out lastPay));
                        Main_Classes.Budget.Instance.AddSavingsTarget(indexOfSavingsTarget + 1, sT);
                        OnPropertyChanged("Update_sTDG");
                        if (lastPostponedAmount != 0)
                        {
                            int indexOfSinglePayment = Main_Classes.Budget.Instance.Payments.Count > 0 ? Main_Classes.Budget.Instance.Payments.Keys.Max() + 1 : 1;
                            sP = new Main_Classes.SinglePayment(NoteTextBox.Text, lastPostponedAmount, 0, true, "Oszczędzanie: " + NameTextBox.Text, lastPay);
                            Main_Classes.Budget.Instance.AddSinglePayment(indexOfSinglePayment, sP);
                            OnPropertyChanged("Update_sHDG");
                        }
                        Main_Classes.Budget.Instance.AddPeriodPayment(indexOfPeriodPayment, pP);
                        OnPropertyChanged("Update_pPDG");
                        this.Close();
                    }
                    else
                    {
                        if (Amount2TextBox.Text.Equals(String.Empty) || FrequencyTextBox.Text.Equals(String.Empty))
                        {
                            text = "Uzupełnij:\n";
                            if (Amount2TextBox.Text.Equals(String.Empty))
                            {
                                text += "- ile chcesz odkładać co zadany okres czasu\n";
                            }
                            if (FrequencyTextBox.Text.Equals(String.Empty))
                            {
                                text += "- częstotliwość odkładania danej kwoty\n";
                            }
                        }
                        if (PeriodComboBox.SelectedIndex == -1)
                        {
                            text += "Wybierz:\n - jednostkę częstotliwości odkładania kwoty";
                        }
                        MessageBox.Show(text);
                        text = String.Empty;
                    }
                }
            }
            else
            {
                if (AmountTextBox.Text.Equals(String.Empty) || NameTextBox.Text.Equals(String.Empty))
                {
                    text = "Uzupełnij:\n";
                    if (NameTextBox.Text.Equals(String.Empty))
                    {
                        text += "- cel, na który chcesz oszczędzić\n";
                    }
                    if (AmountTextBox.Text.Equals(String.Empty))
                    {
                        text += "- kwotę, którą chcesz oszczędzić\n";
                    }
                }
                if (PriorityComboBox.SelectedIndex == -1)
                {
                    text += "Wybierz:\n - priorytet oszczędzania";
                }
                MessageBox.Show(text);
                text = String.Empty;
            }
        }

        private DateTime CountDeadline(out double lastPostponedAmount, out DateTime lastPay)
        {
            double neededAmount = Convert.ToDouble(AmountTextBox.Text), postponedAmount = Convert.ToDouble(Amount2TextBox.Text);
            int frequency = Convert.ToInt32(FrequencyTextBox.Text);
            string period = Convert.ToString(PeriodComboBox.SelectedItem);
            DateTime deadLine = StartDatePicker.SelectedDate.Value;
            neededAmount -= postponedAmount;
            while(neededAmount >= postponedAmount)
            {
                AddTime(out deadLine, deadLine, period, frequency);
                neededAmount -= postponedAmount;
            }

            lastPostponedAmount = neededAmount;
            lastPay = DateTime.Today;
            if ( lastPostponedAmount != 0)
            {
                AddTime(out lastPay, deadLine, period, frequency);
            }
            return deadLine;
        }

        private void AddTime(out DateTime a, DateTime b, String period, int frequency)
        {
            if (period == "MIESIĄC")
                a = b.AddMonths(frequency);
            else if (period == "DZIEŃ")
                a = b.AddDays(frequency);
            else if (period == "TYDZIEŃ")
                a = b.AddDays(7 * frequency);
            else if (period == "ROK")
                a = b.AddYears(frequency);
            else
                a = DateTime.Now; 
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
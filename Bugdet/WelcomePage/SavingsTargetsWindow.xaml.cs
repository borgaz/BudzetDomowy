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
    public partial class SavingsTargetsWindow : Window, INotifyPropertyChanged
    {
        private static SavingsTargetsWindow _instance = null;

        private SavingsTargetsWindow()
        {
            InitializeComponent();

            AutoCheckBox.IsChecked = false;
            Amount2TextBox.IsEnabled = false;
            FrequencyTextBox.IsEnabled = false;
            PeriodComboBox.IsEnabled = false;
            Label1.IsEnabled = false;
            Label2.IsEnabled = false;

            PriorityComboBox.Items.Add(Main_Classes.SavingsTarget.Priorities.BardzoWysoki);
            PriorityComboBox.Items.Add(Main_Classes.SavingsTarget.Priorities.Wysoki);
            PriorityComboBox.Items.Add(Main_Classes.SavingsTarget.Priorities.Normalny);
            PriorityComboBox.Items.Add(Main_Classes.SavingsTarget.Priorities.Niski);
            PriorityComboBox.Items.Add(Main_Classes.SavingsTarget.Priorities.BardzoNiski);
            New_Budget.MakeBudgetWindow.InsertDateTypes(PeriodComboBox);
            DatePicker.SelectedDate = DateTime.Now;
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

        private void AutoCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Amount2TextBox.IsEnabled = true;
            FrequencyTextBox.IsEnabled = true;
            PeriodComboBox.IsEnabled = true;
            Label1.IsEnabled = true;
            Label2.IsEnabled = true;
            DatePicker.IsEnabled = false;
        }

        private void AutoCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            AutoCheckBox.IsChecked = false;
            Amount2TextBox.IsEnabled = false;
            FrequencyTextBox.IsEnabled = false;
            PeriodComboBox.IsEnabled = false;
            Label1.IsEnabled = false;
            Label2.IsEnabled = false;
            DatePicker.IsEnabled = true;
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
                    sT = new Main_Classes.SavingsTarget(NameTextBox.Text, NoteTextBox.Text, DatePicker.SelectedDate.Value,
                                                           Convert.ToInt32(PriorityComboBox.SelectedItem), 0, DateTime.Now, Convert.ToDouble(AmountTextBox.Text), false);
                    Main_Classes.Budget.Instance.AddSavingsTarget(indexOfSavingsTarget, sT);
                    OnPropertyChanged("Update_sTDG");
                    clearSavingsTargetsWindow();
                }
                else if (AutoCheckBox.IsChecked == true)
                {
                    if (!Amount2TextBox.Text.Equals(String.Empty) && !FrequencyTextBox.Text.Equals(String.Empty) && PeriodComboBox.SelectedIndex != -1)
                    {
                        sT = new Main_Classes.SavingsTarget(NameTextBox.Text, NoteTextBox.Text, countDeadline(out lastPostponedAmount, out lastPay),
                                                            Convert.ToInt32(PriorityComboBox.SelectedItem), 0, DateTime.Now, Convert.ToDouble(AmountTextBox.Text), true);
                        int indexOfPeriodPayment = Main_Classes.Budget.Instance.Payments.Count > 0 ? Main_Classes.Budget.Instance.Payments.Keys.Min() - 1 : -1;
                        pP = new Main_Classes.PeriodPayment(0, lastPostponedAmount, NoteTextBox.Text, true,
                                                                "Oszczędzanie: " + NameTextBox.Text, Convert.ToInt32(FrequencyTextBox.Text), Convert.ToString(PeriodComboBox.SelectedItem), DateTime.Now, DateTime.Now, countDeadline(out lastPostponedAmount, out lastPay));
                        int indexOfSinglePayment = Main_Classes.Budget.Instance.Payments.Count > 0 ? Main_Classes.Budget.Instance.Payments.Keys.Max() + 1 : 1;
                        sP = new Main_Classes.SinglePayment(NoteTextBox.Text, lastPostponedAmount, 0, true, "Oszczędzanie: " + NameTextBox.Text, lastPay);

                        Main_Classes.Budget.Instance.AddSavingsTarget(indexOfSavingsTarget + 1, sT);
                        OnPropertyChanged("Update_sTDG");   
                        Main_Classes.Budget.Instance.AddSinglePayment(indexOfSinglePayment, sP);
                        OnPropertyChanged("Update_sHDG");
                        Main_Classes.Budget.Instance.AddPeriodPayment(indexOfPeriodPayment, pP);
                        OnPropertyChanged("Update_pPDG");
                       

                        Amount2TextBox.Text = String.Empty;
                        FrequencyTextBox.Text = String.Empty;
                        PeriodComboBox.SelectedIndex = -1;
                       
                        clearSavingsTargetsWindow();
                        
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

        private void clearSavingsTargetsWindow()
        {
            NameTextBox.Text = String.Empty;
            NoteTextBox.Text = String.Empty;
            AmountTextBox.Text = String.Empty;
            AutoCheckBox.IsChecked = false;
            PriorityComboBox.SelectedIndex = -1;
            InfoTextBox.Text = "Dodano!";
            InfoTextBox.Foreground = Brushes.Green;
        }

        private DateTime countDeadline(out double lastPostponedAmount, out DateTime lastPay)
        {
            double neededAmount = Convert.ToDouble(AmountTextBox.Text), postponedAmount = Convert.ToDouble(Amount2TextBox.Text);
            int frequency = Convert.ToInt32(FrequencyTextBox.Text);
            string period = Convert.ToString(PeriodComboBox.SelectedItem);
            DateTime deadLine = DateTime.Now;
            while(neededAmount > postponedAmount)
            {
                if (period == "MIESIĄC")
                    deadLine = deadLine.AddMonths(frequency);
                else if (period == "DZIEŃ")
                    deadLine = deadLine.AddDays(frequency);
                else if (period == "TYDZIEŃ")
                    deadLine = deadLine.AddDays(7 * frequency);
                else if (period == "ROK")
                    deadLine = deadLine.AddYears(frequency);
                neededAmount -= postponedAmount;
            }
            lastPostponedAmount = neededAmount;

            if (period == "MIESIĄC")
                lastPay = deadLine.AddMonths(frequency);
            else if (period == "DZIEŃ")
                lastPay = deadLine.AddDays(frequency);
            else if (period == "TYDZIEŃ")
                lastPay = deadLine.AddDays(7 * frequency);
            else if (period == "ROK")
                lastPay = deadLine.AddYears(frequency);
            else
                lastPay = DateTime.Now;

            return deadLine;
        }

        private void NameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            InfoTextBox.Text = String.Empty;
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
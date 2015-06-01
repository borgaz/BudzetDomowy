using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Budget.WelcomePage
{
    /// <summary>
    /// Interaction logic for AddAmountToSavingsTargetWindow.xaml
    /// </summary>
    public partial class AddAmountToSavingsTargetWindow : Window, INotifyPropertyChanged
    {
        private static AddAmountToSavingsTargetWindow _instance = null;
        private int index;

        private AddAmountToSavingsTargetWindow()
        {
            InitializeComponent();
        }

        public static AddAmountToSavingsTargetWindow Instance
        {
            get
            {
                return _instance == null ? (_instance = new AddAmountToSavingsTargetWindow()) : _instance;
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            String text = String.Empty;
            double amount, missingAmount;
            Main_Classes.SinglePayment sP;

            if (TargetComboBox.SelectedIndex != -1 && !AmountTextBox.Text.Equals(String.Empty))
            {
                amount = Convert.ToDouble(AmountTextBox.Text);
                missingAmount = Convert.ToDouble(MissingAmountTextBox.Text);
                if (amount > missingAmount)
                {
                    amount = missingAmount;
                }
                if (Main_Classes.Budget.Instance.SavingsTargets[index].AddMoney(amount, index) == true)
                {
                    OnPropertyChanged("Update_sTDG");
                }

                int indexOfSinglePayment = Main_Classes.Budget.Instance.Payments.Count > 0 ? Main_Classes.Budget.Instance.Payments.Keys.Max() + 1 : 1;
                sP = new Main_Classes.SinglePayment("", amount, 0, true, "Oszczędzanie: " + Main_Classes.Budget.Instance.SavingsTargets[index].Target, DateTime.Now);
                Main_Classes.Budget.Instance.AddSinglePayment(indexOfSinglePayment, sP);
                OnPropertyChanged("Update_sHDG");
                OnPropertyChanged("Refresh_sTDG");
                this.Close();
            }
            else
            {
                if (AmountTextBox.Text.Equals(String.Empty))
                {
                    text = "Uzupełnij:\n- ile chcesz odłożyć\n";
                }
                if (TargetComboBox.SelectedIndex == -1)
                {
                    text += "Wybierz:\n - na który cel chcesz odłożyć";
                }
                MessageBox.Show(text);
                text = String.Empty;
            }
        }

        private void SubtractButton_Click(object sender, RoutedEventArgs e)
        {
            String text = String.Empty;
            double amount, possessedAmount;
            Main_Classes.SinglePayment sP;
            if (TargetComboBox.SelectedIndex != -1 && !AmountTextBox.Text.Equals(String.Empty))
            {
                amount = Convert.ToDouble(AmountTextBox.Text);
                possessedAmount = Convert.ToDouble(PossessedAmountTextBox.Text);
                if (amount > possessedAmount)
                {
                    amount = possessedAmount;
                }
                Main_Classes.Budget.Instance.SavingsTargets[index].AddMoney(-1 * amount, index);
               
                int indexOfSinglePayment = Main_Classes.Budget.Instance.Payments.Count > 0 ? Main_Classes.Budget.Instance.Payments.Keys.Max() + 1 : 1;
                sP = new Main_Classes.SinglePayment("", amount, 0, false, "Oszczędzanie: " + Main_Classes.Budget.Instance.SavingsTargets[index].Target, DateTime.Now);
                Main_Classes.Budget.Instance.AddSinglePayment(indexOfSinglePayment, sP);
                OnPropertyChanged("Update_sHDG");
                OnPropertyChanged("Refresh_sTDG");
                this.Close();
            }
            else
            {
                if (AmountTextBox.Text.Equals(String.Empty))
                {
                    text = "Uzupełnij:\n- ile chcesz odjąć\n";
                }
                if (TargetComboBox.SelectedIndex == -1)
                {
                    text += "Wybierz:\n - z którego celu chcesz odjąć";
                }
                MessageBox.Show(text);
                text = String.Empty;
            }
        }

        private void TargetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            index = Main_Classes.Budget.Instance.SavingsTargets.FirstOrDefault(x => x.Value.Target.Equals(Convert.ToString(TargetComboBox.SelectedItem))).Key;

            double neededAmount = Main_Classes.Budget.Instance.SavingsTargets[index].NeededAmount, moneyHoldings = Main_Classes.Budget.Instance.SavingsTargets[index].MoneyHoldings;
            TargetAmountTextBox.Text = Convert.ToString(neededAmount);
            PossessedAmountTextBox.Text = Convert.ToString(moneyHoldings);
            MissingAmountTextBox.Text = Convert.ToString(neededAmount - moneyHoldings);
        }

        private void InsertTargets()
        {
            foreach(Main_Classes.SavingsTarget sT in Main_Classes.Budget.Instance.SavingsTargets.Values)
            {
                if (sT.Type == false && sT.CountMoneyLeft() > 0 )
                {
                    TargetComboBox.Items.Add(sT.Target);
                }
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            InsertTargets();

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }     
        }  
    }
}
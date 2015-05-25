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
using System.Windows.Shapes;

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

        protected override void OnClosed(EventArgs e)
        {
            _instance = null;    
        }

        public static AddAmountToSavingsTargetWindow Instance
        {
            get
            {
                return _instance ?? (_instance = new AddAmountToSavingsTargetWindow());
            }
            set
            {
                _instance = value;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
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
                Main_Classes.Budget.Instance.SavingsTargets[index].AddMoney(amount, index);

                int indexOfSinglePayment = Main_Classes.Budget.Instance.Payments.Count > 0 ? Main_Classes.Budget.Instance.Payments.Keys.Max() + 1 : 1;
                sP = new Main_Classes.SinglePayment("", amount, 0, true, "Oszczędzanie: " + Main_Classes.Budget.Instance.SavingsTargets[index].Target, DateTime.Now);
                Main_Classes.Budget.Instance.AddSinglePayment(indexOfSinglePayment, sP);
                OnPropertyChanged("Update_sHDG");
                OnPropertyChanged("Refresh_sTDG");
                this.Close();
            }
        }

        private void SubtractButton_Click(object sender, RoutedEventArgs e)
        {
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
                if (sT.Type == false && sT.CountMoneyLeft() > 0)
                {
                    TargetComboBox.Items.Add(sT.Target);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            InsertTargets();
        }
    }
}
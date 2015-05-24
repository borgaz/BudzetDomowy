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
using System.Windows.Shapes;

namespace Budget.WelcomePage
{
    /// <summary>
    /// Interaction logic for AddAmountToSavingsTargetWindow.xaml
    /// </summary>
    public partial class AddAmountToSavingsTargetWindow : Window
    {
        private static AddAmountToSavingsTargetWindow _instance = null;
        public System.Windows.Controls.DataGrid welcomePageSavingsTargetsDataGrid;
        private int index;

        private AddAmountToSavingsTargetWindow()
        {
            InitializeComponent();
            InsertTargets();
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

        private void ReadyButton_Click(object sender, RoutedEventArgs e)
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
                sP = new Main_Classes.SinglePayment("", amount, 0, true, "Oszczędzanie: " + Main_Classes.Budget.Instance.SavingsTargets[index].Target, DateTime.Today);
                Main_Classes.Budget.Instance.AddSinglePayment(indexOfSinglePayment, sP);

                welcomePageSavingsTargetsDataGrid.Items.Refresh();
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
                if (sT.Type == false)
                {
                    TargetComboBox.Items.Add(sT.Target);
                }
            }
        }
    }
}

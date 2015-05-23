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
        }

        private void AutoCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            AutoCheckBox.IsChecked = false;
            AmountTextBox.IsEnabled = false;
            FrequencyTextBox.IsEnabled = false;
            PeriodComboBox.IsEnabled = false;
            Label1.IsEnabled = false;
            Label2.IsEnabled = false;
        }
    }
}
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

namespace Budget.SettingsPage
{
    /// <summary>
    /// Interaction logic for CustomizationPage.xaml
    /// </summary>
    public partial class CustomizationPage : Page
    {
        private int whichPage;

        public CustomizationPage(int i)
        {
            whichPage = i;
            InitializeComponent();
            Payments_Manager.AddSalaryPage.InsertDateTypes(DateTypeComboBox);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (whichPage == 1)
            {
                Label.Content = "w przód.";
                TitleOfPageLabel.Content = "Dostosowywanie wyświetlania predykcji na stronie głównej";
                AmountOfTextBox.Text = Convert.ToString(SettingsPage.Settings.Instance.PP_AmountOf);
                AmountToTextBox.Text = Convert.ToString(SettingsPage.Settings.Instance.PP_AmountTo);
                NumberOfRowTextBox.Text = Convert.ToString(SettingsPage.Settings.Instance.PP_NumberOfRow);
                DateTypeComboBox.SelectedItem = SettingsPage.Settings.Instance.PP_Period;
                FrequencyTextBox.Text = Convert.ToString(SettingsPage.Settings.Instance.PP_Frequency);

            }
            else if (whichPage == 2)
            {
                Label.Content = "w tył.";
                TitleOfPageLabel.Content = "Dostosowywanie wyświetlania historii na stronie głównej";
                AmountOfTextBox.Text = Convert.ToString(SettingsPage.Settings.Instance.SH_AmountOf);
                AmountToTextBox.Text = Convert.ToString(SettingsPage.Settings.Instance.SH_AmountTo);
                NumberOfRowTextBox.Text = Convert.ToString(SettingsPage.Settings.Instance.SH_NumberOfRow);
                DateTypeComboBox.SelectedItem = SettingsPage.Settings.Instance.SH_Period;
                FrequencyTextBox.Text = Convert.ToString(SettingsPage.Settings.Instance.SH_Frequency);
            }
            
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (whichPage == 1)
            {
                SettingsPage.Settings.Instance.PP_AmountOf = Convert.ToDouble(AmountOfTextBox.Text);
                SettingsPage.Settings.Instance.PP_AmountTo = Convert.ToDouble(AmountToTextBox.Text);
                SettingsPage.Settings.Instance.PP_NumberOfRow = Convert.ToInt32(NumberOfRowTextBox.Text);
                SettingsPage.Settings.Instance.PP_Period = Convert.ToString(DateTypeComboBox.SelectedItem);
                SettingsPage.Settings.Instance.PP_Frequency = Convert.ToInt32(FrequencyTextBox.Text);
            }
            else if (whichPage == 2)
            {
                SettingsPage.Settings.Instance.SH_AmountOf = Convert.ToDouble(AmountOfTextBox.Text);
                SettingsPage.Settings.Instance.SH_AmountTo = Convert.ToDouble(AmountToTextBox.Text);
                SettingsPage.Settings.Instance.SH_NumberOfRow = Convert.ToInt32(NumberOfRowTextBox.Text);
                SettingsPage.Settings.Instance.SH_Period = Convert.ToString(DateTypeComboBox.SelectedItem);
                SettingsPage.Settings.Instance.SH_Frequency = Convert.ToInt32(FrequencyTextBox.Text);
            }
            SettingsPage.Settings.Instance.SerializeToXml();
        }
    }
}
using System;
using System.Windows.Controls;
using Budget.Main_Classes;

namespace Budget.SettingsPage
{
    /// <summary>
    /// Interaction logic for GeneralOptionsPage.xaml
    /// </summary>
    public partial class GeneralOptionsPage : Page
    {
        public GeneralOptionsPage()
        {
            InitializeComponent();
            AutoSaveText.Text = "1";
        }

        private void SaveSettingsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (AutoSaveText.Text != "")
            {
                SqlConnect.Instance._savingMinutes = Convert.ToInt32(AutoSaveText.Text);
            }
        }

        private void AutoSaveText_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (MinutesLabel == null)
                return;
            if (MinutesLabel.Content == "")
                return;
            if (Convert.ToInt32(AutoSaveText.Text) == 1)
            {
                MinutesLabel.Content = "minute";
            }
            if (Convert.ToInt32(AutoSaveText.Text) > 1 && Convert.ToInt32(AutoSaveText.Text) < 5)
            {
                MinutesLabel.Content = "minuty";
            }
            if (Convert.ToInt32(AutoSaveText.Text) > 4)
            {
                MinutesLabel.Content = "minut";
            }
        }
    }
}

using System.Windows;
using System.Windows.Controls;

namespace Budget.SettingsPage
{
    /// <summary>
    /// Interaction logic for MainSettingsWindow.xaml
    /// </summary>
    public partial class MainSettingsWindow : Page
    {
        private readonly GeneralOptionsPage _generalOptions = new GeneralOptionsPage();
        private readonly CustomizationPage _customizationHistoryPage = new CustomizationPage(2);
        private readonly CustomizationPage _customizationFuturePage = new CustomizationPage(1);
        public MainSettingsWindow()
        {
            InitializeComponent();
        }

        private void ManagePages(int page)
        {
            switch (page)
            {
                case 1:
                    SettingsContentFrame.Content = _generalOptions;
                    break;
                case 2:
                    SettingsContentFrame.Content = _customizationHistoryPage;
                    break;
                case 3:
                    SettingsContentFrame.Content = _customizationFuturePage;
                    break;
            }
        }

        private void GeneralOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            ManagePages(1);
        }

        private void HistoryOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            ManagePages(2);
        }

        private void FutureOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            ManagePages(3);
        }
    }
}

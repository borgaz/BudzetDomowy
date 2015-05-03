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
        public MainSettingsWindow(int page)
        {
            InitializeComponent();
            ManagePages(page);
        }

        private void ManagePages(int page)
        {
            switch (page)
            {
                case 1:
                    SettingsContentFrame.Content = _generalOptions;
                    break;
            }
        }

        private void GeneralOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            ManagePages(1);
        }
    }
}

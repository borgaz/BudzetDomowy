using System.Windows;
using System.Windows.Controls;

namespace Budget.InterfacePage
{
    /// <summary>
    /// Interaction logic for InterfacePage.xaml
    /// </summary>
    public partial class InterfacePage : Page
    {
        public InterfacePage()
        {
            InitializeComponent();
        }

        private void AddPaymentsButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ActualPage = 1;
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ActualPage = 4;
        }

        private void WelcomePageButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ActualPage = 3;
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ActualPage = 5;
        }
    }
}

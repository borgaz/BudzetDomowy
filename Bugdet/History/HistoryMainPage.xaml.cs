using System.Windows;
using System.Windows.Controls;

namespace Budget.History
{
    /// <summary>
    /// Interaction logic for HistoryMainPage.xaml
    /// </summary>
    public partial class HistoryMainPage : Page
    {
        HistoryTabPage _historyTab = new HistoryTabPage();
        public HistoryMainPage()
        {
            InitializeComponent();

        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            HistoryFrame.Content = _historyTab;
        }
    }
}

using System.Windows;

namespace Bugdet.Nowy_budzet
{
    /// <summary>
    /// Interaction logic for AddedSalariesWindow.xaml
    /// </summary>
    public partial class AddedSalariesWindow : Window
    {
        readonly AddedSalariesPage _page1 = new AddedSalariesPage();
        readonly AddedPaymentPage _page2 = new AddedPaymentPage();
        public AddedSalariesWindow(int page)
        {
            InitializeComponent();
            AddPage(page);
        }
        private void AddPage(int page)
        {
            switch(page)
            {
                case 1:
                    pageFrame.Content = _page1;
                    break;
                case 2:
                    pageFrame.Content = _page2;
                    break;
            }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

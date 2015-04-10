using System.Windows;
using System.Windows.Controls;

namespace Bugdet.zarzadzanie_wydatkami_i_przychodami
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private AddPaymentPage singlePaymentPage = new AddPaymentPage();
        private AddSalaryPage singleSalaryPage = new AddSalaryPage();
        public MainPage()
        {
            InitializeComponent();
        }

        private void addPaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            btnsContentFrame.Content = singlePaymentPage;
        }

        private void addSalaryBtn_Click(object sender, RoutedEventArgs e)
        {
            btnsContentFrame.Content = singleSalaryPage;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Bugdet.Nowy_budzet.MakeBudgetWindow(1).ShowDialog();
        }
    }
}

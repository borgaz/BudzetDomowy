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
            new MakeBudgetWindow(1).ShowDialog();
        }
    }
}

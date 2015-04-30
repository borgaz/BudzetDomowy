using System.Collections.Generic;
using System.Windows;
using Budget.Main_Classes;

namespace Budget.New_Budget
{
    /// <summary>
    /// Interaction logic for AddedSalariesWindow.xaml
    /// </summary>
    public partial class AddedSalariesWindow : Window
    {
        private AddedSalariesPage _page1;
        private AddedPaymentPage _page2;
        public AddedSalariesWindow(int page,Dictionary<int,PeriodPayment> d,Dictionary<int,Category> c)
        {
            InitializeComponent();
            AddPage(page,d,c);
        }
        private void AddPage(int page,Dictionary<int,PeriodPayment> d,Dictionary<int,Category> c)
        {
            switch(page)
            {
                case 1:
                    _page1 = new AddedSalariesPage(d,c);
                    PageFrame.Content = _page1;
                    break;
                case 2:
                    _page2 = new AddedPaymentPage(d,c);
                    PageFrame.Content = _page2;
                    break;
            }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

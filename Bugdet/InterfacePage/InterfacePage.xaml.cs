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
    }
}

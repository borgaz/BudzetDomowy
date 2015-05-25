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

namespace Budget.Analisys
{
    /// <summary>
    /// Interaction logic for AnalisysAvgPage.xaml
    /// </summary>
    public partial class AnalisysAvgPage : Page
    {
        BalanceChart var = new BalanceChart();
        public AnalisysAvgPage()
        {
            InitializeComponent();
            DataContext = var;
        }
    }
}

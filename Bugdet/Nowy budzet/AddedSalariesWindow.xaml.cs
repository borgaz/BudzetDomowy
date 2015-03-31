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
using System.Windows.Shapes;

namespace Bugdet
{
    /// <summary>
    /// Interaction logic for AddedSalariesWindow.xaml
    /// </summary>
    public partial class AddedSalariesWindow : Window
    {
        AddedSalariesPage page1 = new AddedSalariesPage();
        AddedPaymentPage page2 = new AddedPaymentPage();
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
                    pageFrame.Content = page1;
                    break;
                case 2:
                    pageFrame.Content = page2;
                    break;
            }
        }
    }
}

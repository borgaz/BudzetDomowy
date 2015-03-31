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

namespace Bugdet
{
    /// <summary>
    /// Interaction logic for MakeBudzetPage1.xaml
    /// </summary>
    public partial class MakeBudzetPage1 : Page
    {
        public MakeBudzetPage1()
        {
            InitializeComponent();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            MakeBudzetWindow.page = 0;
        }

        private void forwardBtn_Click(object sender, RoutedEventArgs e)
        {
            if(budgetBalance.Text != "" && budgetNameText.Text != "")
            {
                MakeBudzetWindow._budgetstack.Push(budgetNameText);
                MakeBudzetWindow._budgetstack.Push(budgetBalance);
            }
        }
    }
}

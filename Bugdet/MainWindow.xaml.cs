
using Bugdet.zarzadzanie_wydatkami_i_przychodami;
using System;
using System.Collections;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainPage mainPage = new MainPage();
        public MainWindow()
        {
            InitializeComponent();
            SQLConnect.Instance.Connect();
            //SQLConnect.Instance.FetchAll();
            InsertPage();
        }

        private void makeNewBudget_Click(object sender, RoutedEventArgs e)
        {
            MakeBudgetWindow makeBugdet = new MakeBudgetWindow(1);
            makeBugdet.ShowDialog();
        }
        private void InsertPage()
        {
            this.mainContentFrame.Content = mainPage;
        }
    }
}
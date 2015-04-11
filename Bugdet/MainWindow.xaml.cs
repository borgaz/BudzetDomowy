
using System.Windows;
using Bugdet.Nowy_budzet;
using Bugdet.zarzadzanie_wydatkami_i_przychodami;

namespace Bugdet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainPage _mainPage = new MainPage();
        public MainWindow()
        {
            InitializeComponent();
            SqlConnect.Instance.Connect();
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
            this.MainContentFrame.Content = _mainPage;
        }
    }
}
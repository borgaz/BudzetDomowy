using System.Windows;
using Budget.Nowy_budzet;
using Budget.zarzadzanie_wydatkami_i_przychodami;

namespace Budget
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
            SqlConnect.Instance.Connect("budzet");
            
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
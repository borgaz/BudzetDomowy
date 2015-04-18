using System.Windows;
using Budget.Nowy_budzet;
using Budget.zarzadzanie_wydatkami_i_przychodami;
using Budget.LoginWindow;

namespace Budget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainPage _mainPage;
        readonly LoginWindow.LoginWindow loginWindow = new LoginWindow.LoginWindow();
        public MainWindow()
        {
            loginWindow.ShowDialog();
            if (!loginWindow.IsLogged)
            {
                Close();
                return;
            }
            InitializeComponent();
            InsertPage();
        }
        private void InsertPage()
        {
            _mainPage = new MainPage();
            this.MainContentFrame.Content = _mainPage;
        }
    }
}
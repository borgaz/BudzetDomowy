using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using Budget.History;
using Budget.Nowy_budzet;
using Budget.zarzadzanie_wydatkami_i_przychodami;
using Budget.LoginWindow;
using Budget.InterfacePage;
using Budget.SettingsPage;

namespace Budget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainPage _mainPage;
        private InterfacePage.InterfacePage _interfacePage;
        private WelcomePage.WelcomePage _welcomePage;
        private MainSettingsWindow _mainSettingsWindow;
        private HistoryMainPage _historyPage;
        private static int _actualPage = 3;
        private bool running = true;
        private Thread th;

        public MainWindow()
        {
            LoginWindow.LoginWindow.Instance.ShowDialog();
            if (!LoginWindow.LoginWindow.Instance.IsLogged)
            {
                Close();
                return;
            }

            InitializeComponent();
            _mainPage = new MainPage();
            _interfacePage = new InterfacePage.InterfacePage();
            _welcomePage = new WelcomePage.WelcomePage();
            _mainSettingsWindow = new MainSettingsWindow(1);
            _historyPage = new HistoryMainPage();
            InsertPage();
            th = new Thread(OnPageChange);
            th.Start();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (LoginWindow.LoginWindow.Instance.IsLogged == true)
            {
                if (Budget.Instance.ListOfAdds.Count != 0 || Budget.Instance.ListOfEdts.Count != 0 || Budget.Instance.ListOfDels.Count != 0)
                {
                    if (MessageBox.Show("Czy chcesz zapisać zmiany przed zamknięciem?", "Zapisz dane", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        Budget.Instance.Dump();
                    }
                }
                if (th.IsAlive)
                {
                    th.Abort();
                }           
            }
        }

        ~MainWindow()
        {
            running = false;
        }

        private void OnPageChange()
        {
            int page = _actualPage;
            while (running)
            {
                if (page != _actualPage)
                {
                    this.Dispatcher.Invoke(InsertPage);
                    page = _actualPage;
                }
            }
        }
        protected void InsertPage()
        {
            switch (_actualPage)
            {
                case 1:
                    this.MainContentFrame.Content = _mainPage;
                    _mainPage.dataGridView.ItemsSource = null;
                    break;
                case 2:
                    this.MainContentFrame.Content = _interfacePage;
                    break;
                case 3:
                    this.MainContentFrame.Content = _welcomePage;
                    break;
                case 4:
                    this.MainContentFrame.Content = _mainSettingsWindow;
                    break;
                case 5:
                    this.MainContentFrame.Content = _historyPage;
                    break;
            }
        }

        public static int ActualPage
        {
            set
            {
                _actualPage = value;
            }
        }

        private void AdditionalBtn_Click(object sender, RoutedEventArgs e)
        {
            _actualPage = 2;
            InsertPage();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _mainPage = null;
            running = false;
            _interfacePage = null;
        }
    }
}
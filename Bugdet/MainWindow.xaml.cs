using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Budget.History;
using Budget.Nowy_budzet;
using Budget.zarzadzanie_wydatkami_i_przychodami;
using Budget.LoginWindow;
using Budget.InterfacePage;
using Budget.SettingsPage;
using Budget.Utility_Classes;

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
        private static int _actualPage = 2;
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
            InitializeObjects();
        }

        private void InitializeObjects()
        {
            _mainPage = new MainPage();
            _interfacePage = new InterfacePage.InterfacePage();
            _welcomePage = new WelcomePage.WelcomePage();
            _mainSettingsWindow = new MainSettingsWindow(1);
            _historyPage = new HistoryMainPage();
            WelcomePageButton.IsEnabled = false;
            _actualPage = 2;
            CreatorButton.IsEnabled = true;
            SettingsButton.IsEnabled = true;
            HistoryButton.IsEnabled = true;
            InsertPage();
            running = true;
            th = new Thread(OnPageChange);
            th.Start();
        }
        //protected override void OnClosed(EventArgs e)
        //{
   
        //}

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
                    //_mainPage.PeriodPaymentsTable.ItemsSource = null;
                    break;
                //case 2:
                //    this.MainContentFrame.Content = _interfacePage;
                //    break;
                case 2:
                    this.MainContentFrame.Content = _welcomePage;
                    break;
                case 3:
                    this.MainContentFrame.Content = _mainSettingsWindow;
                    break;
                case 4:
                    this.MainContentFrame.Content = _historyPage;
                    break;
            }
        }

        public static int ActualPage
        {
            set { _actualPage = value; }
        }

        private void CreatorButton_Click(object sender, RoutedEventArgs e)
        {
            CreatorButton.IsEnabled = false;
            WelcomePageButton.IsEnabled = true;
            SettingsButton.IsEnabled = true;
            HistoryButton.IsEnabled = true;
            _actualPage = 1;
            InsertPage();
        }

        private void WelcomePageButton_Click(object sender, RoutedEventArgs e)
        {
            CreatorButton.IsEnabled = true;
            WelcomePageButton.IsEnabled = false;
            SettingsButton.IsEnabled = true;
            HistoryButton.IsEnabled = true;
            _actualPage = 2;
            InsertPage();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            CreatorButton.IsEnabled = true;
            WelcomePageButton.IsEnabled = true;
            SettingsButton.IsEnabled = false;
            HistoryButton.IsEnabled = true;
            _actualPage = 3;
            InsertPage();
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            CreatorButton.IsEnabled = true;
            WelcomePageButton.IsEnabled = true;
            SettingsButton.IsEnabled = true;
            HistoryButton.IsEnabled = false;
            _actualPage = 4;
            InsertPage();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (LoginWindow.LoginWindow.Instance.IsLogged == true)
            {
                var msgBox = new CustomMessageBox(CustomMessageBox.MessageBoxType.YesNoCanel,
                    "Czy chcesz zamknąć program?");
                msgBox.ShowDialog();
                switch (msgBox.result)
                {
                    case 1:
                        msgBox = null;
                        e.Cancel = true;
                        break;
                    case 2:
                            running = false;
                            CheckSavings();
                            this.Visibility = Visibility.Hidden;
                            LoginWindow.LoginWindow.Instance.IsLogged = false;
                            LoginWindow.LoginWindow.Instance = null;
                            LoginWindow.LoginWindow.Instance.ShowDialog();
                            if (!LoginWindow.LoginWindow.Instance.IsLogged)
                            {
                                e.Cancel = false;
                                return;
                            }
                            e.Cancel = true;
                            Main_Classes.Budget.ResetInstance();
                            InitializeObjects();
                            this.Visibility = Visibility.Visible;
                        break;
                    case 3:
                            running = false;
                            CheckSavings();
                            e.Cancel = false;
                        break;
                    }
            }
        }

        private void CheckSavings()
        {
            if (Main_Classes.Budget.Instance.ListOfAdds.Count != 0 || Main_Classes.Budget.Instance.ListOfEdts.Count != 0 || Main_Classes.Budget.Instance.ListOfDels.Count != 0)
            {
                if (MessageBox.Show("Czy chcesz zapisać zmiany przed zamknięciem?", "Zapisz dane", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Main_Classes.Budget.Instance.Dump();
                }
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.LeftCtrl)) return;
            if (e.Key.Equals(Key.S))
                Main_Classes.Budget.Instance.Dump();
        }
    }
}
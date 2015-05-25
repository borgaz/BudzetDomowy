using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Budget.Analisys;
using Budget.History;
using Budget.Main_Classes;
using Budget.Payments_Manager;
using Budget.SettingsPage;
using Budget.Utility_Classes;
using System;

namespace Budget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainPage _mainPage;
        private WelcomePage.WelcomePage _welcomePage;
        private MainSettingsWindow _mainSettingsWindow;
        private HistoryMainPage _historyPage;
        private Analisys.AnalisysMainPage _analisysPage;
        private static int _actualPage = 2;
        private bool running = true;
       // private Thread th;

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
            Title = "BUDŻET DOMOWY";
            Title = Title + " (" + Main_Classes.Budget.Instance.Name + ")";
            _mainPage = new MainPage();
            _welcomePage = new WelcomePage.WelcomePage();
            _mainSettingsWindow = new MainSettingsWindow();
            _historyPage = new HistoryMainPage();
            _analisysPage = new AnalisysMainPage();
            WelcomePageButton.IsEnabled = false;
            _actualPage = 2;
            CreatorButton.IsEnabled = true;
            SettingsButton.IsEnabled = true;
            HistoryButton.IsEnabled = true;
            AnalisysButton.IsEnabled = true;
            InsertPage();
            //running = true;
            //th = new Thread(OnPageChange);
            //th.Start();

        }

        ~MainWindow()
        {
            running = false;
        }

        //private void OnPageChange()
        //{
        //    int page = _actualPage;
        //    while (running)
        //    {
        //        Thread.Sleep(1);
        //        if (page != _actualPage)
        //        {
        //            this.Dispatcher.Invoke(InsertPage);
        //            page = _actualPage;
        //        }
        //    }
        //}

        protected void InsertPage()
        {
            switch (_actualPage)
            {
                case 1:
                    this.MainContentFrame.Content = _mainPage;
                    break;
                case 2:
                    this.MainContentFrame.Content = _welcomePage;
                    break;
                case 3:
                    this.MainContentFrame.Content = _mainSettingsWindow;
                    break;
                case 4:
                    this.MainContentFrame.Content = _historyPage;
                    break;
                case 5:
                    MainContentFrame.Content = _analisysPage;
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
            AnalisysButton.IsEnabled = true;
            _actualPage = 1;
            InsertPage();

            List<SinglePayment> list = Main_Classes.Budget.Instance.CheckPeriodPayments();
            if (list.Count > 0)
            {
                new CheckingPeriodPayments(list).ShowDialog();
            }
        }

        private void WelcomePageButton_Click(object sender, RoutedEventArgs e)
        {
            CreatorButton.IsEnabled = true;
            WelcomePageButton.IsEnabled = false;
            SettingsButton.IsEnabled = true;
            HistoryButton.IsEnabled = true;
            AnalisysButton.IsEnabled = true;
            _actualPage = 2;
            InsertPage();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            CreatorButton.IsEnabled = true;
            WelcomePageButton.IsEnabled = true;
            SettingsButton.IsEnabled = false;
            HistoryButton.IsEnabled = true;
            AnalisysButton.IsEnabled = true;
            _actualPage = 3;
            InsertPage();
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            CreatorButton.IsEnabled = true;
            WelcomePageButton.IsEnabled = true;
            SettingsButton.IsEnabled = true;
            HistoryButton.IsEnabled = false;
            AnalisysButton.IsEnabled = true;
            _actualPage = 4;
            InsertPage();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (LoginWindow.LoginWindow.Instance.IsLogged == true)
            {
                var msgBox = new CustomMessageBox(CustomMessageBox.MessageBoxType.YesNoCanel, "Czy chcesz zamknąć program?");
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
                        SettingsPage.Settings.Instance = null;
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
            if (Main_Classes.Budget.Instance.ListOfAdds.Count != 0 || Main_Classes.Budget.Instance.ListOfEdits.Count != 0 || Main_Classes.Budget.Instance.ListOfDels.Count != 0)
            {
                if (MessageBox.Show("Czy chcesz zapisać zmiany przed zamknięciem?", "Zapisz dane", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Main_Classes.Budget.Instance.Dump();
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightAlt) || Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightCtrl)) return;
            if (e.Key.Equals(Key.S))
                Main_Classes.Budget.Instance.Dump();
        }

        private void AnalisysButton_Click(object sender, RoutedEventArgs e)
        {
            CreatorButton.IsEnabled = true;
            WelcomePageButton.IsEnabled = true;
            SettingsButton.IsEnabled = true;
            HistoryButton.IsEnabled = true;
            AnalisysButton.IsEnabled = false;
            _actualPage = 5;
            InsertPage();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Main_Classes.Budget.Instance.Dump();
        }
    }
}
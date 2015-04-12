using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace Bugdet.Nowy_budzet
{
    /// <summary>
    /// Interaction logic for MakeBudzetWindow.xaml
    /// </summary>
    public partial class MakeBudgetWindow : Window
    {
        private static MakeBudgetWindow _instance = null; // jako ze Frame nie jest statyczny, trzeba bylo jakos z Page'ow wywolywac zmiane
        MakeBudgetPage1 _page1 = new MakeBudgetPage1(); // strona pierwsza
        MakeBudgetPage2 _page2 = new MakeBudgetPage2(); // strona druga
        MakeBudgetPage3 _page3 = new MakeBudgetPage3(); // strona trzecia
        public static Dictionary<int, Category> _categories = SqlConnect.Instance.AddDefaultCategories();
        public static Dictionary<int,PeriodPayment> _payments = new Dictionary<int, PeriodPayment>();
        public static Dictionary<int, PeriodPayment> _salaries = new Dictionary<int, PeriodPayment>(); 
        private int _actual = 1;
        private Budget newBudget;
        public static String name;
        public static String password;
        public static BalanceLog balance;
        


        public MakeBudgetWindow(int page)
        {
            InitializeComponent();
            ManagePages(page,0);
        }
        public void ManagePages(int page,int back)
        {
            switch (page)
            {
                case 0:
                    PageFrame.Content = null; // 
                    _instance = null;
                    _page1 = null;
                    _page2 = null;
                    _page3 = null;
                    this.Close(); // 
                    break;
                case 1:
                    if (back == 1)
                        _page1.BackToThisPage();
                    PageFrame.Content = _page1;
                    ExitBtn.Content = "Wyjdz";
                    break;
                case 2:
                    if (back == 1)
                        _page2.BackToThisPage();
                    PageFrame.Content = _page2;
                    ExitBtn.Content = "Wroc";
                    break;
                case 3:
                    if (back == 1)
                        _page3.BackToThisPage();
                    PageFrame.Content = _page3;
                    ForwardBtn.Content = "Zakończ";
                    break;
                case 4:
                    if (CompleteBudget())
                    {
                        ManagePages(0, 0);
                    }
                    else
                        MessageBox.Show("Nie przeszlo w CompleteBudget()");
                    break;
                default: break;
            }
        }
        private Boolean CheckPage(int pageNr)
        {
            switch (pageNr)
            {
                case 0:
                    return false;
                    
                case 1:
                    return _page1.CheckInfo();
                    
                case 2:
                    return _page2.CheckInfo();
                case 3:
                    return _page3.CheckInfo();
                default:
                    return false;
            }
        }
        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_actual == 1)
            {
                _page1 = null;
                _page2 = null;
                this.Close();
            }
            else
                ManagePages(--_actual,1);
        }

        private void forwardBtn_Click(object sender, RoutedEventArgs e)
        {
            if(CheckPage(_actual))
            {
                Console.WriteLine("nowy--------------------");
                ManagePages(++_actual,0);
            }
        }
        private Boolean CompleteBudget()
        {
            return true;
        }
    }
}

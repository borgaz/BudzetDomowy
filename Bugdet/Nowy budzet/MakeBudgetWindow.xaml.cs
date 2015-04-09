using System;
using System.Collections;
using System.Windows;

namespace Bugdet.Nowy_budzet
{
    /// <summary>
    /// Interaction logic for MakeBudzetWindow.xaml
    /// </summary>
    public partial class MakeBudgetWindow : Window
    {
        private static MakeBudgetWindow _instance = null; // jako ze Frame nie jest statyczny, trzeba bylo jakos z Page'ow wywolywac zmiane
        MakeBudgetPage1 page1 = new MakeBudgetPage1(); // strona pierwsza
        MakeBudgetPage2 page2 = new MakeBudgetPage2(); // strona druga
        MakeBudgetPage3 page3 = new MakeBudgetPage3(); // strona trzecia
        private int actual = 1;
        public static Stack _budgetstack = new Stack(); // stos, wrzucane w menu tworzenia budzetu dane sa wrzucane na stosik, ew mozna inna forme wymyslec
                                   // dzieki tej formie mozna latwo zdejmowac wrzucone rzeczy klikajac wroc w kolejnym oknie



        public MakeBudgetWindow(int page)
        {
            InitializeComponent();
            ManagePages(page,0);
        }
        public static MakeBudgetWindow Instance
        {
            get { return _instance ?? (_instance = new MakeBudgetWindow(1)); }
        }
        // Dziala normalnie
        public void ManagePages(int _page,int back)
        {
            switch (_page)
            {
                case 0:
                    pageFrame.Content = null; // 
                    _instance = null;
                    page1 = null;
                    page2 = null;
                    page3 = null;
                    this.Close(); // 
                    break;
                case 1:
                    if (back == 1)
                        page1.BackToThisPage();
                    pageFrame.Content = page1;
                    exitBtn.Content = "Wyjdz";
                    break;
                case 2:
                    if (back == 1)
                        page2.BackToThisPage();
                    pageFrame.Content = page2;
                    exitBtn.Content = "Wroc";
                    break;
                case 3:
                    if (back == 1)
                        page3.BackToThisPage();
                    pageFrame.Content = page3;
                    forwardBtn.Content = "Zakończ";
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
                    return page1.CheckInfo();
                    
                case 2:
                    return page2.CheckInfo();
                case 3:
                    return page3.CheckInfo();
                default:
                    return false;
            }
        }
        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (actual == 1)
            {
                _budgetstack = null;
                page1 = null;
                page2 = null;
                this.Close();
            }
            else
                ManagePages(--actual,1);
        }

        private void forwardBtn_Click(object sender, RoutedEventArgs e)
        {
            if(CheckPage(actual))
            {
                Console.WriteLine("nowy--------------------");
                foreach (Object o in _budgetstack)
                {
                    Console.WriteLine(o);
                }
                ManagePages(++actual,0);
            }
        }
        private Boolean CompleteBudget()
        {
            try
            {
                int j = _budgetstack.Count;
                if (j < 3) // sama nazwa i balance
                {
                    SQLConnect.Instance.ExecuteSQLNoNQuery("INSERT into Budget(balance,name) values(" + int.Parse(_budgetstack.Pop().ToString()) + ",'" + _budgetstack.Pop().ToString() + "')");
                }
                else
                {
                    while(_budgetstack.Count > 0)
                    {
                        j = _budgetstack.Count;
                        if (j < 3) // sama nazwa i balance
                        {
                            SQLConnect.Instance.ExecuteSQLNoNQuery("INSERT into Budget(balance,name) values(" + int.Parse(_budgetstack.Pop().ToString()) + ",'" + _budgetstack.Pop().ToString() + "')");
                            break;
                        }
                        int k = (int)_budgetstack.Pop();
                        int p = (int)_budgetstack.Pop();
                        if(p == -2) // dane periodPayment
                        {
                            for(int o = 0; o < k; o++)
                            {
                                SQLConnect.Instance.ExecuteSQLNoNQuery("INSERT INTO PeriodPayments(type,repeat,income,name,startDate) values(" + (int)_budgetstack.Pop() +
                                                                                                                                            "," + int.Parse(_budgetstack.Pop().ToString()) + 
                                                                                                                                            "," + int.Parse(_budgetstack.Pop().ToString()) * (-1) + 
                                                                                                                                            ",'" + _budgetstack.Pop().ToString() + "', date('now'))");
                            }
                        }
                        else 
                        {
                            if (p == -1) // dane periodSalary
                            {
                                for (int o = 0; o < k; o++)
                                {
                                    SQLConnect.Instance.ExecuteSQLNoNQuery("INSERT INTO PeriodPayments(type,repeat,income,name,startDate) values(" + (int)_budgetstack.Pop() +
                                                                                                                                                "," + int.Parse(_budgetstack.Pop().ToString()) +
                                                                                                                                                "," + int.Parse(_budgetstack.Pop().ToString()) +
                                                                                                                                                ",'" + _budgetstack.Pop().ToString() + "', date('now'))");
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }
    }
}

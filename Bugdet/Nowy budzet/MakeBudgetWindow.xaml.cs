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
        MakeBudgetPage1 _page1 = new MakeBudgetPage1(); // strona pierwsza
        MakeBudgetPage2 _page2 = new MakeBudgetPage2(); // strona druga
        MakeBudgetPage3 _page3 = new MakeBudgetPage3(); // strona trzecia
        private int _actual = 1;
        public static Stack Budgetstack = new Stack(); // stos, wrzucane w menu tworzenia budzetu dane sa wrzucane na stosik, ew mozna inna forme wymyslec
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
                Budgetstack = null;
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
                foreach (Object o in Budgetstack)
                {
                    Console.WriteLine(o);
                }
                ManagePages(++_actual,0);
            }
        }
        private Boolean CompleteBudget()
        {
            try
            {
                int j = Budgetstack.Count;
                if (j < 3) // sama nazwa i balance
                {
                    SqlConnect.Instance.ExecuteSqlNonQuery("INSERT into Budget(balance,name) values(" + int.Parse(Budgetstack.Pop().ToString()) + ",'" + Budgetstack.Pop().ToString() + "')");
                }
                else
                {
                    while(Budgetstack.Count > 0)
                    {
                        j = Budgetstack.Count;
                        if (j < 3) // sama nazwa i balance
                        {
                            SqlConnect.Instance.ExecuteSqlNonQuery("INSERT into Budget(balance,name) values(" + int.Parse(Budgetstack.Pop().ToString()) + ",'" + Budgetstack.Pop().ToString() + "')");
                            break;
                        }
                        int k = (int)Budgetstack.Pop();
                        int p = (int)Budgetstack.Pop();
                        if(p == -2) // dane periodPayment
                        {
                            for(int o = 0; o < k; o++)
                            {
                                SqlConnect.Instance.ExecuteSqlNonQuery("INSERT INTO PeriodPayments(type,repeat,income,name,startDate) values(" + (int)Budgetstack.Pop() +
                                                                                                                                            "," + int.Parse(Budgetstack.Pop().ToString()) + 
                                                                                                                                            "," + int.Parse(Budgetstack.Pop().ToString()) * (-1) + 
                                                                                                                                            ",'" + Budgetstack.Pop().ToString() + "', date('now'))");
                            }
                        }
                        else 
                        {
                            if (p == -1) // dane periodSalary
                            {
                                for (int o = 0; o < k; o++)
                                {
                                    SqlConnect.Instance.ExecuteSqlNonQuery("INSERT INTO PeriodPayments(type,repeat,income,name,startDate) values(" + (int)Budgetstack.Pop() +
                                                                                                                                                "," + int.Parse(Budgetstack.Pop().ToString()) +
                                                                                                                                                "," + int.Parse(Budgetstack.Pop().ToString()) +
                                                                                                                                                ",'" + Budgetstack.Pop().ToString() + "', date('now'))");
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

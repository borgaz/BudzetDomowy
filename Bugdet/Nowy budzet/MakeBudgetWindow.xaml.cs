using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Media.Animation;

namespace Budget.Nowy_budzet
{
    /// <summary>
    /// Interaction logic for MakeBudzetWindow.xaml
    /// </summary>
    public partial class MakeBudgetWindow : Window
    {
        private static MakeBudgetWindow _instance = null;
        private static Dictionary<int, Category> _categories = SqlConnect.Instance.AddDefaultCategories(); // 'adddefaultcategories' mozna przeniesc tutaj, bo i tak tylko tutaj jest uzywana
        private static Dictionary<int, PeriodPayment> _payments = new Dictionary<int, PeriodPayment>();
        private static Dictionary<int, PeriodPayment> _salaries = new Dictionary<int, PeriodPayment>();
        private static SalaryInfo salaryInfo = new SalaryInfo();

        private int _actualPage = 1;
        private MakeBudgetPage1 _page1;
        private MakeBudgetPage2 _page2;
        private MakeBudgetPage3 _page3;


        public static MakeBudgetWindow Instance
        {
            get 
            { 
                return _instance ?? (_instance = new MakeBudgetWindow(1)); 
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_actualPage == 2 || _actualPage == 3)
            {
                CompleteBudget();
            }
        }

        public MakeBudgetWindow(int page)
        {
            InitializeComponent();
            ManagePages(page, 0);
        }

        public void ManagePages(int page, int back)
        {
            switch (page)
            {
                case 0:
                {
                    PageFrame.Content = null;
                    _instance = null;
                    _page1 = null;
                    _page2 = null;
                    _page3 = null;
                    this.Close();
                    break;
                }
                    
                case 1:
                {
                    if (back == 1)
                    {
                        _page1.BackToThisPage();
                    }
                    
                    _page1 = new MakeBudgetPage1(salaryInfo); // strona pierwsza
                    PageFrame.Content = _page1;
                    ExitBtn.Content = "Wyjdz";
                    break;
                }
                    
                case 2:
                {
                    if (back == 1)
                    {
                        _page2.BackToThisPage();
                    }
                        
                    _page2 = new MakeBudgetPage2(_salaries, _categories); // strona druga
                    PageFrame.Content = _page2;
                    ExitBtn.IsEnabled = false;
                    ExitBtn.Content = "Wyjdź";
                    ForwardBtn.Content = "Dodaj wydatki";
                    break;
                }
                    
                case 3:
                {
                    if (back == 1)
                    {
                        _page3.BackToThisPage();
                    }
                    
                    _page3 = new MakeBudgetPage3(_payments, _categories); // strona trzecia
                    PageFrame.Content = _page3;
                    ExitBtn.IsEnabled = true;
                    ExitBtn.Content = "Dodaj przychody";
                    ForwardBtn.Content = "Zakończ";
                    break;
                }
                    
                case 4:
                {
                    if (CompleteBudget())
                    {
                        ManagePages(0, 0);
                    }
                    else
                    {
                        MessageBox.Show("Nie przeszło w CompleteBudget()");
                    } 
                    break;
                }
                    
                default: 
                    break;
            }
        }

        private Boolean CheckPage(int pageNr)
        {
            switch (pageNr)
            {
                case 0:
                {
                    return false;
                }  
    
                case 1:
                {
                    if (_page1.CheckInfo() == true)
                    {
                        if (!WizardQuestion())
                        {
                            CompleteBudget();
                            ManagePages(0, 0);
                                
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                } 

                case 2:
                {
                    return _page2.CheckInfo();
                }

                case 3:
                {
                    return _page3.CheckInfo();
                }

                default:
                    return false;
            }
        }

        private Boolean WizardQuestion()
        {
            if (MessageBox.Show("Czy chcesz dodać pierwsze wydatki/przychody?", "Kreator", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_actualPage == 1)
            {
                _page1 = null;
                _page2 = null;
                this.Close();
            }
            else
            {
                ManagePages(--_actualPage, 1);
            }   
        }

        private void forwardBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckPage(_actualPage))
            {
                ManagePages(++_actualPage, 0);
            }
        }

        private Boolean CompleteBudget()
        {
            Dictionary<int, PeriodPayment> p = new Dictionary<int, PeriodPayment>();
            for (int i = 1; i <= _payments.Count; i++)
            {
                p.Add(i, _payments[i]);
            }
            for (int i = 1 + _payments.Count; i <= _payments.Count + _salaries.Count; i++)
            {
                p.Add(i, _salaries[i - p.Count]);
            }

            if (SqlConnect.Instance.DumpCreator(_categories, p, salaryInfo.Name, salaryInfo.Password, new BalanceLog(salaryInfo.Amount, DateTime.Now, 0, 0), salaryInfo.NumberOfPeople))
            {
                LoginWindow.LoginWindow.Instance.IsLogged = true;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
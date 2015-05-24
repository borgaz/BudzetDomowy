using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Budget.Main_Classes;

namespace Budget.New_Budget
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
                    _page1 = new MakeBudgetPage1(salaryInfo); // strona pierwsza
                    PageFrame.Content = _page1;
                    ExitBtn.Content = "Wyjdz";
                    break;
                }
                    
                case 2:
                {
                    if(_page2 == null)
                        _page2 = new MakeBudgetPage2(_salaries, _categories); // strona druga
                    InsertCategories(_page2.CategoryComboBox, _categories, true);
                    PageFrame.Content = _page2;
                    ExitBtn.IsEnabled = false;
                    ExitBtn.Content = "Wyjdź";
                    ForwardBtn.Content = "Dodaj wydatki";
                    break;
                }
                    
                case 3:
                {
                    if (_page3 == null)
                        _page3 = new MakeBudgetPage3(_payments, _categories); // strona trzecia
                    InsertCategories(_page3.CategoryComboBox, _categories, false);
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
                    return CheckInfo();
                }

                case 3:
                {
                    return CheckInfo();
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

            for (int i = 1; i <= _salaries.Count; i++)     
            {
                p.Add(i, _salaries[i]);
            }
            for (int i = 1 + _salaries.Count; i <= _payments.Count + _salaries.Count; i++)
            {
                p.Add(i, _payments[i - _salaries.Count]);
            }

            if (SqlConnect.Instance.DumpCreator(_categories, p, salaryInfo.Name, salaryInfo.Password, new BalanceLog(salaryInfo.Amount, DateTime.Now, 0, 0)))
            {
                LoginWindow.LoginWindow.Instance.IsLogged = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void InsertDateTypes(ComboBox dataBox)
        {
            dataBox.Items.Add("DZIEŃ");
            dataBox.Items.Add("TYDZIEŃ");
            dataBox.Items.Add("MIESIĄC");
            dataBox.Items.Add("ROK");
        }

        /// <summary>
        /// Inserts categories in CategoryComboBox
        /// </summary>
        public static List<int> InsertCategories(ComboBox categoryBox, Dictionary<int, Category>cat, bool type)
        {
            categoryBox.Items.Clear();
            List<int> originalID = new List<int>();
            try
            {
                for (int i = 0; i < cat.Count; i++)
                {
                    if (cat[i].Type == type)
                    {
                        categoryBox.Items.Add(cat[i].Name);
                        originalID.Add(i);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(cat.Count + "");
            }
            return originalID;
        }

        public static Boolean CheckInfo()
        {
            try
            {
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
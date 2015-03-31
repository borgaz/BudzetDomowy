using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace Bugdet
{
    /// <summary>
    /// Interaction logic for MakeBudzetWindow.xaml
    /// </summary>
    public partial class MakeBudgetWindow : Window
    {
        private static MakeBudgetWindow _instance = null; // jako ze Frame nie jest statyczny, trzeba bylo jakos z Page'ow wywolywac zmiane
        MakeBudgetPage1 page1 = new MakeBudgetPage1(); // strona pierwsza
        MakeBudgetPage2 page2 = new MakeBudgetPage2(); // strona druga
        private int actual = 1;
        public Stack _budgetstack = new Stack(); // stos, wrzucane w menu tworzenia budzetu dane sa wrzucane na stosik, ew mozna inna forme wymyslec
                                   // dzieki tej formie mozna latwo zdejmowac wrzucone rzeczy klikajac wroc w kolejnym oknie
     
        public MakeBudgetWindow(int page)
        {
            InitializeComponent();
            ManagePages(page);
        }
        public static MakeBudgetWindow Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MakeBudgetWindow(1);
                return _instance;
            }
        }
        // Dziala normalnie
        public void ManagePages(int _page)
        {
            switch (_page)
            {
                case 0:
                    pageFrame.Content = null; // 
                    _instance = null;
                    this.Close(); // 
                    break;
                case 1:
                    pageFrame.Content = page1;
                    exitBtn.Content = "Wyjdz";
                    break;
                case 2:
                    pageFrame.Content = page2;
                    exitBtn.Content = "Wroc";
                    break;
                case 3:
                    //   pageFrame.Content = page3;
                    break;
                case 4:
                    //   pageFrame.Content = page4;
                    break;
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
                ManagePages(--actual);
        }

        private void forwardBtn_Click(object sender, RoutedEventArgs e)
        {
            if(CheckPage(actual))
            {
                ManagePages(++actual);
            }
        }
    }
}

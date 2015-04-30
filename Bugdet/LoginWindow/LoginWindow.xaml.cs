using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Budget.Main_Classes;
using Budget.New_Budget;

namespace Budget.LoginWindow
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private static LoginWindow _instance = null;
        private bool isLogged = false;
        
        private LoginWindow()
        {
            InitializeComponent();
            InsertBudgets();
        }

        public static LoginWindow Instance
        {
            get
            {
                return _instance ?? (_instance = new LoginWindow());
            }
            set { _instance = value; }
        }

        private void InsertBudgets()
        {
            foreach (string file in Directory.GetFiles(Directory.GetCurrentDirectory(), "*.sqlite"))
            {
                string name = file.Replace(Directory.GetCurrentDirectory(), "");
                name = name.Replace(".sqlite", "");
                name = name.Replace("\\", "");
                BudgetsComboBox.Items.Add(name);
            }
        }

        public bool IsLogged
        {
            get 
            { 
                return isLogged; 
            }
            set
            {
                isLogged = value;
            }
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            if (BudgetsComboBox.SelectedIndex != -1 && 
                SqlConnect.Instance.CheckPassword(BudgetsComboBox.SelectedValue.ToString(), SqlConnect.Instance.HashPasswordMd5(PasswordTextBox.Password)))
            {
                isLogged = true;
                Close();
            }
            else
            {
                if (BudgetsComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show("Nie wybrałeś budżetu.");
                }
                else if (PasswordTextBox.Password == "")
                {
                    MessageBox.Show("Nie wpisałeś hasła.");
                }
                else
                {
                    MessageBox.Show("Wpisałeś nieprawidłowe hasło!");
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddBudgetButton_Click(object sender, RoutedEventArgs e)
        {
            new MakeBudgetWindow(1).ShowDialog();
            if (isLogged == true)
            {
                Close();
            }
            else
            {
                BudgetsComboBox.SelectedIndex = -1;
                PasswordTextBox.Password = "";
            }
        }

        private void BudgetsComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            PasswordTextBox.Password = "";
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                LogInButton_Click(new object(),new RoutedEventArgs());
        }
    }
}
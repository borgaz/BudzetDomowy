using System.IO;
using System.Windows;

namespace Budget.LoginWindow
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window // klasa do dokonczenia po glownym oknie
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void InsertBudgets()
        {
            foreach (string file in Directory.GetFiles(Directory.GetCurrentDirectory(),"*.sqlite"))
            {
                BudgetsComboBox.Items.Add(file.Replace(".sqlite", ""));
            }
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
         //   new MainWindow().ShowDialog();
        }

        private void AddBudgetButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

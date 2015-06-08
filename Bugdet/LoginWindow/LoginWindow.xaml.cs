using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Budget.Main_Classes;
using Budget.New_Budget;
using System;
using System.IO.Compression;

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

        private void BudgetsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PasswordTextBox.Password = "";
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                LogInButton_Click(new object(),new RoutedEventArgs());
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            string nameOfFile = ImportDateBaseFromZIP();
            if (nameOfFile != "")
            {
                MessageBox.Show("Budżet został zaimportowany. Proszę o zalogowanie.");
                BudgetsComboBox.Items.Clear();
                InsertBudgets();
                BudgetsComboBox.SelectedItem = nameOfFile;
            }         
        }

        private string ImportDateBaseFromZIP()
        {
            var openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

            openFileDialog1.Filter = "Plik zip (.zip)|*.zip";
            openFileDialog1.FilterIndex = 1;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    string SQLFile = System.IO.Path.GetFileNameWithoutExtension(openFileDialog1.SafeFileName) + ".sqlite";
                    if (File.Exists(SQLFile))
                        MessageBox.Show("Baza danych o podanej nazwie już istnieje");
                    string zipPath = openFileDialog1.FileName;
                    ZipArchive archive = ZipFile.OpenRead(zipPath);
                    if (archive.Entries.Count > 2)
                    {
                        MessageBox.Show("Niepoprawny plik ZIP. Zbyt duża ilość plików w archiwum.");
                        return "";
                    }

                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (!entry.FullName.EndsWith(".sqlite", StringComparison.OrdinalIgnoreCase)
                            && !entry.FullName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                        {
                            MessageBox.Show("Niepoprawny plik ZIP. Złe rozszerzenie plików budżetu");
                            return "";
                        }
                    }

                    string extract = @".";
                    ZipFile.ExtractToDirectory(zipPath, extract);
                }
                catch
                {
                    return "";
                }
            }
            else
                return "";
            return System.IO.Path.GetFileNameWithoutExtension(openFileDialog1.SafeFileName);
        }
    }
}
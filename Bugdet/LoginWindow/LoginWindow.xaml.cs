﻿using System.Data.SQLite;
using System.IO;
using System.Windows;
using Budget.Nowy_budzet;

namespace Budget.LoginWindow
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window // klasa do dokonczenia po glownym oknie
    {
        private bool isLogged = false;
        public LoginWindow()
        {
            InitializeComponent();
            InsertBudgets();
        }

        private void InsertBudgets()
        {
            BudgetsComboBox.Items.Clear();
            foreach (string file in Directory.GetFiles(Directory.GetCurrentDirectory(),"*.sqlite"))
            {
                string name = file.Replace(Directory.GetCurrentDirectory(), "");
                name = name.Replace(".sqlite", "");
                name = name.Replace("\\", "");
                BudgetsComboBox.Items.Add(name);
            }
        }

        public bool IsLogged
        {
            get { return isLogged; }
        }
        private void LogInButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (BudgetsComboBox.SelectedIndex != -1 && 
                SqlConnect.Instance.CheckPassword(BudgetsComboBox.SelectedValue.ToString(), SqlConnect.Instance.HashPasswordMd5(PasswordTextBox.Password)))
            {
                isLogged = true;
                Close();
            }
            else
            {
                MessageBox.Show("Nie prawidłowy login bądź hasło!");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            isLogged = false;
            Close();
        }

        private void AddBudgetButton_Click(object sender, RoutedEventArgs e)
        {
            new MakeBudgetWindow(1).ShowDialog();
            InsertBudgets();
        }
    }
}

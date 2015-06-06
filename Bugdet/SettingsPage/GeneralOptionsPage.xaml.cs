using System;
using System.Windows.Controls;
using Budget.Main_Classes;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace Budget.SettingsPage
{
    /// <summary>
    /// Interaction logic for GeneralOptionsPage.xaml
    /// </summary>
    public partial class GeneralOptionsPage : Page
    {
        public GeneralOptionsPage()
        {
            InitializeComponent();
            BudgetNameLabel.Content = Main_Classes.Budget.Instance.Name;
            AutoSaveText.Text = "3";
        }

        private void SaveSettingsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (AutoSaveText.Text != "")
            {
                SqlConnect.Instance._savingMinutes = Convert.ToInt32(AutoSaveText.Text);
            }
        }

        private void AutoSaveText_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (MinutesLabel == null)
                return;
            if (MinutesLabel.Content == "")
                return;
            if (Convert.ToInt32(AutoSaveText.Text) == 1)
            {
                MinutesLabel.Content = "minutę";
            }
            if (Convert.ToInt32(AutoSaveText.Text) > 1 && Convert.ToInt32(AutoSaveText.Text) < 5)
            {
                MinutesLabel.Content = "minuty";
            }
            if (Convert.ToInt32(AutoSaveText.Text) > 4)
            {
                MinutesLabel.Content = "minut";
            }
        }

        private void DeleteDataBase_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //System.Diagnostics.Process.Start(Application.ExecutablePath); // to start new instance of application
            string file = @"" + Main_Classes.Budget.Instance.Name + ".sqlite";
            System.Windows.Forms.Application.Restart();
            App.Current.Shutdown();
            Main_Classes.SqlConnect.Instance.Disconnect();
            System.IO.File.Delete(file);
        }

        private void ExportDataBase_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ExportDataBaseToZip())
                MessageBox.Show("Poprawnie wyeksportowana baza danych");
        }

        private bool ExportDataBaseToZip()
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();

            if (result.ToString() == "OK")
            {
                try
                {
                    string budgetName = Main_Classes.Budget.Instance.Name;
                    string startPath = budgetName;
                    string endPath  = dialog.SelectedPath + "\\" + budgetName + ".zip";
                    if (File.Exists(endPath))
                        MessageBox.Show("Plik " + endPath + " istnieje. Podaj inną ścieżkę.");
                    Directory.CreateDirectory(budgetName);
                    string fileInDir = Main_Classes.Budget.Instance.Name + "\\" + budgetName + ".sqlite";
                    File.Copy(Main_Classes.Budget.Instance.Name + ".sqlite", fileInDir);
                    

                    ZipFile.CreateFromDirectory(startPath, endPath);
                    Directory.Delete(budgetName, true);
                }
                catch (IOException e)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

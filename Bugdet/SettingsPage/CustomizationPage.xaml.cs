using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace Budget.SettingsPage
{
    /// <summary>
    /// Interaction logic for CustomizationPage.xaml
    /// </summary>
    public partial class CustomizationPage : Page
    {
        private int whichPage;

        public CustomizationPage(int i)
        {
            whichPage = i;
            InitializeComponent();
            Payments_Manager.AddSalaryPage.InsertDateTypes(DateTypeComboBox);
            InsertCatogoryCheckBoxesToComboBox(Main_Classes.Budget.Instance.Categories);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (whichPage == 1)
            {
                Label.Content = "w przód.";
                TitleOfPageLabel.Content = "Dostosowywanie wyświetlania predykcji na stronie głównej";
                AmountOfTextBox.Text = Convert.ToString(SettingsPage.Settings.Instance.PP_AmountOf);
                AmountToTextBox.Text = Convert.ToString(SettingsPage.Settings.Instance.PP_AmountTo);
                NumberOfRowTextBox.Text = Convert.ToString(SettingsPage.Settings.Instance.PP_NumberOfRow);
                DateTypeComboBox.SelectedItem = SettingsPage.Settings.Instance.PP_Period;
                FrequencyTextBox.Text = Convert.ToString(SettingsPage.Settings.Instance.PP_Frequency);
                CheckListOfCategory(SettingsPage.Settings.Instance.PP_Categories);
            }
            else if (whichPage == 2)
            {
                Label.Content = "w tył.";
                TitleOfPageLabel.Content = "Dostosowywanie wyświetlania historii na stronie głównej";
                AmountOfTextBox.Text = Convert.ToString(SettingsPage.Settings.Instance.SH_AmountOf);
                AmountToTextBox.Text = Convert.ToString(SettingsPage.Settings.Instance.SH_AmountTo);
                NumberOfRowTextBox.Text = Convert.ToString(SettingsPage.Settings.Instance.SH_NumberOfRow);
                DateTypeComboBox.SelectedItem = SettingsPage.Settings.Instance.SH_Period;
                FrequencyTextBox.Text = Convert.ToString(SettingsPage.Settings.Instance.SH_Frequency);
                CheckListOfCategory(SettingsPage.Settings.Instance.SH_Categories);
            }   
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (whichPage == 1)
            {
                SettingsPage.Settings.Instance.PP_AmountOf = Convert.ToDouble(AmountOfTextBox.Text);
                SettingsPage.Settings.Instance.PP_AmountTo = Convert.ToDouble(AmountToTextBox.Text);
                SettingsPage.Settings.Instance.PP_NumberOfRow = Convert.ToInt32(NumberOfRowTextBox.Text);
                SettingsPage.Settings.Instance.PP_Period = Convert.ToString(DateTypeComboBox.SelectedItem);
                SettingsPage.Settings.Instance.PP_Frequency = Convert.ToInt32(FrequencyTextBox.Text);
                SettingsPage.Settings.Instance.PP_Categories = CreateListOfCategoryToSettings();
            }
            else if (whichPage == 2)
            {
                SettingsPage.Settings.Instance.SH_AmountOf = Convert.ToDouble(AmountOfTextBox.Text);
                SettingsPage.Settings.Instance.SH_AmountTo = Convert.ToDouble(AmountToTextBox.Text);
                SettingsPage.Settings.Instance.SH_NumberOfRow = Convert.ToInt32(NumberOfRowTextBox.Text);
                SettingsPage.Settings.Instance.SH_Period = Convert.ToString(DateTypeComboBox.SelectedItem);
                SettingsPage.Settings.Instance.SH_Frequency = Convert.ToInt32(FrequencyTextBox.Text);
                SettingsPage.Settings.Instance.SH_Categories = CreateListOfCategoryToSettings();
            }
            SettingsPage.Settings.Instance.SerializeToXml();
        }

        private List<Main_Classes.Category> CreateListOfCategoryToSettings()
        {
            List<Main_Classes.Category> categories = new List<Main_Classes.Category>();
            foreach(CheckBox catFromComboBox in CategoriesComboBox.Items)
            {     
                if (catFromComboBox.IsChecked == true)
                {
                    Main_Classes.Category cat = Main_Classes.Budget.Instance.Categories.Values.FirstOrDefault(x => x.Name.Equals(catFromComboBox.Content));
                    categories.Add(cat);
                }  
            }
            return categories;
        }

        private void CheckListOfCategory(List<Main_Classes.Category> list)
        {
            foreach(CheckBox catFromComboBox in CategoriesComboBox.Items)
            {
                Main_Classes.Category cat = list.FirstOrDefault(x => x.Name.Equals(catFromComboBox.Content));
                if (cat == null)
                {
                    catFromComboBox.IsChecked = false;
                }
            }
        }

        private void InsertCatogoryCheckBoxesToComboBox(Dictionary<int, Main_Classes.Category> categories)
        {
            foreach(var category in categories)
            {
                CheckBox cB = new CheckBox();
                cB.Content = category.Value.Name;
                cB.IsChecked = true;
                CategoriesComboBox.Items.Add(cB);
            }
        }
    }
}
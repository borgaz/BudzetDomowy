using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Budget.Main_Classes;
using Budget.Utility_Classes;

namespace Budget.Nowy_budzet
{
    /// <summary>
    /// Interaction logic for AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        private Dictionary<int, Category> _categories;
        private bool creator = false; //czy dodajemy w kreatorze
        public AddCategoryWindow()
        {
            InitializeComponent();
        }

        public AddCategoryWindow(Dictionary<int,Category> c) //konstruktor dla kreatora
        {
            InitializeComponent();
            _categories = c;
            creator = true;
        }

        private void AddCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryNameTextBox.Text != "" && categoryTypes.SelectedIndex != -1)
            {
                bool category = false;

                if (categoryTypes.SelectedIndex == 1)
                    category = true;
                if (!creator)
                {
                    Main_Classes.Budget.Instance.ListOfAdds.Add(new Changes(typeof(Category), Main_Classes.Budget.Instance.Categories.Last().Key + 1));
                    Main_Classes.Budget.Instance.Categories.Add(Main_Classes.Budget.Instance.Categories.Last().Key + 1,
                        new Category(CategoryNameTextBox.Text, CategoryNoteTextBox.Text, category));
                }
                else
                {
                    _categories.Add(_categories.Last().Key + 1,
                        new Category(CategoryNameTextBox.Text, CategoryNoteTextBox.Text, category));
                }
                
            }
            else
            {
                MessageBox.Show("Uzupełnij wymagane pola!");
            }
            this.Close();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

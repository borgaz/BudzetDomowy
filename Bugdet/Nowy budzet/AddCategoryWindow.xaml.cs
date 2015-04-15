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

namespace Budget.Nowy_budzet
{
    /// <summary>
    /// Interaction logic for AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        private Dictionary<int, Category> _categories;
        public AddCategoryWindow(Dictionary<int,Category> c)
        {
            InitializeComponent();
            _categories = c;
        }

        private void AddCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryNameTextBox.Text != "" && categoryTypes.SelectedIndex != -1)
            {
                bool category = false;

                if (categoryTypes.SelectedIndex == 1)
                    category = true;
                _categories.Add(_categories.Last().Key + 1,
                    new Category(CategoryNameTextBox.Text, CategoryNoteTextBox.Text, category));
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

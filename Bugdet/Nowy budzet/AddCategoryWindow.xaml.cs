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

namespace Bugdet.Nowy_budzet
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
            if (CategoryNameTextBox.Text != "")
            {
                _categories.Add(_categories.Last().Key + 1,
                    new Category(CategoryNameTextBox.Text, CategoryNoteTextBox.Text));
            }
            this.Close();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

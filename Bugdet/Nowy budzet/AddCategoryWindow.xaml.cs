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
        public AddCategoryWindow()
        {
            InitializeComponent();
        }

        private void AddCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryNameTextBox.Text != "")
            {
                bool category = false; 
                if (categoryTypes.SelectedIndex == 0)
                    category = false;
                if (categoryTypes.SelectedIndex == 1)
                    category = true;
                MakeBudgetWindow._categories.Add(MakeBudgetWindow._categories.Last().Key+1,
                    new Category(CategoryNameTextBox.Text, CategoryNoteTextBox.Text, category));
            }
            this.Close();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

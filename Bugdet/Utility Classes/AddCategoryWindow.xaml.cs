using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Budget.Main_Classes;
using Budget.New_Budget;
using Budget = Budget.Main_Classes.Budget;

namespace Budget.Utility_Classes
{
    /// <summary>
    /// Interaction logic for AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        private Dictionary<int, Category> _categories;
        private bool creator = false; //czy dodajemy w kreatorze
        private ComboBox _comboBox;
        private Main_Classes.Budget.CategoryTypeEnum type;
        private bool _creatorType;
        private List<int> _creatorIdList; 
        public AddCategoryWindow(ComboBox comboBox,Main_Classes.Budget.CategoryTypeEnum type)
        {
            InitializeComponent();
            _comboBox = comboBox;
            this.type = type;
        }

        public AddCategoryWindow(Dictionary<int,Category> c,ComboBox comboBox,bool type,List<int> creatorList ) //konstruktor dla kreatora
        {
            InitializeComponent();
            _categories = c;
            _comboBox = comboBox;
            creator = true;
            _creatorIdList = creatorList;
            _creatorType = type;
        }

        private void AddCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryNameTextBox.Text != "" && categoryTypes.SelectedIndex != -1)
            {
                bool category = categoryTypes.SelectedIndex == 1;

                if (!creator)
                {
                    Main_Classes.Budget.Instance.ListOfAdds.Add(new Changes(typeof(Category), Main_Classes.Budget.Instance.Categories.Last().Key + 1));
                    Main_Classes.Budget.Instance.Categories.Add(Main_Classes.Budget.Instance.Categories.Last().Key + 1,
                        new Category(CategoryNameTextBox.Text, CategoryNoteTextBox.Text, category));
                    Main_Classes.Budget.Instance.InsertCategories(_comboBox, type);
                    _comboBox.SelectedIndex = _comboBox.Items.Count - 1;
                }
                else
                {
                    _categories.Add(_categories.Last().Key + 1,
                        new Category(CategoryNameTextBox.Text, CategoryNoteTextBox.Text, category));
                    _creatorIdList = MakeBudgetWindow.InsertCategories(_comboBox, _categories, _creatorType);
                    _comboBox.SelectedIndex = _comboBox.Items.Count - 1;
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

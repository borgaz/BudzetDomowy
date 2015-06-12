using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Budget.Main_Classes;
using Budget.New_Budget;

namespace Budget.Utility_Classes
{
    /// <summary>
    /// Interaction logic for AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        private Dictionary<int, Category> _categories;
        private bool creator = false; //czy dodajemy w kreatorze
        private ComboBox _actualComboBox;
       // private ComboBox _anotherComboBox;
        private Main_Classes.Budget.CategoryTypeEnum type;
       // private Main_Classes.Budget.CategoryTypeEnum anotherType;
        private bool category;
        private List<int> _creatorIdList;
 
        public AddCategoryWindow(ComboBox actualcomboBox,Main_Classes.Budget.CategoryTypeEnum type)
        {
            InitializeComponent();
            _actualComboBox = actualcomboBox;
          //  _anotherComboBox = anotherComboBox;
            this.type = type;
            if (type == Main_Classes.Budget.CategoryTypeEnum.SALARY)
            {
                CategoryTypeLabel.Content = "Przychody";
                category = true;
            }
            else
            {
                CategoryTypeLabel.Content = "Wydatki";
                category = false;
            }
          //  anotherType = type == Main_Classes.Budget.CategoryTypeEnum.PAYMENT ? Main_Classes.Budget.CategoryTypeEnum.SALARY : Main_Classes.Budget.CategoryTypeEnum.PAYMENT;
        }

        public AddCategoryWindow(Dictionary<int,Category> c,ComboBox actualComboBox,bool type,List<int> creatorList) //konstruktor dla kreatora
        {
            InitializeComponent();
            _categories = c;
            _actualComboBox = actualComboBox;
            creator = true;
            _creatorIdList = creatorList;
            category = type;
            if (category == true)
            {
                CategoryTypeLabel.Content = "Przychody";
            }
            else
            {
                CategoryTypeLabel.Content = "Wydatki";
            }
        }

        private void AddCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryNameTextBox.Text != "")
            {
                if (Main_Classes.Budget.Instance.CheckCategory(CategoryNameTextBox.Text))
                {
                    MessageBox.Show("Istnieje juz taka kategoria!");
                    return;
                }

                if (!creator)
                {
                    Main_Classes.Budget.Instance.ListOfAdds.Add(new Changes(typeof(Category), Main_Classes.Budget.Instance.Categories.Keys.Max() + 2));
                    Main_Classes.Budget.Instance.Categories.Add(Main_Classes.Budget.Instance.Categories.Keys.Max() + 2,
                        new Category(CategoryNameTextBox.Text, CategoryNoteTextBox.Text, category));
                    Main_Classes.Budget.Instance.InsertCategories(_actualComboBox, type);
                    _actualComboBox.SelectedIndex = _actualComboBox.Items.Count - 1;
                }
                else
                {
                    _categories.Add(_categories.Keys.Max() + 1,
                        new Category(CategoryNameTextBox.Text, CategoryNoteTextBox.Text, category));
                    _creatorIdList = MakeBudgetWindow.InsertCategories(_actualComboBox, _categories, category);
                    _actualComboBox.SelectedIndex = _actualComboBox.Items.Count - 1;

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

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Budget.Main_Classes;
using Budget.New_Budget;
using Budget = Budget.Main_Classes.Budget;
using ComboBox = System.Windows.Controls.ComboBox;
using MessageBox = System.Windows.MessageBox;

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
        private bool _creatorType;
        private List<int> _creatorIdList; 
        public AddCategoryWindow(ComboBox actualcomboBox,Main_Classes.Budget.CategoryTypeEnum type)
        {
            InitializeComponent();
            _actualComboBox = actualcomboBox;
          //  _anotherComboBox = anotherComboBox;
            this.type = type;
          //  anotherType = type == Main_Classes.Budget.CategoryTypeEnum.PAYMENT ? Main_Classes.Budget.CategoryTypeEnum.SALARY : Main_Classes.Budget.CategoryTypeEnum.PAYMENT;
        }

        public AddCategoryWindow(Dictionary<int,Category> c,ComboBox actualComboBox,bool type,List<int> creatorList ) //konstruktor dla kreatora
        {
            InitializeComponent();
            _categories = c;
            _actualComboBox = actualComboBox;

            creator = true;
            _creatorIdList = creatorList;
            _creatorType = type;
        }

        private void AddCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CategoryNameTextBox.Text != "" && categoryTypes.SelectedIndex != -1)
            {
                if (Main_Classes.Budget.Instance.CheckCategory(CategoryNameTextBox.Text))
                {
                    MessageBox.Show("Isnieje juz taka kategoria!");
                    return;
                }
                bool category = categoryTypes.SelectedIndex == 1;

                if (!creator)
                {
                    Main_Classes.Budget.Instance.ListOfAdds.Add(new Changes(typeof(Category), Main_Classes.Budget.Instance.Categories.Last().Key + 1));
                    Main_Classes.Budget.Instance.Categories.Add(Main_Classes.Budget.Instance.Categories.Last().Key + 1,
                        new Category(CategoryNameTextBox.Text, CategoryNoteTextBox.Text, category));
                    Main_Classes.Budget.Instance.InsertCategories(_actualComboBox, type);
                    _actualComboBox.SelectedIndex = _actualComboBox.Items.Count - 1;
                }
                else
                {
                    _categories.Add(_categories.Last().Key + 1,
                        new Category(CategoryNameTextBox.Text, CategoryNoteTextBox.Text, category));
                    _creatorIdList = MakeBudgetWindow.InsertCategories(_actualComboBox, _categories, _creatorType);
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

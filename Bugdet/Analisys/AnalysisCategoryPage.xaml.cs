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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.DataVisualization.Charting;
using System.Globalization;
using Budget.Main_Classes;

namespace Budget.Analisys
{
    /// <summary>
    /// Interaction logic for AnalysisCategoryPage.xaml
    /// </summary>
    public partial class AnalysisCategoryPage : Page
    {
        private List<KeyValuePair<string, int>> data = new List<KeyValuePair<string, int>>();

        public AnalysisCategoryPage()
        {
            InitializeComponent();
            CheckBoxForCategories();

            SetData();
        }

        public void SetData()
        {
            DateTime actualMonth = DateTime.Today;
            DateTime previousMonth = DateTime.Today.AddMonths(-1);
            DateTime prev2Month = DateTime.Today.AddMonths(-2);
            DateTime prev3Month = DateTime.Today.AddMonths(-3);
            DateTime prev4Month = DateTime.Today.AddMonths(-4);
            DateTime prev5Month = DateTime.Today.AddMonths(-5);

            List<CategorySum> catTemp = new List<CategorySum>();

            foreach (var cat in CategoryComboBox.Items.Cast<CheckBox>().Where(cat => cat.IsChecked == true))
            {
                catTemp.Add(new CategorySum(Convert.ToInt32(cat.Name.Substring(1)), cat.Content.ToString()));
                LineSeries lineSeries = new LineSeries();
                lineSeries.Title = cat.Content;
                lineSeries.DependentValuePath = "Value";
                lineSeries.IndependentValuePath = "Key";
                lineSeries.ItemsSource = data;
                Chart.Series.Add(lineSeries);
            }

            foreach (KeyValuePair<int, Payment> pay in Main_Classes.Budget.Instance.Payments)
            {
                data.Clear();

                if (pay.Value.CompareDateTo(DateTime.Today) <= 0)
                {
                    if (pay.Value.GetType() != typeof(SinglePayment)) continue;

                    foreach (var _cat in catTemp)
                    {
                        if (_cat.catID == pay.Value.CategoryID)
                        {
                            var temp = (SinglePayment)pay.Value;

                            if (temp.Date.Month == actualMonth.Month && temp.Date.Year == actualMonth.Year)
                            {
                                _cat.actualMonth += (int)pay.Value.Amount;
                            }

                            if (temp.Date.Month == previousMonth.Month && temp.Date.Year == previousMonth.Year)
                            {
                                _cat.previousMonth += (int)pay.Value.Amount;
                            }

                            if (temp.Date.Month == prev2Month.Month && temp.Date.Year == prev2Month.Year)
                            {
                                _cat.prev2Month += (int)pay.Value.Amount;
                            }

                            if (temp.Date.Month == prev3Month.Month && temp.Date.Year == prev3Month.Year)
                            {
                                _cat.prev3Month += (int)pay.Value.Amount;
                            }

                            if (temp.Date.Month == prev4Month.Month && temp.Date.Year == prev4Month.Year)
                            {
                                _cat.prev4Month += (int)pay.Value.Amount;
                            }

                            if (temp.Date.Month == prev5Month.Month && temp.Date.Year == prev5Month.Year)
                            {
                                _cat.prev5Month += (int)pay.Value.Amount;
                            }
                        }
                    }
                }

            }

            foreach (CategorySum cat_ in catTemp)
            {
                data.Add(new KeyValuePair<string, int>(getMonthName(prev5Month.Month, prev5Month.Year), cat_.prev5Month));
                data.Add(new KeyValuePair<string, int>(getMonthName(prev4Month.Month, prev4Month.Year), cat_.prev4Month));
                data.Add(new KeyValuePair<string, int>(getMonthName(prev3Month.Month, prev3Month.Year), cat_.prev3Month));
                data.Add(new KeyValuePair<string, int>(getMonthName(prev2Month.Month, prev2Month.Year), cat_.prev2Month));
                data.Add(new KeyValuePair<string, int>(getMonthName(previousMonth.Month, previousMonth.Year), cat_.previousMonth));
                data.Add(new KeyValuePair<string, int>(getMonthName(actualMonth.Month, actualMonth.Year), cat_.actualMonth));

            }

            int i = 0;
            foreach (Series temp in Chart.Series)
            {
                List<KeyValuePair<string, int>> tempList = new List<KeyValuePair<string, int>>();
                for (int j = 0; j < 6; j++, i++)
                {
                    tempList.Add(data[i]);
                }
                ((LineSeries)temp).ItemsSource = tempList;
            }

        }

        private string getMonthName(int mon, int year)
        {
            string Month;
            Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mon);
            Month += " " + Convert.ToString(year);
            return Month;
        }

        private void CheckBoxForCategories()
        {
            foreach (var cat in Main_Classes.Budget.Instance.Categories)
            {
                CheckBox c;
                if (cat.Value.Name !="-")
                    c = new CheckBox { Content = cat.Value.Name, Name = "a" + cat.Key, IsChecked = true };
                else
                    c = new CheckBox { Content = cat.Value.Name, Name = "a" + cat.Key, IsChecked = false };
                CategoryComboBox.Items.Add(c);
            }
        }

        private class CategorySum
        {
            public int actualMonth = 0;
            public int previousMonth = 0;
            public int prev2Month = 0;
            public int prev3Month = 0;
            public int prev4Month = 0;
            public int prev5Month = 0;

            public int catID { get; set; }
            public string catName { get; set; }

            public CategorySum(int _catID, string _catName)
            {
                this.catID = _catID;
                this.catName = _catName;
            }
           
        }

        private void CategoryComboBox_DropDownClosed(object sender, EventArgs e)
        {
            Chart.Series.Clear();
            SetData();
        }
        
    }
}

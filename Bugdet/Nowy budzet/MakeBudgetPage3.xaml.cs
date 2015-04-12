using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Bugdet.Nowy_budzet
{
    /// <summary>
    /// Interaction logic for MakeBudgetPage3.xaml
    /// </summary>
    public partial class MakeBudgetPage3 : Page
    {
        public static Dictionary<int,PeriodPayment> _periodPayments = new Dictionary<int, PeriodPayment>(); 
      //  private int _salaries;
        public MakeBudgetPage3()
        {
            InitializeComponent();
            InsertDateTypes();
            InsertCategories();
        }

        //private void type1Radio_Checked(object sender, RoutedEventArgs e)
        //{
        //    if(DescLbl != null)
        //        DescLbl.Content = "Co ile dni jest wypłata";
        //}

        //private void type2Radio_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (DescLbl != null)
        //        DescLbl.Content = "W konkretny dzień";
        //}
        private void InsertDateTypes()
        {
            DateTypeBox.Items.Add("DZIEŃ");
            DateTypeBox.Items.Add("TYDZIEŃ");
            DateTypeBox.Items.Add("MIESIĄC");
            DateTypeBox.Items.Add("ROK");
        }
        private void addSalaryBtn_Click(object sender, RoutedEventArgs e)
        {
            if(SalaryName.Text != "" && SalaryValue.Text != "" && NumberOfTextBox.Text != "" && DateTypeBox.SelectedIndex != -1 && CategoryComboBox.SelectedIndex != -1 )
            {
                _periodPayments.Add(1,new PeriodPayment(CategoryComboBox.SelectedIndex + 1,
                    Double.Parse(SalaryValue.Text),
                    NoteTextBox.Text,
                    true,SalaryName.Text,
                    int.Parse(NumberOfTextBox.Text),
                    DateTypeBox.SelectedValue.ToString(),
                    StartDatePicker.DisplayDate,StartDatePicker.DisplayDate,
                    (EndDateCheckBox.IsEnabled == true ? EndDatePicker.DisplayDate : StartDatePicker.DisplayDate)));
                InfoLbl.Content = "Dodano";
                InfoLbl.Foreground = Brushes.Green;// ="#00FF0000";
                SalaryName.Text = "";
                SalaryValue.Text = "";
                NumberOfTextBox.Text = "";
            }
            else
            {
                MessageBox.Show("Uzupełnij wszystkie pola!");
                InfoLbl.Content = "Nie Dodano";
                InfoLbl.Foreground = Brushes.Red;// ="#FF000000";
            }
        }

        private void addedSalariesBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddedSalariesWindow(2).ShowDialog();
        }

        public void InsertCategories()
        {
            CategoryComboBox.Items.Clear();
            try
            {
                for (int i = 0; i < MakeBudgetWindow._categories.Count; i++)
                {
                    CategoryComboBox.Items.Add(MakeBudgetWindow._categories[i + 1].Name);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(MakeBudgetWindow._categories.Count + "");
                Console.WriteLine(MakeBudgetWindow._categories.ToArray());
            }
        }

        public Boolean CheckInfo()
        {
        //    try
        //    {
        //        for (int i = 0; i < _salaries; i++)
        //        {
        //        //    MakeBudgetWindow.Budgetstack.Push(BudgetList.ElementAt(i).Name); // String
        //        //    MakeBudgetWindow.Budgetstack.Push(BudgetList.ElementAt(i).Value); // String
        //        //    MakeBudgetWindow.Budgetstack.Push(BudgetList.ElementAt(i).Repeat); // String
        //        //    MakeBudgetWindow.Budgetstack.Push(BudgetList.ElementAt(i).Type); // int
        //        }
        //        if (_salaries != 0)
        //            MakeBudgetWindow.Budgetstack.Push(-1); // identyfikator periodSalary
        //        MakeBudgetWindow.Budgetstack.Push(_salaries);
        //        return true;
        //    }
        //    catch(InsufficientExecutionStackException)
        //    {
        //        return false;
        //    }
            MakeBudgetWindow._payments = _periodPayments;
            return true;
        }
        public Boolean BackToThisPage()
        {
        //    try
        //    {
        //        int _salaries = (int)MakeBudgetWindow.Budgetstack.Pop(); // ile pakietow
        //        if (_salaries != 0)
        //            MakeBudgetWindow.Budgetstack.Pop(); // identyfikator periodsalary
        //        for (int i = 0; i < _salaries; i++)
        //        {
        //            MakeBudgetWindow.Budgetstack.Pop();
        //            MakeBudgetWindow.Budgetstack.Pop();
        //            MakeBudgetWindow.Budgetstack.Pop();
        //            MakeBudgetWindow.Budgetstack.Pop();
        //        }
        //        return true;
        //    }
        //    catch(InsufficientExecutionStackException)
        //    {
        //        return false;
        //    }
            return true;
        }

        private void AddCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            new AddCategoryWindow().ShowDialog();
            InsertCategories();
        }

        private void EndDateCheckBox_OnChecked(object sender, RoutedEventArgs e)
        {
            EndDatePicker.IsEnabled = true;
        }

        private void EndDateCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            EndDatePicker.IsEnabled = false;
        }

        private void SalaryName_OnGotFocus(object sender, RoutedEventArgs e)
        {
            InfoLbl.Content = "";
        }
    }
}

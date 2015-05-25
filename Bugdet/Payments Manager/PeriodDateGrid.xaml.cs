using System;
using System.Windows;
using System.Windows.Controls;

namespace Budget.Payments_Manager
{
    /// <summary>
    /// Interaction logic for PeriodDateGrid.xaml
    /// </summary>
    public partial class PeriodDateGrid : UserControl
    {
        public PeriodDateGrid(Main_Classes.Budget.CategoryTypeEnum type)
        {
            InitializeComponent();
            ChangeNotes(type);
            EndDateEnableCheckBox.IsChecked = false;
            EndDatePicker.IsEnabled = false;
        }

        private void EndDateEnableCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            EndDatePicker.IsEnabled = true;
        }

        private void EndDateEnableCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
        {
            EndDatePicker.IsEnabled = false;
        }

        public DateTime EndDate
        {
            get 
            {
                if (EndDatePicker.Text == "")
                {
                    return DateTime.Now;
                }
                else
                {
                    return EndDatePicker.SelectedDate.Value;
                }
            }
        }

        public DateTime StartDate
        {
            get 
            {
                if (StartDatePicker.Text == "")
                {
                    return DateTime.Now;
                }
                else
                {
                    return StartDatePicker.SelectedDate.Value; 
                }
            }     
        }

        public ComboBox TypeOfBox
        {
            get 
            { 
                return TypeOfDayComboBox; 
            }
        }

        /// <summary>
        /// gets int from NumberOfTextBox
        /// </summary>
        public int NumberOf
        {
            get 
            { 
                return Convert.ToInt32(NumberOfTextBox.Text);
            }
        }

        /// <summary>
        /// gets Type of day from ComboBox
        /// </summary>
        public string TypeOfDay
        {
            get 
            { 
                return TypeOfDayComboBox.Text; 
            }
        }

        private void ChangeNotes(Main_Classes.Budget.CategoryTypeEnum type)
        {
            switch (type)
            {
                case Main_Classes.Budget.CategoryTypeEnum.PAYMENT:
                    InfoLbl.Content = "Opłata co:";
                    break;
                case Main_Classes.Budget.CategoryTypeEnum.SALARY:
                    InfoLbl.Content = "Przychód co:";
                    break;
                default:
                    break;
            }
        }

        public void ClearComponents()
        {
            NumberOfTextBox.Text = "";
            StartDatePicker.Text = "";
            EndDatePicker.Text = "";
            TypeOfDayComboBox.SelectedIndex = -1;
            EndDateEnableCheckBox.IsChecked = false;
        }
    }
}
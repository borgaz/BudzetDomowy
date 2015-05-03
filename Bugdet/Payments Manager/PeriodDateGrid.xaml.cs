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
            get { return StartDatePicker.SelectedDate.Value; }
        }

        public DateTime StartDate
        {
            get { return StartDatePicker.SelectedDate.Value; }     
        }

        public ComboBox TypeofBox
        {
            get { return TypeOfDayComboBox; }
        }
        public bool EndDateChecker
        {
            get { return EndDateEnableCheckBox.IsChecked == true ; }
        }
        /// <summary>
        /// gets int from NumberOfTextBox
        /// </summary>
        public int NumberOf
        {
            get { return Convert.ToInt32(NumberOfTexBox.Text); }
        }
        /// <summary>
        /// gets Type of day from ComboBox
        /// </summary>
        public string TypeOfDay
        {
            get { return TypeOfDayComboBox.Text; }
        }
        private void ChangeNotes(Main_Classes.Budget.CategoryTypeEnum type)
        {
            switch (type)
            {
                case Main_Classes.Budget.CategoryTypeEnum.PAYMENT:
                    InfoLbl.Content = "Opłata co:";
                    break;
                case Main_Classes.Budget.CategoryTypeEnum.SALARY:
                    InfoLbl.Content = "przychód co:";
                    break;
                default:
                    break;
            }
        }
        public void ClearComponents()
        {
            NumberOfTexBox.Text = "";
            TypeOfDayComboBox.SelectedIndex = -1;
            EndDateEnableCheckBox.IsChecked = false;
        }
    }
}

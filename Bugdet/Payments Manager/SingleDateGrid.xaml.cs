using System;
using System.Windows.Controls;

namespace Budget.Payments_Manager
{
    /// <summary>
    /// Interaction logic for SingleDateGrid.xaml
    /// </summary>
    public partial class SingleDateGrid : UserControl
    {
        public SingleDateGrid(Main_Classes.Budget.CategoryTypeEnum type)
        {
            InitializeComponent();
            ChangeNotes(type);
        }
        /// <summary>
        /// gets Selected Date of DatePicker
        /// </summary>
        public DateTime SelectedDate
        {
            get 
            {
                if (SingleDatePicker.Text == "")
                {
                    return DateTime.Now;
                }
                else
                {
                    return SingleDatePicker.SelectedDate.Value;
                }
            }
        }

        private void ChangeNotes(Main_Classes.Budget.CategoryTypeEnum type)
        {
            switch (type)
            {
                case Main_Classes.Budget.CategoryTypeEnum.PAYMENT:
                    InfoLabel.Content = "Data wydatku:";
                    break;
                case Main_Classes.Budget.CategoryTypeEnum.SALARY:
                    InfoLabel.Content = "Data przychodu:";
                    break;
                default:
                    break;
            }
        }
    }
}
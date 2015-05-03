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
            get { return SingleDatePicker.SelectedDate.Value; }
        }

        private void ChangeNotes(Main_Classes.Budget.CategoryTypeEnum type)
        {
            switch (type)
            {
                case Main_Classes.Budget.CategoryTypeEnum.PAYMENT:
                    InfoLabel.Content = "Data Wydatku:";
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

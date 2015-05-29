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

namespace Budget.WelcomePage
{
    /// <summary>
    /// Interaction logic for PDFOptionsWindow.xaml
    /// </summary>
    public partial class PDFOptionsWindow : Window
    {
        private DateTime startDate = DateTime.Today;
        private DateTime endDate = DateTime.Today;
        private Boolean generate = false;
        private static PDFOptionsWindow _instance;
        private PDFOptionsWindow()
        {
            InitializeComponent();
            StartDatePicker.SelectedDate = DateTime.Today.AddMonths(-1);
            EndDatePicker.SelectedDate = DateTime.Today;
        }

        public static PDFOptionsWindow Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PDFOptionsWindow();
                }
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        private void PDFButton_Click(object sender, RoutedEventArgs e)
        {
            startDate = StartDatePicker.SelectedDate.Value;
            endDate = EndDatePicker.SelectedDate.Value;
            generate = true;
            this.Close();
        }

        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
        }

        public Boolean Generate
        {
            get
            {
                return generate;
            }
        }
    }
}

using System;
using System.Windows;

namespace Budget.WelcomePage
{
    /// <summary>
    /// Interaction logic for PDFOptionsWindow.xaml
    /// </summary>
    public partial class PDFOptionsWindow : Window
    {
        private static PDFOptionsWindow _instance;
        private DateTime startDate = DateTime.Today;
        private DateTime endDate = DateTime.Today;
        private Boolean generate = false;
        
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
            if (startDate < endDate)
            {
                generate = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Wybierz prawidłowy zakres!");
            }
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
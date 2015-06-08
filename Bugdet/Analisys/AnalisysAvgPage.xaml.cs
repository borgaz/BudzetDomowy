using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Budget.Analisys
{
    /// <summary>
    /// Interaction logic for AnalisysAvgPage.xaml
    /// </summary>
    public partial class AnalisysAvgPage : Page
    {
        private BalanceChart var;
        public AnalisysAvgPage()
        {
            InitializeComponent();
            DateOneCalendar.DisplayDate = DateTime.Now;
            DateTwoCalendar.DisplayDate = DateTime.Now.AddMonths(-1);
            DateThreeCalendar.DisplayDate = DateTime.Now.AddMonths(-2);
            SetDates(DateOneCalendar.DisplayDate, DateTwoCalendar.DisplayDate, DateThreeCalendar.DisplayDate);
        }

        private void SetDates(DateTime one, DateTime two, DateTime three)
        {
            var = new BalanceChart(one, two, three);
            DataContext = var;
        }

        private void DateCalendar_OnDisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            SetDates(DateOneCalendar.DisplayDate, DateTwoCalendar.DisplayDate, DateThreeCalendar.DisplayDate);
        }

    }
}

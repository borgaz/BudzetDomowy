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
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace Budget.Analisys
{
    /// <summary>
    /// Interaction logic for AnalisysAvgPage.xaml
    /// </summary>
    public partial class AnalisysAvgPage : Page
    {
        private BalanceChart var;
        private bool user;
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
            if (!user)
                return;
            if (((Calendar)sender).Equals(DateOneCalendar))
                if (DateOneCalendar.DisplayDate.Month == DateTwoCalendar.DisplayDate.Month)
                    DateOneCalendar.DisplayDate = DateOneCalendar.DisplayDate.AddMonths(1);
            if (((Calendar)sender).Equals(DateTwoCalendar))
            {
                if (DateTwoCalendar.DisplayDate.Month == DateThreeCalendar.DisplayDate.Month) { DateTwoCalendar.DisplayDate = DateTwoCalendar.DisplayDate.AddMonths(1); }
                else if (DateTwoCalendar.DisplayDate.Month == DateOneCalendar.DisplayDate.Month) { DateTwoCalendar.DisplayDate = DateTwoCalendar.DisplayDate.AddMonths(-1); }
            }
            if (((Calendar)sender).Equals(DateThreeCalendar))
                if (DateThreeCalendar.DisplayDate.Month == DateTwoCalendar.DisplayDate.Month)
                    DateThreeCalendar.DisplayDate = DateThreeCalendar.DisplayDate.AddMonths(-1);
            if (((Calendar)sender).DisplayDate.Month > DateTime.Now.Month)
                System.Windows.MessageBox.Show(
                    "Przyszły miesiąc zawiera przewidywaną sumę wydatków i przychodów wynikającą z analizy dotychczasowych danych.\nNie należy traktować tego jako wiarygodnego źródła informacji");

            SetDates(DateOneCalendar.DisplayDate, DateTwoCalendar.DisplayDate, DateThreeCalendar.DisplayDate);
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (!user)
                user = true;
        }
    }
}
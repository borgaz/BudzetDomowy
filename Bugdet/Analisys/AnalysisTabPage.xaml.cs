using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using Budget.Main_Classes;
using System.Linq;

namespace Budget.Analisys
{
    /// <summary>
    /// Interaction logic for AnalysisTabPage.xaml
    /// </summary>
    public partial class AnalysisTabPage : Page
    {
        //CategoryChart var = new CategoryChart(DateTime.MinValue, DateTime.Today);
        CategoryChart var = new CategoryChart(DateTime.Today.AddMonths(-1), DateTime.Today);

        public AnalysisTabPage()
        {
            InitializeComponent();

            startDate.Text = DateTime.Today.AddMonths(-1).Date.ToString();
            endDate.Text = DateTime.Today.Date.ToString();

            this.DataContext = var;
        }

        private void startDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (startDate.SelectedDate.Value.CompareTo(endDate.SelectedDate.Value) >= 0)
            {
                startDate.Text = endDate.SelectedDate.Value.AddDays(-1).Date.ToString();
            }
            var = new CategoryChart(startDate.SelectedDate.Value, endDate.SelectedDate.Value);
            this.DataContext = var;
        }

        private void endDate_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (endDate.SelectedDate.Value.CompareTo(startDate.SelectedDate.Value) <= 0)
            {
                endDate.Text = startDate.SelectedDate.Value.AddDays(1).Date.ToString();
            }
            var = new CategoryChart(startDate.SelectedDate.Value, endDate.SelectedDate.Value);
            this.DataContext = var;
        }
    }

}

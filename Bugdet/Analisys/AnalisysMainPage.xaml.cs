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

namespace Budget.Analisys
{
    /// <summary>
    /// Interaction logic for AnalisysMainPage.xaml
    /// </summary>
    public partial class AnalisysMainPage : Page
    {
        private AnalysisTabPage _chartTab;
        private AnalisysAvgPage _balanceTab;
        private AnalysisAvgTrendline _trendlineTab;
        private AnalysisCategoryPage _categoryPage;

        public AnalisysMainPage()
        {
            InitializeComponent();
        }

        private void ChartsTab_OnLoaded(object sender, RoutedEventArgs e)
        {
            _chartTab = new AnalysisTabPage();
            _balanceTab = new AnalisysAvgPage();
            _trendlineTab = new AnalysisAvgTrendline();
            _categoryPage = new AnalysisCategoryPage();
            CategoryPaymentFrame.Content = _chartTab;
            MonthAvgFrame.Content = _balanceTab;
            LineChartFrame.Content = _trendlineTab;
            CategoryTabFrame.Content = _categoryPage;
        }
    }
}

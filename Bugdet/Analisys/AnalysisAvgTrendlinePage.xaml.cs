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
using System.Windows.Controls.DataVisualization.Charting;

namespace Budget.Analisys
{
    /// <summary>
    /// Interaction logic for AnalysisAvgTrendline.xaml
    /// </summary>
    public partial class AnalysisAvgTrendline : Page
    {
        private List<KeyValuePair<string, int>> data = new List<KeyValuePair<string, int>>();

        public AnalysisAvgTrendline()
        {
            InitializeComponent();

            //Tymczasowo
            data.Add(new KeyValuePair<string, int>("Luty 2015", 6580));
            data.Add(new KeyValuePair<string, int>("Marzec 2015", 7130));
            data.Add(new KeyValuePair<string, int>("Kwiecień 2015", 6890));
            data.Add(new KeyValuePair<string, int>("Maj 2015", 7500));
            data.Add(new KeyValuePair<string, int>("Czerwiec 2015", 7740));

            ((LineSeries)Chart.Series[0]).ItemsSource = data;

        }

    }
}

using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Budget.Analisys
{
    /// <summary>
    /// Interaction logic for AnalysisTabPage.xaml
    /// </summary>
    public partial class AnalysisTabPage : Page
    {
        TestChart var = new TestChart();
        public AnalysisTabPage()
        {
            InitializeComponent();
            this.DataContext = var;
        }
    }

}

using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Budget.Analisys
{
    /// <summary>
    /// Interaction logic for AnalysisTabPage.xaml
    /// </summary>
    public partial class AnalysisTabPage : Page
    {
        CategoryChart var = new CategoryChart();
        public AnalysisTabPage()
        {
            InitializeComponent();
            this.DataContext = var;
        }
    }

}

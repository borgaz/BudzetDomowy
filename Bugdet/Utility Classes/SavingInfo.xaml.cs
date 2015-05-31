using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Budget.Utility_Classes
{
    /// <summary>
    /// Interaction logic for SavingInfo.xaml
    /// </summary>
    public partial class SavingInfo : Window
    {
        private bool running = false;
        public SavingInfo()
        {
            InitializeComponent();
            SavingProgress.Maximum = GetChangesCount();
            running = true;
            new Thread(ShowProgress).Start();
            new Thread(Main_Classes.Budget.Instance.Dump).Start();
        }
        private static int GetChangesCount()
        {
            int result = 0;
            result += Main_Classes.Budget.Instance.ListOfAdds.Count;
            result += Main_Classes.Budget.Instance.ListOfDels.Count;
            result += Main_Classes.Budget.Instance.ListOfEdits.Count;
            return result;
        }
        private void ShowProgress()
        {
            while(running)
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
                {
                    SavingProgress.Value = GetChangesCount();
                    Thread.Sleep(500);
                }));
                if (GetChangesCount() == 0)
                    running = false;
            }
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(Close));
        }
    }
}

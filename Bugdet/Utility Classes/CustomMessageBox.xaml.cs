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

namespace Budget.Utility_Classes
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public enum MessageBoxType
        {
            Ok,
            YesNo,
            YesNoCanel
        };
        public CustomMessageBox(MessageBoxType type,String text) // TODO: Dokonczyc
        {
            InitializeComponent();
            LoadButtons(type);
            Text(text);
        }

        private void LoadButtons(MessageBoxType type)
        {
            switch (type)
            {
                case MessageBoxType.Ok:
                    break;
                case MessageBoxType.YesNo:
                    break;
                case MessageBoxType.YesNoCanel:
                    break;
            }
        }

        public void Text(String text)
        {
            MessageBoxText.Text = text;
        }

        private void YesButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void NoButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}

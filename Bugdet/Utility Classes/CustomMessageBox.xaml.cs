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

        private readonly MessageBoxType windowType;
        public int result;
        public CustomMessageBox(MessageBoxType type,String text) // 
        {
            InitializeComponent();
            windowType = type;
            LoadButtons(windowType);
            Text(text);
        }

        private void LoadButtons(MessageBoxType type)
        {
            switch (type)
            {
                case MessageBoxType.Ok:
                    CancelButton.Content = "Ok";
                    YesButton.Visibility = Visibility.Hidden;
                    NoButton.Visibility = Visibility.Hidden;
                    break;
                case MessageBoxType.YesNo:
                    CancelButton.Visibility = Visibility.Hidden;
                    YesButton.Content = "Tak";
                    NoButton.Content = "Nie";
                    break;
                case MessageBoxType.YesNoCanel:
                    YesButton.Content = "Anuluj";
                    CancelButton.Content = "Wyloguj";
                    NoButton.Content = "Wyjdź";
                    break;
                    
            }
        }

        public void Text(String text)
        {
            MessageBoxText.Text = text;
        }
        private void YesButton_OnClick(object sender, RoutedEventArgs e)
        {
            switch(windowType)
            {
                case MessageBoxType.Ok:
                    break;
                case MessageBoxType.YesNo:
                    break;
                case MessageBoxType.YesNoCanel:
                    result = 1;
                    this.Close();
                    break;
            }
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            switch (windowType)
            {
                case MessageBoxType.Ok:
                    break;
                case MessageBoxType.YesNo:
                    break;
                case MessageBoxType.YesNoCanel:
                    result = 2;
                    this.Close();
                    break;
            }
        }

        private void NoButton_OnClick(object sender, RoutedEventArgs e)
        {
            switch (windowType)
            {
                case MessageBoxType.Ok:
                    break;
                case MessageBoxType.YesNo:
                    break;
                case MessageBoxType.YesNoCanel:
                    result = 3;
                    this.Close();
                    break;
            }
        }
    }
}

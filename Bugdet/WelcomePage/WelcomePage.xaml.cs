using System.Windows.Controls;
using System.Collections.Generic;

namespace Budget.WelcomePage
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {

        public WelcomePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            providedPaymentsDataGrid.ItemsSource = createDataForProvidedPaymentsDataGrid();
        }

        private List<ProvidedPayment> createDataForProvidedPaymentsDataGrid()
        {
            List<ProvidedPayment> providedPayments = new List<ProvidedPayment>();
            foreach(Main_Classes.Payment payment in Main_Classes.Budget.Instance.Payments.Values)
            {
                if (payment.GetType() == typeof(Main_Classes.PeriodPayment))
                {
                    Main_Classes.PeriodPayment pP = (Main_Classes.PeriodPayment) payment;
                    if (pP.countNextDate() < SettingsPage.Settings.Instance.LastDateToShow() && pP.Amount < SettingsPage.Settings.Instance.AmountTo && pP.Amount > SettingsPage.Settings.Instance.AmountOf)
                    {
                        providedPayments.Add(new ProvidedPayment(pP.Name, pP.Amount, "Okresowy", pP.countNextDate(), pP.Type));
                    }
                }
                else
                {
                    Main_Classes.SinglePayment sP = (Main_Classes.SinglePayment) payment;
                    if (sP.CompareDate() >= 0 && sP.Amount < SettingsPage.Settings.Instance.AmountTo && sP.Amount > SettingsPage.Settings.Instance.AmountOf && sP.Date < SettingsPage.Settings.Instance.LastDateToShow())
                    {
                        providedPayments.Add(new ProvidedPayment(sP.Name, sP.Amount, "Pojedynczy", sP.Date, sP.Type));
                    }    
                }
            }

            providedPayments.Sort();
            if (providedPayments.Count > SettingsPage.Settings.Instance.NumberOfRow)
            {
                providedPayments.RemoveRange(SettingsPage.Settings.Instance.NumberOfRow, providedPayments.Count - SettingsPage.Settings.Instance.NumberOfRow);
            }
           
            return providedPayments;
        }
    }
}
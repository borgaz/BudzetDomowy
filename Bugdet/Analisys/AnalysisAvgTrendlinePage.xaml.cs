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
using System.Globalization;
using Budget.Main_Classes;

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
            setData();
            ((LineSeries)Chart.Series[0]).ItemsSource = data;
        }

        private void setData()
        {
            DateTime actualMonth = DateTime.Today;
            DateTime previousMonth = DateTime.Today.AddMonths(-1);
            DateTime prevtwoMonth = DateTime.Today.AddMonths(-2);
            DateTime prevthreeMonth = DateTime.Today.AddMonths(-3);
            DateTime futureMonth = DateTime.Today.AddMonths(1);
            DateTime future2Month = DateTime.Today.AddMonths(2);

            int sumaM = 0;
            int sumpM = 0;
            int sump2M = 0;
            int sump3M = 0;
            int sumF = 0;
            int sum2F = 0;
            int sumPredF1_1 = 0;
            int sumPredF1_2 = 0;
            int sumPredF2_1 = 0;
            int sumPredF2_2 = 0;
            int sumPredaM1_1 = 0;
            int sumPredaM1_2 = 0;

            foreach (KeyValuePair<int, Payment> pay in Main_Classes.Budget.Instance.Payments)
            {
                if (pay.Value.GetType() != typeof(SinglePayment)) continue;

                var temp = (SinglePayment)pay.Value;

                if (temp.Date.Month == actualMonth.Month && temp.Date.Year == actualMonth.Year - 1)
                {
                    if (temp.Type == false)
                    {
                        sumPredaM1_1 += (int)temp.Amount;
                    }
                    else
                    {
                        sumPredaM1_1 -= (int)temp.Amount;
                    }       
                }

                if (temp.Date.Month == actualMonth.Month && temp.Date.Year == actualMonth.Year - 2)
                {
                    if (temp.Type == false)
                    {
                        sumPredaM1_2 += (int)temp.Amount;
                    }
                    else
                    {
                        sumPredaM1_2 -= (int)temp.Amount;
                    }
                }

                if (temp.Date.Month == previousMonth.Month && temp.Date.Year == previousMonth.Year)
                {
                    if (temp.Type == false)
                    {
                        sumpM += (int)temp.Amount;
                    }
                    else
                    {
                        sumpM -= (int)temp.Amount;
                    }  
                }

                if (temp.Date.Month == prevtwoMonth.Month && temp.Date.Year == prevtwoMonth.Year)
                {
                    if (temp.Type == false)
                    {
                        sump2M += (int)temp.Amount;
                    }
                    else
                    {
                        sump2M -= (int)temp.Amount;
                    }  
                }

                if (temp.Date.Month == prevthreeMonth.Month && temp.Date.Year == prevthreeMonth.Year)
                {
                    if (temp.Type == false)
                    {
                        sump3M += (int)temp.Amount;
                    }
                    else
                    {
                        sump3M -= (int)temp.Amount;
                    }  
                }

                if (temp.Date.Month == futureMonth.Month && temp.Date.Year == futureMonth.Year)
                {
                    if (temp.Type == false)
                    {
                        sumF += (int)temp.Amount;
                    }
                    else
                    {
                        sumF -= (int)temp.Amount;
                    }
                }

                if (temp.Date.Month == future2Month.Month && temp.Date.Year == future2Month.Year)
                {
                    if (temp.Type == false)
                    {
                        sum2F += (int)temp.Amount;
                    }
                    else
                    {
                        sum2F -= (int)temp.Amount;
                    }
                }

                if (temp.Date.Month == futureMonth.Month && temp.Date.Year == futureMonth.Year - 1)
                {
                    if (temp.Type == false)
                    {
                        sumPredF1_1 += (int)temp.Amount;
                    }
                    else
                    {
                        sumPredF1_1 -= (int)temp.Amount;
                    }
                }

                if (temp.Date.Month == future2Month.Month && temp.Date.Year == future2Month.Year - 1)
                {
                    if (temp.Type == false)
                    {
                        sumPredF2_1 += (int)temp.Amount;
                    }
                    else
                    {
                        sumPredF2_1 -= (int)temp.Amount;
                    }
                }

                if (temp.Date.Month == futureMonth.Month && temp.Date.Year == futureMonth.Year - 2)
                {
                    if (temp.Type == false)
                    {
                        sumPredF1_2 += (int)temp.Amount;
                    }
                    else
                    {
                        sumPredF1_2 -= (int)temp.Amount;
                    }
                }

                if (temp.Date.Month == future2Month.Month && temp.Date.Year == future2Month.Year - 2)
                {
                    if (temp.Type == false)
                    {
                        sumPredF2_2 += (int)temp.Amount;
                    }
                    else
                    {
                        sumPredF2_2 -= (int)temp.Amount;
                    }
                }

            }

            int tempCount1 = 4;
            if (sump2M == 0)
                tempCount1--;
            if (sumPredF1_1 == 0)
                tempCount1--;
            if (sumPredF1_2 == 0)
                tempCount1--;

            int tempCount2 = 4;
            if (sump2M == 0)
                tempCount2--;
            if (sumPredF2_1 == 0)
                tempCount2--;
            if (sumPredF2_2 == 0)
                tempCount2--;

            int tempCount3 = 4;
            if (sumPredaM1_1 == 0)
                tempCount3--;
            if (sumPredaM1_2 == 0)
                tempCount3--;
            if (sump2M == 0)
                tempCount3--;

            sumF += ((sumPredF1_1 + sumPredF1_2 + sumpM + sump2M) / tempCount1);
            sum2F += ((sumPredF2_1 + sumPredF2_2 + sumpM + sump2M) / tempCount2);
            sumaM += ((sumPredaM1_1 + sumPredaM1_2 + sumpM + sump2M) / tempCount3);

            data.Add(new KeyValuePair<string, int>(getMonthName(prevthreeMonth.Month, prevthreeMonth.Year), sump3M));
            data.Add(new KeyValuePair<string, int>(getMonthName(prevtwoMonth.Month, prevtwoMonth.Year), sump2M));
            data.Add(new KeyValuePair<string, int>(getMonthName(previousMonth.Month, previousMonth.Year), sumpM));
            data.Add(new KeyValuePair<string, int>(getMonthName(actualMonth.Month, actualMonth.Year), sumaM)); 
            data.Add(new KeyValuePair<string, int>(getMonthName(futureMonth.Month, futureMonth.Year), sumF));
            data.Add(new KeyValuePair<string, int>(getMonthName(future2Month.Month, future2Month.Year), sum2F));
        }

        private string getMonthName(int mon, int year)
        {
            string Month;
            Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mon);
            Month += " " + Convert.ToString(year);
            return Month;
        }

    }
}

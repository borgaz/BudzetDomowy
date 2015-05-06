using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.SettingsPage
{
    public class Settings
    {
        private static Settings instance;
        /*Wyswietlanie przyszlych platnosci*/
        /*1. z jakiego okresu: */
        private String period; // co jaki czas (dzien/tydzine/miesiac itp)
        private int frequency; // co ile "periodow" (dni/tygodni itp)
        /*2. jaka kwota: */
        private double amountOf; // kwota od
        private double amountTo; // kwota do
        /*3. liczba wyswietlanych rekordow: */
        private int numberOfRow; // ile przyszlych rekordow wyswietlamy

        private Settings() // pozniej trzeba dorobic wczytywanie ustawien z pliku/bazy.
        {
            period = "MIESIĄC";
            frequency = 6;
            amountOf = 0;
            amountTo = Double.MaxValue;
            numberOfRow = 20;
        }

        public DateTime LastDateToShow()
        {
            if (period == "MIESIĄC")
                return DateTime.Now.AddMonths(frequency);
            else if (period == "DZIEŃ")
                return DateTime.Now.AddDays(frequency);
            else if (period == "TYDZIEŃ")
                return DateTime.Now.AddDays(7 * frequency);
            else if (period == "ROK")
                return DateTime.Now.AddYears(frequency);
            else
                return DateTime.Now;
        }

        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Settings();
                }
                return instance;
            } 
        }

        public String Period
        {
            get
            {
                return period;
            }
            set
            {
                period = value;
            }
        }

        public int Frequency
        {
            get
            {
                return frequency;
            }
            set
            {
                frequency = value;
            }
        }

        public int NumberOfRow
        {
            get
            {
                return numberOfRow;
            }
            set
            {
                numberOfRow = value;
            }
        }

        public double AmountOf
        {
            get
            {
                return amountOf;
            }
            set
            {
                amountOf = value;
            }
        }

        public double AmountTo
        {
            get
            {
                return amountTo;
            }
            set
            {
                amountTo = value;
            }
        }
    }
}
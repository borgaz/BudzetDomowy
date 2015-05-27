using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Budget.SettingsPage
{
    [Serializable]
    public sealed class Settings
    {
        private static Settings instance;

        /*Wyswietlanie przyszlych platnosci*/
        /*1. z jakiego okresu: */
        private String PP_period; // co jaki czas (dzien/tydzine/miesiac itp)
        private int PP_frequency; // co ile "periodow" (dni/tygodni itp)
        /*2. jaka kwota: */
        private double PP_amountOf; // kwota od
        private double PP_amountTo; // kwota do
        /*3. liczba wyswietlanych rekordow: */
        private int PP_numberOfRow; // ile przyszlych rekordow wyswietlamy

        /*Wyswietlanie krotkiej historii*/
        /*1. z jakiego okresu: */
        private String SH_period; // co jaki czas (dzien/tydzine/miesiac itp)
        private int SH_frequency; // co ile "periodow" (dni/tygodni itp)
        /*2. jaka kwota: */
        private double SH_amountOf; // kwota od
        private double SH_amountTo; // kwota do
        /*3. liczba wyswietlanych rekordow: */
        private int SH_numberOfRow; // ile przyszlych rekordow wyswietlamy
        private Boolean serializable;

        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    if (! DeserializeFromXml())
                    {
                        instance = new Settings();
                    }
                }

                return instance;
            }
            set
            {
                instance = value;
            }

        }

        private Settings() // pozniej trzeba dorobic wczytywanie ustawien z pliku/bazy.
        {
            PP_period = "MIESIĄC";
            PP_frequency = 1;
            PP_amountOf = 0;
            PP_amountTo = Main_Classes.Budget.Instance.MaxAmount;
            PP_numberOfRow = 20;

            SH_period = "MIESIĄC";
            SH_frequency = 1;
            SH_amountOf = 0;
            SH_amountTo = Main_Classes.Budget.Instance.MaxAmount;
            SH_numberOfRow = 20;

            serializable = false;
        }

        public string SerializeToXml()
        {
            serializable = true;
            StreamWriter writer = new StreamWriter(Main_Classes.Budget.Instance.Name + "Settings.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            serializer.Serialize(writer, this);
            writer.Close();
            return writer.ToString();
        }

        private static bool DeserializeFromXml()
        {
            try
            {
                StreamReader reader = new StreamReader(Main_Classes.Budget.Instance.Name + "Settings.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                instance = (Settings)serializer.Deserialize(reader);
                reader.Close();
                return true;
            }
            catch(Exception)
            { 
                return false; 
            }    
        }

        public DateTime PP_LastDateToShow()
        {
            if (PP_period == "MIESIĄC")
                return DateTime.Today.AddMonths(PP_frequency);
            else if (PP_period == "DZIEŃ")
                return DateTime.Today.AddDays(PP_frequency);
            else if (PP_period == "TYDZIEŃ")
                return DateTime.Today.AddDays(7 * PP_frequency);
            else if (PP_period == "ROK")
                return DateTime.Today.AddYears(PP_frequency);
            else
                return DateTime.Today;
        }

        public DateTime SH_LastDateToShow()
        {
            if (SH_period == "MIESIĄC")
                return DateTime.Today.AddMonths(-1 * SH_frequency); 
            else if (SH_period == "DZIEŃ")
                return DateTime.Today.AddDays(-1 * SH_frequency);
            else if (SH_period == "TYDZIEŃ")
                return DateTime.Today.AddDays(-7 * SH_frequency);
            else if (SH_period == "ROK")
                return DateTime.Today.AddYears(-1 * SH_frequency);
            else
                return DateTime.Today;
        }

        // Nizej sa juz tylko wlasciwosci(settery i gettery)

        public String PP_Period
        {
            get
            {
                return PP_period;
            }
            set
            {
                PP_period = value;
            }
        }

        public int PP_Frequency
        {
            get
            {
                return PP_frequency;
            }
            set
            {
                PP_frequency = value;
            }
        }

        public int PP_NumberOfRow
        {
            get
            {
                return PP_numberOfRow;
            }
            set
            {
                PP_numberOfRow = value;
            }
        }

        public double PP_AmountOf
        {
            get
            {
                return PP_amountOf;
            }
            set
            {
                PP_amountOf = value;
            }
        }

        public double PP_AmountTo
        {
            get
            {
                return PP_amountTo;
            }
            set
            {
                PP_amountTo = value;
            }
        }

        public String SH_Period
        {
            get
            {
                return SH_period;
            }
            set
            {
                SH_period = value;
            }
        }

        public int SH_Frequency
        {
            get
            {
                return SH_frequency;
            }
            set
            {
                SH_frequency = value;
            }
        }

        public int SH_NumberOfRow
        {
            get
            {
                return SH_numberOfRow;
            }
            set
            {
                SH_numberOfRow = value;
            }
        }

        public double SH_AmountOf
        {
            get
            {
                return SH_amountOf;
            }
            set
            {
                SH_amountOf = value;
            }
        }

        public double SH_AmountTo
        {
            get
            {
                return SH_amountTo;
            }
            set
            {
                SH_amountTo = value;
            }
        }
        
        public Boolean Serializable
        {
            get
            {
                return serializable;
            }
        }
    }
}
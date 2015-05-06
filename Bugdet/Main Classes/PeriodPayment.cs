using System;

namespace Budget.Main_Classes
{
    public class PeriodPayment : Payment
    {
        private DateTime startDate; // poczatkowa data
        private String period; // co jaki czas (dzien/tydzine/miesiac itp)
        private int frequency; // co ile dni/tygodni itp
        private DateTime lastUpdate; // data ostatniego wystapienia
        private DateTime endDate; // koncowa data

        public override string ToString()
        {
            return "START_DATE: " + startDate + ", PERIOD: " + period + ", FREQUENCY: " + frequency + ", LAST_UPDATE: " + lastUpdate
                + ", END_DATE: " + endDate + ", BASE_CATEGORY_ID: " + base.CategoryID + ", BASE_AMOUNT: " + base.Amount 
                + ", BASE_NAME: " + base.Name + ", BASE_TYPE: " + base.Type + ", BASE_NOTE: " + base.Note + "\n";
        }

        public PeriodPayment(int categoryID, double amount, String note, bool type, String name, int frequency, String period, DateTime lastUpdate, DateTime startDate, DateTime endDate)
                : base (categoryID, amount, note, type, name)
        {
            this.lastUpdate = lastUpdate;
            this.startDate = startDate;
            this.endDate = endDate;
            this.frequency = frequency;
            this.period = period;
        }

        public int Frequency
        {
            get
            {
                return frequency;
            }
        }

        public String Period
        {
            get
            {
                return period;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
        }

        public DateTime LastUpdate
        {
            get
            {
                return lastUpdate;
            }

        }

        override public int CompareDate()
        {
            return DateTime.Compare(this.lastUpdate, DateTime.Now);
            //Mniej niż zero - lastUpdate jest wcześniejsza niż Now.
            //Zero - lastUpdate jest taka sama jak Now.
            //Większe od zera - lastUpdate jest późniejsza niż Now. 
        }

       public DateTime countNextDate()
        {
            if (period == "MIESIĄC")
                return lastUpdate.AddMonths(frequency);
            else if (period == "DZIEŃ")
                return lastUpdate.AddDays(frequency);
            else if (period == "TYDZIEŃ")
                return lastUpdate.AddDays(7 * frequency);
            else if (period == "ROK")
                return lastUpdate.AddYears(frequency);
            else
                return DateTime.Now;
        }
    }
}
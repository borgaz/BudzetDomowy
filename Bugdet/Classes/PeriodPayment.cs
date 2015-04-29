using System;

namespace Budget.Classes
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
    }
}
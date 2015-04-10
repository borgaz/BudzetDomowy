using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugdet
{
    public class PeriodPayment : Payment
    {
        private DateTime startDate;
        private String period;      
        private int frequency;
        private DateTime lastUpdate;
        private DateTime endDate;

        public PeriodPayment(int categoryID, double amount, String note, int type, String name, 
            int frequency, String period, DateTime lastUpdate, DateTime startDate, DateTime endDate)
        {
            this.lastUpdate = lastUpdate;
            this.startDate = startDate;
            this.endDate = endDate;
            this.frequency = frequency;
            this.period = period;
        }

        // default constructor
        public PeriodPayment() 
            : base ()
        {
            lastUpdate = DateTime.MinValue;
            startDate = DateTime.MinValue;
            frequency = 0;
            period = "";
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
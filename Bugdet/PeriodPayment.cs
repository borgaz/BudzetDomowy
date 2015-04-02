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

        public PeriodPayment(int ID, int categoryID, double amount, String note, String type, String name, int frequency, String period, DateTime lastUpdate, DateTime startDate)
            : base(ID, categoryID, amount, note, type, name)
        {
            this.lastUpdate = lastUpdate;
            this.startDate = startDate;
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
            set
            {
                frequency = value;
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

        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return lastUpdate;
            }
            set
            {
                lastUpdate = value;
            }
        }
    }
}
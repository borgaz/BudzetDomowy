using System;

namespace Bugdet
{
    public class PeriodPayment : Payment
    {
        private DateTime _startDate;
        private String _period;      
        private int _frequency;
        private DateTime _lastUpdate;
        private DateTime _endDate;

        public PeriodPayment(int categoryId, double amount, String note, int type, String name, 
            int frequency, String period, DateTime lastUpdate, DateTime startDate, DateTime endDate)
            : base (categoryId, amount, note, type, name)
        {
            this._lastUpdate = lastUpdate;
            this._startDate = startDate;
            this._endDate = endDate;
            this._frequency = frequency;
            this._period = period;
        }

        public int Frequency
        {
            get
            {
                return _frequency;
            }
        }

        public String Period
        {
            get
            {
                return _period;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
        }

        public DateTime LastUpdate
        {
            get
            {
                return _lastUpdate;
            }

        }
    }
}
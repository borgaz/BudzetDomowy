﻿using System;

namespace Bugdet
{
    public class PeriodPayment : Payment
    {
        public PeriodPayment(int ID, int categoryID, double amount, String note, int type, String name, int frequency, String period, DateTime lastUpdate, DateTime startDate,DateTime endDate)
            : base(ID, categoryID, amount, note, type, name)
        {
            this.LastUpdate = lastUpdate;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Frequency = frequency;
            this.Period = period;
        }

        // default constructor
        public PeriodPayment() 
            : base ()
        {
            LastUpdate = DateTime.MinValue;
            StartDate = DateTime.MinValue;
            Frequency = 0;
            Period = "";
        }

        public int Frequency { get; set; }

        public String Period { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
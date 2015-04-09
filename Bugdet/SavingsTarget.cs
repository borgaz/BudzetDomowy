using System;

namespace Bugdet
{
    public class SavingsTarget
    {
        public SavingsTarget (String target, String note, DateTime deadLine, String priority, double moneyHoldings, DateTime addedDate, double neededAmount)
        {
            this.Target = target;
            this.Note = note;
            this.DeadLine = deadLine;
            this.DaysLeft = (int)(deadLine - DateTime.Today).TotalDays;
            this.Priority = priority;
            this.MoneyHoldings = moneyHoldings;
            this.AddedDate = addedDate;
            this.NeededAmount = neededAmount;
        }

        //default constructor
        public SavingsTarget ()
        {
            this.Target = "";
            this.Note = "";
            this.DeadLine = DateTime.Today;
            this.DaysLeft = 0;
            this.Priority = "";
            this.MoneyHoldings = 0.0;
            this.AddedDate = DateTime.Today;
            this.NeededAmount = 0.0;
        }

        public String Target { get; set; }

        public String Note { get; set; }

        public DateTime DeadLine { get; set; }

        public int DaysLeft { get; set; }

        public String Priority { get; set; }

        public double MoneyHoldings { get; set; }

        public DateTime AddedDate { get; set; }

        public double NeededAmount { get; set; }
    }
}
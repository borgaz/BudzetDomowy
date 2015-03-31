using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugdet
{
    class SavingsTarget
    {
        private String target; //description of the target
        private String note;
        private DateTime deadLine;
        private int daysLeft;
        private String priority;
        private double moneyHoldings;
        private DateTime addedDate;
        private double neededAmount;

        public SavingsTarget (String target, String note, DateTime deadLine, String priority, double moneyHoldings, DateTime addedDate, double neededAmount)
        {
            this.target = target;
            this.note = note;
            this.deadLine = deadLine;
            this.daysLeft = (int)(deadLine - DateTime.Today).TotalDays;
            this.priority = priority;
            this.moneyHoldings = moneyHoldings;
            this.addedDate = addedDate;
            this.neededAmount = neededAmount;
        }

        //default constructor
        public SavingsTarget ()
        {
            this.target = "";
            this.note = "";
            this.deadLine = DateTime.Today;
            this.daysLeft = 0;
            this.priority = "";
            this.moneyHoldings = 0.0;
            this.addedDate = DateTime.Today;
            this.neededAmount = 0.0;
        }

        public String Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        public String Note
        {
            get
            {
                return note;
            }
            set
            {
                note = value;
            }
        }

        public DateTime DeadLine
        {
            get
            {
                return deadLine;
            }
            set
            {
                deadLine = value;
            }
        }

        public int DaysLeft
        {
            get
            {
                return daysLeft;
            }
            set
            {
                daysLeft = value;
            }
        }

        public String Priority
        {
            get
            {
                return priority;
            }
            set
            {
                priority = value;
            }
        }

        public double MoneyHoldings
        {
            get
            {
                return moneyHoldings;
            }
            set
            {
                moneyHoldings = value;
            }
        }

        public DateTime AddedDate
        {
            get
            {
                return addedDate;
            }
            set
            {
                addedDate = value;
            }
        }

        public double NeededAmount
        {
            get
            {
                return neededAmount;
            }
            set
            {
                neededAmount = value;
            }
        }
   }
}
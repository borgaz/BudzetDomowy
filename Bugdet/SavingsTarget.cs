using System;

namespace Bugdet
{
    public class SavingsTarget
    {
        private String _target; //description of the target
        private String _note;
        private DateTime _deadLine;
        private int _daysLeft;
        private String _priority;
        private double _moneyHoldings;
        private DateTime _addedDate;
        private double _neededAmount;

        public SavingsTarget (String target, String note, DateTime deadLine, String priority, double moneyHoldings, DateTime addedDate, double neededAmount)
        {
            this._target = target;
            this._note = note;
            this._deadLine = deadLine;
            this._daysLeft = (int)(deadLine - DateTime.Today).TotalDays;
            this._priority = priority;
            this._moneyHoldings = moneyHoldings;
            this._addedDate = addedDate;
            this._neededAmount = neededAmount;
        }

        public String Target
        {
            get
            {
                return _target;
            }
        }

        public String Note
        {
            get
            {
                return _note;
            }
        }

        public DateTime DeadLine
        {
            get
            {
                return _deadLine;
            }
        }

        public int DaysLeft
        {
            get
            {
                return _daysLeft;
            }
        }

        public String Priority
        {
            get
            {
                return _priority;
            }
        }

        public double MoneyHoldings
        {
            get
            {
                return _moneyHoldings;
            }
        }

        public DateTime AddedDate
        {
            get
            {
                return _addedDate;
            }
        }

        public double NeededAmount
        {
            get
            {
                return _neededAmount;
            }
        }
   }
}
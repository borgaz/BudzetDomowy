using System;

namespace Bugdet
{
    public class SinglePayment : Payment
    {
        private DateTime _date;

        public SinglePayment(String note, double amount, int categoryId, int type, String name, DateTime date)
            : base( categoryId, amount, note, type, name)
        {
            this._date = date;
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }
        }
    }
}
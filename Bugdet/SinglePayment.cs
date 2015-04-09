using System;

namespace Bugdet
{
    public class SinglePayment : Payment
    {
        public SinglePayment(int ID, String note, double amount, int categoryID, int type, String name, DateTime date)
            : base(ID, categoryID, amount, note, type, name)
        {
            this.Date = date;
        }

        // default constructor
        public SinglePayment()
            : base()
        {
            Date = DateTime.Today;
        }

        public DateTime Date { get; set; }
    }
}
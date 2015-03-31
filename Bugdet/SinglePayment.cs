using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugdet
{
    class SinglePayment : Payment
    {
        private DateTime date;

        public SinglePayment(int ID, String note, double amount, int categoryID, String type, String name, DateTime date)
            : base(ID, categoryID, amount, note, type, name)
        {
            this.date = date;
        }

        // default constructor
        public SinglePayment()
            : base()
        {
            date = DateTime.Today;
        }

        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugdet
{
    public class SinglePayment : Payment
    {
        private DateTime date; // data wykonania

        public SinglePayment(String note, double amount, int categoryID, bool type, String name, DateTime date)
            : base( categoryID, amount, note, type, name)
        {
            this.date = date;
        }

        public DateTime Date
        {
            get
            {
                return date;
            }
        }
    }
}
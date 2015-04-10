using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugdet
{
    public class BalanceLog
    {
        private DateTime date;
        private string note;
        private int id;
        private int amount;
        private int categoryId;
        private int periodPaymentId;

        public BalanceLog(int id, int amount, int categoryId, int periodPaymentId, DateTime date, string note)
        {
            this.id = id;
            this.amount = amount;
            this.categoryId = categoryId;
            this.periodPaymentId = periodPaymentId;
            this.date = date;
            this.note = note;

        }

        public BalanceLog(int amount, int categoryId, int periodPaymentId, DateTime date, string note)
        {
            this.amount = amount;
            this.categoryId = categoryId;
            this.periodPaymentId = periodPaymentId;
            this.date = date;
            this.note = note;

        }

        public DateTime Date
        {
            get
            {
                return date;
            }
        }

        public string Note
        {
            get
            {
                return note;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public int Amount
        {
            get
            {
                return amount;
            }
        }

        public int CategoryId
        {
            get
            {
                return categoryId;
            }
        }

        public int PeriodPaymentId
        {
            get
            {
                return periodPaymentId;
            }
        }
    }
}
using System;

namespace Bugdet
{
    public class BalanceLog
    {
        public BalanceLog() { }

        public BalanceLog(int id, int amount, int categoryId, int periodPaymentId, DateTime date, string note)
        {
            this.Id = id;
            this.Amount = amount;
            this.CategoryId = categoryId;
            this.PeriodPaymentId = periodPaymentId;
            this.Date = date;
            this.Note = note;
        }

        public BalanceLog(int amount, int categoryId, int periodPaymentId, DateTime date, string note)
        {
            this.Amount = amount;
            this.CategoryId = categoryId;
            this.PeriodPaymentId = periodPaymentId;
            this.Date = date;
            this.Note = note;
        }

        public DateTime Date { get; set; }

        public string Note { get; set; }

        public int Id { get; private set; }

        public int Amount { get; set; }

        public int CategoryId { get; set; }

        public int PeriodPaymentId { get; set; }
    }
}
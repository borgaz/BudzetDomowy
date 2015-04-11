using System;

namespace Bugdet
{
    public class BalanceLog
    {
        private DateTime _date;
        private string _note;
        private int _id;
        private int _amount;
        private int _categoryId;
        private int _periodPaymentId;

        public BalanceLog(int id, int amount, int categoryId, int periodPaymentId, DateTime date, string note)
        {
            this._id = id;
            this._amount = amount;
            this._categoryId = categoryId;
            this._periodPaymentId = periodPaymentId;
            this._date = date;
            this._note = note;

        }

        public BalanceLog(int amount, int categoryId, int periodPaymentId, DateTime date, string note)
        {
            this._amount = amount;
            this._categoryId = categoryId;
            this._periodPaymentId = periodPaymentId;
            this._date = date;
            this._note = note;

        }

        public DateTime Date
        {
            get
            {
                return _date;
            }
        }

        public string Note
        {
            get
            {
                return _note;
            }
        }

        public int Id
        {
            get
            {
                return _id;
            }
        }

        public int Amount
        {
            get
            {
                return _amount;
            }
        }

        public int CategoryId
        {
            get
            {
                return _categoryId;
            }
        }

        public int PeriodPaymentId
        {
            get
            {
                return _periodPaymentId;
            }
        }
    }
}
using System;

namespace Budget.Main_Classes
{
    public class BalanceLog // historia sald
    {
        private DateTime date; // data
        private double balance; // kwota
        private int singlePaymentID; // referencja do płatności/wydatku, który spowodował zmianę salda na this
        private int periodPaymentID; // gdy będzie to wydatek single to periodPaymentID ustawiamy na 0 i vice versa

        public override string ToString()
        {
            return "DATE: " + date + ", BALANCE: " + balance + ", SINGLE_PAYMENT_ID: " + singlePaymentID + ", PERIOD_PAYMENT_ID: " + periodPaymentID + "\n";
        }

        public BalanceLog(double balance, DateTime date, int singlepaymentid, int periodpaymentid)
        {
            this.balance = balance;
            this.date = date;
            this.singlePaymentID = singlepaymentid;
            this.periodPaymentID = periodpaymentid;
        }

        public DateTime Date
        {
            get
            {
                return date;
            }
        }

        public double Balance
        {
            get
            {
                return balance;
            }
            set
            {
                balance = value;
            }
        }

        public int SinglePaymentID
        {
            get
            {
                return singlePaymentID;
            }
        }

        public int PeriodPaymentID
        {
            get
            {
                return periodPaymentID;
            }
        }
    }
}
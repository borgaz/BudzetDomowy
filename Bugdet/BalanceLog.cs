﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugdet
{
    public class BalanceLog
    {
        // historia sald

        private DateTime date;
        private double balance;
        private int singlePaymentID; // referencja do płatności/wydatku, który spowodował zmianę salda na this
        private int periodPaymentID; // gdy będzie to wydatek single to periodPaymentID ustawiamy na 0 i vice versa

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
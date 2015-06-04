using System;

namespace Budget.Main_Classes
{
    public class SinglePayment : Payment
    {
        private DateTime date; // data wykonania

        public override string ToString()
        {
            return "DATE: " + date + ", BASE_CATEGORY_ID: " + base.CategoryID + ", BASE_AMOUNT: " + base.Amount 
                + ", BASE_NAME: " + base.Name + ", BASE_TYPE: " + base.Type + ", BASE_NOTE: " + base.Note + "\n";
        }

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

        override public int CompareDate()
        {
            return DateTime.Compare(this.date, DateTime.Now);
            //Mniej niż zero - historia
            //Większe od zera - przyszlosc
        }

        override public void changeUpdateDate(int count = 0)
        {
            this.date = DateTime.Now;
        }

        override public int CompareDateTo(DateTime date)
        {
            return DateTime.Compare(this.date, date);
        }
    }
}
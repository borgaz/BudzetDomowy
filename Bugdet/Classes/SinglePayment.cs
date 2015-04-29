using System;

namespace Budget.Classes
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
    }
}
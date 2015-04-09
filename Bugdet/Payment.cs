using System;

namespace Bugdet
{
    abstract public class Payment
    {
        public Payment(int ID, int categoryID, double amount, String note, int type, String name)
        {
            this.ID = ID;
            this.CategoryID = categoryID;
            this.Amount = amount;
            this.Note = note;
            this.Type = type;
            this.Name = name;
        }

        // default constructor
        public Payment()
        {
            this.ID = 0;
            this.Note = "";
            this.Amount = 0.0;
            this.CategoryID = 0;
            this.Type = 0;
            this.Name = "";
        }

        public int ID { get; set; }

        public int CategoryID { get; set; }

        public double Amount { get; set; }

        public String Note { get; set; }

        public int Type { get; set; }

        public String Name { get; set; }
    }
}
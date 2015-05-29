using System;

namespace Budget.Main_Classes
{
    abstract public class Payment
    {
        private int categoryID; // id kategorii, z ktorej jest dana platnosc
        private double amount; // kwota
        private String name; // nazwa platnosci
        private bool type; // false - przychod, true - wydatek
        private String note; // notatka

        abstract public int CompareDate();
        abstract public void changeUpdateDate(int count);

        public override string ToString()
        {
            return "CATEGORY_ID: " + categoryID + ", AMOUNT: " + amount + ", NAME: " + name + ", TYPE: " + type + ", NOTE: " + note + "\n";
        }

        public Payment(int categoryID, double amount, String note, bool type, String name)
        {
            this.categoryID = categoryID;
            this.amount = amount;
            this.note = note;
            this.type = type;
            this.name = name;
        }

        public int CategoryID
        {
            get
            {
                return categoryID;
            }
        }

        public double Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
            }
        }

        public String Note
        {
            get
            {
                return note;
            }
            set { note = value; }
        }

        public bool Type
        {
            get
            {
                return type;
            }
        }

        public String Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        virtual public int CountPeriods()
        {
            return 0;
        }

        virtual public SinglePayment CreateSingleFromPeriod(int _period)
        {
            return null;
        }

        virtual public SinglePayment CreateFirstSingle()
        {
            return null;
        }
    }
}
using System;

namespace Budget.Main_Classes
{
    abstract public class Payment
    {
        private int categoryID; // id kategorii, z ktorej jest dana platnosc
        private double amount; // kwota
        private String name; // nazwa platnosci
        private bool type; // 0 - przychod, 1 - wydatek
        private String note; // notatka

        abstract public int CompareDate();
        abstract public void changeUpdateDate();

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

        virtual public int countPeriods()
        {
            return 0;
        }

        virtual public SinglePayment createSingleFromPeriod(int _period)
        {
            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget
{
    abstract public class Payment
    {
        private int categoryID; // id kategorii, z ktorej jest dana platnosc
        private double amount; // kwota
        private String name; // nazwa platnosci
        private bool type; // 0 - przychod, 1 - rozchod
        private String note; // notatka

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
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugdet
{
    abstract public class Payment
    {
        private int categoryID;
        private double amount;
        private String name;
        private int type;
        private String note;

        public Payment(int categoryID, double amount, String note, int type, String name)
        {
            this.categoryID = categoryID;
            this.amount = amount;
            this.note = note;
            this.type = type;
            this.name = name;
        }

        // default constructor
        public Payment()
        {
            this.note = "";
            this.amount = 0.0;
            this.categoryID = 0;
            this.type = 0;
            this.name = "";
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

        public int Type
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
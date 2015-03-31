using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugdet
{
    abstract class Payment
    {
        private int id;
        private int categoryID;
        private double amount;
        private String name;
        private String type;
        private String note;

        public Payment(int ID, int categoryID, double amount, String note, String type, String name)
        {
            this.id = ID;
            this.categoryID = categoryID;
            this.amount = amount;
            this.note = note;
            this.type = type;
            this.name = name;
        }

        // default constructor
        public Payment()
        {
            this.id = 0;
            this.note = "";
            this.amount = 0.0;
            this.categoryID = 0;
            this.type = "";
            this.name = "";
        }

        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public int CategoryID
        {
            get
            {
                return categoryID;
            }
            set
            {
                categoryID = value;
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
            set
            {
                note = value;
            }
        }

        public String Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.WelcomePage
{
    public class ProvidedPayment : IComparable<ProvidedPayment>
    {
        private String name;
        private Double amount;
        private String type; // okresowy czy pojedynczy
        private Boolean sign; // 0 - przychod, 1 - wydatek
        private DateTime date;

        public ProvidedPayment(String name, Double amount, String type, DateTime date, Boolean sign)
        {
            this.name = name;
            this.amount = amount;
            this.type = type;
            this.date = date;
            this.sign = sign;
        }

        public int CompareTo(ProvidedPayment pP)
        {
            return this.date.CompareTo(pP.date);
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

        public String Date
        {
            get
            {
                return date.ToShortDateString();
            }
        }

        public String Type
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
  
        public Boolean Sign
        {
            get
            {
                return sign;
            }
        }
    }
}
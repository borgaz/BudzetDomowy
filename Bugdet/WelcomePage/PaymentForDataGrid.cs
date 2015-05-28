using System;
using System.Linq;

namespace Budget.WelcomePage
{
    public class PaymentForDataGrid : IComparable<PaymentForDataGrid>
    {
        private String name;
        private Double amount;
        private String type; // okresowy czy pojedynczy
        private Boolean sign; // 0 - przychod, 1 - wydatek
        private DateTime date;
        private int categoryID;
        private Double singleAmount;
        private String categoryName;
        private Double totalAmount;

        public PaymentForDataGrid(String name, Double amount, String type, DateTime date, Boolean sign, int categoryID, Double tA)
        {
            this.name = name;
            this.amount = amount;
            this.type = type;
            this.date = date;
            this.sign = sign;
            this.categoryID = categoryID;
            this.totalAmount = tA;
            if (this.sign == false)
            {
                this.singleAmount = amount;
            }
            else
            {
                this.singleAmount = -1 * amount;
            }
            this.categoryName = Main_Classes.Budget.Instance.Categories.FirstOrDefault(x => x.Key.Equals(categoryID)).Value.Name;
        }

        public int CompareTo(PaymentForDataGrid pP)
        {
            return this.date.CompareTo(pP.date);
        }

        // Nizej sa juz tylko gettery 

        public Double SingleAmount
        {
            get
            {
                return singleAmount;
            }
        }

        public Double TotalAmount
        {
            get
            {
                return totalAmount;
            }
        }

        public String CategoryName
        {
            get
            {
                return categoryName;
            }
        }

        public double Amount
        {
            get
            {
                return amount;
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

        public int CategoryID
        {
            get
            {
                return categoryID;
            }
        }
    }
}
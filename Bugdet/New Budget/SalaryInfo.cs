using System;

namespace Budget.New_Budget
{
    public class SalaryInfo
    {
        private String name;
        private double amount;
        private String password;
        public SalaryInfo()
        {

        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Password
        {
            get { return password; }
            set { password = value; }
        }

        public double Amount
        {
            get { return amount; }
            set { amount = value; }
        }
    }
}

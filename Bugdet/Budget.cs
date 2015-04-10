using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugdet
{
    public class Budget
    {
        private String note;
        private String name;
        private String password;
        private Dictionary<int, Payment> payments;
        private Dictionary<int, Category> categories;
        private Dictionary<int, SavingsTarget> savingsTargets;
        private BalanceLog balance;
        private int numberOfPeople;
        private DateTime creationDate;

        public Budget(String note, String name, String password, Dictionary<int, Payment> payments, Dictionary<int, Category> categories,
            Dictionary<int, SavingsTarget> savingsTargets, BalanceLog balance, int numberOfPeople, DateTime creationDate)
        {
            this.note = note;
            this.name = name;
            this.password = password;
            this.payments = payments;
            this.categories = categories;
            this.savingsTargets = savingsTargets;
            this.balance = balance;
            this.numberOfPeople = numberOfPeople;
            this.creationDate = creationDate;
        }

        public Budget()
        {
            this.note = "";
            this.name = "";
            this.password = "";
            this.payments = null;
            this.categories = null;
            this.savingsTargets = null;
            this.balance = null;
            this.numberOfPeople = 0;
            this.creationDate = DateTime.Now;
        }

        public String Note
        {
            get
            {
                return note;
            }
        }

        public String Name
        {
            get
            {
                return name;
            }
        }

        public String Password
        {
            get
            {
                return password;
            }
        }

        public int NumberOfPeople
        {
            get
            {
                return numberOfPeople;
            }
        }

        public BalanceLog Balance
        {
            get
            {
                return balance;
            }
        }

        public Dictionary<int, Payment> Payments
        {
            get
            {
                return payments;
            }
        }

        public Dictionary<int, Category> Categories
        {
            get
            {
                return categories;
            }
        }

        public Dictionary<int, SavingsTarget> SavingsTargets
        {
            get
            {
                return savingsTargets;
            }
        }


        public void SetNumberOfPeople(int number)
        {
            this.numberOfPeople = number;
        }

        public void AddNote(String note)
        {
            this.note = note; 
        }

        public void DeleteNumberOfPeople ()
        {
            this.numberOfPeople = 0;
        }

        public void AddSinglePayment(int index, SinglePayment payment)
        {
            payments.Add(index, payment);
        }

        public void DeleteSinglePayment(int index)
        {
            payments.Remove(index);
        }

        public void AddSavingsTarget(int index, SavingsTarget target)
        {
            savingsTargets.Add(index, target);
        }

        public void DeleteSavingsTarget(int index)
        {
            savingsTargets.Remove(index);
        }

        public void AddPeriodPayment(int index, PeriodPayment payment)
        {
            payments.Add(index, payment);
        }

        public void DeletePeriodPayment(int index)
        {
            payments.Remove(index);
        }

    }
}
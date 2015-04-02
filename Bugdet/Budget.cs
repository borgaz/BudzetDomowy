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
        private List<Payment> payments;
        private List<Category> categories;
        private List<SavingsTarget> savingsTargets;
        private BalanceLog balance;
        private int numberOfPeople;
        private DateTime creationDate;

        public Budget(String note, String name, String password, List<Payment> payments, List<Category> categories,
            List<SavingsTarget> savingsTargets, BalanceLog balance, int numberOfPeople, DateTime creationDate)
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

        public String Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }

        public int NumberOfPeople
        {
            get
            {
                return numberOfPeople;
            }
            set
            {
                numberOfPeople = value;
            }
        }

        public BalanceLog Balance
        {
            get
            {
                return balance;
            }
            set
            {
                balance = value;
            }
        }

        public List<Payment> Payments
        {
            get
            {
                return payments;
            }
            set
            {
                payments = value;
            }
        }

        public List<Category> Categories
        {
            get
            {
                return categories;
            }
            set
            {
                categories = value;
            }
        }

        public List<SavingsTarget> SavingsTargets
        {
            get
            {
                return savingsTargets;
            }
            set
            {
                savingsTargets = value;
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

        public void AddSinglePayment(SinglePayment payment)
        {
            payments.Add(payment);
        }

        public void DeleteSinglePayment(SinglePayment payment)
        {
            payments.Remove(payment);
        }

        public void AddSavingsTarget(SavingsTarget target)
        {
            savingsTargets.Add(target);
        }

        public void DeleteSavingsTarget(SavingsTarget target)
        {
            savingsTargets.Remove(target);
        }

        public void AddPeriodPayment(PeriodPayment payment)
        {
            payments.Add(payment);
        }

        public void DeletePeriodPayment(PeriodPayment payment)
        {
            payments.Remove(payment);
        }

    }
}
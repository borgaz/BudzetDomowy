using System;
using System.Collections.Generic;

namespace Bugdet
{
    public class Budget
    {
        private String _note;
        private String _name;
        private String _password;
        private Dictionary<int, Payment> _payments;
        private Dictionary<int, Category> _categories;
        private Dictionary<int, SavingsTarget> _savingsTargets;
        private BalanceLog _balance;
        private int _numberOfPeople;
        private DateTime _creationDate;

        public Budget(String note, String name, String password, Dictionary<int, Payment> payments, Dictionary<int, Category> categories,
            Dictionary<int, SavingsTarget> savingsTargets, BalanceLog balance, int numberOfPeople, DateTime creationDate)
        {
            this._note = note;
            this._name = name;
            this._password = password;
            this._payments = payments;
            this._categories = categories;
            this._savingsTargets = savingsTargets;
            this._balance = balance;
            this._numberOfPeople = numberOfPeople;
            this._creationDate = creationDate;
        }

        public String Note
        {
            get
            {
                return _note;
            }
        }

        public String Name
        {
            get
            {
                return _name;
            }
        }

        public String Password
        {
            get
            {
                return _password;
            }
        }

        public int NumberOfPeople
        {
            get
            {
                return _numberOfPeople;
            }
        }

        public BalanceLog Balance
        {
            get
            {
                return _balance;
            }
        }

        public Dictionary<int, Payment> Payments
        {
            get
            {
                return _payments;
            }
        }

        public Dictionary<int, Category> Categories
        {
            get
            {
                return _categories;
            }
        }

        public Dictionary<int, SavingsTarget> SavingsTargets
        {
            get
            {
                return _savingsTargets;
            }
        }


        public void SetNumberOfPeople(int number)
        {
            this._numberOfPeople = number;
        }

        public void AddNote(String note)
        {
            this._note = note; 
        }

        public void DeleteNumberOfPeople ()
        {
            this._numberOfPeople = 0;
        }

        public void AddSinglePayment(int index, SinglePayment payment)
        {
            _payments.Add(index, payment);
        }

        public void DeleteSinglePayment(int index)
        {
            _payments.Remove(index);
        }

        public void AddSavingsTarget(int index, SavingsTarget target)
        {
            _savingsTargets.Add(index, target);
        }

        public void DeleteSavingsTarget(int index)
        {
            _savingsTargets.Remove(index);
        }

        public void AddPeriodPayment(int index, PeriodPayment payment)
        {
            _payments.Add(index, payment);
        }

        public void DeletePeriodPayment(int index)
        {
            _payments.Remove(index);
        }

    }
}
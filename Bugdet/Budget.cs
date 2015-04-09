using System;
using System.Collections.Generic;

namespace Bugdet
{
    public class Budget
    {
        private DateTime creationDate;

        public Budget(String note, String name, String password, List<Payment> payments, List<Category> categories,
            List<SavingsTarget> savingsTargets, BalanceLog balance, int numberOfPeople, DateTime creationDate)
        {
            this.Note = note;
            this.Name = name;
            this.Password = password;
            this.Payments = payments;
            this.Categories = categories;
            this.SavingsTargets = savingsTargets;
            this.Balance = balance;
            this.NumberOfPeople = numberOfPeople;
            this.creationDate = creationDate;
        }

        public String Note { get; set; }

        public String Name { get; set; }

        public String Password { get; set; }

        public int NumberOfPeople { get; set; }

        public BalanceLog Balance { get; set; }

        public List<Payment> Payments { get; set; }

        public List<Category> Categories { get; set; }

        public List<SavingsTarget> SavingsTargets { get; set; }


        public void SetNumberOfPeople(int number)
        {
            this.NumberOfPeople = number;
        }

        public void AddNote(String note)
        {
            this.Note = note; 
        }

        public void DeleteNumberOfPeople ()
        {
            this.NumberOfPeople = 0;
        }

        public void AddSinglePayment(SinglePayment payment)
        {
            Payments.Add(payment);
        }

        public void DeleteSinglePayment(SinglePayment payment)
        {
            Payments.Remove(payment);
        }

        public void AddSavingsTarget(SavingsTarget target)
        {
            SavingsTargets.Add(target);
        }

        public void DeleteSavingsTarget(SavingsTarget target)
        {
            SavingsTargets.Remove(target);
        }

        public void AddPeriodPayment(PeriodPayment payment)
        {
            Payments.Add(payment);
        }

        public void DeletePeriodPayment(PeriodPayment payment)
        {
            Payments.Remove(payment);
        }

    }
}
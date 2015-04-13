using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugdet
{
    public class SavingsTarget
    {
        private String target; // cel, na jaki chcemy odlozyc pieniadze
        private String note; // notatka
        private DateTime deadline; // data, do ktorej chcemy miec dana kwote
        private int daysLeft; // ile dni pozostalo do deadLine`a
        private String priority; // priorytet
        private double moneyHoldings; // odlozona juz kwota
        private DateTime addedDate; // data dodatania celu
        private double neededAmount; // kwota, jaka chcemy odlozyc

        public override string ToString()
        {
            return "TARGET: " + target + " NOTE: " + note + " DEADLINE: " + deadline + " DAYS_LEFT: " + daysLeft + " PRIORITY: " + priority
                + " MONEY_HOLDINGS: " + moneyHoldings + " ADDED_DATE: " + addedDate + " NEEDED_AMOUNT: " + neededAmount + "\n";
        }

        public SavingsTarget (String target, String note, DateTime deadline, String priority, double moneyHoldings, DateTime addedDate, double neededAmount)
        {
            this.target = target;
            this.note = note;
            this.deadline = deadline;
            CountDaysLeft();
            this.priority = priority;
            this.moneyHoldings = moneyHoldings;
            this.addedDate = addedDate;
            this.neededAmount = neededAmount;
        }

        /// <summary>
        /// Dodawanie pieniędzy do targetu
        /// </summary>
        /// <param name="amount">Kwota którą chcemy dodać (ewentualnie odjąć)</param>
        /// <returns>Zwraca true, jak cel zostanie osiągnięty, false w przeciwnym wypadku</returns>
        public Boolean AddMoney (double amount)
        {
            this.moneyHoldings += amount;
            if (CountMoneyLeft() == 0.0)
                return true;
            else
                return false;
        }

        public int CountDaysLeft()
        {
            int days = (int)(deadline - DateTime.Today).TotalDays;
            this.daysLeft = days;
            return days;
        }

        public double CountMoneyLeft()
        {
            if (this.NeededAmount - this.moneyHoldings <= 0)
                return 0.0;
            else
                return this.NeededAmount - this.moneyHoldings;
        }

        public String Target
        {
            get
            {
                return target;
            }
        }

        public String Note
        {
            get
            {
                return note;
            }
        }

        public DateTime Deadline
        {
            get
            {
                return deadline;
            }
        }

        public int DaysLeft
        {
            get
            {
                return daysLeft;
            }
        }

        public String Priority
        {
            get
            {
                return priority;
            }
        }

        public double MoneyHoldings
        {
            get
            {
                return moneyHoldings;
            }
        }

        public DateTime AddedDate
        {
            get
            {
                return addedDate;
            }
        }

        public double NeededAmount
        {
            get
            {
                return neededAmount;
            }
        }
   }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget
{
    public class SavingsTarget
    {
        private String target; // cel, na jaki chcemy odlozyc pieniadze
        private String note; // notatka
        private DateTime deadline; // data, do ktorej chcemy miec dana kwote
        private int daysLeft; // ile dni pozostalo do deadLine`a
        public enum Priorities    
        {
            VeryLow = -2,
            Low = -1,
            Normal = 0,
            High = 1,
            VeryHigh = 2
        }
        private Priorities priority;    // priorytet
        private double moneyHoldings; // odlozona juz kwota
        private DateTime addedDate; // data dodatania celu
        private double neededAmount; // kwota, jaka chcemy odlozyc

        public override string ToString()
        {
            return "TARGET: " + target + ", NOTE: " + note + ", DEADLINE: " + deadline + ", DAYS_LEFT: " + daysLeft + ", PRIORITY: " + priority
                + ", MONEY_HOLDINGS: " + moneyHoldings + ", ADDED_DATE: " + addedDate + ", NEEDED_AMOUNT: " + neededAmount + "\n";
        }

        public SavingsTarget(String target, String note, DateTime deadline, int priority, double moneyHoldings, DateTime addedDate, double neededAmount)
        {
            this.target = target;
            this.note = note;
            this.deadline = deadline;
            this.daysLeft = (int)(deadline - DateTime.Today).TotalDays;
            this.priority = (Priorities)priority;
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
            {
                return true;
            }
            else
            {
                return false;
            }      
        }

        /// <summary>
        /// Obliczanie, jakiej kwoty brakuje
        /// </summary>
        /// <returns>Zwraca liczbę, która jeśli jest dodatnia, oznacza, ile brakuje do celu, a jak jest ujemna, to ile za dużo odłożyliśmy </returns>
        public double CountMoneyLeft()
        {
            if (this.NeededAmount - this.moneyHoldings < 0)
            {
                return (this.NeededAmount - (this.NeededAmount + this.moneyHoldings));
            }
            else if (this.NeededAmount - this.moneyHoldings > 0)
            {
                return this.NeededAmount - this.moneyHoldings;
            }
            else
            {
                return 0.0;
            }   
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

        public int Priority
        {
            get
            {
                return (int)priority;
            }
        }

        public int DaysLeft
        {
            get
            {
                return daysLeft;
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
﻿using System;

namespace Budget.Main_Classes
{
    public class SavingsTarget : IComparable<SavingsTarget>
    {
        private String target; // cel, na jaki chcemy odlozyc pieniadze
        private String note; // notatka
        private DateTime deadline; // data, do ktorej chcemy miec dana kwote
        private int daysLeft; // ile dni pozostalo do deadLine`a
        private Boolean type; // czy automatycznie odkladaja sie pieniadze, czy robimy to recznie
        public enum Priorities    
        {
            BardzoNiski = -2,
            Niski = -1,
            Normalny = 0,
            Wysoki = 1,
            BardzoWysoki = 2
        }
        private Priorities priority;    // priorytet
        private double moneyHoldings; // odlozona juz kwota
        private DateTime addedDate; // data dodatania celu
        private double neededAmount; // kwota, jaka chcemy odlozyc

        public int CompareTo(SavingsTarget sT)
        {
            return this.priority.CompareTo(sT.priority);
        }

        public override string ToString()
        {
            return "TARGET: " + target + ", NOTE: " + note + ", DEADLINE: " + deadline + ", DAYS_LEFT: " + daysLeft + ", PRIORITY: " + priority
                + ", MONEY_HOLDINGS: " + moneyHoldings + ", ADDED_DATE: " + addedDate + ", NEEDED_AMOUNT: " + neededAmount + ", TYPE: " + type + "\n";
        }

        public SavingsTarget(String target, String note, DateTime deadline, int priority, double moneyHoldings, DateTime addedDate, double neededAmount, Boolean type)
        {
            this.target = target;
            this.note = note;
            this.deadline = deadline;
            this.daysLeft = (int)(deadline - DateTime.Today).TotalDays;
            this.priority = (Priorities)priority;
            this.moneyHoldings = moneyHoldings;
            this.addedDate = addedDate;
            this.neededAmount = neededAmount;
            this.type = type;
        }

        /// <summary>
        /// Dodawanie pieniędzy do targetu
        /// </summary>
        /// <param name="amount">Kwota którą chcemy dodać (ewentualnie odjąć)</param>
        /// <returns>Zwraca true, jak cel zostanie osiągnięty, false w przeciwnym wypadku</returns>
        public Boolean AddMoney (double amount, int index)
        {
            this.moneyHoldings += amount;
            Main_Classes.Budget.Instance.ListOfEdits.Add(new Utility_Classes.Changes(typeof(SavingsTarget), index));

            return CountMoneyLeft() == 0.0;
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

        public Boolean Type
        {
            get
            {
                return type;
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

        public int PriorityToInt
        {
            get
            {
                return (int)priority;
            }
        }

        public String PriorityToString
        {
            get
            {
                return priority.ToString();
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
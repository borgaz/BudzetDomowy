using System;
using System.Collections.Generic;

namespace Budget.Main_Classes
{
    public class PeriodPayment : Payment
    {
        private String period; // co jaki czas (dzien/tydzine/miesiac itp)
        private int frequency; // co ile dni/tygodni itp
        private DateTime startDate; // poczatkowa data
        private DateTime lastUpdate; // data ostatniego wystapienia
        private DateTime endDate; // koncowa data

        public override string ToString()
        {
            return "START_DATE: " + startDate + ", PERIOD: " + period + ", FREQUENCY: " + frequency + ", LAST_UPDATE: " + lastUpdate
                + ", END_DATE: " + endDate + ", BASE_CATEGORY_ID: " + base.CategoryID + ", BASE_AMOUNT: " + base.Amount 
                + ", BASE_NAME: " + base.Name + ", BASE_TYPE: " + base.Type + ", BASE_NOTE: " + base.Note + "\n";
        }

        public PeriodPayment(int categoryID, double amount, String note, bool type, String name, int frequency, String period, DateTime lastUpdate, DateTime startDate, DateTime endDate)
                : base (categoryID, amount, note, type, name)
        {
            this.lastUpdate = lastUpdate;
            this.startDate = startDate;
            this.endDate = endDate;
            this.frequency = frequency;
            this.period = period;
        }

        public PeriodPayment(PeriodPayment pP) : base(pP.CategoryID, pP.Amount, pP.Note, pP.Type, pP.Name)
        {
            this.lastUpdate = pP.CountNextDate();
            this.startDate = pP.startDate;
            this.endDate = pP.endDate;
            this.frequency = pP.frequency;
            this.period = pP.period;
        }

        public int Frequency
        {
            get
            {
                return frequency;
            }
        }

        public String Period
        {
            get
            {
                return period;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
        }

        public DateTime LastUpdate
        {
            get
            {
                return lastUpdate;
            }

        }

        override public int CompareDate()
        {
            return DateTime.Compare(this.lastUpdate, DateTime.Today);
            //Mniej niż zero - lastUpdate jest wcześniejsza niż Now.
            //Zero - lastUpdate jest taka sama jak Now.
            //Większe od zera - lastUpdate jest późniejsza niż Now. 
        }

        override public int CountPeriods()
        {          
            int _periods = 0;
            DateTime temp = this.lastUpdate;

            while (DateTime.Compare(temp, DateTime.Today) <= 0 && DateTime.Compare(temp, this.EndDate) <= 0)
            //Mniej niż zeroooooo - lastUpdate (temp) jest wcześniejsza niż Now.
            //Zero - lastUpdate (temp) jest taka sama jak Now.
            //Większe od zera - lastUpdate (temp) jest późniejsza niż Now.
            {
                _periods++;

                if (this.period == "MIESIĄC")
                    temp = temp.AddMonths(this.frequency);
                else if (this.period == "DZIEŃ")
                    temp =  temp.AddDays(this.frequency);
                else if (this.period == "TYDZIEŃ")
                    temp = temp.AddDays(7 * this.frequency);
                else if (this.period == "ROK")
                    temp = temp.AddYears(this.frequency);          
            }
            return _periods;
        }

        override public void changeUpdateDate(int countPeriod)
        {
            if (this.startDate != this.lastUpdate)
            {
                if (this.period == "MIESIĄC")
                    this.lastUpdate = this.lastUpdate.AddMonths(this.frequency * countPeriod);
                else if (this.period == "DZIEŃ")
                    this.lastUpdate = this.lastUpdate.AddDays(this.frequency * countPeriod);
                else if (this.period == "TYDZIEŃ")
                    this.lastUpdate = this.lastUpdate.AddDays(7 * this.frequency * countPeriod);
                else if (this.period == "ROK")
                    this.lastUpdate = this.lastUpdate.AddYears(this.frequency * countPeriod); 
            }
        }

        override public SinglePayment CreateSingleFromPeriod(int _period)
        {
            _period -= 1;
            System.Console.WriteLine("p:" + _period);
            System.Console.WriteLine(DateTime.MinValue);
            System.Console.WriteLine(DateTime.MinValue.AddDays(0));
            DateTime temp = this.lastUpdate;

            if (this.period == "MIESIĄC")
                temp = temp.AddMonths(this.frequency * _period);
            else if (this.period == "DZIEŃ")
                temp = temp.AddDays(this.frequency * _period);
            else if (this.period == "TYDZIEŃ")
                temp = temp.AddDays(7 * this.frequency * _period);
            else if (this.period == "ROK")
                temp = temp.AddYears(this.frequency * _period);  

            if (this.CategoryID == 0)
            {
                foreach(var sT in Budget.Instance.SavingsTargets)
                {
                    if (sT.Value.Target.Equals(this.Name.Substring(14)))
                    {
                        sT.Value.AddMoney(this.Amount, sT.Key);
                        break;
                    }
                }
            }

            return new SinglePayment(this.Note, this.Amount, this.CategoryID, this.Type, "[Okresowy] " + this.Name, temp);
        }

        public DateTime CountNextDate()
        {
            if (period == "MIESIĄC")
                return lastUpdate.AddMonths(frequency);
            else if (period == "DZIEŃ")
                return lastUpdate.AddDays(frequency);
            else if (period == "TYDZIEŃ")
                return lastUpdate.AddDays(7 * frequency);
            else if (period == "ROK")
                return lastUpdate.AddYears(frequency);
            else
                return DateTime.Now;
        }

        static public List<WelcomePage.PaymentForDataGrid> CreateListOfSelectedPeriodPayments(PeriodPayment pP, DateTime lastDate)
        {
            List<PeriodPayment> periodPayments = new List<PeriodPayment>();
            if (pP.startDate >= DateTime.Today)
            {
                periodPayments.Add(pP);
            }
            else
            {
                PeriodPayment tempPP = new PeriodPayment(pP);
                while (tempPP.lastUpdate < DateTime.Today)
                {
                    tempPP = new PeriodPayment(tempPP);
                }
                periodPayments.Add(tempPP);
            }
            CheckAndAddElement(periodPayments, lastDate);

            List<WelcomePage.PaymentForDataGrid> providedPayments = new List<WelcomePage.PaymentForDataGrid>();
            foreach (PeriodPayment temp in periodPayments)
            {
                providedPayments.Add(new WelcomePage.PaymentForDataGrid(temp.Name, temp.Amount, "Okresowy", temp.lastUpdate, temp.Type, temp.CategoryID, 0));
            }
            return providedPayments;
        }

        static public void CheckAndAddElement(List<PeriodPayment> list, DateTime lastDate)
        {
            if (list[list.Count - 1].CountNextDate() <= lastDate)
            {
                list.Add(new PeriodPayment(list[list.Count - 1]));
                CheckAndAddElement(list, lastDate);
            }
        }
    }
}
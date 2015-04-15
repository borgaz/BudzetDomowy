using System;

namespace Budget.Nowy_budzet
{
    public class SalaryInfo
    {
        public SalaryInfo(String salaryName, int salaryValue, int salaryType, int repeatDate)
        {
            Name = salaryName;
            Value = salaryValue;
            Type = salaryType;
            Repeat = repeatDate;
        }
        public String Name { get; set; }

        public int Value { get; set; }

        public int Type { get; set; }

        public int Repeat { get; set; }
    }
}

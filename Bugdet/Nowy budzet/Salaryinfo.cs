using System;

namespace Bugdet.Nowy_budzet
{
    public class SalaryInfo
    {
        public SalaryInfo(String _salaryName, int _salaryValue, int _salaryType, int _repeatDate)
        {
            Name = _salaryName;
            Value = _salaryValue;
            Type = _salaryType;
            Repeat = _repeatDate;
        }
        public String Name { get; set; }

        public int Value { get; set; }

        public int Type { get; set; }

        public int Repeat { get; set; }
    }
}

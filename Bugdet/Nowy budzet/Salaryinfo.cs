using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugdet.Nowy_budzet
{
    public class Salaryinfo
    {
        private String salaryName;
        private int salaryValue;
        private int salaryType;
        private int repeatDate;
        public Salaryinfo(String _salaryName, int _salaryValue, int _salaryType, int _repeatDate)
        {
            salaryName = _salaryName;
            salaryValue = _salaryValue;
            salaryType = _salaryType;
            repeatDate = _repeatDate;
        }
        public String Name
        {
            get
            {
                return salaryName;
            }
            set
            {
                salaryName = value;
            }
        }
        public int Value
        {
            get
            {
                return salaryValue;
            }
            set
            {
                salaryValue = value;
            }
        }
        public int Type
        {
            get
            {
                return salaryType;
            }
            set
            {
                salaryType = value;
            }
        }
        public int Repeat
        {
            get
            {
                return repeatDate;
            }
            set
            {
                repeatDate = value;
            }
        }
    }
}

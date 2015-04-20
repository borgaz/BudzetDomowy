using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget
{
    class ComboBoxItem
    {
        private int id;
        private string value;

        public ComboBoxItem(int id,string value)
        {
            this.id = id;
            this.value = value;
        }

        public int Id
        {
            get { return id; }
        }

        public string Value
        {
            get { return value; }
        }

        public override string ToString()
        {
            return value;
        }
    }
}

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
        private Object value;

        public ComboBoxItem(int id,Object value)
        {
            this.id = id;
            this.value = value;
        }

        public int Id
        {
            get { return id; }
        }

        public Object Value
        {
            get { return value; }
        }
    }
}

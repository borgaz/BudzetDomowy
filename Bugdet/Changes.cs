using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget
{
    public class Changes
    {
        public Changes (Type type, int id)
        {
            this.Type = type;
            this.ID = id;
        }
        public Type Type
        {
            get;
            set;
        }

        public int ID
        {
            get;
            set;
        }
    }
}

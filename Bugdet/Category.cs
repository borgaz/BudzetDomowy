using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugdet
{
    public class Category
    {
        private string name;
        private string note;

        public Category() { }

        public Category(string name, string note)
        {
            this.name = name;
            this.note = note;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Note
        {
            get
            {
                return note;
            }
        }
    }
}
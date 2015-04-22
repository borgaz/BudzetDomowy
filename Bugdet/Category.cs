using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget
{
    public class Category
    {
        private string name; // nazwa kategorii
        private string note; // opis kategorii
        private bool type;   // typ kategorii (true - przychody, false - wydatki)

        public override string ToString()
        {
            return "NAME: " + name + ", NOTE: " + note + ", TYPE: " + type + "\n";
        }

        public Category(string name, string note, bool type)
        {
            this.name = name;
            this.note = note;
            this.type = type;
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string Note
        {
            get
            {
                return note;
            }
        }

        public bool Type
        {
            get
            {
                return type;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugdet
{
    public class Category
    {
        private int id;
        private string name;
        private string note;

        public String ToString()
        {
            return "ID: " + id + "NAME: " + name + "NOTE: " + note + "\n";
        }

        public Category() { }

        public Category(string name, string note)
        {
            this.name = name;
            this.note = note;
        }

        public Category(int id, string name, string note)
        {
            this.id = id;
            this.name = name;
            this.note = note;
        }

        public int ID
        {
            get
            {
                return id;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set 
            {
                if (value.Equals(""))
                {
                    name = value;
                }
            }
        }

        public string Note
        {
            get
            {
                return note;
            }
            set
            {
                note = value;
            }
        }
    }
}
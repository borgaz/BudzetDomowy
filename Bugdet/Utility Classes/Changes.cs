using System;

namespace Budget.Utility_Classes
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

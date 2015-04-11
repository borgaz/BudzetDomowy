using System;

namespace Bugdet
{
    abstract public class Payment
    {
        private int _categoryId;
        private double _amount;
        private String _name;
        private int _type;
        private String _note;

        public Payment(int categoryId, double amount, String note, int type, String name)
        {
            this._categoryId = categoryId;
            this._amount = amount;
            this._note = note;
            this._type = type;
            this._name = name;
        }

        public int CategoryId
        {
            get
            {
                return _categoryId;
            }
        }

        public double Amount
        {
            get
            {
                return _amount;
            }
        }

        public String Note
        {
            get
            {
                return _note;
            }
        }

        public int Type
        {
            get
            {
                return _type;
            }

        }

        public String Name
        {
            get
            {
                return _name;
            }
        }
    }
}
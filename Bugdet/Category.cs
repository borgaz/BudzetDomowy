namespace Bugdet
{
    public class Category
    {
        private string _name;
        private string _note;

        public Category(string name, string note)
        {
            this._name = name;
            this._note = note;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Note
        {
            get
            {
                return _note;
            }
        }
    }
}
namespace Bugdet
{
    public class Category
    {
        private string name;

        public Category() { }

        public Category(string name, string note)
        {
            this.name = name;
            this.Note = note;
        }

        public Category(int id, string name, string note)
        {
            this.ID = id;
            this.name = name;
            this.Note = note;
        }

        public int ID { get; private set; }

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

        public string Note { get; set; }
    }
}
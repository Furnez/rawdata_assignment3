namespace Assignment_3
{
    public class Category
    {
        public int Cid { get; set; }
        public string Name { get; set; }

        public Category(string name)
        {
            this.Cid = 13;
            this.Name = name;
        }
    }
}
namespace ETIDrive_Entity
{
    public class Category
    {
        public int CategoryId { get; set; }
        public required string Name { get; set; }
        public List<Data>? Datas { get; set; }
    }
}
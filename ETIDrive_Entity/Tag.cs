using ETIDrive_Entity.Juction_Tables;

namespace ETIDrive_Entity
{
    public class Tag
    {
        public int TagId { get; set; }
        public required string Name { get; set; }
        public List<DataTag>? DataTags { get; set;}
    }
}
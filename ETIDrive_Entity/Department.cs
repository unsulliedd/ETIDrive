using ETIDrive_Entity.Identity;

namespace ETIDrive_Entity
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public required string Name { get; set; }
        public int? FolderId { get; set; }
        public Folder? Folder { get; set; }
        public List<User>? User { get; set; }
    }
}

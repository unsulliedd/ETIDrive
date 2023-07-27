using ETIDrive_Entity.Identity;
using ETIDrive_Entity.Juction_Tables;

namespace ETIDrive_Entity
{
    public class Folder
    {
        public int FolderId { get; set; }
        public required string Name { get; set; }
        public string? FolderDescription { get; set; }
        public required string FolderPath { get; set; }
        public int? ParentFolderId { get; set; }
        public Folder? ParentFolder { get; set; }
        public List<Folder>? SubFolders { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public List<UserFolder>? UserFolders { get; set; }
        public List<Data>? Datas { get; }

        public required string CreatedById { get; set; }
        public required User CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string ModifiedById { get; set; }
        public required User ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
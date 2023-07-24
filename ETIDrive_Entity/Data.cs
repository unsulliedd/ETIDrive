using ETIDrive_Entity.Identity;
using ETIDrive_Entity.Juction_Tables;

namespace ETIDrive_Entity
{
    public class Data
    {
        public int DataId { get; set; }
        public required string Name { get; set; }
        public string? DataDescription { get; set; }
        public required string DataPath { get; set; }
        public long DataSize { get; set; }
        public required string DataType { get; set; }
        public int FolderId { get; set; }
        public required Folder Folder { get; set; }
        public required List<DataPermission> DataPermissions { get; set; }
        public int? CategoryId { get; set; }
        public List<DataTag>? DataTags { get; set; }
        public Category? Category { get; set; }

        public required string CreatedById { get; set; }
        public required User CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string ModifiedById { get; set; }
        public required User ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}

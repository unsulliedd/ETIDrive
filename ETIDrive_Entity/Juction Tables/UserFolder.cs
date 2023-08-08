using ETIDrive_Entity.Identity;

namespace ETIDrive_Entity.Juction_Tables
{
    public class UserFolder
    {
        public string UserId { get; set; }
        public required User User { get; set; }
        public int FolderId { get; set; }
        public required Folder Folder { get; set; }
        public bool IsOwner { get; set; }
        public bool HasPermission { get; set; }
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanDownload { get; set; }
        public bool CanUpload { get; set; }
    }
}
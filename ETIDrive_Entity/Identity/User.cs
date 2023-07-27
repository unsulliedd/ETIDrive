using ETIDrive_Entity.Juction_Tables;
using Microsoft.AspNetCore.Identity;

namespace ETIDrive_Entity.Identity
{
    public class User : IdentityUser
    {
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public List<Data>? CreatedFiles { get; set; }
        public List<Data>? LastModifiedFiles { get; set; }
        public List<Folder>? CreatedFolders { get; set; }
        public List<Folder>? LastModifiedFolders { get; set; }
        public List<DataPermission>? FilePermissions { get; set; }
        public List<UserFolder>? UserFolders { get; set; }
    }
}
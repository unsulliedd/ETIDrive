
namespace ETIDrive_WebUI.Models.FolderModels
{
    public class FolderCreationViewModel
    {
        public string FolderName { get; set; }
        public string? FolderDescription { get; set; }
        public List<UserPermissionViewModel>? UserPermissions { get; set; }
    }

    public class UserPermissionViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public bool HasPermission { get; set; }
        public bool IsOwner {get; set; }
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanDownload { get; set; }
        public bool CanUpload { get; set; }
        public int? DepartmentId { get; set; }
    }
}

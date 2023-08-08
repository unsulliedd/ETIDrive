using ETIDrive_Entity;
using ETIDrive_Entity.Identity;
using ETIDrive_Entity.Juction_Tables;

namespace ETIDrive_Data.Abstract
{
    public interface IFolderRepository : IGenericRepository<Folder>
    {
        Task CreateFolderWithPermissions(Folder folder, User user, int? parentFolderId = null);
        Task SetUserFolderPermissions(Folder folder, User user, bool canView, bool canEdit, bool canDelete, bool canDownload, bool canUpload, bool isOwner, bool hasPermission);
        Task ModifyFolderPermissions(int folderId, List<User> users, bool canView, bool canEdit, bool canDelete, bool canDownload, bool canUpload);
        Task<List<Folder>> GetUserCreatedFolders(string createdByUserId);
        Task<List<Folder>> GetFoldersWithUserPermissions(string userId);
        Task<List<Folder>> GetSubfolders(int parentFolderId);
        Task<List<Data>> GetFilesInFolderAndSubfolders(int folderId);
        Task DeleteFolderWithContents(int folderId);
        Task CopyFolder(int folderId, int destinationFolderId);
        Task CutFolder(int folderId, int destinationFolderId);
        Task<List<Folder>> SearchFolders(string searchKeyword);
    }
}

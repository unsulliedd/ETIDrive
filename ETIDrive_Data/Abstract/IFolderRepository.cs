using ETIDrive_Entity;
using ETIDrive_Entity.Identity;

namespace ETIDrive_Data.Abstract
{
    public interface IFolderRepository : IGenericRepository<Folder>
    {
        Task CreateFolderWithPermissions(Folder folder, List<User> usersWithPermissions, Folder? parentFolder = null);
        Task SetUserFolderPermissions(int folderId, string userId, bool canView, bool canEdit, bool canDelete, bool canDownload, bool canUpload);
        Task ModifyFolderPermissions(int folderId, List<User> users, bool canView, bool canEdit, bool canDelete, bool canDownload, bool canUpload);
        Task<List<Folder>> GetUserCreatedFolders(string createdByUserId);
        Task<List<Folder>> GetFoldersWithUserPermissions(string userId);
        Task<List<Folder>> GetSubfolders(int parentFolderId);
        Task<List<Data>> GetFilesInFolderAndSubfolders(int folderId);
        Task DeleteFolderWithContents(int folderId);
        Task CopyFolder(int folderId, int destinationFolderId);
        Task CutFolder(int folderId, int destinationFolderId);
        Task<List<Folder>> SearchFolders(string searchKeyword);
        Task LogFolderAccessHistory(int folderId, string userId);
    }
}

using ETIDrive_Data.Abstract;
using ETIDrive_Entity;
using ETIDrive_Entity.Identity;
using ETIDrive_Entity.Juction_Tables;
using Microsoft.EntityFrameworkCore;

namespace ETIDrive_Data.Concrete
{
    public class FolderRepository : GenericRepository<Folder>, IFolderRepository
    {
        private new readonly ETIDriveContext context;

        public FolderRepository(ETIDriveContext context) : base(context)
        {
            this.context = context;
        }
        public async Task CreateFolderWithPermissions(Folder folder, List<User> usersWithPermissions, Folder? parentFolder = null)
        {
            context.Folders.Add(folder);
            if (parentFolder != null)
            {
                folder.ParentFolderId = parentFolder.FolderId;
            }
            context.SaveChanges();

            foreach (var user in usersWithPermissions)
            {
                var userFolder = new UserFolder
                {
                    User = user,
                    Folder = folder,
                    IsOwner = false,
                    HasPermission = true,
                    CanView = true,
                    CanEdit = true,
                    CanDelete = true,
                    CanDownload = true,
                    CanUpload = true
                };

                context.UserFolders.Add(userFolder);
            }

            await context.SaveChangesAsync();
        }

        public async Task SetUserFolderPermissions(int folderId, string userId, bool canView, bool canEdit, bool canDelete, bool canDownload, bool canUpload)
        {
            var userFolder = await context.UserFolders.FirstOrDefaultAsync(uf => uf.FolderId == folderId && uf.User.Id == userId);

            if (userFolder != null)
            {
                userFolder.CanView = canView;
                userFolder.CanEdit = canEdit;
                userFolder.CanDelete = canDelete;
                userFolder.CanDownload = canDownload;
                userFolder.CanUpload = canUpload;

                context.UserFolders.Update(userFolder);
                await context.SaveChangesAsync();
            }
        }

        public async Task ModifyFolderPermissions(int folderId, List<User> users, bool canView, bool canEdit, bool canDelete, bool canDownload, bool canUpload)
        {
            foreach (var user in users)
            {
                var userFolder = await context.UserFolders.FirstOrDefaultAsync(uf => uf.FolderId == folderId && uf.User.Id == user.Id);

                if (userFolder != null)
                {
                    userFolder.CanView = canView;
                    userFolder.CanEdit = canEdit;
                    userFolder.CanDelete = canDelete;
                    userFolder.CanDownload = canDownload;
                    userFolder.CanUpload = canUpload;

                    context.UserFolders.Update(userFolder);
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task<List<Folder>> GetUserCreatedFolders(string createdByUserId)
        {
            return await context.Folders
                .Where(f => f.CreatedById == createdByUserId)
                .ToListAsync();
        }

        public async Task<List<Folder>> GetFoldersWithUserPermissions(string userId)
        {
            return await context.Folders
                .Where(f => f.UserFolders.Any(uf => uf.User.Id == userId && uf.HasPermission))
                .ToListAsync();
        }

        public async Task<List<Folder>> GetSubfolders(int parentFolderId)
        {
            return await context.Folders
                .Where(f => f.ParentFolderId == parentFolderId)
                .ToListAsync();
        }

        public async Task<List<Data>> GetFilesInFolderAndSubfolders(int folderId)
        {
            var folder = await context.Folders
                .Include(f => f.SubFolders)
                .ThenInclude(sf => sf.SubFolders)
                .Include(f => f.Datas)
                .FirstOrDefaultAsync(f => f.FolderId == folderId);

            if (folder == null)
                return new List<Data>();

            var filesInFolderAndSubfolders = new List<Data>();
            CollectFilesInFolderAndSubfolders(folder, filesInFolderAndSubfolders);
            return filesInFolderAndSubfolders;
        }

        private void CollectFilesInFolderAndSubfolders(Folder folder, List<Data> filesInFolderAndSubfolders)
        {
            filesInFolderAndSubfolders.AddRange(folder.Datas);

            foreach (var subfolder in folder.SubFolders)
            {
                CollectFilesInFolderAndSubfolders(subfolder, filesInFolderAndSubfolders);
            }
        }

        public async Task DeleteFolderWithContents(int folderId)
        {
            var folder = await context.Folders
                .Include(f => f.Datas)
                .Include(f => f.SubFolders)
                .FirstOrDefaultAsync(f => f.FolderId == folderId);

            if (folder == null)
                return;

            DeleteFolderContents(folder);
            context.Folders.Remove(folder);
            await context.SaveChangesAsync();
        }

        private void DeleteFolderContents(Folder folder)
        {
            context.Datas.RemoveRange(folder.Datas);

            foreach (var subfolder in folder.SubFolders)
            {
                DeleteFolderContents(subfolder);
            }
        }

        public async Task CopyFolder(int folderId, int destinationFolderId)
        {
            var sourceFolder = await context.Folders
                .Include(f => f.Datas)
                .Include(f => f.SubFolders)
                .FirstOrDefaultAsync(f => f.FolderId == folderId);

            if (sourceFolder == null)
                return;

            var destinationFolder = await context.Folders
                .FirstOrDefaultAsync(f => f.FolderId == destinationFolderId);

            if (destinationFolder == null)
                return;

            var newFolder = new Folder
            {
                Name = sourceFolder.Name,
                FolderDescription = sourceFolder.FolderDescription,
                FolderPath = sourceFolder.FolderPath,
                DepartmentId = sourceFolder.DepartmentId,
                CreatedById = sourceFolder.CreatedById,
                CreatedBy = sourceFolder.CreatedBy,
                ModifiedById = sourceFolder.ModifiedById,
                ModifiedBy = sourceFolder.ModifiedBy,
            };

            destinationFolder.SubFolders.Add(newFolder);
            await context.SaveChangesAsync();

            CopyDataAndSubfolders(sourceFolder, newFolder);
        }

        private void CopyDataAndSubfolders(Folder sourceFolder, Folder destinationFolder)
        {
            foreach (var data in sourceFolder.Datas)
            {
                var newData = new Data
                {
                    Name = data.Name,
                    DataDescription = data.DataDescription,
                    DataPath = data.DataPath,
                    DataSize = data.DataSize,
                    DataType = data.DataType,
                    FolderId = destinationFolder.FolderId,
                    DataPermissions = data.DataPermissions,
                    CreatedBy = destinationFolder.CreatedBy,
                    CreatedById = destinationFolder.CreatedById,
                    ModifiedBy = destinationFolder.ModifiedBy,
                    ModifiedById = destinationFolder.ModifiedById,
                    Folder = data.Folder,
                };

                destinationFolder.Datas.Add(newData);
            }

            foreach (var subfolder in sourceFolder.SubFolders)
            {
                var newSubfolder = new Folder
                {
                    Name = subfolder.Name,
                    FolderDescription = subfolder.FolderDescription,
                    FolderPath = subfolder.FolderPath,
                    DepartmentId = subfolder.DepartmentId,
                    CreatedBy = subfolder.CreatedBy,
                    CreatedById = subfolder.CreatedById,
                    ModifiedBy= subfolder.ModifiedBy,
                    ModifiedById = subfolder.ModifiedById,
                };

                destinationFolder.SubFolders.Add(newSubfolder);
                context.SaveChanges();

                CopyDataAndSubfolders(subfolder, newSubfolder);
            }
        }

        public async Task CutFolder(int folderId, int destinationFolderId)
        {
            var sourceFolder = await context.Folders
                .Include(f => f.ParentFolder)
                .FirstOrDefaultAsync(f => f.FolderId == folderId);

            if (sourceFolder == null)
                return;

            var destinationFolder = await context.Folders
                .FirstOrDefaultAsync(f => f.FolderId == destinationFolderId);

            if (destinationFolder == null)
                return;

            if (sourceFolder.ParentFolderId.HasValue)
            {
                var sourceParentFolder = await context.Folders
                    .FirstOrDefaultAsync(f => f.FolderId == sourceFolder.ParentFolderId.Value);

                if (sourceParentFolder != null)
                {
                    sourceParentFolder.SubFolders.Remove(sourceFolder);
                }
            }

            destinationFolder.SubFolders.Add(sourceFolder);
            sourceFolder.ParentFolderId = destinationFolder.FolderId;

            await context.SaveChangesAsync();
        }

        public async Task<List<Folder>> SearchFolders(string searchKeyword)
        {
            return await context.Folders
                .Where(f => f.Name.Contains(searchKeyword) || f.FolderDescription.Contains(searchKeyword))
                .ToListAsync();
        }

        public async Task LogFolderAccessHistory(int folderId, string userId)
        {
            // Implement logging of folder access history
            // You can add a new table/entity to log folder access with folderId, userId, and timestamp
            // Alternatively, update the Folder entity to store access history properties like LastAccessedAt and LastAccessedByUserId
            // Depending on your application's requirements, you can customize this method accordingly.
        }
    }
}

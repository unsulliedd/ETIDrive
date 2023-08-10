using ETIDrive_Data.Abstract;
using ETIDrive_Entity;
using ETIDrive_Entity.Identity;
using ETIDrive_WebUI.Models.FolderModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETIDrive_WebUI.Controllers
{
    [Authorize]
    public class FolderController : Controller
    {
        private readonly IFolderRepository _folderRepository;
        private readonly UserManager<User> _userManager;

        public FolderController(IFolderRepository folderRepository, UserManager<User> userManager)
        {
            _folderRepository = folderRepository;
            _userManager = userManager;
        }

        public IActionResult CreateFolder()
        {
            var viewModel = new FolderCreationViewModel();

            return View(viewModel);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFolder(FolderCreationViewModel model, int? folderId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (folderId.HasValue)
            {
                var newFolder = new Folder
                {
                    Name = model.FolderName,
                    FolderDescription = model.FolderDescription,
                    FolderPath = Path.Combine("ProjectFiles", model.FolderName),
                    CreatedById = currentUser.Id,
                    CreatedBy = currentUser,
                    CreatedAt = DateTime.Now,
                    ModifiedBy = currentUser,
                    ModifiedAt = DateTime.Now,
                    ModifiedById = currentUser.Id,
                    ParentFolderId = folderId
                };
                await _folderRepository.CreateFolderWithPermissions(newFolder, currentUser, folderId);

                Directory.CreateDirectory(newFolder.FolderPath);
                foreach (var userPermission in model.UserPermissions)
                {
                    var user = await _userManager.FindByIdAsync(userPermission.UserId);

                    if (userPermission.CanView || userPermission.CanEdit || userPermission.CanDelete || userPermission.CanDownload || userPermission.CanUpload)
                    {
                        await _folderRepository.SetUserFolderPermissions(newFolder, user, userPermission.CanView, userPermission.CanEdit, userPermission.CanDelete, userPermission.CanDownload, userPermission.CanUpload, false, true);
                    }
                }
                return RedirectToAction("FolderContent", new { folderId });
            }
            else
            {
                var newFolder = new Folder
                {
                    Name = model.FolderName,
                    FolderDescription = model.FolderDescription,
                    FolderPath = Path.Combine("ProjectFiles", model.FolderName),
                    CreatedById = currentUser.Id,
                    CreatedBy = currentUser,
                    CreatedAt = DateTime.Now,
                    ModifiedBy = currentUser,
                    ModifiedAt = DateTime.Now,
                    ModifiedById = currentUser.Id,
                };
                await _folderRepository.CreateFolderWithPermissions(newFolder, currentUser, null);

                Directory.CreateDirectory(newFolder.FolderPath);
                foreach (var userPermission in model.UserPermissions)
                {
                    var user = await _userManager.FindByIdAsync(userPermission.UserId);

                    if (userPermission.CanView || userPermission.CanEdit || userPermission.CanDelete || userPermission.CanDownload || userPermission.CanUpload)
                    {
                        await _folderRepository.SetUserFolderPermissions(newFolder, user, userPermission.CanView, userPermission.CanEdit, userPermission.CanDelete, userPermission.CanDownload, userPermission.CanUpload, false, true);
                    }
                }
                return RedirectToAction("UserFolder", "Folder");
            }
        }
        public async Task<IActionResult> GetUserListAsync(int pageIndex = 1, int pageSize = 8, int? selectedDepartmentId = null)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var query = _userManager.Users.Where(user => user.Id != currentUser.Id);

            if (selectedDepartmentId.HasValue)
            {
                query = query.Where(user => user.DepartmentId == selectedDepartmentId.Value);
            }

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var startIndex = (pageIndex - 1) * pageSize;
            var endIndex = Math.Min(startIndex + pageSize - 1, totalCount - 1);

            var pagedUsers = await query.Skip(startIndex).Take(pageSize).ToListAsync();

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = pageIndex;

            var model = pagedUsers.Select(user => new UserPermissionViewModel
            {
                UserId = user.Id,
                Username = user.UserName,
                CanView = false,
                CanEdit = false,
                CanDelete = false,
                CanDownload = false,
                CanUpload = false,
                DepartmentId = user.DepartmentId
            }).ToList();

            return PartialView("Partials/_UserListPartial", model);
        }
        public async Task<IActionResult> UserFolder()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userId = currentUser.Id;
            var userFolders = await _folderRepository.GetUserCreatedFolders(userId);

            var model = new UserFolderViewModel
            {
                Folders = userFolders
            };
            return View(model);
        }
        public async Task<IActionResult> FolderContent(int folderId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userId = currentUser.Id;

            if (!_folderRepository.CheckUserFolderAccess(userId, folderId))
            {
                return Forbid();
            }

            List<Folder> subfolders = await _folderRepository.GetSubfolders(folderId);

            var model = new FolderSubFoldersViewModel
            {
                Subfolders = subfolders
            };
            var folder = _folderRepository.GetbyId(folderId);
            ViewBag.folder = folder;
            ViewBag.CurrentFolderId = folderId;
            return View(model);
        }
    }
}

using ETIDrive_Data.Abstract;
using ETIDrive_Entity;
using ETIDrive_Entity.Identity;
using ETIDrive_WebUI.Models.FolderModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETIDrive_WebUI.Controllers
{
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
            return View();
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFolder(FolderCreationViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
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
            await _folderRepository.CreateFolderWithPermissions(newFolder, currentUser);

            Directory.CreateDirectory(newFolder.FolderPath);

            foreach (var userPermission in model.UserPermissions)
            {
                var user = await _userManager.FindByIdAsync(userPermission.UserId);
                    
                if (userPermission.CanView || userPermission.CanEdit || userPermission.CanDelete || userPermission.CanDownload || userPermission.CanUpload)
                {
                    await _folderRepository.SetUserFolderPermissions(newFolder, user, userPermission.CanView, userPermission.CanEdit, userPermission.CanDelete, userPermission.CanDownload, userPermission.CanUpload, false, true);
                }
                    
            }
            return RedirectToAction("Index", "Home"); 
        }
        public IActionResult GetUserList(int pageIndex = 1, int pageSize = 8, int? selectedDepartmentId = null)
        {
            var users = _userManager.Users.ToList();

            if (selectedDepartmentId.HasValue)
            {
                users = users.Where(user => user.DepartmentId == selectedDepartmentId.Value).ToList();
            }

            var totalCount = users.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var startIndex = (pageIndex - 1) * pageSize;
            var endIndex = Math.Min(startIndex + pageSize - 1, totalCount - 1);

            var pagedUsers = users.Skip(startIndex).Take(pageSize).ToList();

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
    }
}

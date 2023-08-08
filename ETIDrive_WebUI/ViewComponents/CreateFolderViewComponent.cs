using ETIDrive_Entity.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ETIDrive_WebUI.Models.FolderModels;
using ETIDrive_Data.Abstract;

namespace ETIDrive_WebUI.ViewComponents
{
    public class CreateFolderViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;
        private readonly IDepartmentRepository _departmentRepository;

        public CreateFolderViewComponent(UserManager<User> userManager,IDepartmentRepository departmentRepository)
        {
            _userManager = userManager;
            _departmentRepository = departmentRepository;
        }

        public IViewComponentResult Invoke(int? selectedDepartmentId = null)
        {
            var users = _userManager.Users.ToList();
            ViewBag.Departments = _departmentRepository.GetAll().ToList();
            ViewBag.SelectedDepartment = selectedDepartmentId;
            if (selectedDepartmentId.HasValue)
            {
                users = users.Where(user => user.DepartmentId == selectedDepartmentId.Value).ToList();
            }
            var foldersCreationViewModel = new FolderCreationViewModel
            {
                FolderName = string.Empty,
                FolderDescription = string.Empty,

                UserPermissions = users.Select(user => new UserPermissionViewModel
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    CanView = false,
                    CanEdit = false,
                    CanDelete = false,
                    CanDownload = false,
                    CanUpload = false,
                    DepartmentId = user.Department?.DepartmentId
                }).ToList()
            };

            return View("CreateFolder", foldersCreationViewModel);
        }
    }
}

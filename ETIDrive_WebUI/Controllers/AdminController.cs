using ETIDrive_Data.Abstract;
using ETIDrive_Entity;
using ETIDrive_Entity.Identity;
using ETIDrive_WebUI.Models.AdminModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETIDrive_WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IDepartmentRepository _departmentRepository;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, IDepartmentRepository departmentRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _departmentRepository = departmentRepository;
        }
        public IActionResult AdminPanel()
        {
            return View();
        }
        // User
        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.ToList();
            var usersViewModel = new List<UserListModel>();
            
            foreach (var user in users)
            {
                if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Email))
                {
                    var userViewModel = new UserListModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Roles = await _userManager.GetRolesAsync(user)
                    };
                    usersViewModel.Add(userViewModel);
                }
            }
            return View(usersViewModel);
        }
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && !string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Email))
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roles = _roleManager.Roles.Select(i => i.Name);

                ViewBag.Roles = roles;
                var model = new UserEditModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    SelectedRoles = selectedRoles.ToList(),
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserEditModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.UserName = model.UserName;
                    user.Email = model.Email;

                    var updateResult = await _userManager.UpdateAsync(user);

                    if (updateResult.Succeeded)
                    {
                        var currentRoles = await _userManager.GetRolesAsync(user);
                        selectedRoles ??= Array.Empty<string>();

                        // Remove roles that are not selected
                        var rolesToRemove = currentRoles.Except(selectedRoles);
                        await _userManager.RemoveFromRolesAsync(user, rolesToRemove.ToArray());

                        // Add roles that are selected
                        var rolesToAdd = selectedRoles.Except(currentRoles);
                        await _userManager.AddToRolesAsync(user, rolesToAdd.ToArray());

                        return RedirectToAction("Users");
                    }
                    else
                    {
                        foreach (var error in updateResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                return RedirectToAction("Users");
            }
            foreach (var modelStateEntry in ModelState.Values)
            {
                foreach (var error in modelStateEntry.Errors)
                {
                    Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Users");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("Users");
        }
        // Role
        public IActionResult Roles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var roleExists = await _roleManager.RoleExistsAsync(model.Name);
                if (!roleExists)
                {
                    var role = new IdentityRole(model.Name);
                    var result = await _roleManager.CreateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Roles");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "This role already exists.");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var member = new List<User>();
                var nonmember = new List<User>();

                var userList = await _userManager.Users.ToListAsync();

                foreach (var user in userList)
                {
                    if (role.Name != null && await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        member.Add(user);
                    }
                    else
                    {
                        nonmember.Add(user);
                    }
                }

                var model = new RoleDetails()
                {
                    Role = role,
                    Members = member,
                    NonMembers = nonmember
                };
                return View(model);
            }
            return RedirectToAction("Error", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.IdsToAdd != null)
                {
                    foreach (var userId in model.IdsToAdd)
                    {
                        var user = await _userManager.FindByIdAsync(userId);
                        if (user != null && model.RoleName != null)
                        {
                            await _userManager.AddToRoleAsync(user, model.RoleName);
                        }
                    }
                }
                if (model.IdsToDelete != null)
                {
                    foreach (var userId in model.IdsToDelete)
                    {
                        var user = await _userManager.FindByIdAsync(userId);
                        if (user != null && model.RoleName != null)
                        {
                            await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        }
                    }
                }
            }
            return RedirectToAction("Roles");
        }
        // Departments
        public async Task<IActionResult> Departments()
        {
            var departments = await _departmentRepository.GetAllAsync();
            var departmentViewModels = new List<DepartmentViewModel>();

            foreach (var department in departments)
            {
                var users = await _departmentRepository.GetUserbyDepartment(department.DepartmentId);

                var departmentViewModel = new DepartmentViewModel
                {
                    DepartmentId = department.DepartmentId,
                    Name = department.Name,
                    Users = users
                };

                departmentViewModels.Add(departmentViewModel);
            }

            return View(departmentViewModels);
        }
        [HttpGet]
        public IActionResult CreateDepartment()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                await _departmentRepository.CreateAsync(department);
                return RedirectToAction("Departments");
            }
            return View(department);
        }
        [HttpGet]
        public async Task<IActionResult> EditDepartment(int id)
        {
            var department = await _departmentRepository.GetbyIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            var allUsers = await _userManager.Users.ToListAsync();
            var member = new List<UserWithDepartment>();
            var nonmember = new List<UserWithDepartment>();

            foreach (var user in allUsers)
            {
                var userViewModel = new UserWithDepartment
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    DepartmentName = user.DepartmentId != null
                        ? _departmentRepository.GetbyId(user.DepartmentId.Value)?.Name ?? "Not Assigned"
                        : "Not Assigned"
                };

                if (_departmentRepository.IsInDepartment(user.Id, department.DepartmentId))
                {
                    member.Add(userViewModel);
                }
                else
                {
                    nonmember.Add(userViewModel);
                }
            }

            var departmentEditModel = new DepartmentDetailModel
            {
                DepartmentId = department.DepartmentId,
                Name = department.Name,
                Members = member,
                NonMembersWithDepartment = nonmember
            };

            return View(departmentEditModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditDepartment(DepartmentEditModel model)
        {
            if (ModelState.IsValid)
            {
                var departmentname = await _departmentRepository.GetbyIdAsync(model.DepartmentId);
                if (departmentname == null)
                {
                    return NotFound();
                }

                departmentname.Name = model.Name;
                _departmentRepository.Update(departmentname);

                if (model.IdsToAdd != null || model.IdsToDelete != null) 
                {
                    // Ids to add
                    if (model.IdsToAdd != null)
                    {
                        foreach (var userId in model.IdsToAdd)
                        {
                            var user = await _userManager.FindByIdAsync(userId);
                            if (user != null)
                            {
                                var dep = _departmentRepository.GetbyId(model.DepartmentId);
                                _departmentRepository.AddUserToDepartment(user, dep);
                            }
                        }
                    }
                    // Ids to delete
                    if (model.IdsToDelete != null)
                    {
                        foreach (var userId in model.IdsToDelete)
                        {
                            var user = await _userManager.FindByIdAsync(userId);
                            if (user != null)
                            {
                                _departmentRepository.RemoveUserFromDepartment(user, model.DepartmentId);
                            }
                        }
                    }
                }
                return RedirectToAction("Departments");
            }

            // If ModelState is not valid
            var department = await _departmentRepository.GetbyIdAsync(model.DepartmentId);
            var member = new List<UserWithDepartment>();
            var nonmember = new List<UserWithDepartment>();
            var allUsers = await _userManager.Users.ToListAsync();

            foreach (var user in allUsers)
            {
                var userViewModel = new UserWithDepartment
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    DepartmentName = user.DepartmentId != null
                        ? _departmentRepository.GetbyId(user.DepartmentId.Value)?.Name ?? "Not Assigned"
                        : "Not Assigned"
                };

                if (_departmentRepository.IsInDepartment(user.Id, department.DepartmentId))
                {
                    member.Add(userViewModel);
                }
                else
                {
                    nonmember.Add(userViewModel);
                }
            }

            var departmentDetailModel = new DepartmentDetailModel
            {
                DepartmentId = department.DepartmentId,
                Name = department.Name,
                Members = member,
                NonMembersWithDepartment = nonmember
            };

            return View(departmentDetailModel);
        }

    }
}
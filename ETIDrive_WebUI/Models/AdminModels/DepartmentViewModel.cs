using ETIDrive_Entity.Identity;
using System.ComponentModel.DataAnnotations;

namespace ETIDrive_WebUI.Models.AdminModels
{
    public class DepartmentViewModel
    {
        public int DepartmentId { get; set; }
        public required string Name { get; set; }
        public List<User>? Users { get; set; }
    }
    public class CreateDepartmentModel
    {
        [Required(ErrorMessage = "Department Name is required.")]
        [Display(Name = "Department Name")]
        public required string Name { get; set; }
    }

    public class DepartmentDetailModel
    {
        public int DepartmentId { get; set; }
        [Required(ErrorMessage = "Department Name is required.")]
        [Display(Name = "Department Name")]
        public required string Name { get; set; }
        public IEnumerable<UserWithDepartment>? Members { get; set; }
        public IEnumerable<UserWithDepartment>? NonMembersWithDepartment { get; set; }
    }

    public class UserWithDepartment
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? DepartmentName { get; set; }
    }

    public class DepartmentEditModel
    {
        public int DepartmentId { get; set; }
        public required string Name { get; set; }
        public string[]? IdsToAdd { get; set; }
        public string[]? IdsToDelete { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ETIDrive_WebUI.Models.AdminModels
{
    public class UserListModel
    {
        public required string Id { get; set; }

        [Required]
        [Display(Name = "Username")]
        public required string UserName { get; set; }

        [Required]
        public required string Email { get; set; }
        public IList<string>? Roles { get; set; }
    }

    public class UserEditModel
    {
        public required string Id { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Please enter the user name.")]
        public required string UserName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please enter the email address.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public required string Email { get; set; }
        public List<string>? SelectedRoles { get; set; }
    }
}

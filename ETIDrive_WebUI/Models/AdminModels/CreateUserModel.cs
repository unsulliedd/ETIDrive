using System.ComponentModel.DataAnnotations;

namespace ETIDrive_WebUI.Models.AdminModels
{
    public class CreateUserModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
    }
}

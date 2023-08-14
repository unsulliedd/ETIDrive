using System.ComponentModel.DataAnnotations;

namespace ETIDrive_WebUI.Models.AccountModels
{
    public class LoginModel
    {
        public required string UserName { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}

using ETIDrive_Entity.Identity;
using ETIDrive_WebUI.Models.AccountModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices.Protocols;
using System.Net;

namespace ETIDrive_WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginModel ());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ActiveDirectoryAuthentication(model.UserName, model.Password))
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Hatalı kullanıcı adı veya parola.");
            return View();
        }


        private bool ActiveDirectoryAuthentication(string username, string password)
        {
            try
            {
                using (var ldapConnection = new LdapConnection(new LdapDirectoryIdentifier("ALIENWARE")))
                {
                    ldapConnection.SessionOptions.ProtocolVersion = 3;
                    ldapConnection.AuthType = AuthType.Negotiate;

                    ldapConnection.Bind(new NetworkCredential(username, password));

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

    }

}

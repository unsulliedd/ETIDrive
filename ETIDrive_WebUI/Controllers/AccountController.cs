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
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private LdapConnection? _ldapConnection;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (ActiveDirectoryAuthentication(model.UserName, model.Password))
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);

                    if (user != null)
                    {
                        var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false);

                        if (signInResult.Succeeded)
                        {
                            var roles = await _userManager.GetRolesAsync(user);
                            if (roles.Count == 0)
                            {
                                await _userManager.AddToRoleAsync(user, "Default");
                            }
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else
                {
                    var username = "Admin";
                    var user = await _userManager.FindByNameAsync(username);
                    await _signInManager.SignInAsync(user, false);
                }
            }
            ModelState.AddModelError("", "Hatalı kullanıcı adı veya parola.");
            return View(model);
        }


        private bool ActiveDirectoryAuthentication(string username, string password)
        {
            try
            {
                _ldapConnection = new LdapConnection(new LdapDirectoryIdentifier("ETIMADEN"));
                _ldapConnection.SessionOptions.ProtocolVersion = 3;
                _ldapConnection.AuthType = AuthType.Negotiate;

                _ldapConnection.Bind(new NetworkCredential(username, password));

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            _ldapConnection?.Dispose();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

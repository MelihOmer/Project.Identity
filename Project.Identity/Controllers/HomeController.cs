using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Identity.Entities;
using Project.Identity.Models;
using System.Diagnostics;
using System.Security.Claims;
using System.Xml.Linq;

namespace Project.Identity.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public  IActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new UserCreateModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = model.Email,
                    UserName = model.UserName,
                    FirstName = model.FirstName

                };


                var identityResult = await _userManager.CreateAsync(user, model.Password);
                if (identityResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "member");
                    return RedirectToAction("Index");
                }
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult SignIn(string returnUrl)
        {
            return View(new UserSignInModel() { ReturnUrl=returnUrl});
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInModel model)
        {
            if (ModelState.IsValid)
            {

                var singInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);
                var user = await _userManager.FindByNameAsync(model.UserName);

                    var roles = await _userManager.GetRolesAsync(user);

                    if (singInResult.Succeeded)
                    {

                        if (!string.IsNullOrEmpty(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        if (roles.Contains("Admin"))
                        {
                            return RedirectToAction("AdminPanel");
                        }
                        else
                        {
                            return RedirectToAction("Panel");
                        }

                    }

                else if (singInResult.IsLockedOut)
                {
                    var lockOutEnd = await _userManager.GetLockoutEndDateAsync(user);
                    ModelState.AddModelError("", $"Hesap Askıya Alınmıştır {(lockOutEnd.Value.UtcDateTime-DateTime.UtcNow).Minutes} dk Sonra Tekrar Deneyiniz.");
                }
                else
                {
                    var message = string.Empty;
                    if (user != null)
                    {
                        var failedCount = await _userManager.GetAccessFailedCountAsync(user);
                        message = $"{_userManager.Options.Lockout.MaxFailedAccessAttempts - failedCount} Giriş Deneme Hakkınız Kaldı";
                    }
                    else
                    {
                        message = "Kullanıcı Adı Veya Şifre Hatalı ";
                    }
                    ModelState.AddModelError("", message);
                }
                
                

            }
            return View(model);
        }
        [Authorize]
        public IActionResult GetUserInfo()
        {
            var userName = User.Identity.Name;
            var role = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }
        [Authorize(Roles = "member")]
        public IActionResult Panel()
        {
            return View();
        }
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }




    }
}
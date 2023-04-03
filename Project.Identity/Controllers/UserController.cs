using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Identity.Context;
using Project.Identity.Entities;
using Project.Identity.Models;

namespace Project.Identity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserController(AppDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async  Task<IActionResult> Index()
        {

            //var query = _userManager.Users;
            //var users = _context.Users.Join(_context.UserRoles, user => user.Id, userRole => userRole.UserId, (user, userRole) => new
            //{
            //    user,
            //    userRole
            //}).Join(_context.Roles, two => two.userRole.RoleId, role => role.Id, (two, role) => new
            //{
            //    two.user,
            //    two.userRole,
            //    role
            //}).Where(x => x.role.Name != "Admin").Select(x => new AppUser()
            //{
            //    Id = x.user.Id,
            //    AccessFailedCount = x.user.AccessFailedCount,
            //    ConcurrencyStamp = x.user.ConcurrencyStamp,
            //    Email = x.user.Email,
            //    FirstName = x.user.FirstName,
            //    LastName = x.user.LastName,
            //    LockoutEnabled = x.user.LockoutEnabled,
            //    LockoutEnd = x.user.LockoutEnd,
            //    NormalizedEmail = x.user.NormalizedEmail,
            //    NormalizedUserName = x.user.NormalizedUserName,
            //    PasswordHash = x.user.PasswordHash,
            //    PhoneNumber = x.user.PhoneNumber,
            //    UserName = x.user.UserName,
            //}).ToList();

            ////var user = await _userManager.GetUsersInRoleAsync("member");

            //return View(users);
            List<AppUser> filteredUsers = new();
            var users = _userManager.Users.ToList();
            foreach (var item in users)
            {
                var roles = await _userManager.GetRolesAsync(item);
                if (!roles.Contains("Admin"))
                    filteredUsers.Add(item);
            }
            return View(filteredUsers);
        }
        public IActionResult Create()
        {
            return View(new UserAdminCreateModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserAdminCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName

                };
              var result = await _userManager.CreateAsync(user, model.UserName + "123");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "member");
                    return RedirectToAction("Index");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

            }
            return View(model);

        }
        public async Task<IActionResult> AssignRole(string id)
        {

            var user = await _userManager.FindByIdAsync(id);
            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.ToList();

            RoleAssignSendModel model = new RoleAssignSendModel();

            List<RoleAssignListModel> list = new List<RoleAssignListModel>();

            foreach (var role in roles)
            {
                list.Add(new()
                {
                    Name = role.Name,
                    RoleId = role.Id,
                    Exist = userRoles.Contains(role.Name)
                });
            }
            model.Roles = list;
            model.UserId =id;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole(RoleAssignSendModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var userRoles  = await _userManager.GetRolesAsync(user);

            foreach (var item in model.Roles)
            {
                if (item.Exist)
                {
                    if (!userRoles.Contains(item.Name))
                    {
                       await _userManager.AddToRoleAsync(user, item.Name);
                    }
                }
                else
                {
                    if (userRoles.Contains(item.Name))
                    {
                        await _userManager.RemoveFromRoleAsync(user, item.Name);
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}

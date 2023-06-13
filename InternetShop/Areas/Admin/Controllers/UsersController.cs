using InternetShop.Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using InternetShop.UI.Models;

namespace InternetShop.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(ILogger<UsersController> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger= logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await _userManager.GetUsersInRoleAsync("Admin");

                return View(users);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("Error", new ErrorViewModel() { Description = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDTO userDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityUser user = new IdentityUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = userDTO.Email,
                        Email = userDTO.Email,
                        PhoneNumber = userDTO.Phone
                    };

                    var result = await _userManager.CreateAsync(user, userDTO.Password);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        await _userManager.AddToRoleAsync(user, "Admin");

                        return RedirectToAction("Index");
                    }
                }

                IEnumerable<string> errors = ModelState.Values
                    .SelectMany(value => value.Errors)
                    .Select(error => error.ErrorMessage)
                    .ToList();

                ViewBag.Errors = errors;

                return View(userDTO);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("Error", new ErrorViewModel() { Description = ex.Message });
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login log)
        {
            if (!ModelState.IsValid)
            {
                return View(log);
            }
                var user = await _userManager.FindByNameAsync(log.UsernameOrEmail);
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(log.UsernameOrEmail);
                }

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, log.Password, false, true);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "TaskMS");
                    }
                }

                ModelState.AddModelError("", "Invalid username/email or password");

                return View(log);
            }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }

}

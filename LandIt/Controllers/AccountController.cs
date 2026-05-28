using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LandIt.Models;
using Microsoft.AspNetCore.Authorization;

namespace LandIt.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<AppUser> _signInManager;

        public IActionResult Login(string? returnUrl = null)
        {
            return Redirect($"/Identity/Account/Login?returnUrl={returnUrl}"); // so the user returns to the page they were trying to access after logging in
        }

        public IActionResult Register()
        {
            return Redirect("/Identity/Account/Register");
        }


        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [Authorize]
        public IActionResult EditProfile()
        {
            return View();
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            return View();
        }

        [Authorize]
        public IActionResult MyResumes()
        {
            return View();
        }

        [Authorize]
        public IActionResult MyQuestions()
        {
            return View();
        }


        [Authorize]
        public IActionResult MyBookings()
        {
            return View();
        }
    }
}

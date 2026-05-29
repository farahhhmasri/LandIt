using DocumentFormat.OpenXml.Spreadsheet;
using LandIt.Data;
using LandIt.Models;
using LandIt.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LandIt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext dbcontext, ILogger<HomeController> logger)
        {
            _dbcontext = dbcontext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy() => View();

        public IActionResult Terms() => View();

        public IActionResult Contact() => View();


        // (when the user submits the contact form)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactMessage model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                _dbcontext.ContactMessages.Add(model);
                await _dbcontext.SaveChangesAsync();

                TempData["Success"] = "Your message has been sent! We'll get back to you soon.";
                return RedirectToAction(nameof(Contact));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save contact message");
                ModelState.AddModelError(string.Empty, "Something went wrong. Please try again.");
                return View(model);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel
        {
            RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
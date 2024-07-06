using Microsoft.AspNetCore.Mvc;
using Hardware.Models;
using System.Linq;

namespace Hardware.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var admin = _context.Admins.SingleOrDefault(a => a.Username == username && a.Password == password);
            if (admin != null)
            {
                return RedirectToAction("Index", "Product");
            }
            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }
    }
}

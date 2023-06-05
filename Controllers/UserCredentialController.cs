using Microsoft.AspNetCore.Mvc;
using MyWebApplicationCRUD.EmployeeData;
using MyWebApplicationCRUD.Models;
using System.Diagnostics;
using System.Linq;

namespace MyWebApplicationCRUD.Controllers
{
    public class UserCredentialsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserCredentialsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserCredentials model)
        {
            if (ModelState.IsValid)
            {
                bool isValidCredentials = _db.UserCredentials.Any(c => c.UserId == model.UserId && c.Password == model.Password && c.UserType == model.UserType);

                if (isValidCredentials)
                {
                    HttpContext.Session.SetString("UserType", model.UserType);
                    HttpContext.Session.SetString("UserId", model.UserId);

                    TempData["message"] = "Login successful";
                    TempData["messageClass"] = "text-success";

                    if (model.UserType == "normal")
                    {
                        return RedirectToAction("IndexNormal", "Employee");
                    }
                    else if (model.UserType == "hr")
                    {
                        return RedirectToAction("Index", "Employee");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login credentials");
                    TempData["message"] = "Invalid login credentials";
                    TempData["messageClass"] = "text-danger";
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserCredentials model)
        {
            if (ModelState.IsValid)
            {
                _db.UserCredentials.Add(model);
                _db.SaveChanges();

                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, "Registration failed");
            return View(model);
        }

        public IActionResult Index()
        {
            string? userType = HttpContext.Session.GetString("UserType");
            string? userId = HttpContext.Session.GetString("UserId");

            UserCredentials? user = _db.UserCredentials.FirstOrDefault(u => u.UserId == userId);
            string? username = user?.Username;

            ViewBag.UserType = userType;
            ViewBag.Username = username;

            return View();
        }

        public IActionResult IndexNormal()
        {
            string? userType = HttpContext.Session.GetString("UserType");
            string? userId = HttpContext.Session.GetString("UserId");

            UserCredentials? user = _db.UserCredentials.FirstOrDefault(u => u.UserId == userId);
            string? username = user?.Username;

            ViewBag.UserType = userType;
            ViewBag.Username = username;

            return View();
        }
    }
}

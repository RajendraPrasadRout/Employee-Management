using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MyWebApplicationCRUD.Models;
using System.Collections.Generic;
using System.Linq;
using MyWebApplicationCRUD.EmployeeData;

namespace MyWebApplicationCRUD.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly List<EmployeeRecords> _employeeRecords;
        private readonly ApplicationDbContext _db;

        public EmployeeController(ApplicationDbContext db)
        {
            _employeeRecords = new List<EmployeeRecords>();
            _db = db;
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Index()
        {
            string? userType = HttpContext.Session.GetString("UserType");
            string? userId = HttpContext.Session.GetString("UserId");

            ViewBag.UserType = string.IsNullOrEmpty(userType) ? string.Empty : userType.ToUpper();
            ViewBag.UserId = userId;

            if (string.Equals(userType, "normal", System.StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("IndexNormal", "Employee");
            }

            List<EmployeeRecords> employeeRecords = _db.EmployeeRecords.Where(e => e.IsActive).ToList();

            return View(employeeRecords);
        }


        public IActionResult IndexNormal()
        {
            string? userType = HttpContext.Session.GetString("UserType");
            string? userId = HttpContext.Session.GetString("UserId");

            ViewBag.UserType = string.IsNullOrEmpty(userType) ? string.Empty : userType.ToUpper();

            EmployeeRecords? employee = _db.EmployeeRecords.FirstOrDefault(e => e.UserId == userId && e.IsActive);

            if (employee != null)
            {
                ViewBag.DataFound = true;
                ViewBag.Name = employee.Name;
                return View(employee);
            }
            else
            {
                ViewBag.DataFound = false;
                return View();
            }
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(EmployeeRecords model)
        {
            if (ModelState.IsValid)
            {
                string? userId = model.UserId?.ToUpper();
                string? password = model.Password;
                string? userType = model.UserType?.ToUpper();

                EmployeeRecords? employee = _db.EmployeeRecords.FirstOrDefault(c =>
                    c.UserId.ToUpper() == userId &&
                    c.Password == password &&
                    c.UserType.ToUpper() == userType);

                if (employee != null)
                {
                    ViewBag.Name = employee.Name;

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
        public IActionResult Register(EmployeeRecords model)
        {
            if (ModelState.IsValid)
            {
                EmployeeRecords? employee = _db.EmployeeRecords.FirstOrDefault(e => e.UserId == model.UserId);
                if (employee != null)
                {
                    ModelState.AddModelError(string.Empty, "User ID already taken");
                    return View(model);
                }

                model.IsActive = true;
                model.IsDeleted = false;

                _db.EmployeeRecords.Add(model);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Registration failed");
            return View(model);
        }


        public IActionResult Edit(int? id)
        {
            if (!UserIsHR())
            {
                return RedirectToAction("Index");
            }

            if (id == null || id == 0)
            {
                return NotFound();
            }

            EmployeeRecords? employee = _db.EmployeeRecords.FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeRecords model)
        {
            if (!UserIsHR())
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                EmployeeRecords? existingRecord = _db.EmployeeRecords.Find(model.EmployeeId);

                if (existingRecord == null)
                {
                    return NotFound();
                }

                existingRecord.Name = model.Name;
                existingRecord.Email = model.Email;
                existingRecord.Phone = model.Phone;
                existingRecord.Designation = model.Designation;
                existingRecord.Address = model.Address;

                _db.SaveChanges();

                TempData["success"] = "Updated Successfully";

                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Delete(int? id)
        {
            if (!UserIsHR())
            {
                return RedirectToAction("Index");
            }

            if (id == null || id == 0)
            {
                return NotFound();
            }

            EmployeeRecords? employee = _db.EmployeeRecords.FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            if (!UserIsHR())
            {
                return RedirectToAction("Index");
            }

            EmployeeRecords? employee = _db.EmployeeRecords.FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            employee.IsDeleted = true;
            employee.IsActive = false;

            _db.SaveChanges();

            TempData["success"] = "Deleted successfully";

            return RedirectToAction("Index");
        }


        private bool UserIsHR()
        {
            string? userType = HttpContext.Session.GetString("UserType");
            return string.Equals(userType, "HR", System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
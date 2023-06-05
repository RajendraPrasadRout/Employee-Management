using Microsoft.AspNetCore.Mvc;
using MyWebApplicationCRUD.EmployeeData;
using MyWebApplicationCRUD.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyWebApplicationCRUD.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public EmployeeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            string? userType = HttpContext.Session.GetString("UserType");
            string? userId = HttpContext.Session.GetString("UserId");

            UserCredentials? user = _db.UserCredentials.FirstOrDefault(u => u.UserId == userId);
            string? username = user?.Username;

            ViewBag.UserType = string.IsNullOrEmpty(userType) ? string.Empty : userType.ToUpper();
            ViewBag.Username = username;

            if (string.Equals(userType, "normal", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("IndexNormal", "Employee");
            }

            List<EmployeeRecords> objEmployeeList = _db.EmployeeRecords.Where(e => e.IsActive).ToList();
            return View(objEmployeeList);
        }

        public IActionResult IndexNormal()
        {
            string? userType = HttpContext.Session.GetString("UserType");
            string? userId = HttpContext.Session.GetString("UserId");

            UserCredentials? user = _db.UserCredentials.FirstOrDefault(u => u.UserId == userId);
            string? username = user?.Username;

            ViewBag.UserType = string.IsNullOrEmpty(userType) ? string.Empty : userType.ToUpper();
            ViewBag.Username = username;

            if (!string.Equals(userType, "normal", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index");
            }

            List<EmployeeRecords> objEmployeeList = _db.EmployeeRecords.Where(e => e.IsActive).ToList();
            return View(objEmployeeList);
        }
        public IActionResult Create()
        {
            if (!UserIsHR())
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeRecords obj)
        {
            if (!UserIsHR())
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                obj.IsDeleted = false;
                obj.IsActive = true;

                _db.EmployeeRecords.Add(obj);
                _db.SaveChanges();

                TempData["success"] = "Created Successfully";

                return RedirectToAction("Index");
            }

            return View();
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

            EmployeeRecords? employeeRecords = _db.EmployeeRecords.Find(id);

            if (employeeRecords == null)
            {
                return NotFound();
            }

            return View(employeeRecords);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeRecords obj)
        {
            if (!UserIsHR())
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                EmployeeRecords? existingRecord = _db.EmployeeRecords.Find(obj.EmployeeId);

                if (existingRecord == null)
                {
                    return NotFound();
                }

                obj.IsActive = existingRecord.IsActive;
                obj.IsDeleted = existingRecord.IsDeleted;

                existingRecord.Name = obj.Name;
                existingRecord.Email = obj.Email;
                existingRecord.Phone = obj.Phone;
                existingRecord.Designation = obj.Designation;
                existingRecord.Address = obj.Address;

                _db.EmployeeRecords.Update(existingRecord);
                _db.SaveChanges();

                TempData["success"] = "Updated Successfully";

                return RedirectToAction("Index");
            }

            return View();
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

            EmployeeRecords? employeeRecords = _db.EmployeeRecords.Find(id);

            if (employeeRecords == null)
            {
                return NotFound();
            }

            return View(employeeRecords);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            if (!UserIsHR())
            {
                return RedirectToAction("Index");
            }

            EmployeeRecords? obj = _db.EmployeeRecords.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            obj.IsActive = false;
            obj.IsDeleted = true;

            _db.EmployeeRecords.Update(obj);
            _db.SaveChanges();

            TempData["success"] = "Deleted successfully";

            return RedirectToAction("Index");
        }

        private bool UserIsHR()
        {
            string? userType = HttpContext.Session.GetString("UserType");
            return string.Equals(userType, "HR", StringComparison.OrdinalIgnoreCase);
        }

    }
}
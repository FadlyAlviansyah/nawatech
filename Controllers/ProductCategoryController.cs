using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NawatechApp.Data;
using NawatechApp.Models;

namespace NawatechApp.Controllers
{
    [Authorize]    public class ProductCategoryController : Controller
    {
        private readonly AppDbContext _context;
        public ProductCategoryController(AppDbContext context) => _context = context;

        public IActionResult Index()
        {
            var categories = _context.ProductCategories
                .Where(c => !c.IsDeleted)
                .Include(c => c.Users)
                .ToList();
            return View(categories);
        }

        public IActionResult Create() => View();
        [HttpPost]
        public IActionResult Create(ProductCategory category)
        {
            var userEmail = User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                category.UserId = user.Id;
                _context.ProductCategories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Edit(int id)
        {
            var cat = _context.ProductCategories.Find(id);
            if (cat == null || cat.IsDeleted)
            {
                return NotFound();
            }
            return View(cat);
        }
        [HttpPost]
        public IActionResult Edit(ProductCategory category)
        {
            var userEmail = User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return Unauthorized();
            }
            
            var existingCategory = _context.ProductCategories.FirstOrDefault(c => c.Id == category.Id && c.UserId == user.Id);
            if (existingCategory == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                existingCategory.Name = category.Name;
                existingCategory.UserId = user.Id;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Delete(int id)
        {
            var userEmail = User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return Unauthorized();
            }

            var cat = _context.ProductCategories.FirstOrDefault(c => c.Id == id && c.UserId == user.Id);
            if (cat == null || cat.IsDeleted)
            {
                return NotFound();
            }

            cat.IsDeleted = true;
            _context.ProductCategories.Update(cat);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
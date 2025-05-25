using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                .ToList();
            return View(categories);
        }

        public IActionResult Create() => View();
        [HttpPost]
        public IActionResult Create(ProductCategory category)
        {
            if (ModelState.IsValid)
            {
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
            if (ModelState.IsValid)
            {
                _context.ProductCategories.Update(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Delete(int id)
        {
            var cat = _context.ProductCategories.Find(id);
            if (cat != null)
            {
                cat.IsDeleted = true;
                _context.ProductCategories.Update(cat);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
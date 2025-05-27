using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NawatechApp.Data;
using NawatechApp.Models;
using Microsoft.AspNetCore.Authorization;
namespace NawatechApp.Controllers
{

    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Where(p => !p.IsDeleted)
                .Include(p => p.Category)
                .Include(p => p.Users)
                .ToListAsync();

            return View(products);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.ProductCategories.Where(c => !c.IsDeleted).ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product, IFormFile imageFile)
        {
            var userEmail = User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    string path = Path.Combine(_env.WebRootPath, "images/products", fileName);
                    using var stream = new FileStream(path, FileMode.Create);
                    imageFile.CopyTo(stream);
                    product.Image = "/images/products/" + fileName;
                }
                product.UserId = user.Id;
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(_context.ProductCategories.Where(c => !c.IsDeleted).ToList(), "Id", "Name");
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            var prod = _context.Products.Find(id);
            if (prod == null)
                return NotFound();

            ViewBag.Categories = new SelectList(_context.ProductCategories.Where(c => !c.IsDeleted).ToList(), "Id", "Name", prod.CategoryId);
            return View(prod);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            var imageFile = Request.Form.Files["ImageFile"];
            var userEmail = User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return Unauthorized();
            }

            var existingProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id && p.UserId == user.Id);
            if (existingProduct == null)
            {
                return Unauthorized();
            }
            
            if (ModelState.IsValid)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Stock = product.Stock;
                existingProduct.CategoryId = product.CategoryId;
                if (imageFile != null && imageFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existingProduct.Image))
                    {
                        var oldPath = Path.Combine(_env.WebRootPath, existingProduct.Image.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    string folder = Path.Combine(_env.WebRootPath, "images", "products");
                    Directory.CreateDirectory(folder);
                    string path = Path.Combine(folder, fileName);

                    using var stream = new FileStream(path, FileMode.Create);
                    imageFile.CopyTo(stream);

                    existingProduct.Image = "/images/products/" + fileName;
                }

                _context.Products.Update(existingProduct);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_context.ProductCategories.Where(c => !c.IsDeleted).ToList(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var userEmail = User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return Unauthorized();
            }

            var prod = _context.Products.FirstOrDefault(c => c.Id == id && c.UserId == user.Id);
            if (prod == null || prod.IsDeleted)
            {
                return NotFound();
            }

            prod.IsDeleted = true;
            _context.Products.Update(prod);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
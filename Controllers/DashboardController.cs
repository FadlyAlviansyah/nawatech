using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NawatechApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace NawatechApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalCategories = await _context.ProductCategories.CountAsync(c => !c.IsDeleted);
            var totalProducts = await _context.Products.CountAsync(p => !p.IsDeleted);

            ViewBag.TotalCategories = totalCategories;
            ViewBag.TotalProducts = totalProducts;

            return View();
        }
    }
}
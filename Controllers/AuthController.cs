using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NawatechApp.Data;
using NawatechApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;

namespace NawatechApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _hasher;

        private void SendEmail(string toEmail, string subject, string htmlBody)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential("muhamadfadlyalvi@gmail.com", "cqno hbli cmhm fmes"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("muhamadfadlyalvi@gmail.com"),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);
            smtpClient.Send(mailMessage);
        }

        public AuthController(AppDbContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<User>();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
                return RedirectToAction("Login");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound();

            ViewData["UserProfilePicture"] = string.IsNullOrEmpty(user.ProfilePictures)
                ? null
                : user.ProfilePictures;

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(User model, IFormFile? profileImage)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            if (profileImage != null && profileImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "users");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(profileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(fileStream);
                }

                user.ProfilePictures = "/images/users/" + uniqueFileName;
            }

            user.Name = model.Name;
            user.Email = model.Email;

            if (model.Password != null)
            {
                var hasher = new PasswordHasher<User>();
                user.Password = hasher.HashPassword(user, model.Password);
            }

            _context.Update(user);
            await _context.SaveChangesAsync();

            ViewBag.Success = "Profil berhasil diperbarui!";
            return View(user);
        }

        [AllowAnonymous]
        [HttpGet]        
        public IActionResult Register() => View();

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(User user, string password)
        {
            if (!ModelState.IsValid)
                return View(user);

            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Email sudah digunakan.");
                return View(user);
            }

            user.Password = _hasher.HashPassword(user, password);
            user.EmailConfirmationToken = Guid.NewGuid().ToString();
            user.IsEmailConfirmed = false;
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var confirmationLink = Url.Action("ConfirmEmail", "Auth", new
            {
                userId = user.Id,
                token = user.EmailConfirmationToken
            }, protocol: HttpContext.Request.Scheme);

            var message = $"Hi {user.Name},<br/><br/>" +
                          $"Klik link berikut untuk mengonfirmasi email Anda:<br/><a href='{confirmationLink}'>Konfirmasi Email</a>";

            SendEmail(user.Email, "Konfirmasi Email", message);

            ViewBag.Info = "Registrasi berhasil! Silakan cek email untuk konfirmasi.";
            return View("RegisterConfirmation");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(int userId, string token)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.EmailConfirmationToken != token)
            {
                return BadRequest("Link konfirmasi tidak valid atau sudah digunakan.");
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;

            _context.Update(user);
            await _context.SaveChangesAsync();

            ViewBag.Success = "Email berhasil dikonfirmasi! Silakan login.";
            return View("Login");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = "/")
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    
            if (user == null)
            {
                ViewBag.Error = "Email tidak ditemukan.";
                return View();
            }

            if (!user.IsEmailConfirmed)
            {
                ViewBag.Error = "Silakan konfirmasi email Anda terlebih dahulu.";
                return View();
            }

            var result = _hasher.VerifyHashedPassword(user, user.Password, password);

            if (result == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "Password salah.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");
            var authProperties = new AuthenticationProperties
            {
                RedirectUri = returnUrl
            };

            await HttpContext.SignInAsync("MyCookieAuth",
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login");
        }
    }
}

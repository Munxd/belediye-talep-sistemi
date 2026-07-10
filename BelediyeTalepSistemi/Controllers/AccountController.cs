using BelediyeTalepSistemi.Data;
using BelediyeTalepSistemi.Models;
using BelediyeTalepSistemi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BelediyeTalepSistemi.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var emailVarMi = await _context.ApplicationUsers
                .AnyAsync(x => x.Email == model.Email);

            if (emailVarMi)
            {
                ModelState.AddModelError("", "Bu e-posta adresi zaten kayıtlı.");
                return View(model);
            }

            var user = new ApplicationUser
            {
                AdSoyad = model.AdSoyad,
                Email = model.Email,
                Sifre = model.Sifre,
                Rol = "Vatandas",
                KayitTarihi = DateTime.Now
            };

            _context.ApplicationUsers.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(x => x.Email == model.Email && x.Sifre == model.Sifre);

            if (user == null)
            {
                ModelState.AddModelError("", "E-posta veya şifre hatalı.");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("AdSoyad", user.AdSoyad);
            HttpContext.Session.SetString("Rol", user.Rol);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
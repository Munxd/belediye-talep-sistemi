using BelediyeTalepSistemi.Data;
using BelediyeTalepSistemi.Models;
using BelediyeTalepSistemi.Services;
using BelediyeTalepSistemi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BelediyeTalepSistemi.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AccountController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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

            var token = Guid.NewGuid().ToString("N");

            var user = new ApplicationUser
            {
                AdSoyad = model.AdSoyad,
                Email = model.Email,
                Sifre = model.Sifre,
                Rol = "Vatandas",
                EmailConfirmed = false,
                EmailConfirmationToken = token,
                EmailConfirmationTokenExpiresAt = DateTime.Now.AddHours(24),
                KayitTarihi = DateTime.Now
            };

            _context.ApplicationUsers.Add(user);
            await _context.SaveChangesAsync();

            var confirmationLink = Url.Action(
                "ConfirmEmail",
                "Account",
                new
                {
                    email = user.Email,
                    token = user.EmailConfirmationToken
                },
                Request.Scheme
            );

            var mailBody = $@"
                <h3>Belediye Talep Sistemi E-posta Doğrulama</h3>
                <p>Merhaba {user.AdSoyad},</p>
                <p>Hesabınızı doğrulamak için aşağıdaki bağlantıya tıklayın:</p>
                <p>
                    <a href='{confirmationLink}'>Hesabımı Doğrula</a>
                </p>
                <p>Bu bağlantı 24 saat geçerlidir.</p>
            ";

            try
            {
                await _emailService.SendEmailAsync(
                    user.Email,
                    "Belediye Talep Sistemi - E-posta Doğrulama",
                    mailBody
                );

                TempData["SuccessMessage"] = "Kayıt başarılı. Hesabınızı doğrulamak için e-posta adresinize gönderilen bağlantıya tıklayın.";
                return RedirectToAction("Login");
            }
            catch
            {
                TempData["ErrorMessage"] = "Kullanıcı kaydı oluşturuldu ancak doğrulama e-postası gönderilemedi. Mail ayarlarını kontrol edin.";
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "Geçersiz doğrulama bağlantısı.";
                return RedirectToAction("Login");
            }

            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(x => x.Email == email && x.EmailConfirmationToken == token);

            if (user == null)
            {
                TempData["ErrorMessage"] = "Doğrulama bağlantısı geçersiz.";
                return RedirectToAction("Login");
            }

            if (user.EmailConfirmationTokenExpiresAt < DateTime.Now)
            {
                TempData["ErrorMessage"] = "Doğrulama bağlantısının süresi dolmuş.";
                return RedirectToAction("Login");
            }

            user.EmailConfirmed = true;
            user.EmailConfirmationToken = null;
            user.EmailConfirmationTokenExpiresAt = null;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "E-posta adresiniz başarıyla doğrulandı. Artık giriş yapabilirsiniz.";
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

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Giriş yapmadan önce e-posta adresinizi doğrulamanız gerekiyor.");
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
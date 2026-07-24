using BelediyeTalepSistemi.Data;
using BelediyeTalepSistemi.Helpers;
using BelediyeTalepSistemi.Models;
using BelediyeTalepSistemi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BelediyeTalepSistemi.Controllers
{
    [RoleAuthorize(Roles.Vatandas)]
    public class TalepController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TalepController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var talepler = await _context.Talepler
                .Include(t => t.Mudurluk)
                .Include(t => t.TalepDurumu)
                .Where(t => t.ApplicationUserId == userId.Value)
                .OrderByDescending(t => t.OlusturulmaTarihi)
                .ToListAsync();

            return View(talepler);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var talep = await _context.Talepler
                .Include(t => t.ApplicationUser)
                .Include(t => t.Mudurluk)
                .Include(t => t.TalepDurumu)
                .FirstOrDefaultAsync(t => t.Id == id && t.ApplicationUserId == userId.Value);

            if (talep == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            return View(talep);
        }

        public IActionResult Create()
        {
            ViewBag.Mudurlukler = new SelectList(_context.Mudurlukler.ToList(), "Id", "MudurlukAdi");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TalepCreateViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Mudurlukler = new SelectList(_context.Mudurlukler.ToList(), "Id", "MudurlukAdi");
                return View(model);
            }

            var yeniDurum = await _context.TalepDurumlari
                .FirstOrDefaultAsync(d => d.DurumAdi == "Yeni");

            if (yeniDurum == null)
            {
                TempData["ErrorMessage"] = "Talep durumu bulunamadı.";
                ViewBag.Mudurlukler = new SelectList(_context.Mudurlukler.ToList(), "Id", "MudurlukAdi");
                return View(model);
            }

            string? fotografYolu = await FotografKaydet(model.Fotograf);

            var talep = new Talep
            {
                Baslik = model.Baslik,
                Aciklama = model.Aciklama,
                Kategori = model.Kategori,
                MudurlukId = model.MudurlukId,
                TalepDurumuId = yeniDurum.Id,
                ApplicationUserId = userId.Value,
                OncelikSeviyesi = "Orta",
                OlusturulmaTarihi = DateTime.Now,
                AcikAdres = model.AcikAdres,
                Enlem = KonumDegeriCevir(model.Enlem),
                Boylam = KonumDegeriCevir(model.Boylam),
                FotografYolu = fotografYolu
            };

            _context.Talepler.Add(talep);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Talebiniz başarıyla oluşturuldu.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var talep = await _context.Talepler
                .FirstOrDefaultAsync(t => t.Id == id && t.ApplicationUserId == userId.Value);

            if (talep == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var model = new TalepCreateViewModel
            {
                Baslik = talep.Baslik,
                Aciklama = talep.Aciklama,
                Kategori = talep.Kategori,
                MudurlukId = talep.MudurlukId,
                AcikAdres = talep.AcikAdres ?? string.Empty,
                Enlem = talep.Enlem?.ToString(CultureInfo.InvariantCulture),
                Boylam = talep.Boylam?.ToString(CultureInfo.InvariantCulture)
            };

            ViewBag.Mudurlukler = new SelectList(_context.Mudurlukler.ToList(), "Id", "MudurlukAdi", talep.MudurlukId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TalepCreateViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Mudurlukler = new SelectList(_context.Mudurlukler.ToList(), "Id", "MudurlukAdi", model.MudurlukId);
                return View(model);
            }

            var talep = await _context.Talepler
                .FirstOrDefaultAsync(t => t.Id == id && t.ApplicationUserId == userId.Value);

            if (talep == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            talep.Baslik = model.Baslik;
            talep.Aciklama = model.Aciklama;
            talep.Kategori = model.Kategori;
            talep.MudurlukId = model.MudurlukId;
            talep.AcikAdres = model.AcikAdres;
            talep.Enlem = KonumDegeriCevir(model.Enlem);
            talep.Boylam = KonumDegeriCevir(model.Boylam);

            string? yeniFotografYolu = await FotografKaydet(model.Fotograf);

            if (yeniFotografYolu != null)
            {
                talep.FotografYolu = yeniFotografYolu;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Talep başarıyla güncellendi.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var talep = await _context.Talepler
                .Include(t => t.Mudurluk)
                .Include(t => t.TalepDurumu)
                .FirstOrDefaultAsync(t => t.Id == id && t.ApplicationUserId == userId.Value);

            if (talep == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            return View(talep);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var talep = await _context.Talepler
                .FirstOrDefaultAsync(t => t.Id == id && t.ApplicationUserId == userId.Value);

            if (talep == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            _context.Talepler.Remove(talep);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Talep başarıyla silindi.";

            return RedirectToAction("Index");
        }

        private double? KonumDegeriCevir(string? deger)
        {
            if (string.IsNullOrWhiteSpace(deger))
            {
                return null;
            }

            deger = deger.Replace(",", ".");

            if (double.TryParse(deger, NumberStyles.Any, CultureInfo.InvariantCulture, out double sonuc))
            {
                return sonuc;
            }

            return null;
        }

        private async Task<string?> FotografKaydet(IFormFile? fotograf)
        {
            if (fotograf == null || fotograf.Length == 0)
            {
                return null;
            }

            var izinliUzantilar = new[] { ".jpg", ".jpeg", ".png" };
            var uzanti = Path.GetExtension(fotograf.FileName).ToLower();

            if (!izinliUzantilar.Contains(uzanti))
            {
                return null;
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "talepler");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var dosyaAdi = Guid.NewGuid().ToString() + uzanti;
            var dosyaYolu = Path.Combine(uploadsFolder, dosyaAdi);

            using (var stream = new FileStream(dosyaYolu, FileMode.Create))
            {
                await fotograf.CopyToAsync(stream);
            }

            return "/uploads/talepler/" + dosyaAdi;
        }
    }
}
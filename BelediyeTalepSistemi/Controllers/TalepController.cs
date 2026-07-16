using BelediyeTalepSistemi.Data;
using BelediyeTalepSistemi.Helpers;
using BelediyeTalepSistemi.Models;
using BelediyeTalepSistemi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BelediyeTalepSistemi.Controllers
{
    public class TalepController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TalepController(ApplicationDbContext context)
        {
            _context = context;
        }

        [RoleAuthorize(Roles.Vatandas)]
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

        [RoleAuthorize(Roles.Vatandas)]
        public async Task<IActionResult> Details(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var talep = await _context.Talepler
                .Include(t => t.Mudurluk)
                .Include(t => t.TalepDurumu)
                .Include(t => t.ApplicationUser)
                .FirstOrDefaultAsync(t => t.Id == id && t.ApplicationUserId == userId.Value);

            if (talep == null)
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            return View(talep);
        }

        [RoleAuthorize(Roles.Vatandas)]
        public async Task<IActionResult> Create()
        {
            ViewBag.Mudurlukler = new SelectList(
                await _context.Mudurlukler.ToListAsync(),
                "Id",
                "MudurlukAdi"
            );

            return View();
        }

        [HttpPost]
        [RoleAuthorize(Roles.Vatandas)]
        public async Task<IActionResult> Create(TalepCreateViewModel model)
        {
            ViewBag.Mudurlukler = new SelectList(
                await _context.Mudurlukler.ToListAsync(),
                "Id",
                "MudurlukAdi",
                model.MudurlukId
            );

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var yeniDurum = await _context.TalepDurumlari
                .FirstOrDefaultAsync(x => x.DurumAdi == "Yeni");

            if (yeniDurum == null)
            {
                ModelState.AddModelError("", "Talep durumu bulunamadı.");
                return View(model);
            }

            var talep = new Talep
            {
                Baslik = model.Baslik,
                Aciklama = model.Aciklama,
                Kategori = model.Kategori,
                MudurlukId = model.MudurlukId,
                TalepDurumuId = yeniDurum.Id,
                ApplicationUserId = userId.Value,
                OncelikSeviyesi = "Orta",
                OlusturulmaTarihi = DateTime.Now
            };

            _context.Talepler.Add(talep);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Talebiniz başarıyla oluşturuldu.";

            return RedirectToAction("Create");
        }

        [RoleAuthorize(Roles.Vatandas)]
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

            ViewBag.Mudurlukler = new SelectList(
                await _context.Mudurlukler.ToListAsync(),
                "Id",
                "MudurlukAdi",
                talep.MudurlukId
            );

            var model = new TalepCreateViewModel
            {
                Baslik = talep.Baslik,
                Aciklama = talep.Aciklama,
                Kategori = talep.Kategori,
                MudurlukId = talep.MudurlukId
            };

            return View(model);
        }

        [HttpPost]
        [RoleAuthorize(Roles.Vatandas)]
        public async Task<IActionResult> Edit(int id, TalepCreateViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Mudurlukler = new SelectList(
                await _context.Mudurlukler.ToListAsync(),
                "Id",
                "MudurlukAdi",
                model.MudurlukId
            );

            if (!ModelState.IsValid)
            {
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

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Talep başarıyla güncellendi.";

            return RedirectToAction("Index");
        }

        [RoleAuthorize(Roles.Vatandas)]
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
        [RoleAuthorize(Roles.Vatandas)]
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
    }
}
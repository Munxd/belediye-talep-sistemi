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
    }
}
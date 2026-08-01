using BelediyeTalepSistemi.Data;
using BelediyeTalepSistemi.Helpers;
using BelediyeTalepSistemi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BelediyeTalepSistemi.Controllers
{
    [RoleAuthorize(Roles.Personel)]
    public class PersonelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var personel = await _context.ApplicationUsers
                .Include(u => u.Mudurluk)
                .FirstOrDefaultAsync(u => u.Id == userId.Value);

            if (personel == null || personel.MudurlukId == null)
            {
                ViewBag.Uyari = "Bu personel henüz bir müdürlüğe atanmamış.";

                return View(new List<Talep>());
            }

            ViewBag.PersonelMudurluk = personel.Mudurluk?.MudurlukAdi;

            var talepler = await _context.Talepler
                .Include(t => t.ApplicationUser)
                .Include(t => t.Mudurluk)
                .Include(t => t.TalepDurumu)
                .Where(t => t.MudurlukId == personel.MudurlukId && t.AktifMi == true)
                .OrderBy(t => t.OncelikSeviyesi == "Yüksek" ? 0 :
                              t.OncelikSeviyesi == "Orta" ? 1 : 2)
                .ThenByDescending(t => t.OlusturulmaTarihi)
                .ToListAsync();

            return View(talepler);
        }

        public async Task<IActionResult> Details(int id)
        {
            var talep = await _context.Talepler
                .Include(t => t.ApplicationUser)
                .Include(t => t.Mudurluk)
                .Include(t => t.TalepDurumu)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (talep == null)
            {
                return NotFound();
            }

            ViewBag.Durumlar = new SelectList(
                await _context.TalepDurumlari.ToListAsync(),
                "Id",
                "DurumAdi",
                talep.TalepDurumuId
            );

            return View(talep);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, int talepDurumuId, bool problemCozulduMu)
        {
            var talep = await _context.Talepler
                .FirstOrDefaultAsync(t => t.Id == id);

            if (talep == null)
            {
                return NotFound();
            }

            var secilenDurum = await _context.TalepDurumlari
                .FirstOrDefaultAsync(d => d.Id == talepDurumuId);

            if (secilenDurum == null)
            {
                return NotFound();
            }

            talep.TalepDurumuId = talepDurumuId;

            if (secilenDurum.DurumAdi == "Tamamlandı" && problemCozulduMu)
            {
                talep.AktifMi = false;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
using BelediyeTalepSistemi.Data;
using BelediyeTalepSistemi.Helpers;
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
            var talepler = await _context.Talepler
                .Include(t => t.ApplicationUser)
                .Include(t => t.Mudurluk)
                .Include(t => t.TalepDurumu)
                .OrderByDescending(t => t.OlusturulmaTarihi)
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
        public async Task<IActionResult> UpdateStatus(int id, int talepDurumuId)
        {
            var talep = await _context.Talepler.FindAsync(id);

            if (talep == null)
            {
                return NotFound();
            }

            var durumVarMi = await _context.TalepDurumlari
                .AnyAsync(d => d.Id == talepDurumuId);

            if (!durumVarMi)
            {
                TempData["ErrorMessage"] = "Seçilen durum bulunamadı.";
                return RedirectToAction("Details", new { id });
            }

            talep.TalepDurumuId = talepDurumuId;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Talep durumu başarıyla güncellendi.";

            return RedirectToAction("Details", new { id });
        }
    }
}
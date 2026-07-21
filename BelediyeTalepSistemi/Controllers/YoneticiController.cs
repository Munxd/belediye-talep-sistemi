using BelediyeTalepSistemi.Data;
using BelediyeTalepSistemi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BelediyeTalepSistemi.Controllers
{
    [RoleAuthorize(Roles.Yonetici)]
    public class YoneticiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public YoneticiController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.ToplamTalep = await _context.Talepler.CountAsync();

            ViewBag.YeniTalep = await _context.Talepler
                .Include(t => t.TalepDurumu)
                .CountAsync(t => t.TalepDurumu != null && t.TalepDurumu.DurumAdi == "Yeni");

            ViewBag.ToplamVatandas = await _context.ApplicationUsers
                .CountAsync(u => u.Rol == Roles.Vatandas);

            ViewBag.ToplamPersonel = await _context.ApplicationUsers
                .CountAsync(u => u.Rol == Roles.Personel);

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

            return View(talep);
        }

        public async Task<IActionResult> AssignMudurluk(int id)
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

            ViewBag.Mudurlukler = new SelectList(
                await _context.Mudurlukler.ToListAsync(),
                "Id",
                "MudurlukAdi",
                talep.MudurlukId
            );

            return View(talep);
        }

        [HttpPost]
        public async Task<IActionResult> AssignMudurluk(int id, int mudurlukId)
        {
            var talep = await _context.Talepler.FindAsync(id);

            if (talep == null)
            {
                return NotFound();
            }

            var mudurlukVarMi = await _context.Mudurlukler.AnyAsync(m => m.Id == mudurlukId);

            if (!mudurlukVarMi)
            {
                TempData["ErrorMessage"] = "Seçilen müdürlük bulunamadı.";
                return RedirectToAction("Index");
            }

            talep.MudurlukId = mudurlukId;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Talebin müdürlüğü başarıyla güncellendi.";

            return RedirectToAction("Index");
        }
    }
}
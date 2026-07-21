using BelediyeTalepSistemi.Data;
using BelediyeTalepSistemi.Helpers;
using BelediyeTalepSistemi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BelediyeTalepSistemi.Controllers
{
    [RoleAuthorize(Roles.Yonetici)]
    public class MudurlukController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MudurlukController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var mudurlukler = await _context.Mudurlukler
                .Include(m => m.Talepler)
                .ToListAsync();

            return View(mudurlukler);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Mudurluk mudurluk)
        {
            if (!ModelState.IsValid)
            {
                return View(mudurluk);
            }

            _context.Mudurlukler.Add(mudurluk);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Müdürlük başarıyla eklendi.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var mudurluk = await _context.Mudurlukler.FindAsync(id);

            if (mudurluk == null)
            {
                return NotFound();
            }

            return View(mudurluk);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Mudurluk mudurluk)
        {
            if (id != mudurluk.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(mudurluk);
            }

            _context.Mudurlukler.Update(mudurluk);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Müdürlük başarıyla güncellendi.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var mudurluk = await _context.Mudurlukler
                .Include(m => m.Talepler)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mudurluk == null)
            {
                return NotFound();
            }

            return View(mudurluk);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mudurluk = await _context.Mudurlukler
                .Include(m => m.Talepler)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mudurluk == null)
            {
                return NotFound();
            }

            if (mudurluk.Talepler.Any())
            {
                TempData["ErrorMessage"] = "Bu müdürlüğe bağlı talepler olduğu için silme işlemi yapılamaz.";
                return RedirectToAction("Index");
            }

            _context.Mudurlukler.Remove(mudurluk);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Müdürlük başarıyla silindi.";

            return RedirectToAction("Index");
        }
    }
}
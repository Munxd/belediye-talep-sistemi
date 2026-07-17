using BelediyeTalepSistemi.Data;
using BelediyeTalepSistemi.Helpers;
using Microsoft.AspNetCore.Mvc;
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

            return View(talep);
        }
    }
}
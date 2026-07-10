using BelediyeTalepSistemi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BelediyeTalepSistemi.Controllers
{
    public class PersonelController : Controller
    {
        [RoleAuthorize(Roles.Personel)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
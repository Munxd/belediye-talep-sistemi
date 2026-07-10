using BelediyeTalepSistemi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BelediyeTalepSistemi.Controllers
{
    public class YoneticiController : Controller
    {
        [RoleAuthorize(Roles.Yonetici)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
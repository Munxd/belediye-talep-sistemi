using BelediyeTalepSistemi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BelediyeTalepSistemi.Controllers
{
    public class VatandasController : Controller
    {
        [RoleAuthorize(Roles.Vatandas)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
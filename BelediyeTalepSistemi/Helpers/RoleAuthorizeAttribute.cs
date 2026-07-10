using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BelediyeTalepSistemi.Helpers
{
    public class RoleAuthorizeAttribute : Attribute, IActionFilter
    {
        private readonly string[] _roles;

        public RoleAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var rol = context.HttpContext.Session.GetString("Rol");

            if (string.IsNullOrEmpty(rol))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            if (!_roles.Contains(rol))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
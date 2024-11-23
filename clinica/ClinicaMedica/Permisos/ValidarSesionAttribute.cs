using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ClinicaMedica.Permisos
{
    public class ValidarSesionAttribute : ActionFilterAttribute
    {
        private readonly int[] _rolesPermitidos;

        public ValidarSesionAttribute(params int[] rolesPermitidos)
        {
            _rolesPermitidos = rolesPermitidos;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var usuario = session.GetString("USUARIO");
            var rolId = session.GetInt32("RolId");

            if (string.IsNullOrEmpty(usuario) || !_rolesPermitidos.Contains(rolId.GetValueOrDefault()))
            {
                // Redirige al login si no está autenticado o no tiene el rol permitido
                context.Result = new RedirectToActionResult("Login", "Acceso", null);
            }

            base.OnActionExecuting(context);
        }
    }
}

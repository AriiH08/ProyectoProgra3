using ClinicaMedica.Data.Models;
using ClinicaMedica.Permisos;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ClinicaMedica.Controllers
{

    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Acción principal
        public IActionResult Index()
        {
            // Verificar si el usuario está autenticado
            
            return View();
        }

        // Acción para la página de privacidad
        public IActionResult Privacy()
        {

            return View();
        }





       
            public IActionResult CerrarSesion()
        {
            // Limpiar toda la sesión
            HttpContext.Session.Clear();

            // Redirigir al usuario a la página de inicio de sesión
            return RedirectToAction("Login", "Acceso");
        }

        




        // Manejo de errores
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

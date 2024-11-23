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

        // Acci�n principal
        public IActionResult Index()
        {
            // Verificar si el usuario est� autenticado
            
            return View();
        }

        // Acci�n para la p�gina de privacidad
        public IActionResult Privacy()
        {

            return View();
        }





       
            public IActionResult CerrarSesion()
        {
            // Limpiar toda la sesi�n
            HttpContext.Session.Clear();

            // Redirigir al usuario a la p�gina de inicio de sesi�n
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
